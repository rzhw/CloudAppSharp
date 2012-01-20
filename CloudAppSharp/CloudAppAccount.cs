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
using System.Runtime.Serialization;
using System.Text;

namespace CloudAppSharp
{
    public partial class CloudApp
    {
        /// <summary>
        /// Details of the user currently authenticated.
        /// </summary>
        public CloudAppUser AccountDetails { get; private set; }

        /// <summary>
        /// Sends a password reset message to a given user's email.
        /// </summary>
        /// <param name="email">The email of the user to send the password reset message to.</param>
        public static void ForgotPassword(string email)
        {
            WebClient wc = new WebClient();
            wc.UploadString("http://my.cl.ly/account", "POST",
                JsonHelper.Serialize<CloudAppForgotPassword>(new CloudAppForgotPassword(email)));
        }

        /// <summary>
        /// Registers a new account with the given email and password on CloudApp.
        /// </summary>
        /// <param name="email">The email of the user to register.</param>
        /// <param name="password">The string to use as the user's password.</param>
        public static void Register(string email, string password)
        {
            WebClient wc = new WebClient();
            wc.UploadString("http://my.cl.ly/register", "POST",
                JsonHelper.Serialize<CloudAppRegister>(new CloudAppRegister(email, password)));
        }

        /// <summary>
        /// Changes the default security toggle of items created by a user. Requires authentication.
        /// </summary>
        /// <param name="privateItems">Whether to set the default privacy setting to private.</param>
        public void ChangeDefaultSecurity(bool privateItems)
        {
            ChangeAccountDetail<CloudAppChangeDefaultSecurity>(new CloudAppChangeDefaultSecurity(privateItems));
        }

        /// <summary>
        /// Changes the email of a user. Requires authentication.
        /// </summary>
        /// <param name="newEmail">The new email of the user.</param>
        /// <param name="currentPassword">The current password of the user, for verification.</param>
        public void ChangeEmail(string newEmail, string currentPassword)
        {
            ChangeAccountDetail<CloudAppChangeEmail>(new CloudAppChangeEmail(newEmail, currentPassword));
        }

        /// <summary>
        /// Changes the password of a user. Requires authentication.
        /// </summary>
        /// <param name="newPassword">The new password of the user.</param>
        /// <param name="currentPassword">The current password of the user, for verification.</param>
        public void ChangePassword(string newPassword, string currentPassword)
        {
            ChangeAccountDetail<CloudAppChangePassword>(new CloudAppChangePassword(newPassword, currentPassword));
        }

        /// <summary>
        /// Sets the custom domain of a user. Requires authentication with a CloudApp Pro account.
        /// </summary>
        /// <param name="domain">The domain to set (e.g. example.com, cloud.example.com) </param>
        /// <param name="domainHomePage">Where to redirect from the domain root (e.g. http://www.google.com/) </param>
        /// <returns></returns>
        public bool SetCustomDomain(string domain, string domainHomePage)
        {
            try
            {
                ChangeAccountDetail<CloudAppSetCustomDomain>(new CloudAppSetCustomDomain(domain, domainHomePage));
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (response != null && (int)response.StatusCode == 422)
                {
                    if (AccountDetails.Alpha || AccountDetails.Subscribed)
                        return false;
                    throw new CloudAppProNeededException("You need a CloudApp Pro account to set a custom domain.", e);
                }
            }

            return true;
        }

        private void ChangeAccountDetail<T>(T detailsObject)
        {
            HttpWebRequest wr = _reqHelper.Create("http://my.cl.ly/account", "PUT",
                JsonHelper.Serialize<T>(detailsObject));

            using (HttpWebResponse response = _reqHelper.GetResponse(wr))
            {
                AccountDetails = JsonHelper.Deserialize<CloudAppUser>(response);
            }
        }
    }

