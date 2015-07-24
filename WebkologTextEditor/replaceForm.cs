using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebkologTextEditor
{
    public partial class replaceForm : Form
    {
        public replaceForm() { InitializeComponent(); }

        string mbTitle, mbConComplete, mbConNotFound;

        private void replaceForm_Load(object sender, EventArgs e)
        {
            Configuration conf = ((Application.OpenForms["Form1"] as Form1).conf as Configuration);
            this.Text = conf.GetValue("FRM_RPL_TITLE");
            label1.Text = conf.GetValue("FRM_FND_LABEL");
            label2.Text = conf.GetValue("FRM_RPL_LABELNEW");
            button1.Text = conf.GetValue("FRM_FND_BTNFIND");
            button2.Text = conf.GetValue("FRM_RPL_BTN_REPLACE");
            button3.Text = conf.GetValue("FRM_RPL_BTN_REPLACE_ALL");
            button4.Text = conf.GetValue("FRM_FND_BTNCANCEL");
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
        private void button1_Click(object sender, EventArgs e) { findOrReplace(false); }

        private void button4_Click(object sender, EventArgs e) { this.Close(); }

        private void button2_Click(object sender, EventArgs e) { findOrReplace(true); }

        private void button3_Click(object sender, EventArgs e)
        {
            TextBox tb = ((Application.OpenForms["Form1"] as Form1).tabControl1.SelectedTab.Controls[0] as TextBox);
            if (checkBox1.Checked)
                tb.Text = tb.Text.Replace(textBox1.Text, textBox2.Text);
            else
                tb.Text = ignoreCaseSensitiveReplace(tb.Text, textBox1.Text, textBox2.Text);
        }

        string ignoreCaseSensitiveReplace(string sourceText, string searchText, string newText)
        {
            string lowerSourceText = sourceText.ToLower();
            string lowerSearchText = searchText.ToLower();
            string resultText = sourceText;
            int index = lowerSourceText.IndexOf(lowerSearchText);
            while (index != -1)
            {
                resultText = resultText.Substring(0, index) + newText + resultText.Substring(index + searchText.Length);
                lowerSourceText = resultText.ToLower();
                index = lowerSourceText.IndexOf(lowerSearchText, index + newText.Length);
            }
            return resultText;
        }

        void findOrReplace(bool singleReplacement)
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
            else
                if (checkBox1.Checked)
                    tb.SelectedText = tb.SelectedText.Replace(textBox1.Text, textBox2.Text);
                else
                    tb.SelectedText = ignoreCaseSensitiveReplace(tb.SelectedText, textBox1.Text, textBox2.Text);
        }
    }
}
