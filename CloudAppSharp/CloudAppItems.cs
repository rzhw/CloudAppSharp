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
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace CloudAppSharp
{
    public partial class CloudApp
    {
        /// <summary>
        /// Retrieves information about an item on CloudApp.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee). </param>
        /// <returns></returns>
        public static CloudAppItem GetItemFromUri(string uri)
        {
            return JsonHelper.Deserialize<CloudAppItem>(GetJsonStatic(uri));
        }

        /// <summary>
        /// Retrieves information about an item on CloudApp.
        /// </summary>
        /// <param name="uri">The uri to the item in question (e.g. http://cl.ly/gee). </param>
        /// <returns></returns>
        public static CloudAppItem GetItemFromUri(Uri uri)
        {
            return GetItemFromUri(uri.ToString());
        }

        /// <summary>
        /// Deletes an item on CloudApp added by the logged in user. Requires authentication.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public void DeleteItem(CloudAppItem item)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "DELETE");
            GetRequestResponse(wr).Close();
        }

        /// <summary>
        /// Sets the privacy toggle of an item on CloudApp added by the logged in user. Requires authentication.
        /// </summary>
        /// <param name="item">The item to set the privacy of.</param>
        /// <param name="setPrivate">Whether to set the item to private.</param>
        /// <returns></returns>
        public CloudAppItem SetPrivacy(CloudAppItem item, bool setPrivate)
        {
            HttpWebRequest wr = CreateRequest(item.Href, "PUT",
                JsonHelper.Serialize<CloudAppItemSecurity>(new CloudAppItemSecurity(setPrivate)));

            using (HttpWebResponse response = GetRequestResponse(wr))
            {
                return JsonHelper.Deserialize<CloudAppItem>(response);
            }
        }

        /// <summary>
        /// Renames an item on CloudApp added by the logged in user. Requires authentication.
        /// </summary>
        /// <param name="item">The item to rename.</param>
        /// <param name="newName">The new name for the item.</param>
        /// <returns></returns>
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
        /// Retrieves a list of items added by the logged in user to CloudApp. Requires authentication.
        /// </summary>
        /// <returns></returns>
        public List<CloudAppItem> GetItems()
        {
            return this.GetObjects<CloudAppItem>("http://my.cl.ly/items");
        }

        /// <summary>
        /// Retrieves a list of items added by the logged in user to CloudApp. Requires authentication.
        /// </summary>
        /// <param name="limit">How many items to retrieve at most.</param>
        /// <returns></returns>
        public List<CloudAppItem> GetItems(int limit)
        {
            return this.GetObjects<CloudAppItem>("http://my.cl.ly/items?per_page=" + limit.ToString());
        }

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(string uri)
        {
            return AddBookmark(new Uri(uri));
        }

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(Uri uri)
        {
            return AddBookmark(uri, uri.ToString());
        }

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <param name="name">The name for the bookmark.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(string uri, string name)
        {
            return AddBookmark(new Uri(uri), name);
        }

        /// <summary>
        /// Creates a bookmark from a given URI. Requires authentication.
        /// </summary>
        /// <param name="uri">The URI to create a bookmark from.</param>
        /// <param name="name">The name for the bookmark.</param>
        /// <returns></returns>
        public CloudAppItem AddBookmark(Uri uri, string name)
        {
            HttpWebRequest wr = CreateRequest("http://my.cl.ly/items", "POST",
                JsonHelper.Serialize<CloudAppNewBookmark>(new CloudAppNewBookmark(name, uri.ToString())));

            using (HttpWebResponse response = GetRequestResponse(wr))
                return JsonHelper.Deserialize<CloudAppItem>(response);
        }
    }

    /// <summary>
    /// Specifies a CloudApp item type.
    /// </summary>
    public enum CloudAppItemType
    {
        Bookmark,
        Image,
        Text,
        Archive,
        Audio,
        Video,
        Other
    }

    [DataContract]
    public class CloudAppItem : CloudAppJsonBase
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }

        private string _name = null;
        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                // Passing over a null string is not a very good idea, so...
                if (String.IsNullOrEmpty(_name))
                {
                    // If it's a bookmark we have, then the name is the URL!
                    if (this.ItemType == CloudAppItemType.Bookmark)
                    {
                        _name = this.RedirectUrl;
                    }
                    // Otherwise, we'll just pass a blank string.
                    else
                    {
                        _name = "";
                    }
                }

                // And now we return _name.
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [DataMember(Name = "private")]
        public bool Private { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        public string StandardUrl
        {
            get
            {
                return Url.Replace(new Uri(Url).Host, "cl.ly");
            }
        }

        [DataMember(Name = "content_url")]
        public string ContentUrl { get; set; }

        [DataMember(Name = "item_type")]
        private string item_type { get; set; }

        private CloudAppItemType _itemType;
        private bool _itemTypeSet = false;
        public CloudAppItemType ItemType
        {
            get
            {
                // Have we already determined the enum equivalent?
                if (!_itemTypeSet)
                {
                    // Loop through the enum till we find our item type
                    foreach (CloudAppItemType itemType in Enum.GetValues(typeof(CloudAppItemType)))
                    {
                        if (itemType.ToString().ToLower() == item_type)
                        {
                            _itemType = itemType;
                            _itemTypeSet = true;
                            return itemType;
                        }
                    }

                    // And if we couldn't find it (perhaps because there's since been a new item type), we set it to other!
                    _itemType = CloudAppItemType.Other;
                    _itemTypeSet = true;
                    return CloudAppItemType.Other;
                }
                // Looks like we haven't!
                else
                {
                    return _itemType;
                }
            }
            set
            {
                _itemTypeSet = true;
                _itemType = value;
            }
        }

        [DataMember(Name = "view_counter")]
        public int ViewCounter { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "redirect_url")]
        public string RedirectUrl { get; set; }

        [DataMember(Name = "remote_url")]
        public string RemoteUrl { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; set; }

        [DataMember(Name = "deleted_at")]
        public string DeletedAt { get; set; }
    }

    [DataContract]
    internal class CloudAppNewItem : CloudAppJsonBase
    {
        [DataMember(Name = "max_upload_size")]
        public int MaximumUploadSize { get; set; }

        [DataMember(Name = "uploads_remaining")]
        public int UploadsRemaining { get; set; }

        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }

        public NameValueCollection Params
        {
            get
            {
                // CloudApp will not return any parameters if the free plan limits are exceeded
                if (ParamsClass == null)
                    return null;

                NameValueCollection dParams = new NameValueCollection();
                dParams.Add("signature", ParamsClass.signature);
                dParams.Add("acl", ParamsClass.acl);
                dParams.Add("policy", ParamsClass.policy);
                dParams.Add("success_action_redirect", ParamsClass.success_action_redirect);
                dParams.Add("key", ParamsClass.key);
                dParams.Add("AWSAccessKeyId", ParamsClass.AWSAccessKeyId);
                return dParams;
            }
        }

        [DataMember(Name = "params")]
        internal CloudAppNewItemParams ParamsClass { get; set; }

        [DataContract]
        internal class CloudAppNewItemParams : CloudAppJsonBase
        {
            [DataMember]
            public string signature { get; set; }

            [DataMember]
            public string acl { get; set; }

            [DataMember]
            public string policy { get; set; }

            [DataMember]
            public string success_action_redirect { get; set; }

            [DataMember]
            public string key { get; set; }

            [DataMember]
            public string AWSAccessKeyId { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppNewBookmark : CloudAppJsonBase
    {
        public CloudAppNewBookmark() { }

        public CloudAppNewBookmark(string name, string redirectUrl)
        {
            item = new CloudAppItem { Name = name, RedirectUrl = redirectUrl };
        }

        [DataMember]
        public CloudAppItem item { get; set; }
    }

    [DataContract]
    internal class CloudAppItemSecurity : CloudAppJsonBase
    {
        public CloudAppItemSecurity() { }

        public CloudAppItemSecurity(bool setPrivate)
        {
            item = new CloudAppItemSecurityDetails { Private = setPrivate };
        }

        [DataMember]
        public CloudAppItemSecurityDetails item { get; set; }

        [DataContract]
        public class CloudAppItemSecurityDetails : CloudAppJsonBase
        {
            [DataMember(Name = "private")]
            public bool Private { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppItemRename : CloudAppJsonBase
    {
        public CloudAppItemRename() { }
        public CloudAppItemRename(string name)
        {
            Item = new CloudAppItemRenameDetails { Name = name };
        }

        [DataMember(Name = "item")]
        public CloudAppItemRenameDetails Item { get; set; }

        [DataContract]
        public class CloudAppItemRenameDetails : CloudAppJsonBase
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }
        }
    }
}
