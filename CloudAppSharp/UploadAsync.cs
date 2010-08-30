// /*!
//  * Project: Salient.Web.HttpLib
//  * http://salient.codeplex.com
//  *
//  * This extract of the library has been licensed by the author under The Code Project Open License (CPOL)
//  * http://www.codeproject.com/Articles/72232/Csharp-File-Upload-with-form-fields-cookies-and-he.aspx
//  *
//  * Modified by a2h to be a non-static class, allow disabling of automatic redirection and to upload asynchronously
//  * THIS TOOK SO DAMN LONG TO GET WORKING; FIRST FOUR ATTEMPTS INVOLVED PLACING new Thread() EVERYWHERE
//  *
//  * Date: August 13 2010 
//  */

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
    internal class SalientUploadAsync
    {
        //byte[] buffer;
        byte[] header;
        byte[] footer;
        string _fileName;

        public int chunkSize = 128;
        public int Progress = 0;

        //public event EventHandler ProgressChanged;
        //public event EventHandler Uploaded;
        //public HttpWebResponse Response;

        HttpWebRequest webrequest;
        public SalientUploadAsync(Uri requestUri, NameValueCollection postData, string fileName, string fileFieldName, CookieContainer cookies,
             NameValueCollection headers, bool allowAutoRedirect)
        {
            // The filename
            _fileName = fileName;

            // The web request
            webrequest = (HttpWebRequest)WebRequest.Create(requestUri);

            // Do we want to allow automatic redirection?
            if (!allowAutoRedirect)
            {
                webrequest.AllowAutoRedirect = false;
            }

            // Read the file into a stream for use
            FileStream fileData = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Blah
            string ctype;

            string fileContentType = TryGetContentType(fileName, out ctype) ? ctype : "application/octet-stream";

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

                sbHeader.AppendFormat("Content-Type: {0}\r\n\r\n", fileContentType);
            }

            header = Encoding.UTF8.GetBytes(sbHeader.ToString());
            footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            long contentLength = header.Length + (fileData != null ?
                fileData.Length : 0) + footer.Length;

            webrequest.ContentLength = contentLength;
        }

        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }
            else
            {
                BackgroundWorker worker = (BackgroundWorker)sender;

                FileStream fileData = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
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

        internal static bool TryGetContentType(string fileName, out string contentType)
        {
            try
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey
                    (@"MIME\Database\Content Type");

                if (key != null)
                {
                    foreach (string keyName in key.GetSubKeyNames())
                    {
                        RegistryKey subKey = key.OpenSubKey(keyName);
                        if (subKey != null)
                        {
                            string subKeyValue = (string)subKey.GetValue("Extension");

                            if (!string.IsNullOrEmpty(subKeyValue))
                            {
                                if (string.Compare(Path.GetExtension
                    (fileName).ToUpperInvariant(),
                                         subKeyValue.ToUpperInvariant(),
                    StringComparison.OrdinalIgnoreCase) ==
                                    0)
                                {
                                    contentType = keyName;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            {
                // fail silently
                // TODO: rethrow registry access denied errors
            }
            // ReSharper restore EmptyGeneralCatchClause
            contentType = "";
            return false;
        }
    }
}