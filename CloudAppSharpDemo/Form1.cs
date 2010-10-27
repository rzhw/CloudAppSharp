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
        private bool cloudAppLogged = false;
        private CloudApp cloudApp;

        public Form1()
        {
            Font = SystemFonts.MessageBoxFont;
            AutoScaleMode = AutoScaleMode.Font;
            InitializeComponent();
            labelDetailsName.Location = new Point(28, 21);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!cloudAppLogged)
            {
                cloudApp = new CloudApp(textBoxEmail.Text, textBoxPassword.Text);
                cloudAppLogged = true;
                textBoxEmail.Enabled = false;
                textBoxPassword.Enabled = false;
                groupBoxAddBookmark.Enabled = true;
                groupBoxUploadFile.Enabled = true;
                groupBoxUploads.Enabled = true;
                buttonLogin.Text = "Logout";
                DigestCredentials digestCredentials = cloudApp.GetCredentials();
                labelStatus.Text = String.Format("Logged in as {0} with hash {1}",
                    digestCredentials.Username, digestCredentials.Ha1);
            }
            else
            {
                cloudApp = null;
                cloudAppLogged = false;
                textBoxEmail.Enabled = true;
                textBoxPassword.Enabled = true;
                groupBoxAddBookmark.Enabled = false;
                groupBoxUploadFile.Enabled = false;
                groupBoxUploads.Enabled = false;
                buttonLogin.Text = "Login";
                labelStatus.Text = "Not logged in";
            }
        }

        private void buttonAddBookmark_Click(object sender, EventArgs e)
        {
            CloudAppItem bookmark = cloudApp.AddBookmark(new Uri(textBoxAddBookmark.Text));
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
                uploadedItem = cloudApp.Upload(textBoxUploadFile.Text);
                UpdateDetailsArea(uploadedItem);
            }
            else
            {
                MessageBox.Show("The file you tried to upload doesn't exist!", "CloudAppSharp Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonUploadsRefresh_Click(object sender, EventArgs e)
        {
            listViewUploads.Items.Clear();

            List<CloudAppItem> items = cloudApp.GetItems();

            foreach (CloudAppItem item in items)
            {
                ListViewItem itemListViewItem = listViewUploads.Items.Add(item.Name);
                itemListViewItem.Tag = item;
                itemListViewItem.SubItems.Add(""); // icon
                itemListViewItem.SubItems.Add(item.ViewCounter.ToString());
                itemListViewItem.SubItems.Add(item.Private ? "N" : "Y");
                itemListViewItem.SubItems.Add(item.CreatedAt);
                itemListViewItem.SubItems.Add(item.UpdatedAt);
            }
        }

        private void listViewUploads_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonUploadsPrivacy.Enabled = true;
            buttonUploadsDelete.Enabled = true;
            buttonUploadsDetails.Enabled = true;
        }

        private void buttonUploadsPrivacy_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            if (MessageBox.Show("Do you want to make this item " + (item.Private ? "public" : "private") + "?",
                "CloudAppSharp Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                cloudApp.SetPrivacy(item, !item.Private);
            }
        }

        private void buttonUploadsDelete_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            if (MessageBox.Show("Are you sure you want to delete " + item.Name + "?",
                "CloudAppSharp Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                cloudApp.DeleteItem(item);
                buttonUploadsRefresh.PerformClick();
            }
        }

        private void buttonUploadsDetails_Click(object sender, EventArgs e)
        {
            CloudAppItem item = (CloudAppItem)listViewUploads.FocusedItem.Tag;
            UpdateDetailsArea(item);
        }

        private void buttonDetailsFromUrl_Click(object sender, EventArgs e)
        {
            UpdateDetailsArea(CloudApp.GetItemFromUri(new Uri(textBoxDetailsFromUrl.Text)));
        }

        private void UpdateDetailsArea(CloudAppItem item)
        {
            pictureBoxDetails.Image = UriToBitmap(item.Icon);

            labelDetailsName.Text = String.Format("{0} ({1}, {2} views, {3})",
                item.Name, item.ItemType, item.ViewCounter, item.Private ? "Private" : "Public");

            textBoxDetails.Text = String.Format("URL: {0}\r\nHref: {1}\r\n{2}\r\nCreated: {3}\r\nUpdated: {4}\r\nDeleted: {5}",
                item.Url,
                item.Href,
                item.ItemType == CloudAppItemType.Bookmark ? "Redirect URL: " + item.RedirectUrl : "Remote URL: " + item.RemoteUrl,
                item.CreatedAt,
                item.UpdatedAt,
                String.IsNullOrEmpty(item.DeletedAt) ? "null" : item.DeletedAt
            );
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
