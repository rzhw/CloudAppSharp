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

namespace CloudAppSharp
{
    public partial class CloudApp
    {
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
            return new StreamReader(wc.OpenRead(uri)).ReadToEnd();
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
