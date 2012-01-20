using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CloudAppSharp
{
    internal class RequestHelper
    {
        CloudApp _cloudAppInstance;

        public RequestHelper(CloudApp cloudAppInstance)
        {
            _cloudAppInstance = cloudAppInstance;
        }

        public HttpWebRequest Create(string requestUri, string method)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(requestUri);
            wr.Timeout = _cloudAppInstance.Timeout;
            wr.CookieContainer = _cloudAppInstance.CookieContainer;
            wr.Proxy = _cloudAppInstance.Proxy;
            wr.Method = method;
            wr.Accept = "application/json";
            return wr;
        }

        public HttpWebRequest Create(string requestUri, string method, string toSend)
        {
            HttpWebRequest wr = Create(requestUri, method);
            wr.ContentType = "application/json";
            byte[] byteArray = Encoding.UTF8.GetBytes(toSend);
            wr.ContentLength = byteArray.Length;
            Stream dataStream = wr.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return wr;
        }

        public HttpWebResponse GetResponse(HttpWebRequest wr)
        {
            try
            {
                return (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException e)
            {
                HttpWebResponse response = (HttpWebResponse)e.Response;

                if (response == null)
                    throw /*(CloudAppWebException)*/e; // TODO: This cast fails; find a workaround/fix
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new CloudAppInvalidCredentialsException(e);
                else if (response.StatusCode != HttpStatusCode.OK)
                    throw new CloudAppInvalidProtocolException(HttpStatusCode.OK, response);
                else
                    throw /*(CloudAppWebException)*/e;
            }
        }

        public HttpWebResponse GetResponse(string requestUri, string method)
        {
            return GetResponse(Create(requestUri, method));
        }

        public HttpWebResponse GetResponse(string requestUri, string method, string toSend)
        {
            return GetResponse(Create(requestUri, method, toSend));
        }
    }
}
