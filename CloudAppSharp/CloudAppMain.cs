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
        public CloudAppUser AccountDetails { get; private set; }

        public static IWebProxy Proxy { get; set; }

        static CloudApp()
        {
            AuthenticationManager.Register(new CloudAppDigestAuth());
            Proxy = WebRequest.GetSystemWebProxy();
        }

        /// <summary>
        /// Initialises a new instance of the CloudAppSharp.CloudApp class with the specified email and password.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password associated with the credentials.</param>
        public CloudApp(string email, string password)
            : this(email, password, false)
        {
        }

        /// <summary>
        /// Initialises a new instance of the CloudAppSharp.CloudApp class with the specified email and password or HA1 hash.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password or precalculated HA1 associated with the credentials.</param>
        /// <param name="isHA1">This specifies if the password field is a password, or a hash. True if a hash, false if a password.</param>
        public CloudApp(string email, string password, bool isHA1)
        {
            // CloudApp seems to store emails in its database lowercased.
            email = email.ToLower();

            // One stone, two birds: Get our account details AND our cookies!
            HttpWebRequest wr = CreateRequest("http://my.cl.ly/account", "GET");
            wr.Credentials = new DigestCredentials(email, password, isHA1);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                    throw e;
                else if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized)
                    throw new CloudAppInvalidCredentialsException(e);
                else
                    throw e;
            }

            // No problems? Let's store our stuff, then.
            AccountDetails = JsonHelper.Deserialize<CloudAppUser>(response);
            _credentials = (DigestCredentials)wr.Credentials;
            _cookies = wr.CookieContainer;
        }

        public DigestCredentials GetCredentials()
        {
            return new DigestCredentials(_credentials.Username, _credentials.Ha1, true);
        }

        internal HttpWebRequest CreateRequest(Uri requestUri, string method)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(requestUri);
            wr.CookieContainer = this._cookies;
            wr.Proxy = Proxy;
            wr.Method = method;
            wr.Accept = "application/json";
            return wr;
        }

        internal HttpWebRequest CreateRequest(string requestUriString, string method)
        {
            return this.CreateRequest(new Uri(requestUriString), method);
        }

        internal HttpWebRequest CreateRequest(Uri requestUri, string method, string toSend)
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

        internal HttpWebRequest CreateRequest(string requestUriString, string method, string toSend)
        {
            return this.CreateRequest(new Uri(requestUriString), method, toSend);
        }

        internal HttpWebResponse GetRequestResponse(HttpWebRequest wr)
        {
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudAppInvalidProtocolException(HttpStatusCode.OK, response);
            }
            else
            {
                return response;
            }
        }
    }
}
