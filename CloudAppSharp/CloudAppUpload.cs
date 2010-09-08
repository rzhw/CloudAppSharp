/*!
 * CloudAppSharp
 * Copyright (c) 2010 a2h <http://a2h.uni.cc/>
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
using System.ComponentModel;
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
            CloudAppNewItem newItem = this.GetObject<CloudAppNewItem>();
            HttpWebResponse uploadResponse = (HttpWebResponse)SalientUpload.PostFile(new Uri(newItem.Url), newItem.Params, fileName, null, "file", null, null, false);

            if (uploadResponse.StatusCode == HttpStatusCode.SeeOther)
            {
                return this.GetObject<CloudAppItem>(new Uri(uploadResponse.Headers["Location"]));
            }
            else
            {
                throw new WebException("CloudAppSharp: Expected status to be \"303 See Other\"; got \"" + uploadResponse.StatusCode + " " + uploadResponse.StatusDescription + "\" instead", WebExceptionStatus.ProtocolError);
            }
        }

        public event CloudAppUploadProgressChangedEventHandler UploadAsyncProgressChanged;
        public event CloudAppUploadCompletedEventHandler UploadAsyncCompleted;

        /// <summary>
        /// Uploads the specified local file to CloudApp asynchronously. Requires authentication.
        /// </summary>
        /// <param name="fileName">The file to send to CloudApp.</param>
        /// <returns></returns>
        public void UploadAsync(string fileName)
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler((sender, e) =>
                {
                    e.Result = this.GetObject<CloudAppNewItem>();
                });

            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) =>
                {
                    CloudAppNewItem newItem = (CloudAppNewItem)e.Result;
                    SalientUploadAsync uploader = new SalientUploadAsync(new Uri(newItem.Url), newItem.Params, fileName, "file", null, null, false);

                    BackgroundWorker bw2 = new BackgroundWorker();
                    bw2.WorkerReportsProgress = true;
                    bw2.DoWork += new DoWorkEventHandler(uploader.DoWork);

                    if (UploadAsyncProgressChanged != null)
                    {
                        bw2.ProgressChanged += new ProgressChangedEventHandler((sender2, e2) =>
                            {
                                UploadAsyncProgressChanged(this, new CloudAppUploadProgressChangedEventArgs(e2.ProgressPercentage));
                            });
                    }

                    bw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender2, e2) =>
                        {
                            HttpWebResponse uploadResponse = (HttpWebResponse)e2.Result;

                            if (uploadResponse.StatusCode == HttpStatusCode.SeeOther)
                            {
                                if (UploadAsyncCompleted != null)
                                {
                                    BackgroundWorker bw3 = new BackgroundWorker();
                                    bw3.DoWork += new DoWorkEventHandler((sender3, e3) =>
                                        {
                                            e3.Result = this.GetObject<CloudAppItem>(new Uri(uploadResponse.Headers["Location"]));
                                        });
                                    bw3.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender3, e3) =>
                                        {
                                            UploadAsyncCompleted(this, new CloudAppUploadCompletedEventArgs((CloudAppItem)e3.Result));
                                        });
                                    bw3.RunWorkerAsync();
                                }
                            }
                            else
                            {
                                throw new WebException("CloudAppSharp: Expected status to be \"303 See Other\"; got \"" + uploadResponse.StatusCode + " " + uploadResponse.StatusDescription + "\" instead", WebExceptionStatus.ProtocolError);
                            }
                        });

                    bw2.RunWorkerAsync();
                });

            bw.RunWorkerAsync();
        }
    }

    // TODO: Add support for cancelling the upload and checking whether it has been cancelled.

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

        public int ProgressPercentage { get; set; }
    }

    public delegate void CloudAppUploadCompletedEventHandler(object sender, CloudAppUploadCompletedEventArgs e);

    /// <summary>
    /// Provides data for the CloudApp.CloudApp.UploadAsyncCompleted event.
    /// </summary>
    public class CloudAppUploadCompletedEventArgs : EventArgs
    {
        public CloudAppUploadCompletedEventArgs(CloudAppItem uploadedItem)
        {
            UploadedItem = uploadedItem;
        }

        public CloudAppItem UploadedItem { get; set; }
    }
}
