using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms.Integration;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ScienceResearchWpfApplication.ApplicationProgram
{
    /// <summary>
    /// AppUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppUserControl : System.Windows.Controls.UserControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent); 
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetForegroundWindow(IntPtr hWnd);

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;

        IntPtr handle_panel;
        IntPtr handle_application;

        public AppUserControl()
        {
            InitializeComponent();
            System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = panel;
            grid_panel.Children.Add(host);
            handle_panel = panel.Handle;
        }

        private void btnLoadInk_Click(object sender, RoutedEventArgs e)
        {
            //设置图片            
            Bitmap catchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Graphics g = Graphics.FromImage(catchBmp);
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));
            catchBmp.Save(MainWindow.path_database + @"\科学研究\应用程序图片\0.tif", ImageFormat.Tiff);


            //打开程序
            string processStr =MainWindow.path_database+ @"\科学研究\重庆大学\ScienceResearch\ScienceResearch_New\InkScienceResearchWpfApplication\bin\Debug\InkScienceResearchWpfApplication.exe";
            loadProcess(processStr);    
        }

        public void loadProcess(string proceddStr)
        {        
            if (MainWindow.app_included)
            {
                Process process = Process.Start(proceddStr);
                if (process != null)
                {
                    process.EnableRaisingEvents = true;
                    process.Exited += App_Exited;

                    handle_application = (IntPtr)0;
                    while ((int)handle_application == 0)
                        handle_application = process.MainWindowHandle;
                }
                else
                {
                    //handle_application = FindWindow("Windows.UI.Core.CoreWindow", "Microsoft Edge");
                }
                SetParent(handle_application, handle_panel);
                SetForegroundWindow(handle_application);
                SendMessage(handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                MainWindow.intPtrs.Add(handle_application);

                AppButton bt = new AppButton(handle_application);
                if (process != null)
                    bt.Content = process.ProcessName;
                appStackPanel.Children.Add(bt);
            }
            else
            {
                Process process = Process.Start(proceddStr);

                //设置两个窗体的位置 


            }

        }

        private void App_Exited(object sender, EventArgs e)
        {
            //关闭应用程序时，关闭相应的按钮
            IntPtr handle= ((Process)sender).MainWindowHandle;
            Dispatcher.BeginInvoke(new Action(delegate
            {
                int buttonCount = appStackPanel.Children.Count;

                for (int i=0;i< buttonCount;i++)
                {
                    AppButton bt = (AppButton)appStackPanel.Children[i];
                    if (bt.handle_application == handle)
                    {
                        appStackPanel.Children.Remove(bt);
                        MainWindow.intPtrs.Remove(handle);
                        break;
                    }
                }
            }));
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }
    }
}
