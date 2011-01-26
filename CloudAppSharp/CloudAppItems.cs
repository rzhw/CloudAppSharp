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
using System.Net;
using System.Text;

namespace CloudAppSharp
{
    public partial class CloudApp
    {
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
            GetRequestResponse(wr).Close();
        }

        public CloudAppItem SetPrivacy(CloudAppItem item, bool setPrivate)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "PUT",
                JsonHelper.Serialize<CloudAppItemSecurity>(new CloudAppItemSecurity(setPrivate)));

            using (HttpWebResponse response = GetRequestResponse(wr))
            {
                return JsonHelper.Deserialize<CloudAppItem>(response);
            }
        }

        public CloudAppItem RenameItem(CloudAppItem item, string newName)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "PUT",
                JsonHelper.Serialize<CloudAppItemRename>(new CloudAppItemRename(newName)));

            using (HttpWebResponse response = GetRequestResponse(wr))
            {
                return JsonHelper.Deserialize<CloudAppItem>(response);
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

            using (HttpWebResponse response = GetRequestResponse(wr))
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

    }
}
