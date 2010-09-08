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
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace CloudAppSharp
{
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
    public class CloudAppItem
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }

        private string _name = null;
        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                // If we have a null name and it's a bookmark we have, then the name is the URL!
                if (String.IsNullOrEmpty(_name) && this.ItemType == CloudAppItemType.Bookmark)
                {
                    _name = this.RedirectUrl;
                }

                // Return _name. Even if it's null.
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [DataMember(Name = "url")]
        public string Url { get; set; }

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

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; set; }
    }

    [DataContract]
    public class CloudAppNewItem
    {
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }

        [DataMember(Name = "params", IsRequired = true)]
        internal CloudAppNewItemParams ParamsClass { get; set; }

        public NameValueCollection Params
        {
            get
            {
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
    }

    [DataContract]
    internal class CloudAppNewItemParams
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
