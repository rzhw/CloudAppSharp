namespace CloudAppSharpDemo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBoxCredentials = new System.Windows.Forms.GroupBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxUploads = new System.Windows.Forms.GroupBox();
            this.buttonUploadsRename = new System.Windows.Forms.Button();
            this.buttonUploadsPrivacy = new System.Windows.Forms.Button();
            this.buttonUploadsDelete = new System.Windows.Forms.Button();
            this.buttonUploadsDetails = new System.Windows.Forms.Button();
            this.listViewUploads = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderViews = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPublic = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAdded = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUpdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonUploadsRefresh = new System.Windows.Forms.Button();
            this.groupBoxUploadFile = new System.Windows.Forms.GroupBox();
            this.buttonUploadFile = new System.Windows.Forms.Button();
            this.buttonUploadFileBrowse = new System.Windows.Forms.Button();
            this.textBoxUploadFile = new System.Windows.Forms.TextBox();
            this.labelDetailsName = new System.Windows.Forms.Label();
            this.pictureBoxDetails = new System.Windows.Forms.PictureBox();
            this.textBoxDetails = new System.Windows.Forms.TextBox();
            this.groupBoxDetailsFromUrl = new System.Windows.Forms.GroupBox();
            this.buttonDetailsFromUrl = new System.Windows.Forms.Button();
            this.textBoxDetailsFromUrl = new System.Windows.Forms.TextBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.groupBoxAddBookmark = new System.Windows.Forms.GroupBox();
            this.buttonAddBookmark = new System.Windows.Forms.Button();
            this.textBoxAddBookmark = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageItems = new System.Windows.Forms.TabPage();
            this.tabPageNew = new System.Windows.Forms.TabPage();
            this.tabPageAccount = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxAccountDetails = new System.Windows.Forms.TextBox();
            this.tabPageItemDetails = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxCredentials.SuspendLayout();
            this.groupBoxUploads.SuspendLayout();
            this.groupBoxUploadFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDetails)).BeginInit();
            this.groupBoxDetailsFromUrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.groupBoxAddBookmark.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageItems.SuspendLayout();
            this.tabPageNew.SuspendLayout();
            this.tabPageAccount.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageItemDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCredentials
            // 
            this.groupBoxCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCredentials.Controls.Add(this.buttonLogin);
            this.groupBoxCredentials.Controls.Add(this.textBoxPassword);
            this.groupBoxCredentials.Controls.Add(this.textBoxEmail);
            this.groupBoxCredentials.Controls.Add(this.label2);
            this.groupBoxCredentials.Controls.Add(this.label1);
            this.groupBoxCredentials.Location = new System.Drawing.Point(12, 48);
            this.groupBoxCredentials.Name = "groupBoxCredentials";
            this.groupBoxCredentials.Size = new System.Drawing.Size(600, 52);
            this.groupBoxCredentials.TabIndex = 0;
            this.groupBoxCredentials.TabStop = false;
            this.groupBoxCredentials.Text = "Credentials";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLogin.Location = new System.Drawing.Point(517, 17);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 4;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPassword.Location = new System.Drawing.Point(275, 19);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(160, 20);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxEmail.Location = new System.Drawing.Point(47, 19);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(160, 20);
            this.textBoxEmail.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email:";
            // 
            // groupBoxUploads
            // 
            this.groupBoxUploads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUploads.Controls.Add(this.buttonUploadsRename);
            this.groupBoxUploads.Controls.Add(this.buttonUploadsPrivacy);
            this.groupBoxUploads.Controls.Add(this.buttonUploadsDelete);
            this.groupBoxUploads.Controls.Add(this.buttonUploadsDetails);
            this.groupBoxUploads.Controls.Add(this.listViewUploads);
            this.groupBoxUploads.Controls.Add(this.buttonUploadsRefresh);
            this.groupBoxUploads.Enabled = false;
            this.groupBoxUploads.Location = new System.Drawing.Point(6, 6);
            this.groupBoxUploads.Name = "groupBoxUploads";
            this.groupBoxUploads.Size = new System.Drawing.Size(580, 176);
            this.groupBoxUploads.TabIndex = 1;
            this.groupBoxUploads.TabStop = false;
            this.groupBoxUploads.Text = "Uploads";
            // 
            // buttonUploadsRename
            // 
            this.buttonUploadsRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadsRename.Enabled = false;
            this.buttonUploadsRename.Location = new System.Drawing.Point(171, 145);
            this.buttonUploadsRename.Name = "buttonUploadsRename";
            this.buttonUploadsRename.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadsRename.TabIndex = 5;
            this.buttonUploadsRename.Text = "Rename...";
            this.buttonUploadsRename.UseVisualStyleBackColor = true;
            this.buttonUploadsRename.Click += new System.EventHandler(this.buttonUploadsRename_Click);
            // 
            // buttonUploadsPrivacy
            // 
            this.buttonUploadsPrivacy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadsPrivacy.Enabled = false;
            this.buttonUploadsPrivacy.Location = new System.Drawing.Point(90, 145);
            this.buttonUploadsPrivacy.Name = "buttonUploadsPrivacy";
            this.buttonUploadsPrivacy.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadsPrivacy.TabIndex = 4;
            this.buttonUploadsPrivacy.Text = "Privacy...";
            this.buttonUploadsPrivacy.UseVisualStyleBackColor = true;
            this.buttonUploadsPrivacy.Click += new System.EventHandler(this.buttonUploadsPrivacy_Click);
            // 
            // buttonUploadsDelete
            // 
            this.buttonUploadsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadsDelete.Enabled = false;
            this.buttonUploadsDelete.Location = new System.Drawing.Point(252, 145);
            this.buttonUploadsDelete.Name = "buttonUploadsDelete";
            this.buttonUploadsDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadsDelete.TabIndex = 3;
            this.buttonUploadsDelete.Text = "Delete";
            this.buttonUploadsDelete.UseVisualStyleBackColor = true;
            this.buttonUploadsDelete.Click += new System.EventHandler(this.buttonUploadsDelete_Click);
            // 
            // buttonUploadsDetails
            // 
            this.buttonUploadsDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadsDetails.Enabled = false;
            this.buttonUploadsDetails.Location = new System.Drawing.Point(9, 145);
            this.buttonUploadsDetails.Name = "buttonUploadsDetails";
            this.buttonUploadsDetails.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadsDetails.TabIndex = 2;
            this.buttonUploadsDetails.Text = "Details";
            this.buttonUploadsDetails.UseVisualStyleBackColor = true;
            this.buttonUploadsDetails.Click += new System.EventHandler(this.buttonUploadsDetails_Click);
            // 
            // listViewUploads
            // 
            this.listViewUploads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUploads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderViews,
            this.columnHeaderPublic,
            this.columnHeaderAdded,
            this.columnHeaderUpdated});
            this.listViewUploads.FullRowSelect = true;
            this.listViewUploads.GridLines = true;
            this.listViewUploads.Location = new System.Drawing.Point(9, 19);
            this.listViewUploads.Name = "listViewUploads";
            this.listViewUploads.Size = new System.Drawing.Size(563, 120);
            this.listViewUploads.TabIndex = 1;
            this.listViewUploads.UseCompatibleStateImageBehavior = false;
            this.listViewUploads.View = System.Windows.Forms.View.Details;
            this.listViewUploads.SelectedIndexChanged += new System.EventHandler(this.listViewUploads_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 200;
            // 
            // columnHeaderViews
            // 
            this.columnHeaderViews.Text = "Views";
            this.columnHeaderViews.Width = 42;
            // 
            // columnHeaderPublic
            // 
            this.columnHeaderPublic.Text = "Pub";
            this.columnHeaderPublic.Width = 35;
            // 
            // columnHeaderAdded
            // 
            this.columnHeaderAdded.Text = "Added";
            this.columnHeaderAdded.Width = 140;
            // 
            // columnHeaderUpdated
            // 
            this.columnHeaderUpdated.Text = "Updated";
            this.columnHeaderUpdated.Width = 140;
            // 
            // buttonUploadsRefresh
            // 
            this.buttonUploadsRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUploadsRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUploadsRefresh.Location = new System.Drawing.Point(497, 145);
            this.buttonUploadsRefresh.Name = "buttonUploadsRefresh";
            this.buttonUploadsRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadsRefresh.TabIndex = 0;
            this.buttonUploadsRefresh.Text = "Refresh";
            this.buttonUploadsRefresh.UseVisualStyleBackColor = true;
            this.buttonUploadsRefresh.Click += new System.EventHandler(this.buttonUploadsRefresh_Click);
            // 
            // groupBoxUploadFile
            // 
            this.groupBoxUploadFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUploadFile.Controls.Add(this.buttonUploadFile);
            this.groupBoxUploadFile.Controls.Add(this.buttonUploadFileBrowse);
            this.groupBoxUploadFile.Controls.Add(this.textBoxUploadFile);
            this.groupBoxUploadFile.Enabled = false;
            this.groupBoxUploadFile.Location = new System.Drawing.Point(6, 67);
            this.groupBoxUploadFile.Name = "groupBoxUploadFile";
            this.groupBoxUploadFile.Size = new System.Drawing.Size(580, 52);
            this.groupBoxUploadFile.TabIndex = 2;
            this.groupBoxUploadFile.TabStop = false;
            this.groupBoxUploadFile.Text = "Upload File";
            // 
            // buttonUploadFile
            // 
            this.buttonUploadFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUploadFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUploadFile.Location = new System.Drawing.Point(497, 17);
            this.buttonUploadFile.Name = "buttonUploadFile";
            this.buttonUploadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadFile.TabIndex = 2;
            this.buttonUploadFile.Text = "Upload";
            this.buttonUploadFile.UseVisualStyleBackColor = true;
            this.buttonUploadFile.Click += new System.EventHandler(this.buttonUploadFile_Click);
            // 
            // buttonUploadFileBrowse
            // 
            this.buttonUploadFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUploadFileBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUploadFileBrowse.Location = new System.Drawing.Point(215, 17);
            this.buttonUploadFileBrowse.Name = "buttonUploadFileBrowse";
            this.buttonUploadFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadFileBrowse.TabIndex = 1;
            this.buttonUploadFileBrowse.Text = "Browse...";
            this.buttonUploadFileBrowse.UseVisualStyleBackColor = true;
            this.buttonUploadFileBrowse.Click += new System.EventHandler(this.buttonUploadFileBrowse_Click);
            // 
            // textBoxUploadFile
            // 
            this.textBoxUploadFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxUploadFile.Location = new System.Drawing.Point(9, 19);
            this.textBoxUploadFile.Name = "textBoxUploadFile";
            this.textBoxUploadFile.Size = new System.Drawing.Size(200, 20);
            this.textBoxUploadFile.TabIndex = 0;
            // 
            // labelDetailsName
            // 
            this.labelDetailsName.AutoSize = true;
            this.labelDetailsName.Location = new System.Drawing.Point(26, 7);
            this.labelDetailsName.Name = "labelDetailsName";
            this.labelDetailsName.Size = new System.Drawing.Size(72, 13);
            this.labelDetailsName.TabIndex = 2;
            this.labelDetailsName.Text = "No file loaded";
            // 
            // pictureBoxDetails
            // 
            this.pictureBoxDetails.Location = new System.Drawing.Point(5, 5);
            this.pictureBoxDetails.MaximumSize = new System.Drawing.Size(16, 16);
            this.pictureBoxDetails.MinimumSize = new System.Drawing.Size(16, 16);
            this.pictureBoxDetails.Name = "pictureBoxDetails";
            this.pictureBoxDetails.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxDetails.TabIndex = 1;
            this.pictureBoxDetails.TabStop = false;
            // 
            // textBoxDetails
            // 
            this.textBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDetails.Location = new System.Drawing.Point(0, 28);
            this.textBoxDetails.Multiline = true;
            this.textBoxDetails.Name = "textBoxDetails";
            this.textBoxDetails.ReadOnly = true;
            this.textBoxDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDetails.Size = new System.Drawing.Size(592, 390);
            this.textBoxDetails.TabIndex = 0;
            // 
            // groupBoxDetailsFromUrl
            // 
            this.groupBoxDetailsFromUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDetailsFromUrl.Controls.Add(this.buttonDetailsFromUrl);
            this.groupBoxDetailsFromUrl.Controls.Add(this.textBoxDetailsFromUrl);
            this.groupBoxDetailsFromUrl.Location = new System.Drawing.Point(6, 188);
            this.groupBoxDetailsFromUrl.Name = "groupBoxDetailsFromUrl";
            this.groupBoxDetailsFromUrl.Size = new System.Drawing.Size(580, 52);
            this.groupBoxDetailsFromUrl.TabIndex = 4;
            this.groupBoxDetailsFromUrl.TabStop = false;
            this.groupBoxDetailsFromUrl.Text = "View Item Details by Short URL";
            // 
            // buttonDetailsFromUrl
            // 
            this.buttonDetailsFromUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetailsFromUrl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonDetailsFromUrl.Location = new System.Drawing.Point(497, 17);
            this.buttonDetailsFromUrl.Name = "buttonDetailsFromUrl";
            this.buttonDetailsFromUrl.Size = new System.Drawing.Size(75, 23);
            this.buttonDetailsFromUrl.TabIndex = 1;
            this.buttonDetailsFromUrl.Text = "View";
            this.buttonDetailsFromUrl.UseVisualStyleBackColor = true;
            this.buttonDetailsFromUrl.Click += new System.EventHandler(this.buttonDetailsFromUrl_Click);
            // 
            // textBoxDetailsFromUrl
            // 
            this.textBoxDetailsFromUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxDetailsFromUrl.Location = new System.Drawing.Point(6, 19);
            this.textBoxDetailsFromUrl.Name = "textBoxDetailsFromUrl";
            this.textBoxDetailsFromUrl.Size = new System.Drawing.Size(320, 20);
            this.textBoxDetailsFromUrl.TabIndex = 0;
            this.textBoxDetailsFromUrl.Text = "http://cl.ly/itemurl";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(207, 30);
            this.pictureBoxLogo.TabIndex = 5;
            this.pictureBoxLogo.TabStop = false;
            // 
            // groupBoxAddBookmark
            // 
            this.groupBoxAddBookmark.Controls.Add(this.buttonAddBookmark);
            this.groupBoxAddBookmark.Controls.Add(this.textBoxAddBookmark);
            this.groupBoxAddBookmark.Enabled = false;
            this.groupBoxAddBookmark.Location = new System.Drawing.Point(6, 9);
            this.groupBoxAddBookmark.Name = "groupBoxAddBookmark";
            this.groupBoxAddBookmark.Size = new System.Drawing.Size(580, 52);
            this.groupBoxAddBookmark.TabIndex = 6;
            this.groupBoxAddBookmark.TabStop = false;
            this.groupBoxAddBookmark.Text = "Add Bookmark";
            // 
            // buttonAddBookmark
            // 
            this.buttonAddBookmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddBookmark.Location = new System.Drawing.Point(497, 17);
            this.buttonAddBookmark.Name = "buttonAddBookmark";
            this.buttonAddBookmark.Size = new System.Drawing.Size(75, 23);
            this.buttonAddBookmark.TabIndex = 1;
            this.buttonAddBookmark.Text = "Add";
            this.buttonAddBookmark.UseVisualStyleBackColor = true;
            this.buttonAddBookmark.Click += new System.EventHandler(this.buttonAddBookmark_Click);
            // 
            // textBoxAddBookmark
            // 
            this.textBoxAddBookmark.Location = new System.Drawing.Point(9, 19);
            this.textBoxAddBookmark.Name = "textBoxAddBookmark";
            this.textBoxAddBookmark.Size = new System.Drawing.Size(281, 20);
            this.textBoxAddBookmark.TabIndex = 0;
            this.textBoxAddBookmark.Text = "http://www.google.com/";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageItems);
            this.tabControl1.Controls.Add(this.tabPageNew);
            this.tabControl1.Controls.Add(this.tabPageItemDetails);
            this.tabControl1.Controls.Add(this.tabPageAccount);
            this.tabControl1.Location = new System.Drawing.Point(12, 106);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 444);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPageItems
            // 
            this.tabPageItems.Controls.Add(this.groupBoxUploads);
            this.tabPageItems.Controls.Add(this.groupBoxDetailsFromUrl);
            this.tabPageItems.Location = new System.Drawing.Point(4, 22);
            this.tabPageItems.Name = "tabPageItems";
            this.tabPageItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageItems.Size = new System.Drawing.Size(592, 418);
            this.tabPageItems.TabIndex = 0;
            this.tabPageItems.Text = "Items";
            this.tabPageItems.UseVisualStyleBackColor = true;
            // 
            // tabPageNew
            // 
            this.tabPageNew.Controls.Add(this.groupBoxAddBookmark);
            this.tabPageNew.Controls.Add(this.groupBoxUploadFile);
            this.tabPageNew.Location = new System.Drawing.Point(4, 22);
            this.tabPageNew.Name = "tabPageNew";
            this.tabPageNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNew.Size = new System.Drawing.Size(592, 418);
            this.tabPageNew.TabIndex = 1;
            this.tabPageNew.Text = "New Item";
            this.tabPageNew.UseVisualStyleBackColor = true;
            // 
            // tabPageAccount
            // 
            this.tabPageAccount.Controls.Add(this.groupBox1);
            this.tabPageAccount.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccount.Name = "tabPageAccount";
            this.tabPageAccount.Size = new System.Drawing.Size(592, 418);
            this.tabPageAccount.TabIndex = 2;
            this.tabPageAccount.Text = "Account";
            this.tabPageAccount.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxAccountDetails);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // textBoxAccountDetails
            // 
            this.textBoxAccountDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAccountDetails.Location = new System.Drawing.Point(3, 16);
            this.textBoxAccountDetails.Multiline = true;
            this.textBoxAccountDetails.Name = "textBoxAccountDetails";
            this.textBoxAccountDetails.ReadOnly = true;
            this.textBoxAccountDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAccountDetails.Size = new System.Drawing.Size(577, 131);
            this.textBoxAccountDetails.TabIndex = 0;
            // 
            // tabPageItemDetails
            // 
            this.tabPageItemDetails.Controls.Add(this.textBoxDetails);
            this.tabPageItemDetails.Controls.Add(this.panel1);
            this.tabPageItemDetails.Location = new System.Drawing.Point(4, 22);
            this.tabPageItemDetails.Name = "tabPageItemDetails";
            this.tabPageItemDetails.Size = new System.Drawing.Size(592, 418);
            this.tabPageItemDetails.TabIndex = 3;
            this.tabPageItemDetails.Text = "Item Details";
            this.tabPageItemDetails.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelDetailsName);
            this.panel1.Controls.Add(this.pictureBoxDetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 28);
            this.panel1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(624, 562);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.groupBoxCredentials);
            this.Name = "Form1";
            this.Text = "CloudAppSharp Demo";
            this.groupBoxCredentials.ResumeLayout(false);
            this.groupBoxCredentials.PerformLayout();
            this.groupBoxUploads.ResumeLayout(false);
            this.groupBoxUploadFile.ResumeLayout(false);
            this.groupBoxUploadFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDetails)).EndInit();
            this.groupBoxDetailsFromUrl.ResumeLayout(false);
            this.groupBoxDetailsFromUrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.groupBoxAddBookmark.ResumeLayout(false);
            this.groupBoxAddBookmark.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageItems.ResumeLayout(false);
            this.tabPageNew.ResumeLayout(false);
            this.tabPageAccount.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageItemDetails.ResumeLayout(false);
            this.tabPageItemDetails.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCredentials;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxUploads;
        private System.Windows.Forms.ListView listViewUploads;
        private System.Windows.Forms.Button buttonUploadsRefresh;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderAdded;
        private System.Windows.Forms.ColumnHeader columnHeaderUpdated;
        private System.Windows.Forms.ColumnHeader columnHeaderViews;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.GroupBox groupBoxUploadFile;
        private System.Windows.Forms.Button buttonUploadFile;
        private System.Windows.Forms.Button buttonUploadFileBrowse;
        private System.Windows.Forms.TextBox textBoxUploadFile;
        private System.Windows.Forms.Button buttonUploadsDetails;
        private System.Windows.Forms.TextBox textBoxDetails;
        private System.Windows.Forms.Label labelDetailsName;
        private System.Windows.Forms.PictureBox pictureBoxDetails;
        private System.Windows.Forms.GroupBox groupBoxDetailsFromUrl;
        private System.Windows.Forms.TextBox textBoxDetailsFromUrl;
        private System.Windows.Forms.Button buttonDetailsFromUrl;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.GroupBox groupBoxAddBookmark;
        private System.Windows.Forms.TextBox textBoxAddBookmark;
        private System.Windows.Forms.Button buttonAddBookmark;
        private System.Windows.Forms.Button buttonUploadsDelete;
        private System.Windows.Forms.Button buttonUploadsPrivacy;
        private System.Windows.Forms.ColumnHeader columnHeaderPublic;
        private System.Windows.Forms.Button buttonUploadsRename;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageItems;
        private System.Windows.Forms.TabPage tabPageNew;
        private System.Windows.Forms.TabPage tabPageAccount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxAccountDetails;
        private System.Windows.Forms.TabPage tabPageItemDetails;
        private System.Windows.Forms.Panel panel1;
    }
}

