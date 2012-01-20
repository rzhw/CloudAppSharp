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
        private RequestHelper _reqHelper = null;

        public bool IsConnected { get; set; }
        public int Timeout { get; set; }
        public IWebProxy Proxy { get; set; }
        public CookieContainer CookieContainer { get; internal set; }

        /// <summary>
        /// Occurs immediately before an asynchronous connection attempt to the CloudApp service.
        /// </summary>
        public event EventHandler Connecting;
        /// <summary>
        /// Occurs when an asynchronous connection to the CloudApp service completes.
        /// </summary>
        public event RunWorkerCompletedEventHandler ConnectionCompleted;

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
            CookieContainer = new CookieContainer();
            _reqHelper = new RequestHelper(this);
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
            ConnectCode(email, password, isHA1);
        }
        /// <summary>
        /// Attempts to connect to the CloudApp service asynchronously with the specified email and password.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password associated with the credentials.</param>
        public void ConnectAsync(string email, string password)
        {
            ConnectAsync(email, password, false);
        }
        /// <summary>
        /// Attempts to connect to the CloudApp service asynchronously with the specified email and password or HA1 hash.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password or precalculated HA1 associated with the credentials.</param>
        /// <param name="isHA1">This specifies if the password field is a password, or a hash. True if a hash, false if a password.</param>
        public void ConnectAsync(string email, string password, bool isHA1)
        {
            if (Connecting != null)
                Connecting(this, new EventArgs());
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) => ConnectCode(email, password, isHA1);
            if (ConnectionCompleted != null)
                bw.RunWorkerCompleted += ConnectionCompleted;
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Returns the HTTP digest credentials used to sign into CloudApp.
        /// </summary>
        /// <returns>A DigestCredentials instance containing the HTTP digest credentials used to sign into CloudApp.</returns>
        public DigestCredentials GetCredentials()
        {
            return new DigestCredentials(_credentials.Username, _credentials.Ha1, true);
        }

        private void ConnectCode(string email, string password, bool isHA1)
        {
            // CloudApp seems to store emails in its database lowercased.
            email = email.ToLower();

            // Two birds with one stone; get our account details AND our cookies!
            HttpWebRequest wr = _reqHelper.Create("http://my.cl.ly/account", "GET");
            wr.Credentials = new DigestCredentials(email, password, isHA1);
            HttpWebResponse response = _reqHelper.GetResponse(wr);

            // No problems? Let's store our stuff, then.
            IsConnected = true;
            AccountDetails = JsonHelper.Deserialize<CloudAppUser>(response);
            _credentials = (DigestCredentials)wr.Credentials;
            CookieContainer = wr.CookieContainer;
        }
    }
}
