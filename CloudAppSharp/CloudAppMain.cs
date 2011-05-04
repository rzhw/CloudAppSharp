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
using System.IO;
using System.Net;
using System.Text;
using CloudAppSharp.Auth;

namespace CloudAppSharp
{
    /// <summary>
    /// Provides methods for interaction with the CloudApp service.
    /// </summary>
    public partial class CloudApp
    {
        private DigestCredentials _credentials = null;
        private CookieContainer _cookies = new CookieContainer();

        public bool IsConnected { get; set; }
        public int Timeout { get; set; }
        public IWebProxy Proxy { get; set; }

        static CloudApp()
        {
            AuthenticationManager.Register(new CloudAppDigestAuth());
        }

        /// <summary>
        /// Initialises a new instance of the CloudAppSharp.CloudApp class.
        /// </summary>
        public CloudApp()
        {
            IsConnected = false;
            Timeout = 5000;
            Proxy = WebRequest.GetSystemWebProxy();
        }

        /// <summary>
        /// Attempts to connect to the CloudApp service with the specified email and password.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password associated with the credentials.</param>
        public void Connect(string email, string password)
        {
            Connect(email, password, false);
        }

        /// <summary>
        /// Attempts to connect to the CloudApp service with the specified email and password or HA1 hash.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password or precalculated HA1 associated with the credentials.</param>
        /// <param name="isHA1">This specifies if the password field is a password, or a hash. True if a hash, false if a password.</param>
        public void Connect(string email, string password, bool isHA1)
        {
            // CloudApp seems to store emails in its database lowercased.
            email = email.ToLower();

            // Two birds with one stone; get our account details AND our cookies!
            HttpWebRequest wr = CreateRequest("http://my.cl.ly/account", "GET");
            wr.Credentials = new DigestCredentials(email, password, isHA1);
            HttpWebResponse response = GetRequestResponse(wr);

            // No problems? Let's store our stuff, then.
            IsConnected = true;
            AccountDetails = JsonHelper.Deserialize<CloudAppUser>(response);
            _credentials = (DigestCredentials)wr.Credentials;
            _cookies = wr.CookieContainer;
        }

        /// <summary>
        /// Returns the HTTP digest credentials used to sign into CloudApp.
        /// </summary>
        /// <returns>A DigestCredentials instance containing the HTTP digest credentials used to sign into CloudApp.</returns>
        public DigestCredentials GetCredentials()
        {
            return new DigestCredentials(_credentials.Username, _credentials.Ha1, true);
        }

        internal HttpWebRequest CreateRequest(string requestUri, string method)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(requestUri);
            wr.Timeout = Timeout;
            wr.CookieContainer = this._cookies;
            wr.Proxy = Proxy;
            wr.Method = method;
            wr.Accept = "application/json";
            return wr;
        }

        internal HttpWebRequest CreateRequest(string requestUri, string method, string toSend)
        {
            HttpWebRequest wr = this.CreateRequest(requestUri, method);
            wr.ContentType = "application/json";
            byte[] byteArray = Encoding.UTF8.GetBytes(toSend);
            wr.ContentLength = byteArray.Length;
            Stream dataStream = wr.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return wr;
        }

        internal HttpWebResponse GetRequestResponse(HttpWebRequest wr)
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
    }
}