    [DataContract]
    public class CloudAppUser : CloudAppJsonBase
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "domain")]
        public string Domain { get; set; }

        [DataMember(Name = "domain_home_page")]
        public string DomainHomePage { get; set; }

        [DataMember]
        private bool? private_items { get; set; } // In case we get null
        public bool PrivateItems
        {
            get { return private_items == true; }
            set { private_items = value; }
        }

        [DataMember]
        private bool? subscribed { get; set; } // In case we get null
        public bool Subscribed
        {
            get { return subscribed == true; }
            set { subscribed = value; }
        }

        [DataMember]
        private bool? alpha { get; set; } // In case we get null
        public bool Alpha
        {
            get { return alpha == true; }
            set { alpha = value; }
        }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; set; }

        [DataMember(Name = "activated_at")]
        public string ActivatedAt { get; set; }

        [DataMember(Name = "socket")]
        public CloudAppPusherDetails Socket { get; set; }
    }

    [DataContract]
    public class CloudAppPusherDetails : CloudAppJsonBase
    {
        [DataMember(Name = "auth_url")]
        public string AuthUrl { get; set; }
        [DataMember(Name = "api_key")]
        public string ApiKey { get; set; }
        [DataMember(Name = "app_id")]
        public string AppId { get; set; }
        [DataMember(Name = "channels")]
        public PusherChannels Channels { get; set; }

        [DataContract]
        public class PusherChannels : CloudAppJsonBase
        {
            [DataMember]
            public string items { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppRegister : CloudAppJsonBase
    {
        public CloudAppRegister() { }
        public CloudAppRegister(string email, string password)
        {
            user = new CloudAppRegisterDetails { email = email, password = password };
        }

        [DataMember]
        public CloudAppRegisterDetails user { get; set; }

        [DataContract]
        public class CloudAppRegisterDetails : CloudAppJsonBase
        {
            [DataMember]
            public string email { get; set; }
            [DataMember]
            public string password { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppForgotPassword : CloudAppJsonBase
    {
        public CloudAppForgotPassword() { }
        public CloudAppForgotPassword(string email)
        {
            user = new CloudAppForgotPasswordDetails { email = email };
        }

        [DataMember]
        public CloudAppForgotPasswordDetails user { get; set; }

        [DataContract]
        public class CloudAppForgotPasswordDetails : CloudAppJsonBase
        {
            [DataMember]
            public string email { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppChangeDefaultSecurity : CloudAppJsonBase
    {
        public CloudAppChangeDefaultSecurity() { }
        public CloudAppChangeDefaultSecurity(bool privateItems)
        {
            user = new CloudAppChangeDefaultSecurityDetails { private_items = privateItems };
        }

        [DataMember]
        public CloudAppChangeDefaultSecurityDetails user { get; set; }

        [DataContract]
        public class CloudAppChangeDefaultSecurityDetails : CloudAppJsonBase
        {
            [DataMember]
            public bool private_items { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppChangeEmail : CloudAppJsonBase
    {
        public CloudAppChangeEmail() { }
        public CloudAppChangeEmail(string newEmail, string currentPassword)
        {
            user = new CloudAppChangeEmailDetails { email = newEmail, current_password = currentPassword };
        }

        [DataMember]
        public CloudAppChangeEmailDetails user { get; set; }

        [DataContract]
        public class CloudAppChangeEmailDetails : CloudAppJsonBase
        {
            [DataMember]
            public string email { get; set; }
            [DataMember]
            public string current_password { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppChangePassword : CloudAppJsonBase
    {
        public CloudAppChangePassword() { }
        public CloudAppChangePassword(string newPassword, string currentPassword)
        {
            user = new CloudAppChangePasswordDetails { password = newPassword, current_password = currentPassword };
        }

        [DataMember]
        public CloudAppChangePasswordDetails user { get; set; }

        [DataContract]
        public class CloudAppChangePasswordDetails : CloudAppJsonBase
        {
            [DataMember]
            public string password { get; set; }
            [DataMember]
            public string current_password { get; set; }
        }
    }

    [DataContract]
    internal class CloudAppSetCustomDomain : CloudAppJsonBase
    {
        public CloudAppSetCustomDomain() { }
        public CloudAppSetCustomDomain(string domain, string domainHomePage)
        {
            user = new CloudAppSetCustomDomainDetails { domain = domain, domain_home_page = domainHomePage };
        }

        [DataMember]
        public CloudAppSetCustomDomainDetails user { get; set; }

        [DataContract]
        public class CloudAppSetCustomDomainDetails : CloudAppJsonBase
        {
            [DataMember]
            public string domain { get; set; }
            [DataMember]
            public string domain_home_page { get; set; }
        }
    }
}
