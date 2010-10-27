/**
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
        private DigestCredentials credentials = null;
        private CookieContainer cookies = new CookieContainer();
        private static Dictionary<Type, string> jsonUris = new Dictionary<Type, string>();
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

            // JSON URIs
            jsonUris.Clear();
            jsonUris.Add(typeof(CloudAppItem), "http://my.cl.ly/items");
            jsonUris.Add(typeof(CloudAppNewItem), "http://my.cl.ly/items/new");

            // Two stones, one bird: Validate our credentials, AND get our cookies!
            HttpWebRequest wr = CreateRequest("http://my.cl.ly/items/new", "GET");
            wr.Credentials = new DigestCredentials(email, password, isHA1);

            wr.GetResponse(); // Problem? It'll throw an exception.
            credentials = (DigestCredentials)wr.Credentials;
            cookies = wr.CookieContainer;
        }

        public DigestCredentials GetCredentials()
        {
            return new DigestCredentials(credentials.Username, credentials.Ha1, true);
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
        /// <param name="item">The item to delete</param>
        public void DeleteItem(CloudAppItem item)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "DELETE");

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("CloudAppSharp: Expected status to be \"200 OK\"; got \"" + response.StatusCode + " " + response.StatusDescription + "\" instead",
                        null, WebExceptionStatus.ProtocolError, response);
                }
            }
        }

        public CloudAppItem SetPrivacy(CloudAppItem item, bool setPrivate)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "PUT",
                JsonHelper.Serialize<CloudAppItemSecurity>(new CloudAppItemSecurity(setPrivate)));

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("CloudAppSharp: Expected status to be \"200 OK\"; got \"" + response.StatusCode + " " + response.StatusDescription + "\" instead",
                        null, WebExceptionStatus.ProtocolError, response);
                }
                else
                {
                    return JsonHelper.Deserialize<CloudAppItem>(response);
                }
            }
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

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(Uri uri)
        {
            HttpWebRequest wr = CreateRequest("http://my.cl.ly/items", "POST",
                JsonHelper.Serialize<CloudAppNewBookmark>(new CloudAppNewBookmark(uri.ToString(), uri.ToString())));

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                return JsonHelper.Deserialize<CloudAppItem>(response);
            }
        }

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(string uri)
        {
            return this.AddBookmark(new Uri(uri));
        }

        internal HttpWebRequest CreateRequest(Uri requestUri, string method)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(requestUri);
            wr.CookieContainer = this.cookies;
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
    }
}
