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
        public T GetObject<T>(Uri uri)
        {
            using (HttpWebResponse response = (HttpWebResponse)CreateRequest(uri, "GET").GetResponse())
            {
                return JsonHelper.Deserialize<T>(response);
            }
        }

        public List<T> GetObjects<T>(Uri uri)
        {
            return GetObject<List<T>>(uri);
        }

        private string GetJson(Uri uri)
        {
            HttpWebRequest wr = CreateRequest(uri, "GET");

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
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
