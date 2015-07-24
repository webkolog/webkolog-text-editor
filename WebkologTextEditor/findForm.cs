using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebkologTextEditor
{
    public partial class findForm : Form
    {
        public findForm() { InitializeComponent(); }

        string mbTitle, mbConComplete, mbConNotFound;

        private void findForm_Load(object sender, EventArgs e)
        {
            Configuration conf = ((Application.OpenForms["Form1"] as Form1).conf as Configuration);
            this.Text = conf.GetValue("FRM_FND_TITLE");
            label1.Text = conf.GetValue("FRM_FND_LABEL");
            button1.Text = conf.GetValue("FRM_FND_BTNFIND");
            button2.Text = conf.GetValue("FRM_FND_BTNCANCEL");
            checkBox1.Text = conf.GetValue("FRM_FND_CASESENSITIVE");
            checkBox2.Text = conf.GetValue("FRM_FND_BACKWARD");
            mbTitle = conf.GetValue("FRM_FND_INFO");
            mbConComplete = conf.GetValue("FRM_FND_COMPLETE");
            mbConNotFound = conf.GetValue("FRM_FND_NOTFOUND");
        }

        private void findForm_FormClosing(object sender, FormClosingEventArgs e) { (Application.OpenForms["Form1"] as Form1).isActiveExtraForm = false; }

        private int lastFoundIndex = -1;
        private bool forward = true;
        private bool firstSearch = true;
        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            TextBox tb = ((Application.OpenForms["Form1"] as Form1).tabControl1.SelectedTab.Controls[0] as TextBox);
            lastFoundIndex = tb.SelectionStart;
            int selectedLength = tb.SelectionLength;
            int index = -1;
            bool caseSensitive = checkBox1.Checked;
            bool backwards = checkBox2.Checked;
            bool bulundu = false;
            if (backwards)
            {
                if (lastFoundIndex > 0)
                    if (caseSensitive)
                        index = tb.Text.LastIndexOf(searchText, lastFoundIndex - 1);
                    else
                        index = tb.Text.ToLower().LastIndexOf(searchText.ToLower(), lastFoundIndex - 1);
                else if (firstSearch || tb.Text.Length > 0)
                    if (caseSensitive)
                        index = tb.Text.LastIndexOf(searchText);
                    else
                        index = tb.Text.ToLower().LastIndexOf(searchText.ToLower());
                forward = false;
            }
            else
            {
                int startIndex = (lastFoundIndex != -1 && tb.Text.Length > lastFoundIndex + searchText.Length) ? lastFoundIndex + selectedLength : 0;
                if (caseSensitive)
                    index = tb.Text.IndexOf(searchText, startIndex);
                else
                    index = tb.Text.ToLower().IndexOf(searchText.ToLower(), startIndex);
                forward = true;
            }
            if (index != -1)
            {
                tb.Select(index, searchText.Length);
                tb.Focus();
                lastFoundIndex = index;
                firstSearch = false;
                bulundu = true;
            }
            else
            {
                lastFoundIndex = -1;
            }
            bool containsText;
            if (caseSensitive)
                containsText = tb.Text.Contains(searchText);
            else
                containsText = tb.Text.ToLower().Contains(searchText.ToLower());
            if (!bulundu && !firstSearch && containsText)
                MessageBox.Show(mbConComplete, mbTitle);
            else if (!bulundu && firstSearch)
                MessageBox.Show(mbConNotFound, mbTitle);
            else if (!bulundu && !containsText)
                MessageBox.Show(mbConNotFound, mbTitle);
        }

        private void button2_Click(object sender, EventArgs e) { this.Close(); }
    }
}