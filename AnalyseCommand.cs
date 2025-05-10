using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CongMingDe
{
    internal static class AnalyseCommand
    {
        private static string EnterChar => Environment.NewLine;

        public static void Execute(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }
            Analyse(command, Program.MainForm.textBox1);

        }

        private static void Analyse(string command, TextBox output)
        {
            command = new Regex("[\\s]+").Replace(command, " ");
            var operation = command.Split(' ').First();
            var others = command.Remove(0, operation.Length);
            if (others.Length > 0)
            {
                if (others.First() == ' ')
                {
                    others = others.Remove(0, 1);
                }
            }

            try
            {
                switch (operation.ToLower())
                {
                    case "version":
                    case "版本":
                        Program.MainForm.OldString += $"当前 Windows 版本：{VersionHelper.GetOSVersion()}{EnterChar}当前 Windows 内核版本：{Environment.OSVersion}{EnterChar}当前 聪明的 版本：{new Version(1, 0, 0)}";
                        break;
                    case "echo":
                    case "say":
                    case "说":
                        Program.MainForm.OldString += others;
                        break;
                    case "exit":
                    case "退出":
                    case "quit":
                        Application.Exit();
                        break;
                    case "返回上一级":
                    case "退回上一级":
                    case "回到上一级":
                    Label_ToLastDirec:
                        var cdn = Program.MainForm.CurrentPath.Split('\\').Last();
                        var cd = Program.MainForm.CurrentPath;
                        Program.MainForm.CurrentPath = Program.MainForm.CurrentPath.Remove(cd.Length - cdn.Length);
                        if (Program.MainForm.CurrentPath.Last() == '\\')
                        {
                            Program.MainForm.CurrentPath = Program.MainForm.CurrentPath.Remove(Program.MainForm.CurrentPath.Length - 1);
                        }
                        break;
                    case "cd":
                    case "到":
                        if (others.First() == '\\')
                        {
                            if (Program.MainForm.CurrentPath.Last() == '\\')
                            {
                                others = Program.MainForm.CurrentPath + others.Remove(0, 1);
                            }
                            else
                            {
                                others = Program.MainForm.CurrentPath + others;
                            }
                        }
                        if (others == "..")
                        {
                            goto Label_ToLastDirec;
                        }
                        if (!Directory.Exists(others))
                        {
                            Program.MainForm.OldString += $"未知的路径\"{others}\"。";
                            break;
                        }
                        Program.MainForm.CurrentPath = others;
                        break;
                    case "cls":
                    case "clear":
                    case "清屏":
                        Program.MainForm.OldString = string.Empty;
                        Program.MainForm.textBox1.Text = string.Empty;
                        break;
                    case "dir":
                    case "扫描":
                        var ds = Directory.GetDirectories(Program.MainForm.CurrentPath);
                        var fs = Directory.GetFiles(Program.MainForm.CurrentPath);
                        Program.MainForm.OldString += $"名称     创建时间    大小（字节）{EnterChar}";
                        foreach (var dn in ds)
                        {
                            var di = new DirectoryInfo(dn);
                            Program.MainForm.OldString += $".\\{di.Name}    {di.CreationTime.ToString("g")}    --{EnterChar}";
                        }
                        foreach (var fn in fs)
                        {
                            var fi = new FileInfo(fn);
                            Program.MainForm.OldString += $"{fi.Name}    {fi.CreationTime.ToString("g")}    {fi.Length}{EnterChar}";
                        }
                        break;
                    case "start":
                    case "启动":
                        if (others.Length == 0)
                        {
                            Process.Start(Application.ExecutablePath);
                            break;
                        }
                        if (others.StartsWith("\\"))
                        {
                            Process.Start(Program.MainForm.CurrentPath + others, others.Remove(0, others.Split(' ').First().Length));
                        }
                        else
                        {
                            Process.Start(others, others.Remove(0, others.Split(' ').First().Length));
                        }
                        break;
                    case "set":
                    case "设置":
                        switch (others.Split(' ').First())
                        {
                            case "MaxLenght":
                                Program.MainForm.textBox1.MaxLength = int.Parse(others.Split(' ')[1]);
                                Program.MainForm.OldString += $"已将 MaxLenght 设为 {others.Split(' ')[1]} 。";
                                break;
                            case "ShowPath":
                                Program.MainForm.ShowPath = bool.Parse(others.Split(' ')[1]);
                                break;
                            case "Title":
                                Program.MainForm.Text = others.Split(' ')[1];
                                break;
                            case "ForeColor":
                                Program.MainForm.textBox1.ForeColor = Color.FromName(others.Split(' ')[1]);
                                break;
                            case "BackColor":
                                Program.MainForm.textBox1.BackColor = Color.FromName(others.Split(' ')[1]);
                                break;
                        }
                        break;
                    case "reset":
                    case "重置":
                        Program.MainForm.Size = new Size(949, 677);
                        Program.MainForm.textBox1.MaxLength = 32767;
                        Program.MainForm.Text = "终端 - 聪明的";
                        Program.MainForm.ShowPath = true;
                        Program.MainForm.textBox1.ForeColor = Color.FromKnownColor(KnownColor.Window);
                        Program.MainForm.textBox1.BackColor = Color.FromArgb(64, 64, 64);
                        break;
                    case "cp":
                    case "cf":
                    case "当前文件夹":
                    case "当前路径":
                        var cdi = new DirectoryInfo(Program.MainForm.CurrentPath);
                        Program.MainForm.OldString += $"名称：{cdi.Name}{EnterChar}全名：{cdi.FullName}{EnterChar}根：{cdi.Root}{EnterChar}创建时间：{cdi.CreationTime.ToString("F")}{EnterChar}当前目录特性：{cdi.Attributes}{EnterChar}上次访问时间：{cdi.LastAccessTime.ToString("F")}{EnterChar}上次写入时间：{cdi.LastWriteTime.ToString("F")}";
                        break;
                    case "delete":
                    case "del":
                    case "删除":
                        if (Directory.Exists(others))
                        {
                            Directory.Delete(others, true);
                        }
                        else if (File.Exists(others))
                        {
                            File.Delete(others);
                        }
                        else
                        {
                            throw new ArgumentException("请输入要删除的文件或目录。");
                        }
                        break;
                    case "read":
                    case "rd":
                    case "查看":
                        if (others.Length == 0)
                        {
                            throw new ArgumentException("查看文件的参数为空。");
                        }
                        Program.MainForm.OldString += File.ReadAllText(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others);
                        break;
                    case "rbin":
                    case "查看二进制":
                    case "readbin":
                        var br = new BinaryReader(File.OpenRead(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others));
                        Program.MainForm.OldString += $"二进制：{EnterChar}";
                        var bindata = new byte[br.BaseStream.Length];
                        br.Read(bindata, 0, bindata.Length);
                        Program.MainForm.OldString += $"{bindata.ToBinary()}{EnterChar}";
                        
                        break;
                    case "crf":
                    case "createfile":
                    case "新建文件":
                    case "创建文件":
                        File.Create(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others).Close();
                        break;
                    case "crd":
                    case "createdirectory":
                    case "创建文件夹":
                    case "新建文件夹":
                        Directory.CreateDirectory(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others);
                        break;
                    case "wrf":
                    case "writefile":
                    case "写入文件":
                        Program.MainForm.OldString = File.ReadAllText(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others, Encoding.UTF8);
                        Program.MainForm.textBox1.Text = Program.MainForm.OldString;
                        Program.MainForm.WhenItsWriting = true;
                        Program.MainForm.WritingPath = others.First() == '\\' ? Program.MainForm.CurrentPath + others : others;
                        break;
                    case "execute":
                    case "执行":
                        foreach (var line in File.ReadAllLines(others.First() == '\\' ? Program.MainForm.CurrentPath + others : others))
                        {
                            if (line.Replace(" ", "").First() == '#')
                            {
                                continue;
                            }
                            Analyse(line, output);
                        }
                        break;
                    case "color":
                    case "颜色":
                        foreach (var param in others.Split(' '))
                        {
                            if (param.Remove(2).ToLower() == "f:")
                            {
                                Program.MainForm.textBox1.ForeColor = Color.FromName(param.Remove(0, 2));
                            }
                            if (param.Remove(2).ToLower() == "b:")
                            {
                                Program.MainForm.textBox1.BackColor = Color.FromName(param.Remove(0, 2));
                            }
                        }
                        break;
                    case "help":
                    case "帮助":
                    case "?":
                    case "？":
                        Program.MainForm.OldString += Helper.HELP_TEXT;
                        break;
                    default:
                        try
                        {
                            Process.Start(operation, others);
                        }
                        catch (Win32Exception)
                        {
                            Program.MainForm.OldString += $"未知的指令\"{operation}\"，请使用\"帮助\"指令查看可用的指令。";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.MainForm.OldString += $"程序引发了一个异常：{ex}";
            }
            if (!Directory.Exists(Program.MainForm.CurrentPath))
            {
                Program.MainForm.CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }
    }
}
