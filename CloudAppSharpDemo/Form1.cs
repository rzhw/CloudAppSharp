using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CloudAppSharp;
using CloudAppSharp.Auth;

namespace CloudAppSharpDemo
{
    public partial class Form1 : Form
    {
        private bool _cloudAppLogged = false;
        public CloudApp _cloudApp;

        public Form1()
        {
            Font = SystemFonts.MessageBoxFont;
            AutoScaleMode = AutoScaleMode.Font;
            InitializeComponent();
            labelDetailsName.Location = new Point(28, 21);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!_cloudAppLogged)
            {
                _cloudApp = new CloudApp(textBoxEmail.Text, textBoxPassword.Text);
                _cloudAppLogged = true;
                textBoxEmail.Enabled = false;
                textBoxPassword.Enabled = false;
                groupBoxAddBookmark.Enabled = true;
                groupBoxUploadFile.Enabled = true;
                groupBoxUploads.Enabled = true;
                buttonLogin.Text = "Logout";
                UpdateAccountDetails();
            }
            else
            {
                _cloudApp = null;
                _cloudAppLogged = false;
                textBoxEmail.Enabled = true;
                textBoxPassword.Enabled = true;
                groupBoxAddBookmark.Enabled = false;
                groupBoxUploadFile.Enabled = false;
                groupBoxUploads.Enabled = false;
                buttonLogin.Text = "Login";
                textBoxAccountDetails.Text = "Not logged in";
            }
        }

        private void buttonAddBookmark_Click(object sender, EventArgs e)
        {
            CloudAppItem bookmark = _cloudApp.AddBookmark(new Uri(textBoxAddBookmark.Text));
            UpdateDetailsArea(bookmark);
        }

