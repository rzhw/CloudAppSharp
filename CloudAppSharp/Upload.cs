/**
 * Salient Upload
 * <http://www.codeproject.com/Articles/72232/..aspx>
 * The original class was been licensed by its author under
 * The Code Project Open License (CPOL)
 *
 * Modified by a2h to allow disabling of automatic redirection (31/07/2010)
 * Modified by a2h to remove MIME detection (14/09/2010)
 */

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;

namespace CloudAppSharp
{
    /// <summary>
    /// This class contains methods excepted from Salient.Web.HttpLib.HttpRequestUtility
    /// for demonstration purposes. Please see http://salient.codeplex.com for full 
    /// implementation
    /// </summary>
    internal static class SalientUpload
    {
        /// <summary>
        /// Uploads a stream using a multipart/form-data POST.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="postData">A NameValueCollection containing form fields 
        /// to post with file data</param>
        /// <param name="fileData">An open, positioned stream containing the file data</param>
        /// <param name="fileName">Optional, a name to assign to the file data.</param>
        /// <param name="fileContentType">Optional. 
        /// If omitted, registry is queried using <paramref name="fileName"/>. 
        /// If content type is not available from registry, 
        /// application/octet-stream will be submitted.</param>
        /// <param name="fileFieldName">Optional, 
        /// a form field name to assign to the uploaded file data. 
        /// If omitted the value 'file' will be submitted.</param>
        /// <param name="cookies">Optional, can pass null. Used to send and retrieve cookies. 
        /// Pass the same instance to subsequent calls to maintain state if required.</param>
        /// <param name="headers">Optional, headers to be added to request.</param>
        /// <returns></returns>
        /// Reference: 
        /// http://tools.ietf.org/html/rfc1867
        /// http://tools.ietf.org/html/rfc2388
        /// http://www.w3.org/TR/html401/interact/forms.html#h-17.13.4.2
        /// 
        public static WebResponse PostFile
        (Uri requestUri, NameValueCollection postData, Stream fileData, string fileName,
                string fileFieldName, CookieContainer cookies,
                NameValueCollection headers, bool allowAutoRedirect)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(requestUri);

            if (!allowAutoRedirect)
            {
                webrequest.AllowAutoRedirect = false;
            }

            fileFieldName = string.IsNullOrEmpty(fileFieldName) ? "file" : fileFieldName;

            // Timeouts
            webrequest.Timeout = System.Threading.Timeout.Infinite;

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

            byte[] header = Encoding.UTF8.GetBytes(sbHeader.ToString());
            byte[] footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            long contentLength = header.Length + (fileData != null ?
                fileData.Length : 0) + footer.Length;

            webrequest.ContentLength = contentLength;

            using (Stream requestStream = webrequest.GetRequestStream())
            {
                requestStream.Write(header, 0, header.Length);


                if (fileData != null)
                {
                    // write the file data, if any
                    byte[] buffer = new Byte[checked((uint)Math.Min(4096,
                        (int)fileData.Length))];
                    int bytesRead;
                    while ((bytesRead = fileData.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                }

                // write footer
                requestStream.Write(footer, 0, footer.Length);

                return webrequest.GetResponse();
            }
        }

        /// <summary>
        /// Uploads a file using a multipart/form-data POST.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="postData">A NameValueCollection containing 
        /// form fields to post with file data</param>
        /// <param name="fileName">The physical path of the file to upload</param>
        /// <param name="fileContentType">Optional. 
        /// If omitted, registry is queried using <paramref name="fileName"/>. 
        /// If content type is not available from registry, 
        /// application/octet-stream will be submitted.</param>
        /// <param name="fileFieldName">Optional, a form field name 
        /// to assign to the uploaded file data. 
        /// If omitted the value 'file' will be submitted.</param>
        /// <param name="cookies">Optional, can pass null. Used to send and retrieve cookies. 
        /// Pass the same instance to subsequent calls to maintain state if required.</param>
        /// <param name="headers">Optional, headers to be added to request.</param>
        /// <returns></returns>
        public static WebResponse PostFile
        (Uri requestUri, NameValueCollection postData, string fileName,
             string fileFieldName, CookieContainer cookies,
             NameValueCollection headers, bool allowAutoRedirect)
        {
            using (FileStream fileData = File.Open
            (fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return PostFile(requestUri, postData, fileData,
            fileName, fileFieldName, cookies,
                                headers, allowAutoRedirect);
            }
        }
    }
}