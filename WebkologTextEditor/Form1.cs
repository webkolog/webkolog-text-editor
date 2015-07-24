using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace WebkologTextEditor
{
    public partial class Form1 : Form
    {
        string LanguageFile;
        int selTabIndex, fileAutoNameCounter, holdTabIndex;
        bool isFileLoading, wordWrap, showStatusBar;
        internal bool isActiveExtraForm = false;
        Font defFont;
        string lngNewFile = "New File %n", lngDiaTitleOpenFile, lngMsgBoxNotFoundFile, lngDiaTitleSaveFile, lngMsgBoxSave, lngMsgBoxSaveHeader, lngStsBarLineCol, lngMsgBoxDelete, lngMsgBoxDeleteHeader, lngDiaAlreadyFileOpen;
        ToolStripLabel tsl = new ToolStripLabel();
        ToolStripMenuItem tsmiLanguage = new ToolStripMenuItem("Language");
        ToolStripMenuItem tsmiSave = new ToolStripMenuItem("Save");
        ToolStripMenuItem tsmiSaveAs = new ToolStripMenuItem("Save As...");
        ToolStripMenuItem tsmiCloseAll = new ToolStripMenuItem("Close All");
        ToolStripMenuItem tsmiClose = new ToolStripMenuItem("Close");
        ToolStripMenuItem tsmiCloseAllMenuItem = new ToolStripMenuItem("Close All");
        ToolStripMenuItem tsmiCloseMenuItem = new ToolStripMenuItem("Close");
        ToolStripMenuItem tsmiPrint = new ToolStripMenuItem("Print");
        ToolStripMenuItem tsmiEnglish = new ToolStripMenuItem("English");
        ToolStripMenuItem tsmiStatusBar = new ToolStripMenuItem("Status Bar");
        ToolStripMenuItem tsmiView = new ToolStripMenuItem("&View");
        ToolStripMenuItem tsmiWordWrap = new ToolStripMenuItem("Word Wrap");
        ToolStripMenuItem tsmiFont = new ToolStripMenuItem("Font");
        ToolStripMenuItem tsmiFormat = new ToolStripMenuItem("Format");
        ToolStripMenuItem tsmiDocumentation = new ToolStripMenuItem("Documentation");
        ToolStripMenuItem tsmiSupport = new ToolStripMenuItem("Support");
        ToolStripMenuItem tsmiCheckForUpdates = new ToolStripMenuItem("Check for Updates");
        ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
        ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
        ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
        ToolStripMenuItem tsmiSelectAll = new ToolStripMenuItem("Select All");
        ToolStripMenuItem tsmiCloseOthers = new ToolStripMenuItem("Close Others");
        ToolStripMenuItem tsmiCloseLeft = new ToolStripMenuItem("Close All to the Left");
        ToolStripMenuItem tsmiCloseRight = new ToolStripMenuItem("Close All to the Right");
        ToolStripMenuItem tsmiOpenContainingFolder = new ToolStripMenuItem("Open Containing Folder");
        ToolStripMenuItem tsmiReadOnly = new ToolStripMenuItem("Read-Only");
        ToolStripMenuItem tsmiCopyFullFilePath = new ToolStripMenuItem("Copy Full File Path to Clipboard");
        ToolStripMenuItem tsmiCopyFilename = new ToolStripMenuItem("Copy Filename to Clipboard");
        ToolStripMenuItem tsmiCopyDirectory = new ToolStripMenuItem("Copy Current Directory to Clipboard");
        ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
        ToolStripMenuItem tsmiFind = new ToolStripMenuItem("Find...");
        ToolStripMenuItem tsmiReplace = new ToolStripMenuItem("Replace...");
        ToolStripMenuItem tsmiDateTime = new ToolStripMenuItem("Add Date/Time");
        ToolStripSeparator tssSeperator = new ToolStripSeparator();
        internal Configuration conf;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Webkolog Text Editor";
            this.AutoScaleMode = AutoScaleMode.Font;
            this.MinimumSize = new Size(400, 300);
            this.Size = new Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = Properties.Settings.Default.WindowsState;
            menuStrip1.TabIndex = 1;
            tabControl1.TabIndex = 2;
            toolStrip1.TabIndex = 3;
            toolStrip1.Dock = DockStyle.Bottom;
            tsl.Alignment = ToolStripItemAlignment.Right;
            tsl.AutoSize = false;
            tsl.Text = "Row 0, Column 0 ";
            tsl.TextAlign = ContentAlignment.MiddleLeft;
            tsl.Size = new System.Drawing.Size(200, 22);
            ToolStripSeparator tss = new ToolStripSeparator() { Alignment = ToolStripItemAlignment.Right };
            toolStrip1.Items.Add(tsl);
            toolStrip1.Items.Add(tss);
            tabControl1.TabPages.Clear();
            printDialog1.UseEXDialog = true;
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Items.RemoveAt(2);
            editToolStripMenuItem.DropDownItems.RemoveAt(1);
            for (int i = 0; i < 3; i++) { helpToolStripMenuItem.DropDownItems.RemoveAt(0); }
            fileToolStripMenuItem.DropDownItems.Insert(6, new ToolStripSeparator());
            fileToolStripMenuItem.DropDownItems.Insert(6, tsmiCloseAllMenuItem);
            fileToolStripMenuItem.DropDownItems.Insert(6, tsmiCloseMenuItem);
            tsmiLanguage.DropDownItems.Add(tsmiEnglish);
            tsmiView.DropDownItems.AddRange(new ToolStripMenuItem[] { tsmiStatusBar, tsmiLanguage });
            tsmiFormat.DropDownItems.AddRange(new ToolStripMenuItem[2] { tsmiWordWrap, tsmiFont });
            tsmiDateTime.ShortcutKeys = Keys.F5;
            tsmiReplace.ShortcutKeys = Keys.Control | Keys.H;
            tsmiFind.ShortcutKeys = Keys.Control | Keys.F;
            tsmiDelete.ShortcutKeys = Keys.Delete;
            editToolStripMenuItem.DropDownItems.Insert(7, tsmiDateTime);
            editToolStripMenuItem.DropDownItems.Insert(5, tsmiReplace);
            editToolStripMenuItem.DropDownItems.Insert(5, tsmiFind);
            editToolStripMenuItem.DropDownItems.Insert(5, new ToolStripSeparator());
            editToolStripMenuItem.DropDownItems.Insert(5, tsmiDelete);
            menuStrip1.Items.Insert(2, tsmiView);
            menuStrip1.Items.Insert(2, tsmiFormat);
            helpToolStripMenuItem.DropDownItems.Insert(0, tsmiCheckForUpdates);
            helpToolStripMenuItem.DropDownItems.Insert(0, tsmiSupport);
            helpToolStripMenuItem.DropDownItems.Insert(0, tsmiDocumentation);
            contextMenuStripTextBox.Items.Add(tsmiCut);
            contextMenuStripTextBox.Items.Add(tsmiCopy);
            contextMenuStripTextBox.Items.Add(tsmiPaste);
            contextMenuStripTextBox.Items.Add(new ToolStripSeparator());
            contextMenuStripTextBox.Items.Add(tsmiSelectAll);
            contextMenuStripTab.Items.Add(tsmiClose);
            contextMenuStripTab.Items.Add(tsmiCloseAll);
            contextMenuStripTab.Items.Add(tsmiCloseOthers);
            contextMenuStripTab.Items.Add(tsmiCloseLeft);
            contextMenuStripTab.Items.Add(tsmiCloseRight);
            contextMenuStripTab.Items.Add(new ToolStripSeparator());
            contextMenuStripTab.Items.Add(tsmiSave);
            contextMenuStripTab.Items.Add(tsmiSaveAs);
            contextMenuStripTab.Items.Add(new ToolStripSeparator());
            contextMenuStripTab.Items.Add(tsmiPrint);
            contextMenuStripTab.Items.Add(new ToolStripSeparator());
            contextMenuStripTab.Items.Add(tsmiOpenContainingFolder);
            contextMenuStripTab.Items.Add(tsmiReadOnly);
            contextMenuStripTab.Items.Add(tsmiCopyFullFilePath);
            contextMenuStripTab.Items.Add(tsmiCopyFilename);
            contextMenuStripTab.Items.Add(tsmiCopyDirectory);
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            tabControl1.ControlAdded += new ControlEventHandler(tabControl1_ControlAdded);
            tabControl1.ControlRemoved += new ControlEventHandler(tabControl1_ControlRemoved);
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
            contextMenuStripTab.VisibleChanged += new EventHandler(contextMenuStripTab_VisibleChanged);
            newToolStripMenuItem.Click += new EventHandler(tsmiNew_Click);
            openToolStripMenuItem.Click += new EventHandler(tsmiOpen_Click);
            saveToolStripMenuItem.Click += new EventHandler(tsmiSave_Click);
            saveAsToolStripMenuItem.Click += new EventHandler(tsmiSaveAs_Click);
            tsmiSave.Click += new EventHandler(tsmiSave_Click);
            tsmiSaveAs.Click += new EventHandler(tsmiSaveAs_Click);
            printToolStripMenuItem.Click += new EventHandler(tsmiPrint_Click);
            printPreviewToolStripMenuItem.Click += new EventHandler(tsmiPrintPreview_Click);
            tsmiPrint.Click += new EventHandler(tsmiPrint_Click);
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            exitToolStripMenuItem.Click += new EventHandler(tsmiExit_Click);
            undoToolStripMenuItem.Click += new EventHandler(tsmiUndo_Click);
            cutToolStripMenuItem.Click += new EventHandler(tsmiCut_Click);
            copyToolStripMenuItem.Click += new EventHandler(tsmiCopy_Click);
            pasteToolStripMenuItem.Click += new EventHandler(tsmiPaste_Click);
            selectAllToolStripMenuItem.Click += new EventHandler(tsmiSelectAll_Click);
            tsmiSelectAll.Click += new EventHandler(tsmiSelectAll_Click);
            aboutToolStripMenuItem.Click += new EventHandler(tsmiAbout_Click);
            tsmiCloseAll.Click += new EventHandler(tsmiCloseAll_Click);
            tsmiClose.Click += new EventHandler(tsmiClose_Click);
            tsmiCloseAllMenuItem.Click += new EventHandler(tsmiCloseAll_Click);
            tsmiCloseMenuItem.Click += new EventHandler(tsmiClose_Click);
            tsmiEnglish.Click += new EventHandler(tsmiEnglish_Click);
            tsmiStatusBar.Click += new EventHandler(tsmiStatusBar_Click);
            tsmiWordWrap.Click += new EventHandler(tsmiWordWrap_Click);
            tsmiFont.Click += new EventHandler(tsmiFont_Click);
            tsmiDocumentation.Click += new EventHandler(tsmiDocumentation_Click);
            tsmiSupport.Click += new EventHandler(tsmiSupport_Click);
            tsmiCheckForUpdates.Click += new EventHandler(tsmiCheckForUpdates_Click);
            tsmiCloseOthers.Click += new EventHandler(tsmiCloseOthers_Click);
            tsmiCloseLeft.Click += new EventHandler(tsmiCloseLeft_Click);
            tsmiCloseRight.Click += new EventHandler(tsmiCloseRight_Click);
            tsmiOpenContainingFolder.Click += new EventHandler(tsmiOpenContainingFolder_Click);
            tsmiReadOnly.Click += new EventHandler(tsmiReadOnly_Click);
            tsmiCopyFullFilePath.Click += new EventHandler(tsmiCopyFullFilePath_Click);
            tsmiCopyFilename.Click += new EventHandler(tsmiCopyFilename_Click);
            tsmiCopyDirectory.Click += new EventHandler(tsmiCopyDirectory_Click);
            tabControl1.MouseClick += new MouseEventHandler(tabControl1_MouseClick);
            tsmiCut.Click += new EventHandler(tsmiCut_Click);
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
            tsmiPaste.Click += new EventHandler(tsmiPaste_Click);
            tsmiDateTime.Click += new EventHandler(tsmiDateTime_Click);
            tsmiReplace.Click += new EventHandler(tsmiReplace_Click);
            tsmiFind.Click += new EventHandler(tsmiFind_Click);
            tsmiDelete.Click += new EventHandler(tsmiDelete_Click);
        }

        private void LoadLanguageFiles()
        {
            try
            {
                string langFilePath = Application.StartupPath + @"\Languages\";
                string[] lngFiles = Directory.GetFiles(langFilePath, "*.lng");
                Image img = new Bitmap(1, 1);
                foreach (string lngFile in lngFiles)
                {
                    string lngName = Path.GetFileNameWithoutExtension(lngFile);
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(lngName, img, tsmiEnglish_Click);
                    tsmi.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    tsmiLanguage.DropDownItems.Add(tsmi);
                }
            }
            catch { }
        }

        private void SelectLanguage(string lngFile)
        {
            try
            {
                string LngFilePath = Application.StartupPath + @"\Languages\" + lngFile + ".lng";
                StreamReader sr;
                if (File.Exists(LngFilePath))
                {
                    sr = new StreamReader(LngFilePath);
                    LanguageFile = lngFile;
                }
                else
                {
                    sr = new StreamReader(new MemoryStream(Properties.Resources.English));
                    LanguageFile = "English";
                }
                foreach (ToolStripMenuItem item in tsmiLanguage.DropDownItems)
                {
                    item.Checked = false;
                    if (item.Text == LanguageFile)
                        item.Checked = true;
                }
                conf = new Configuration(sr);
                sr.Close();
                fileToolStripMenuItem.Text = conf.GetValue("MNU_FILE");
                newToolStripMenuItem.Text = conf.GetValue("MMU_NEW");
                openToolStripMenuItem.Text = conf.GetValue("MNU_OPEN");
                saveToolStripMenuItem.Text = conf.GetValue("MNU_SAVE");
                saveAsToolStripMenuItem.Text = conf.GetValue("MNU_SAVEAS");
                tsmiClose.Text = conf.GetValue("MNU_CLOSE");
                tsmiCloseAll.Text = conf.GetValue("MNU_CLOSEALL");
                tsmiCloseMenuItem.Text = conf.GetValue("MNU_CLOSE");
                tsmiCloseAllMenuItem.Text = conf.GetValue("MNU_CLOSEALL");
                printToolStripMenuItem.Text = conf.GetValue("MNU_PRINT");
                printPreviewToolStripMenuItem.Text = conf.GetValue("MNU_PRINTPRV");
                exitToolStripMenuItem.Text = conf.GetValue("MNU_EXIT");
                editToolStripMenuItem.Text = conf.GetValue("MNU_EDIT");
                undoToolStripMenuItem.Text = conf.GetValue("MNU_UNDO");
                cutToolStripMenuItem.Text = conf.GetValue("MNU_CUT");
                tsmiCut.Text = cutToolStripMenuItem.Text;
                copyToolStripMenuItem.Text = conf.GetValue("MNU_COPY");
                pasteToolStripMenuItem.Text = conf.GetValue("MNU_PASTE");
                selectAllToolStripMenuItem.Text = conf.GetValue("MNU_SELALL");
                tsmiCopy.Text = conf.GetValue("MNU_COPY");
                tsmiPaste.Text = conf.GetValue("MNU_PASTE");
                tsmiSelectAll.Text = conf.GetValue("MNU_SELALL");
                toolsToolStripMenuItem.Text = conf.GetValue("MNU_FORMAT");
                tsmiWordWrap.Text = conf.GetValue("MNU_WORDWRAP");
                tsmiFont.Text = conf.GetValue("MNU_FONT");
                tsmiView.Text = conf.GetValue("MNU_VIEW");
                tsmiStatusBar.Text = conf.GetValue("MNU_STSBAR");
                tsmiLanguage.Text = conf.GetValue("MNU_LANG");
                helpToolStripMenuItem.Text = conf.GetValue("MNU_HELP");
                tsmiDocumentation.Text = conf.GetValue("MNU_HELPDOC");
                tsmiSupport.Text = conf.GetValue("MNU_SUPPORT");
                tsmiCheckForUpdates.Text = conf.GetValue("MNU_UPDATE");
                aboutToolStripMenuItem.Text = conf.GetValue("MNU_ABOUT");
                tsmiDelete.Text = conf.GetValue("MNU_DELETE");
                tsmiDateTime.Text = conf.GetValue("MNU_ADDDATE");
                tsmiFind.Text = conf.GetValue("MNU_FIND");
                tsmiReplace.Text = conf.GetValue("MNU_REPLACE");
                cutToolStripMenuItem.Text = conf.GetValue("NP_CON_CUT");
                copyToolStripMenuItem.Text = conf.GetValue("NP_CON_COPY");
                pasteToolStripMenuItem.Text = conf.GetValue("NP_CON_PASTE");
                selectAllToolStripMenuItem.Text = conf.GetValue("NP_CON_SELALL");
                tsmiClose.Text = conf.GetValue("TP_CON_CLOSE");
                tsmiCloseOthers.Text = conf.GetValue("TP_CON_OTHERS");
                tsmiCloseLeft.Text = conf.GetValue("TP_CON_LEFT");
                tsmiCloseRight.Text = conf.GetValue("TP_CON_RIGHT");
                tsmiSave.Text = conf.GetValue("TP_CON_SAVE");
                tsmiSaveAs.Text = conf.GetValue("TP_CON_SAVEAS");
                tsmiPrint.Text = conf.GetValue("TP_CON_PRINT");
                tsmiOpenContainingFolder.Text = conf.GetValue("TP_CON_OPENCONFOLDER");
                tsmiReadOnly.Text = conf.GetValue("TP_CON_READONLY");
                tsmiCopyFullFilePath.Text = conf.GetValue("TP_CON_COPYFILEPATH");
                tsmiCopyFilename.Text = conf.GetValue("TP_CON_COPYFILENAME");
                tsmiCopyDirectory.Text = conf.GetValue("TP_CON_COPYDIRECTORY");
                lngDiaTitleOpenFile = conf.GetValue("DIA_OPEN");
                lngDiaTitleSaveFile = conf.GetValue("DIA_SAVE");
                lngMsgBoxNotFoundFile = conf.GetValue("MSG_NOTTXTFILE");
                lngMsgBoxSaveHeader = conf.GetValue("MSG_SAVE_HEADER");
                lngMsgBoxSave = conf.GetValue("MSG_SAVE");
                lngMsgBoxDeleteHeader = conf.GetValue("MSG_DELETE_HEADER");
                lngMsgBoxDelete = conf.GetValue("MSG_DELETE");
                lngStsBarLineCol = conf.GetValue("STS_TXTPOS");
                lngDiaAlreadyFileOpen = "Alread file open!";
            }
            catch
            {
                MessageBox.Show(lngFile + " language file is damaged or not found!");
                SelectLanguage("English");
            }
        }

        private void menuUpdate()
        {
            bool enabled = false;
            int tabCount = tabControl1.Controls.Count;
            if (tabCount > 0)
                enabled = true;
            else
                enabled = false;
            saveToolStripMenuItem.Enabled = enabled;
            saveAsToolStripMenuItem.Enabled = enabled;
            tsmiCloseMenuItem.Enabled = enabled;
            tsmiCloseAllMenuItem.Enabled = enabled;
            printToolStripMenuItem.Enabled = enabled;
            printPreviewToolStripMenuItem.Enabled = enabled;
            undoToolStripMenuItem.Enabled = enabled;
            cutToolStripMenuItem.Enabled = enabled;
            copyToolStripMenuItem.Enabled = enabled;
            pasteToolStripMenuItem.Enabled = enabled;
            selectAllToolStripMenuItem.Enabled = enabled;
            tsmiDelete.Enabled = enabled;
            tsmiFind.Enabled = enabled;
            tsmiReplace.Enabled = enabled;
            tsmiDateTime.Enabled = enabled;
        }

        private void menuCheckUpdate()
        {
            tsmiWordWrap.Checked = wordWrap;
            tsmiStatusBar.Checked = showStatusBar;
            wordWrapper();
            toolStrip1.Visible = showStatusBar;
        }

        private void wordWrapper()
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            foreach (TabPage tp in tabControl1.TabPages)
                foreach (TextBox tb in tp.Controls)
                    tb.WordWrap = wordWrap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllowDrop = true;
            wordWrap = Properties.Settings.Default.WordWrap;
            showStatusBar = Properties.Settings.Default.StatusBar;
            defFont = Properties.Settings.Default.Font;
            LoadLanguageFiles();
            SelectLanguage(Properties.Settings.Default.Language);
            menuCheckUpdate();
            isFileLoading = true;
            string[] args = Environment.GetCommandLineArgs();
            string proPath = Application.ExecutablePath;
            foreach (string file in args)
            {
                if (Path.GetExtension(file) == ".txt")
                {
                    try
                    {
                        StreamReader rdr = new StreamReader(file);
                        tsmiNew_Click(null, null);
                        (tabControl1.SelectedTab.Controls[0] as TextBox).Text = "";
                        TabPage tp = tabControl1.SelectedTab;
                        TextBox tb = tp.Controls[0] as TextBox;
                        tb.Text = rdr.ReadToEnd();
                        rdr.Close();
                        tp.Tag = new TextDoc(file, true);
                        tp.Text = (tp.Tag as TextDoc).FileName;
                    }
                    catch { }
                }
            }
            isFileLoading = false;
            menuUpdate();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            isFileLoading = true;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".txt")
                {
                    try
                    {
                        bool isFileOpened = false;
                        foreach (TabPage tabPage in tabControl1.TabPages)
                            if ((tabPage.Tag as TextDoc).FilePath == file)
                                isFileOpened = true;
                        if (!isFileOpened)
                        {
                            StreamReader rdr = new StreamReader(file);
                            tsmiNew_Click(null, null);
                            (tabControl1.SelectedTab.Controls[0] as TextBox).Text = "";
                            TabPage tp = tabControl1.SelectedTab;
                            TextBox tb = tp.Controls[0] as TextBox;
                            tb.Text = rdr.ReadToEnd();
                            rdr.Close();
                            tp.Tag = new TextDoc(file, true);
                            tp.Text = (tp.Tag as TextDoc).FileName;
                        }
                    }
                    catch { }
                }
            }
            isFileLoading = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms.Count > 1)
                Application.OpenForms[1].Close();
            tsmiCloseAll_Click(null, null);
            Properties.Settings.Default.WordWrap = wordWrap;
            Properties.Settings.Default.StatusBar = showStatusBar;
            Properties.Settings.Default.Font = defFont;
            Properties.Settings.Default.WindowsState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Height = this.Height;
            }
            AllowDrop = true;
            LoadLanguageFiles();
            Properties.Settings.Default.Language = LanguageFile;
            Properties.Settings.Default.Save();
            Application.Exit();
        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e) { menuUpdate(); }

        private void tabControl1_ControlRemoved(object sender, ControlEventArgs e) { menuUpdate(); }

        private void tabControl1_Selected(object sender, TabControlEventArgs e) { selTabIndex = e.TabPageIndex; }

        private void contextMenuStripTab_VisibleChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TabPage tp = tabControl1.SelectedTab;
            TextBox tb = tp.Controls[0] as TextBox;
            tsmiReadOnly.Checked = tb.ReadOnly ? true : false;
        }

        void tsmiClose_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TabPage tp = tabControl1.TabPages[selTabIndex];
            TextDoc td = tp.Tag as TextDoc;
            if (td.Changed)
            {
                DialogResult result = MessageBox.Show(lngMsgBoxSave.Replace("%f", td.FileName), lngMsgBoxSaveHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    tsmiSave_Click(null, null);
            }
            tp.Controls.Clear();
            tabControl1.TabPages.RemoveAt(selTabIndex);
        }

        void tsmiCloseAll_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            bool checkTabs = true;
            while (checkTabs)
            {
                int tabCount = tabControl1.TabPages.Count;
                if (tabCount > 0)
                {
                    TabPage tp = tabControl1.TabPages[tabCount - 1];
                    tabControl1.SelectTab(tp);
                    tsmiClose_Click(null, null);
                }
                else
                {
                    checkTabs = false;
                }
            }
        }

        void tsmiEnglish_Click(object sender, EventArgs e) { SelectLanguage((sender as ToolStripItem).Text); }

        void tsmiStatusBar_Click(object sender, EventArgs e)
        {
            showStatusBar = !showStatusBar;
            menuCheckUpdate();
        }

        void tsmiWordWrap_Click(object sender, EventArgs e)
        {
            wordWrap = wordWrap ? false : true;
            menuCheckUpdate();
        }

        void tsmiFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = defFont;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                defFont = fontDialog1.Font;
                foreach (TabPage tp in tabControl1.TabPages)
                    foreach (TextBox tb in tp.Controls)
                        tb.Font = defFont;
            }
        }

        void tsmiCut_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TextBox tb = tabControl1.SelectedTab.Controls[0] as TextBox;
            tb.Cut();
        }

        void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TextBox tb = tabControl1.SelectedTab.Controls[0] as TextBox;
            tb.Copy();
        }

        void tsmiPaste_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TextBox tb = tabControl1.SelectedTab.Controls[0] as TextBox;
            tb.Paste();
        }

        void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TextBox tb = tabControl1.SelectedTab.Controls[0] as TextBox;
            tb.SelectAll();
        }

        void tsmiCloseOthers_Click(object sender, EventArgs e)
        {
            holdTabIndex = selTabIndex;
            bool checkTabs = true;
            while (checkTabs)
            {
                int tabCount = tabControl1.TabPages.Count;
                if (tabCount > 1)
                {
                    int currentTabIndex = tabCount - 1;
                    if (holdTabIndex == currentTabIndex)
                    {
                        currentTabIndex--;
                        holdTabIndex--;
                    }
                    TabPage tp = tabControl1.TabPages[currentTabIndex];
                    tabControl1.SelectTab(tp);
                    tsmiClose_Click(null, null);
                }
                else
                {
                    checkTabs = false;
                }
            }
        }

        void tsmiCloseLeft_Click(object sender, EventArgs e)
        {
            holdTabIndex = selTabIndex;
            bool checkTabs = true;
            while (checkTabs)
            {
                int tabCount = tabControl1.TabPages.Count;
                if (tabCount > 1)
                {
                    if (holdTabIndex > 0)
                    {
                        int currentTabIndex = holdTabIndex - 1;
                        holdTabIndex--;
                        TabPage tp = tabControl1.TabPages[currentTabIndex];
                        tabControl1.SelectTab(tp);
                        tsmiClose_Click(null, null);
                    }
                    else
                    {
                        checkTabs = false;
                    }
                }
                else
                {
                    checkTabs = false;
                }
            }
            tabControl1.SelectTab(holdTabIndex);
        }

        void tsmiCloseRight_Click(object sender, EventArgs e)
        {
            holdTabIndex = selTabIndex;
            bool checkTabs = true;
            while (checkTabs)
            {
                int tabCount = tabControl1.TabPages.Count;
                if (tabCount > 1)
                {
                    int currentTabIndex = tabCount - 1;
                    if (holdTabIndex == currentTabIndex)
                    {
                        checkTabs = false;
                    }
                    else
                    {
                        TabPage tp = tabControl1.TabPages[currentTabIndex];
                        tabControl1.SelectTab(tp);
                        tsmiClose_Click(null, null);
                    }
                }
                else
                {
                    checkTabs = false;
                }
            }
            tabControl1.SelectTab(holdTabIndex);
        }

        void tsmiOpenContainingFolder_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = tp.Tag as TextDoc;
            string filePath = td.FilePath;
            if (!File.Exists(filePath))
                return;
            string argument = @"/select, " + filePath;
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        void tsmiReadOnly_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            TabPage tp = tabControl1.SelectedTab;
            TextBox tb = tp.Controls[0] as TextBox;
            tsmi.Checked = tsmi.Checked ? false : true;
            tb.ReadOnly = tsmi.Checked;
        }

        void tsmiCopyFullFilePath_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = tp.Tag as TextDoc;
            Clipboard.SetText(td.FilePath);
        }

        void tsmiCopyFilename_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = tp.Tag as TextDoc;
            Clipboard.SetText(td.FileName);
        }

        void tsmiCopyDirectory_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = tp.Tag as TextDoc;
            string dirPath = Path.GetDirectoryName(td.FilePath);
            Clipboard.SetText(dirPath);
        }

        void tsmiUndo_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TextBox tb = tabControl1.SelectedTab.Controls[0] as TextBox;
            tb.Undo();
        }

        void tsmiExit_Click(object sender, EventArgs e) { Application.Exit(); }

        void tsmiPrintPreview_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.AllowMargins = true;
            pageSetupDialog1.AllowOrientation = true;
            pageSetupDialog1.AllowPaper = true;
            pageSetupDialog1.AllowPrinter = true;
            pageSetupDialog1.ShowNetwork = true;
            pageSetupDialog1.ShowHelp = true;
            pageSetupDialog1.EnableMetric = true;
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.DefaultPageSettings = pageSetupDialog1.PageSettings;
        }

        void tsmiPrint_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = tp.Tag as TextDoc;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.DocumentName = td.FileName;
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (tabControl1.TabPages.Count == 0)
                return;
            TabPage tp = tabControl1.SelectedTab;
            TextBox tb = tp.Controls[0] as TextBox;
            e.Graphics.DrawString(tb.Text, tb.Font, Brushes.Black, 10, 25);
        }

        void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount == 0)
                return;
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = (tp.Tag as TextDoc);
            saveFileDialog1.Filter = "Text File|*.txt";
            saveFileDialog1.Title = lngDiaTitleSaveFile;
            saveFileDialog1.FileName = td.FilePath;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Unicode);
                TextBox textBox = (TextBox)tabControl1.SelectedTab.Controls[0];
                sw.Write(textBox.Text);
                sw.Close();
                td.Changed = false;
                tp.Text = tp.Text.Replace("* ", "");
                tp.Tag = new TextDoc(saveFileDialog1.FileName, true);
                tp.Text = (tp.Tag as TextDoc).FileName;
            }
        }

        void tsmiSave_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount == 0)
                return;
            TabPage tp = tabControl1.SelectedTab;
            TextDoc td = (tp.Tag as TextDoc);
            if (td.Saved)
            {
                StreamWriter sw = new StreamWriter(td.FilePath);
                TextBox textBox = (TextBox)tabControl1.SelectedTab.Controls[0];
                sw.Write(textBox.Text);
                sw.Close();
                td.Changed = false;
                tp.Text = tp.Text.Replace("* ", "");
            }
            else
            {
                saveFileDialog1.Filter = "Text File|*.txt";
                saveFileDialog1.Title = lngDiaTitleSaveFile;
                saveFileDialog1.FileName = td.FilePath;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Unicode);
                    TextBox textBox = (TextBox)tabControl1.SelectedTab.Controls[0];
                    sw.Write(textBox.Text);
                    sw.Close();
                    td.Changed = false;
                    tp.Text = tp.Text.Replace("* ", "");
                    tp.Tag = new TextDoc(saveFileDialog1.FileName, true);
                    tp.Text = (tp.Tag as TextDoc).FileName;
                }
            }
        }

        void tsmiOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text File|*.txt";
            openFileDialog1.Title = lngDiaTitleOpenFile;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (TabPage tabPage in tabControl1.TabPages)
                    {
                        TextDoc td = tabPage.Tag as TextDoc;
                        if (openFileDialog1.FileName == td.FilePath)
                        {
                            MessageBox.Show(lngDiaAlreadyFileOpen);
                            return;
                        }
                    }
                    isFileLoading = true;
                    tsmiNew_Click(null, null);
                    int tabCount = tabControl1.TabPages.Count;
                    int lastTabIndex = tabCount - 1;
                    TabPage tp = tabControl1.TabPages[lastTabIndex];
                    TextBox tb = tp.Controls[0] as TextBox;
                    StreamReader rdr = new StreamReader(openFileDialog1.FileName);
                    tb.Text = rdr.ReadToEnd();
                    rdr.Close();
                    tp.Tag = new TextDoc(openFileDialog1.FileName, true);
                    tp.Text = (tp.Tag as TextDoc).FileName;
                }
                catch { MessageBox.Show(lngMsgBoxNotFoundFile); }
            }
            isFileLoading = false;
        }

        void tsmiNew_Click(object sender, EventArgs e)
        {
            if (isFileLoading == false)
                fileAutoNameCounter++;
            string fileName = lngNewFile.Replace("%n", fileAutoNameCounter.ToString());
            int tabCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add(fileName);
            TabPage tp = tabControl1.TabPages[tabCount];
            tp.Controls.Add(
                new TextBox()
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    Dock = DockStyle.Fill,
                    Font = defFont,
                    Name = "txtbox" + tabCount,
                    ContextMenuStrip = contextMenuStripTextBox,
                    WordWrap = wordWrap
                }
                    );
            (tp.Controls[0] as TextBox).TextChanged += new EventHandler(textBox_TextChanged);
            (tp.Controls[0] as TextBox).GotFocus += new EventHandler(statusFollow);
            (tp.Controls[0] as TextBox).Click += new EventHandler(statusFollow);
            (tp.Controls[0] as TextBox).KeyUp += new KeyEventHandler(textBox_KeyUp);
            (tp.Controls[0] as TextBox).LostFocus += new EventHandler(statusClean);
            tp.Tag = new TextDoc(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + ".txt");
            tabControl1.SelectTab(tp);
            if (selTabIndex == -1)
                selTabIndex = 0;
        }

        private void openLink(string url)
        {
            try { System.Diagnostics.Process.Start(url); }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void tsmiDocumentation_Click(object sender, EventArgs e) { openLink("https://www.webkolog.net/p/webkolog-text-editor.html"); }

        void tsmiSupport_Click(object sender, EventArgs e) { openLink("https://www.webkolog.net/p/contact.html"); }

        void tsmiCheckForUpdates_Click(object sender, EventArgs e)
        {
            tsmiDocumentation_Click(null, null);
        }

        void tsmiDateTime_Click(object sender, EventArgs e)
        {
            TextBox tb = (tabControl1.SelectedTab.Controls[0] as TextBox);
            tb.SelectedText = DateTime.Now.ToString();
        }

        void tsmiReplace_Click(object sender, EventArgs e)
        {
            if (isActiveExtraForm)
                return;
            replaceForm form = new replaceForm();
            showForm(form);
        }

        void tsmiFind_Click(object sender, EventArgs e)
        {
            if (isActiveExtraForm)
                return;
            findForm form = new findForm();
            showForm(form);
        }

        void showForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.TopMost = true;
            form.Show();
            isActiveExtraForm = true;
        }

        void tsmiDelete_Click(object sender, EventArgs e)
        {
            TextBox tb = (tabControl1.SelectedTab.Controls[0] as TextBox);
            tb.SelectedText = "";
        }

        void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl1.TabCount; ++i)
                {
                    if (tabControl1.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl1.SelectTab(i);
                        selTabIndex = i;
                        contextMenuStripTab.Show(tabControl1, e.Location);
                        break;
                    }
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < tabControl1.TabCount; ++i)
                {
                    if (tabControl1.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl1.SelectTab(i);
                        selTabIndex = i;
                        break;
                    }
                }
                tsmiClose_Click(null, null);
            }
        }

        void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog(this);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (isFileLoading == false)
            {
                TabPage tp = (TabPage)(sender as TextBox).Parent;
                TextDoc td = (tp.Tag as TextDoc);
                if (td.Changed != true)
                {
                    td.Changed = true;
                    tp.Text = "* " + tp.Text;
                }
                statusFollow(null, null);
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e) { statusFollow(null, null); }

        private void statusFollow(object sender, EventArgs e)
        {
            if (showStatusBar)
            {
                TextBox tb = (tabControl1.SelectedTab.Controls[0] as TextBox);
                int txtLine = tb.GetLineFromCharIndex(tb.SelectionStart);
                int txtCol = tb.SelectionStart;
                int firstIndex = tb.Text.LastIndexOf("\n", txtCol);
                txtCol = txtCol - firstIndex;
                txtLine++;
                string stsBarText = lngStsBarLineCol.Replace("%l", txtLine.ToString());
                tsl.Text = stsBarText.Replace("%c", txtCol.ToString());
            }
        }

        private void statusClean(object sender, EventArgs e) { tsl.Text = ""; }

    }

    class TextDoc
    {
        public bool Changed = false, Saved = false;
        public string FileName, FilePath;
        public TextDoc(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(filePath);
        }
        public TextDoc(string filePath, bool saved)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(filePath);
            Saved = saved;
        }
        public TextDoc(string filePath, bool saved, bool changed)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(filePath);
            Saved = saved;
            Changed = changed;
        }
    }
}
