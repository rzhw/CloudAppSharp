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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace CloudAppSharp
{
    /// <summary>
    /// Provides methods for interaction with the CloudApp service.
    /// </summary>
    public partial class CloudApp
    {
        //private CloudAppSharpWebClient wcMain = new CloudAppSharpWebClient();
        //private CloudAppSharpWebClient wcCloneable = new CloudAppSharpWebClient();
        private CredentialCache credentials = new CredentialCache();
        private CookieContainer cookies = new CookieContainer();
        private static Dictionary<Type, string> jsonUris = new Dictionary<Type,string>();

        /// <summary>
        /// Provides common methods for sending data to and receiving data from a resource identified by a URI,
        /// and in addition, retains cookies and accepts JSON data where possible.
        /// <para>Original class (CookieAwareWebClient) written by Yuriy Solodkyy.</para>
        /// </summary>
        internal class CloudAppSharpWebClient : WebClient
        {
            public CookieContainer m_container = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    // We'll need to take in cookies to allow for the third step of the upload process to complete
                    (request as HttpWebRequest).CookieContainer = m_container;

                    // We can understand JSON! Really!
                    (request as HttpWebRequest).Accept = "application/json";
                }
                return request;
            }
        }

        /// <summary>
        /// Initialises a new instance of the CloudAppSharp.CloudApp class with the specified email and password.
        /// </summary>
        /// <param name="email">The email associated with the credentials.</param>
        /// <param name="password">The password associated with the credentials.</param>
        public CloudApp(string email, string password)
        {
            // JSON URIs
            jsonUris.Clear();
            jsonUris.Add(typeof(CloudAppItem), "http://my.cl.ly/items");
            jsonUris.Add(typeof(CloudAppNewItem), "http://my.cl.ly/items/new");

            // Our working client
            CloudAppSharpWebClient wc = new CloudAppSharpWebClient();

            // Authentication
            credentials.Add(new Uri("http://my.cl.ly/"), "Digest", new NetworkCredential(email, password));
            wc.Credentials = credentials;

            //// The user agent, so Linebreak can do some fun analytics if they so choose
            //wcMain.Headers.Add("User-Agent", String.Format("CloudAppSharp/{0} {1}/{2} {3}",
            //    "0.8.1",
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
            //    ""));

            // Say, are these credentials valid?
            wc.OpenRead("http://my.cl.ly/items/new");

            // We now have some cookies! Yum.
            cookies = wc.m_container;
        }

        /// <summary>
        /// Retrieves information about an item hosted on CloudApp.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee) </param>
        /// <returns></returns>
        public static CloudAppItem GetItemFromUri(Uri uri)
        {
            return JsonHelper.Deserialize<CloudAppItem>(GetJsonStatic(uri));
        }

        /// <summary>
        /// Retrieves information about an item hosted on CloudApp.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee) </param>
        /// <returns></returns>
        public static CloudAppItem GetItemFromUri(string uri)
        {
            return GetItemFromUri(new Uri(uri));
        }

        /// <summary>
        /// Deletes an item hosted on CloudApp uploaded by the logged in user. Requires authentication.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee) </param>
        public void DeleteItemFromUri(Uri uri)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);
            wr.CookieContainer = this.cookies;
            wr.Method = "DELETE";
            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("CloudAppSharp: Expected status to be \"200 OK\"; got \"" + response.StatusCode + " " + response.StatusDescription + "\" instead", WebExceptionStatus.ProtocolError);
                }
            }
        }

        /// <summary>
        /// Deletes an item hosted on CloudApp uploaded by the logged in user. Requires authentication.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee) </param>
        public void DeleteItemFromUri(string uri)
        {
            DeleteItemFromUri(new Uri(uri));
        }
        
        /// <summary>
        /// Retrieves a list of items uploaded by the user to CloudApp. Requires authentication.
        /// </summary>
        /// <returns></returns>
        public List<CloudAppItem> GetItems()
        {
            return this.GetObjects<CloudAppItem>(new Uri("http://my.cl.ly/items"));
        }

        /// <summary>
        /// Retrieves a list of items uploaded by the user to CloudApp. Requires authentication.
        /// </summary>
        /// <param name="limit">How many items to retrieve at most.</param>
        /// <returns></returns>
        public List<CloudAppItem> GetItems(int limit)
        {
            return this.GetObjects<CloudAppItem>(new Uri("http://my.cl.ly/items?per_page=" + limit.ToString()));
        }

        public CloudAppItem AddBookmark(Uri uri)
        {
            return null;
        }

        public CloudAppItem AddBookmark(string uri)
        {
            return this.AddBookmark(new Uri(uri));
        }

        public T GetObject<T>(Uri uri)
        {
            return JsonHelper.Deserialize<T>(this.GetJson(uri));
        }

        public T GetObject<T>()
        {
            return JsonHelper.Deserialize<T>(this.GetJson(this.GetUriFromCloudAppType<T>()));
        }

        public List<T> GetObjects<T>(Uri uri)
        {
            return JsonHelper.Deserialize<List<T>>(this.GetJson(uri));
        }

        private string GetJson(Uri uri)
        {
            CloudAppSharpWebClient wc = new CloudAppSharpWebClient();
            wc.m_container = this.cookies;
            Stream stream = wc.OpenRead(uri);
            return new StreamReader(stream).ReadToEnd();
        }

        private static string GetJsonStatic(Uri uri)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Accept", "application/json");
            return new StreamReader(wc.OpenRead(uri)).ReadToEnd();
        }

        private Uri GetUriFromCloudAppType<T>()
        {
            if (jsonUris.ContainsKey(typeof(T)))
            {
                return new Uri(jsonUris[typeof(T)]);
            }
            else
            {
                throw new ArgumentException("The type passed to a method in the CloudAppSharp namespace doesn't have an assigned URI. If the type exists, then you will need to manually pass a URI.");
            }
        }
    }
}
