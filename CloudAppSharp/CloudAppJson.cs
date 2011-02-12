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

namespace CloudAppSharp
{
    public partial class CloudApp
    {
        /// <summary>
        /// Returns a deserialised JSON response from the given URI as the logged in user.
        /// </summary>
        /// <typeparam name="T">The type to deserialise the JSON to.</typeparam>
        /// <param name="uri">The URI to retrieve the JSON from.</param>
        /// <returns>The deserialised JSON.</returns>
        public T GetObject<T>(Uri uri)
        {
            using (HttpWebResponse response = GetRequestResponse(CreateRequest(uri, "GET")))
            {
                return JsonHelper.Deserialize<T>(response);
            }
        }

        /// <summary>
        /// Returns a deserialised JSON response from the given URI as the logged in user as a List of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="uri">The URI to retrieve the JSON from.</param>
        /// <returns>The list.</returns>
        public List<T> GetObjects<T>(Uri uri)
        {
            return GetObject<List<T>>(uri);
        }

        private string GetJson(Uri uri)
        {
            using (HttpWebResponse response = GetRequestResponse(CreateRequest(uri, "GET")))
            {
                Stream dataStream = response.GetResponseStream();
                return new StreamReader(dataStream).ReadToEnd();
            }
        }

        private static string GetJsonStatic(Uri uri)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Accept", "application/json");
            return wc.DownloadString(uri);
        }
    }
}
