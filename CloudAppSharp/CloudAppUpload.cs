/**
 * CloudAppSharp
 * Copyright (c) 2010-2011 Richard Z.H. Wang <http://rewrite.name/>
 *
 * This library is licened under The Code Project Open License (CPOL) 1.02
 * which can be found online at <http://www.codeproject.com/info/cpol10.aspx>
 * 
 * THIS WORK IS PROVIDED "AS IS", "WHERE IS" AND "AS AVAILABLE", WITHOUT
 * ANY EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS OR GUARANTEES. YOU,
 * THE USER, ASSUME ALL RISK IN ITS USE, INCLUDING COPYRIGHT INFRINGEMENT,
 * PATENT INFRINGEMENT, SUITABILITY, ETC. AUTHOR EXPRESSLY DISCLAIMS ALL
 * EXPRESS, IMPLIED OR STATUTORY WARRANTIES OR CONDITIONS, INCLUDING
 * WITHOUT LIMITATION, WARRANTIES OR CONDITIONS OF MERCHANTABILITY,
 * MERCHANTABLE QUALITY OR FITNESS FOR A PARTICULAR PURPOSE, OR ANY
 * WARRANTY OF TITLE OR NON-INFRINGEMENT, OR THAT THE WORK (OR ANY
 * PORTION THEREOF) IS CORRECT, USEFUL, BUG-FREE OR FREE OF VIRUSES.
 * YOU MUST PASS THIS DISCLAIMER ON WHENEVER YOU DISTRIBUTE THE WORK OR
 * DERIVATIVE WORKS.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace CloudAppSharp
{
    public partial class CloudApp
    {
        /// <summary>
        /// Uploads the specified local file to CloudApp. Requires authentication.
        /// </summary>
        /// <param name="fileName">The file to send to CloudApp.</param>
        /// <returns></returns>
        public CloudAppItem Upload(string fileName)
        {
            CloudAppNewItem newItem = this.GetObject<CloudAppNewItem>(new Uri("http://my.cl.ly/items/new"));

            if (newItem.Params == null)
                throw new CloudAppUploadCountLimitExceededException();
            else if (newItem.MaximumUploadSize > 0 && new FileInfo(fileName).Length > newItem.MaximumUploadSize)
                throw new CloudAppUploadSizeLimitExceededException();

            HttpWebResponse uploadResponse = (HttpWebResponse)SalientUpload.PostFile(new Uri(newItem.Url), newItem.Params, fileName, "file", null, null, false);

            if (uploadResponse.StatusCode == HttpStatusCode.SeeOther)
            {
                return GetObject<CloudAppItem>(new Uri(uploadResponse.Headers["Location"]));
            }
            else
            {
                // TODO: Implement setting the uploader WebRequest's ServicePoint.Expect100Continue to false
                // if we get a 417 Expectation Failed, and then try upload again(?) - see the comment regarding
                // this below in CloudAppAsyncUploader.PrepareUpload()
                throw new CloudAppInvalidProtocolException(HttpStatusCode.SeeOther, uploadResponse);
            }
        }
    }

    /// <summary>
    /// Facilitates the uploading of a file to CloudApp.
    /// </summary>
    public class CloudAppAsyncUploader
    {
        private string _fileName;
        private CloudApp _cloudApp;
        internal BackgroundWorker Worker { get; set; }
        public bool IsCancelled { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsBusy { get; private set; }

        public event CloudAppUploadProgressChangedEventHandler ProgressChanged;
        public event CloudAppUploadCompletedEventHandler Completed;
        public event EventHandler Ready;

        /// <summary>
        /// Initialises a new instance of the CloudAppSharp.CloudAppAsyncUploader class with a logged in instance of CloudAppSharp.CloudApp and the path to the file to upload.
        /// </summary>
        /// <param name="cloudApp">A logged in instance of CloudAppSharp.CloudApp</param>
        /// <param name="fileName">The path to the file to upload.</param>
        public CloudAppAsyncUploader(CloudApp cloudApp, string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;

            _fileName = fileName;
            _cloudApp = cloudApp;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) => e.Result = _cloudApp.GetObject<CloudAppNewItem>(new Uri("http://my.cl.ly/items/new"));
            bw.RunWorkerCompleted += (sender, e) => PrepareUpload((CloudAppNewItem)e.Result);
            bw.RunWorkerAsync();
        }

        private void PrepareUpload(CloudAppNewItem newItem)
        {
            if (newItem.Params == null)
            {
                IsCancelled = true;
                if (Completed != null)
                    Completed(this, new CloudAppUploadCompletedEventArgs(new CloudAppUploadCountLimitExceededException(), true));
                return;
            }
            else if (newItem.MaximumUploadSize > 0 && new FileInfo(_fileName).Length > newItem.MaximumUploadSize)
            {
                IsCancelled = true;
                if (Completed != null)
                    Completed(this, new CloudAppUploadCompletedEventArgs(new CloudAppUploadSizeLimitExceededException(), true));
                return;
            }

            // Try and create our uploader
            SalientUploadAsync uploader;
            try
            {
                uploader = new SalientUploadAsync(new Uri(newItem.Url), newItem.Params, _fileName, "file", null, null, false);
            }
            catch (Exception e)
            {
                IsCancelled = true;
                if (Completed != null)
                    Completed(this, new CloudAppUploadCompletedEventArgs(e, true));
                return;
            }

            // If we got it, then attach our worker to its task.
            Worker.DoWork += uploader.DoWork;

            // For updating upload progress
            Worker.ProgressChanged += (sender, e) =>
            {
                if (ProgressChanged != null)
                    ProgressChanged(this, new CloudAppUploadProgressChangedEventArgs(e.ProgressPercentage));
            };

            // When we're done...
            Worker.RunWorkerCompleted += (sender, e) =>
            {
                IsCompleted = true;
                uploader.Dispose();

                if (e.Cancelled)
                {
                    if (Completed != null)
                    {
                        CloudAppUploadCompletedEventArgs e2 = new CloudAppUploadCompletedEventArgs();
                        e2.Cancelled = true;
                        Completed(this, e2);
                    }
                }
                else
                {
                    HttpWebRequest uploadRequest = (HttpWebRequest)e.Result;
                    HttpWebResponse uploadResponse = null;

                    try
                    {
                        uploadResponse = (HttpWebResponse)uploadRequest.GetResponse();
                    }
                    catch (WebException e2)
                    {
                        uploadResponse = (HttpWebResponse)e2.Response;
                    }

                    if (uploadResponse != null && uploadResponse.StatusCode == HttpStatusCode.SeeOther)
                    {
                        if (Completed != null)
                        {
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += (sender2, e2) =>
                                e2.Result = _cloudApp.GetObject<CloudAppItem>(new Uri(uploadResponse.Headers["Location"]));
                            bw.RunWorkerCompleted += (sender2, e2) =>
                                Completed(this, new CloudAppUploadCompletedEventArgs((CloudAppItem)e2.Result));
                            bw.RunWorkerAsync();
                        }
                    }
                    else if (uploadResponse != null && uploadResponse.StatusCode == HttpStatusCode.ExpectationFailed
                        && uploader.webrequest.ServicePoint.Expect100Continue)
                    {
                        // It appears f.cl.ly (i.e. S3) may return 417 Expectation Failed for some users...
                        // I don't know where the exact trigger of the problem is, but considering that sending
                        // Expect: 100-Continue requires the server to respond with 100 Continue, if the server
                        // or whatever is in-between doesn't support it, I would expect it to respond with this
                        // error before any of the file data is uploaded. I hope. If that's not the case, double
                        // uploading sure sounds like a fun thing to deal with.
                        //
                        // References:
                        // http://stackoverflow.com/q/566437/566847#566847
                        // http://haacked.com/archive/2004/05/15/http-web-request-expect-100-continue.aspx#1908
                        uploader.webrequest.ServicePoint.Expect100Continue = false;

                        // Time to reupload!
                        Worker.RunWorkerAsync();
                    }
                    else
                    {
                        if (Completed != null)
                            Completed(this, new CloudAppUploadCompletedEventArgs(
                                new CloudAppInvalidProtocolException(HttpStatusCode.SeeOther, uploadResponse), false));
                    }
                }
            };

            if (Ready != null)
                Ready(this, new EventArgs());
        }

        public void Upload()
        {
            IsBusy = true;
            Worker.RunWorkerAsync();
        }

        public void Cancel()
        {
            IsCancelled = true;
            Worker.CancelAsync();
        }
    }

    public delegate void CloudAppUploadProgressChangedEventHandler(object sender, CloudAppUploadProgressChangedEventArgs e);

    /// <summary>
    /// Provides data for the CloudApp.CloudApp.UploadAsyncProgressChanged event.
    /// </summary>
    public class CloudAppUploadProgressChangedEventArgs : EventArgs
    {
        public CloudAppUploadProgressChangedEventArgs(int progressPercentage)
        {
            ProgressPercentage = progressPercentage;
        }

        public int ProgressPercentage { get; internal set; }
    }

    public delegate void CloudAppUploadCompletedEventHandler(object sender, CloudAppUploadCompletedEventArgs e);

    /// <summary>
    /// Provides data for the CloudApp.CloudApp.UploadAsyncCompleted event.
    /// </summary>
    public class CloudAppUploadCompletedEventArgs : EventArgs
    {
        public CloudAppUploadCompletedEventArgs() { }

        public CloudAppUploadCompletedEventArgs(CloudAppItem uploadedItem)
        {
            Cancelled = false;
            UploadedItem = uploadedItem;
        }

        public CloudAppUploadCompletedEventArgs(Exception error, bool uploadWasCancelled)
        {
            Cancelled = uploadWasCancelled;
            Error = error;
        }

        public CloudAppItem UploadedItem { get; internal set; }
        public Exception Error { get; internal set; }
        public bool Cancelled { get; internal set; }
    }
}
