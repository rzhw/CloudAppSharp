using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CloudAppSharp;

namespace CloudAppSharpDemo
{
    public partial class FormRename : Form
    {
        private Form1 _caller;
        private CloudAppItem _item;

        public FormRename(Form1 caller, CloudAppItem item)
        {
            InitializeComponent();
            _caller = caller;
            _item = item;
            textBox1.Text = item.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _caller._tempItem = _caller._cloudApp.RenameItem(_item, textBox1.Text);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
