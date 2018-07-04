using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Security.Permissions;
using System.Windows.Threading;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication.ApplicationProgram
{
    /// <summary>
    /// WindowsFormsHostUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WindowsFormsHostUserControl : UserControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;

        public IntPtr handle_panel;
        public IntPtr handle_application;

        public int applicationId;

        string logPath;
        string log_str = "";

        public PaperUserControl wordPaperUserControl;

        public WindowsFormsHostUserControl()
        {
            InitializeComponent();

            System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            //panel.BackColor = System.Drawing.Color.Red;

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = panel;
            grid.Children.Add(host);
            //host.Background = System.Windows.Media.Brushes.Red;

            handle_panel = panel.Handle;
        }
        
        public void loadProcess(string proceddStr)
        {
            if (MainWindow.app_included)
            {
                string houzhui = proceddStr.Substring(proceddStr.LastIndexOf('.')+1);

                Process process = Process.Start(proceddStr);
                //string str1="";
                //Process myProcess = new Process();
                //Process[] wordProcess;                

                if (houzhui == "doc" || houzhui == "docx")
                {
                    log_str = log_str + "正在打开word文件\r\n";

                    //do
                    //{                        
                    //    wordProcess = Process.GetProcessesByName("WinWord");
                    //    log_str = log_str + "wordProcess.Length="+wordProcess.Length+"\r\n";
                    //    try
                    //    {
                    //        log_str = log_str + "word进程数为："+wordProcess.Length+"\r\n";
                    //        foreach (Process pro in wordProcess)
                    //        {
                    //            str1 = pro.MainWindowTitle;
                    //            log_str = log_str + "进程名：" + str1 + "\r\n";
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show(ex.Message);
                    //    }
                    //} while (wordProcess.Length!=1);
                    //log_str = log_str + "跳出打开循环" + "\r\n";

                    int time_waiting = 15 * 1000;
                    var t1 = DateTime.Now.AddMilliseconds(time_waiting);
                    while (DateTime.Now < t1)
                        DispatcherHelper.DoEvents();
                }

                if (process != null)
                {
                    process.EnableRaisingEvents = true;
                    process.Exited += App_Exited;

                    handle_application = (IntPtr)0;
                    while ((int)handle_application == 0)
                        handle_application = process.MainWindowHandle;
                    log_str = log_str + "process != null,handle_application=" + handle_application + "\r\n";

                }
                else
                {
                    var t = DateTime.Now.AddMilliseconds(2000);
                    while (DateTime.Now < t)
                        DispatcherHelper.DoEvents();
                    //handle_application = FindWindow("CabinetWClass", null);
                    handle_application = GetForegroundWindow();
                    log_str = log_str + "process == null,handle_application=" + handle_application + "\r\n";
                }
                SetParent(handle_application, handle_panel);
                SetForegroundWindow(handle_application);
                SendMessage(handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                MainWindow.intPtrs.Add(handle_application);

                AppButton bt = new AppButton(handle_application);
                if (process != null)
                    bt.Content = process.ProcessName;
            }
            else
            {
                Process process = Process.Start(proceddStr);
            }


            //保存文件
            logPath = MainWindow.path_database + "\\log.txt";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(logPath, true);
            sw.WriteLine(log_str);
            sw.Close();
        }

        public void loadProcess2(IntPtr handle)
        {
            handle_application = handle;
            SetParent(handle_application, handle_panel);
            //SetForegroundWindow(handle_application);
            SendMessage(handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            MainWindow.intPtrs.Add(handle_application);
        }

        public void Duli()
        {
            SetParent(handle_application, IntPtr.Zero);
        }

        private void App_Exited(object sender, EventArgs e)
        {
            //关闭应用程序时，关闭相应的按钮
            IntPtr handle = ((Process)sender).MainWindowHandle;
            Dispatcher.BeginInvoke(new Action(delegate
            {
                MainWindow.intPtrs.Remove(handle);
            }));
        }
    }

    public static class DispatcherHelper
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
            try { Dispatcher.PushFrame(frame); }
            catch (InvalidOperationException) { }
        }
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
