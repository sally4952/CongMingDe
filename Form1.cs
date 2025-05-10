using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CongMingDe
{
    public partial class Form1 : Form
    {
        public string CurrentPath;
        public string OldString;
        private readonly Thread UpdateTextThread;
        public bool ShowPath = true;
        public bool WhenItsWriting = false;
        public string WritingPath = string.Empty;

        public Form1()
        {
            InitializeComponent();
            CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OldString = $"聪明的 [{Assembly.GetExecutingAssembly().GetName().Version}（版本）]{Environment.NewLine}sally4953 保留所有权利。{Environment.NewLine}" + Environment.NewLine + CurrentPath + ">";
            UpdateTextThread = new Thread(async () =>
            {
                while (true)
                {
                    if (textBox1.TextLength < OldString.Length)
                    {
                        if (WhenItsWriting)
                            goto Label_Continue;
                        textBox1.Text = OldString;
                        textBox1.Select(textBox1.TextLength, 0);
                    }
                Label_Continue:
                    await Task.Delay(0);
                }
            });
            UpdateTextThread.Start();

            this.FormClosed += Form1_FormClosed;
            textBox1.KeyDown += TextBox1_KeyDown;
            textBox1.KeyUp += TextBox1_KeyUp;
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                IsPressCtrl = false;
            }
        }

        private bool IsPressCtrl = false;
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (WhenItsWriting)
                {
                    return;
                }
                textBox1.ReadOnly = true;
                var os = OldString;
                //OldString += textBox1.Text.Remove(0, OldString.Length);
                OldString = textBox1.Text;
                OldString += Environment.NewLine;
                //OldString += textBox1.Text.Remove(0, os.Length);
                AnalyseCommand.Execute(textBox1.Text.Remove(0, os.Length));
                OldString += Environment.NewLine;
                OldString += ShowPath ? CurrentPath + ">" : ">";
                textBox1.ReadOnly = false;
                //textBox1.Select(textBox1.TextLength, 0);
                //textBox1.ScrollToCaret();
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                IsPressCtrl = true;
            }
            if (IsPressCtrl)
            {
                if (e.KeyCode == Keys.Q)
                {
                    if (WhenItsWriting)
                    {
                        return;
                    }
                    OldString = string.Empty;
                    textBox1.Text = string.Empty;
                }
                if (e.KeyCode == Keys.R)
                {
                    if (WhenItsWriting)
                    {
                        return;
                    }
                    Program.MainForm.Size = new Size(949, 677);
                    Program.MainForm.textBox1.MaxLength = 32767;
                    Program.MainForm.Text = "终端 - 聪明的";
                    Program.MainForm.ShowPath = true;
                    Program.MainForm.textBox1.ForeColor = Color.FromKnownColor(KnownColor.Window);
                    Program.MainForm.textBox1.BackColor = Color.FromArgb(64, 64, 64);
                }
                if (e.KeyCode == Keys.E)
                {
                    if (!WhenItsWriting)
                    {
                        Application.Exit();
                        return;
                    }
                    WhenItsWriting = false;
                    File.WriteAllText(WritingPath, textBox1.Text);
                    OldString = Environment.NewLine + Environment.NewLine + CurrentPath + ">";
                    textBox1.Text = OldString;
                    textBox1.Select(textBox1.TextLength, 0);
                }
                if (e.KeyCode == Keys.T)
                {
                    textBox1.Text += "    ";
                    textBox1.Select(textBox1.TextLength, 0);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateTextThread.Abort();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            textBox1.Size = this.ClientSize;
        }
    }
}
