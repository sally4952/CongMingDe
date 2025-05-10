using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CongMingDe
{
    internal static class Program
    {
        public static Form1 MainForm;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm = new Form1();
            if (args.Length == 1)
            {
                if (Directory.Exists(args[0]))
                {
                    MainForm.CurrentPath = args[0];
                }
            }

            Application.Run(MainForm);
        }
    }
}