        private void buttonUploadFileBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            DialogResult openDialogResult = openDialog.ShowDialog();
            if (openDialogResult == DialogResult.OK)
            {
                textBoxUploadFile.Text = openDialog.FileName;
            }
        }

        private void buttonUploadFile_Click(object sender, EventArgs e)
        {
            string filePath = textBoxUploadFile.Text;
            CloudAppItem uploadedItem = null;

            if (File.Exists(filePath))
            {
                uploadedItem = _cloudApp.Upload(textBoxUploadFile.Text);
                UpdateDetailsArea(uploadedItem);
            }
            else
            {
                MessageBox.Show("The file you tried to upload doesn't exist!", "CloudAppSharp Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSetCustomDomain_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cloudApp.SetCustomDomain(textBoxCustomDomain.Text, textBoxCustomDomainRedirect.Text))
                {
                    MessageBox.Show("Success!");
                    UpdateAccountDetails();
                }
                else
                {
                    MessageBox.Show("Failure!");
                }
            }
            catch (CloudAppProNeededException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Item listing
        private void buttonUploadsRefresh_Click(object sender, EventArgs e)
        {
            listViewUploads.Items.Clear();

            List<CloudAppItem> items = _cloudApp.GetItems();

            foreach (CloudAppItem item in items)
            {
                FillListItem(listViewUploads.Items.Add(""), item);
            }
        }

        private void FillListItem(ListViewItem listViewItem, CloudAppItem _cloudAppItem)
        {
            listViewItem.SubItems.Clear();
            listViewItem.Tag = _cloudAppItem;
            listViewItem.Text = _cloudAppItem.Name;
            listViewItem.SubItems.Add(""); // icon
            listViewItem.SubItems.Add(_cloudAppItem.ViewCounter.ToString());
            listViewItem.SubItems.Add(_cloudAppItem.Private ? "N" : "Y");
            listViewItem.SubItems.Add(_cloudAppItem.CreatedAt);
            listViewItem.SubItems.Add(_cloudAppItem.UpdatedAt);
        }

        private void listViewUploads_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonUploadsDetails.Enabled = true;
            buttonUploadsPrivacy.Enabled = true;
            buttonUploadsRename.Enabled = true;
            buttonUploadsDelete.Enabled = true;
        }

        private void buttonUploadsPrivacy_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            if (MessageBox.Show("Do you want to make this item " + (item.Private ? "public" : "private") + "?",
                "CloudAppSharp Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                CloudAppItem itemNew = _cloudApp.SetPrivacy(item, !item.Private);
                FillListItem(listViewUploads.FocusedItem, itemNew);
            }
        }

        public CloudAppItem _tempItem;
        private void buttonUploadsRename_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            using (FormRename formRename = new FormRename(this, item))
            {
                formRename.ShowDialog();
                FillListItem(listViewUploads.FocusedItem, _tempItem);
            }
        }

        private void buttonUploadsDelete_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            if (MessageBox.Show("Are you sure you want to delete " + item.Name + "?",
                "CloudAppSharp Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                _cloudApp.DeleteItem(item);
                buttonUploadsRefresh.PerformClick();
            }
        }

        private void buttonUploadsDetails_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            UpdateDetailsArea(item);
        }
        #endregion

        private void buttonDetailsFromUrl_Click(object sender, EventArgs e)
        {
            UpdateDetailsArea(CloudApp.GetItemFromUri(new Uri(textBoxDetailsFromUrl.Text)));
        }

        private void UpdateDetailsArea(CloudAppItem item)
        {
            tabControl1.SelectedTab = tabPageItemDetails;

            pictureBoxDetails.Image = UriToBitmap(item.Icon);

            labelDetailsName.Text = String.Format("{0} ({1}, {2} views, {3})",
                item.Name, item.ItemType, item.ViewCounter, item.Private ? "Private" : "Public");

            bool hasCustomDomain = !String.IsNullOrEmpty(_cloudApp.AccountDetails.Domain);

            textBoxDetails.Text = String.Format(
                "URL: {0}\r\nContent URL: {1}\r\nHref: {2}\r\n{3}"
                    + "\r\nCreated: {4}\r\nUpdated: {5}\r\nDeleted: {6}",
                item.Url + (hasCustomDomain ? " (" + item.StandardUrl + ")" : ""),
                item.ContentUrl,
                item.Href,
                item.ItemType == CloudAppItemType.Bookmark ? "Redirect URL: " + item.RedirectUrl : "Remote URL: " + item.RemoteUrl,
                item.CreatedAt,
                item.UpdatedAt,
                String.IsNullOrEmpty(item.DeletedAt) ? "null" : item.DeletedAt
            );
        }

        private void UpdateAccountDetails()
        {
            DigestCredentials digestCredentials = _cloudApp.GetCredentials();
            textBoxAccountDetails.Text = String.Format("Logged in with HA1 hash {10}\r\n\r\n"
                + "ID: {0}\r\nEmail: {1}\r\nDomain: {2}\r\nDomain home page: {3}\r\n"
                + "Item default privacy: {4}\r\nSubscribed: {5}\r\nAlpha user: {6}\r\n"
                + "Account creation date: {7}\r\nAccount updated date: {8}\r\nAccount activated date: {9}",
                _cloudApp.AccountDetails.ID,
                _cloudApp.AccountDetails.Email,
                _cloudApp.AccountDetails.Domain,
                _cloudApp.AccountDetails.DomainHomePage,
                _cloudApp.AccountDetails.PrivateItems,
                _cloudApp.AccountDetails.Subscribed,
                _cloudApp.AccountDetails.Alpha,
                _cloudApp.AccountDetails.CreatedAt,
                _cloudApp.AccountDetails.UpdatedAt,
                _cloudApp.AccountDetails.ActivatedAt,
                digestCredentials.Ha1);
        }

        /// <summary>
        /// Downloads a given uri and returns it as a bitmap.
        /// Written by Marian, see http://bytes.com/topic/c-sharp/answers/471313-displaying-image-url-c#post1813162
        /// </summary>
        /// <param name="uri">A Uri to retrieve the bitmap from.</param>
        /// <returns>A bitmap from the given uri.</returns>
        private Bitmap UriToBitmap(string uri)
        {
            HttpWebRequest wreq;
            HttpWebResponse wresp;
            Stream mystream;
            Bitmap bmp;

            bmp = null;
            mystream = null;
            wresp = null;
            try
            {
                wreq = (HttpWebRequest)WebRequest.Create(uri);
                wreq.AllowWriteStreamBuffering = true;

                wresp = (HttpWebResponse)wreq.GetResponse();

                if ((mystream = wresp.GetResponseStream()) != null)
                    bmp = new Bitmap(mystream);
            }
            finally
            {
                if (mystream != null)
                    mystream.Close();

                if (wresp != null)
                    wresp.Close();
            }

            return (bmp);
        }
    }
}
