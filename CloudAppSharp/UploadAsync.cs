/**
 * Salient Upload
 * <http://www.codeproject.com/Articles/72232/..aspx>
 * The original class was been licensed by its author under
 * The Code Project Open License (CPOL)
 *
 * Modified by a2h to allow disabling of automatic redirection (31/07/2010)
 * Modified by a2h to support asynchronous uploading (13/08/2010)
 * Modified by a2h to remove MIME detection (14/09/2010)
 * Modified by a2h to add proxy support (23/10/2010)
 */

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;
using System.ComponentModel;

namespace CloudAppSharp
{
    internal class SalientUploadAsync : IDisposable
    {
        byte[] header;
        byte[] footer;
        string _fileName;
        public int chunkSize = 128;
        HttpWebRequest webrequest;
        FileStream fileData;

        #region Disposal
        private bool disposed = false;

        ~SalientUploadAsync()
        {
            // Managed resources will be disposed of with deconstruction anyway
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            // Tell the garbage collector the finalise process no longer needs to be run for this object
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!disposed)
            {
                if (disposeManagedResources)
                {
                    fileData.Dispose();
                }
                disposed = true;
            }
        }
        #endregion

        public SalientUploadAsync(Uri requestUri, NameValueCollection postData, string fileName, string fileFieldName, CookieContainer cookies,
             NameValueCollection headers, bool allowAutoRedirect)
        {
            // Open the file
            fileData = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // The filename
            _fileName = fileName;

            // The web request
            webrequest = (HttpWebRequest)WebRequest.Create(requestUri);

            // Proxy
            webrequest.Proxy = CloudApp.Proxy;

            // Timeouts
            webrequest.Timeout = System.Threading.Timeout.Infinite;

            // Do we want to allow automatic redirection?
            if (!allowAutoRedirect)
            {
                webrequest.AllowAutoRedirect = false;
            }

            // Blah
            fileFieldName = string.IsNullOrEmpty(fileFieldName) ? "file" : fileFieldName;

            if (headers != null)
            {
                // set the headers
                foreach (string key in headers.AllKeys)
                {
                    string[] values = headers.GetValues(key);
                    if (values != null)
                        foreach (string value in values)
                        {
                            webrequest.Headers.Add(key, value);
                        }
                }
            }
            webrequest.Method = "POST";

            if (cookies != null)
            {
                webrequest.CookieContainer = cookies;
            }

            string boundary = "----------" + DateTime.Now.Ticks.ToString
                        ("x", CultureInfo.InvariantCulture);

            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;

            StringBuilder sbHeader = new StringBuilder();

            // add form fields, if any
            if (postData != null)
            {
                foreach (string key in postData.AllKeys)
                {
                    string[] values = postData.GetValues(key);
                    if (values != null)
                        foreach (string value in values)
                        {
                            sbHeader.AppendFormat("--{0}\r\n", boundary);
                            sbHeader.AppendFormat("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}\r\n", key,
                                                    value);
                        }
                }
            }

            if (fileData != null)
            {
                sbHeader.AppendFormat("--{0}\r\n", boundary);
                sbHeader.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; {1}\r\n", fileFieldName,
                                        string.IsNullOrEmpty(fileName)
                                            ?
                                                ""
                                            : string.Format(CultureInfo.InvariantCulture,
                        "filename=\"{0}\";",
                                                            Path.GetFileName(fileName)));

                sbHeader.AppendFormat("Content-Type: {0}\r\n\r\n", "application/octet-stream");
            }

            header = Encoding.UTF8.GetBytes(sbHeader.ToString());
            footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            long contentLength = header.Length + (fileData != null ?
                fileData.Length : 0) + footer.Length;

            webrequest.ContentLength = contentLength;
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }
            else
            {
                BackgroundWorker worker = (BackgroundWorker)sender;

                Stream requestStream = webrequest.GetRequestStream();

                requestStream.Write(header, 0, header.Length);

                if (fileData != null)
                {
                    // Write the file data, if any
                    byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileData.Length))];
                    int bytesRead;
                    int i = 0;
                    while ((bytesRead = fileData.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        worker.ReportProgress(Math.Min(100, (int)(((double)(bytesRead * i) / (double)fileData.Length) * (double)100)));
                        requestStream.Write(buffer, 0, bytesRead);
                        i++;
                    }
                }

                // Write the footer
                requestStream.Write(footer, 0, footer.Length);

                // The result
                e.Result = (HttpWebResponse)webrequest.GetResponse();
            }
        }
    }
}