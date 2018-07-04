using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Collections.Generic;
using XYDES;

namespace ScienceResearchWpfApplication.ApplicationProgram
{
    /// <summary>
    /// ApplicationUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ApplicationUserControl : UserControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;

        public string headerStr="文章";

        IntPtr handle;

        int application_id;
        List<int> application_id_list;

        public ApplicationUserControl()
        {
            InitializeComponent();
            application_id = 0;
            application_id_list = new List<int>();
        }

        private void btnLoadInk_Click(object sender, RoutedEventArgs e)
        {
            WindowsFormsHostUserControl host = (WindowsFormsHostUserControl)((TabItem)applicationTabControl.SelectedItem).Content;
            handle = host.handle_application;

            ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
            //paragraphFigureUserControl.HideToolBar();
            TabItem paragraphFigureTabItem = new TabItem();
            paragraphFigureTabItem.Header = "图片";
            paragraphFigureTabItem.Content = paragraphFigureUserControl;
            applicationTabControl.Items.Add(paragraphFigureTabItem);

            applicationTabControl.SelectedItem = paragraphFigureTabItem;

            //加载图片
            System.Drawing.Bitmap bitBmp =MainWindow.PrtWindow(handle);
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bitBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource img = bs;
            paragraphFigureUserControl.img.Source = img;
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();            
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();            
        }


        public void loadProcess(string processStr)
        {
            if (MainWindow.app_included)
            {
                WindowsFormsHostUserControl windowsFormsHostUserControl = new WindowsFormsHostUserControl();
                windowsFormsHostUserControl.applicationId = application_id;
                application_id_list.Add(application_id);
                application_id += 1;
                TabItem windowsFormsHostTabItem = new TabItem();
                Label headerLabel = new Label();
                headerLabel.Content = headerStr;
                headerLabel.MouseDoubleClick += headerLabel_MouseDoubleClick;
                windowsFormsHostTabItem.Header = headerLabel;
                windowsFormsHostTabItem.Content = windowsFormsHostUserControl;
                applicationTabControl.Items.Add(windowsFormsHostTabItem);

                applicationTabControl.SelectedItem = windowsFormsHostTabItem;

                try
                {
                    windowsFormsHostUserControl.loadProcess(processStr);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                Process process=null;
                try
                {
                    process = Process.Start(processStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                //IntPtr handle_application = (IntPtr)0;
                //if (process != null)
                //{
                //    while ((int)handle_application == 0)
                //        handle_application = process.MainWindowHandle;
                //}
                //else
                //{
                //    var t = DateTime.Now.AddMilliseconds(1000);
                //    while (DateTime.Now < t)
                //        DispatcherHelper.DoEvents();
                //    //handle_application = FindWindow("CabinetWClass", null);
                //    handle_application = GetForegroundWindow();
                //}

                ////设置两个窗体的位置 
                //if (MainWindow.setupUserControl.leftRightRadioButton.IsChecked == true)
                //{
                //    MainWindow.mainWindow.rightButton_Click2();
                //    System.Drawing.Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point(0, 0));
                //    double scaleX = PrimaryScreen.ScaleX;
                //    double scaleY = PrimaryScreen.ScaleY;
                //    double nWidth = rect.Width/2;
                //    double nHeight = rect.Height;
                //    MoveWindow(handle_application, 0, 0, (int)nWidth, (int)nHeight, true);
                //}
            }
        }

        private void headerLabel_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            application_id_list.Remove(((WindowsFormsHostUserControl)(((TabItem)applicationTabControl.SelectedItem).Content)).applicationId);
            applicationTabControl.Items.Remove(applicationTabControl.SelectedItem);
        }

        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            SetParent(((WindowsFormsHostUserControl)((TabItem)applicationTabControl.SelectedItem).Content).handle_application, ((WindowsFormsHostUserControl)((TabItem)applicationTabControl.SelectedItem).Content).handle_panel);
            SetForegroundWindow(((WindowsFormsHostUserControl)((TabItem)applicationTabControl.SelectedItem).Content).handle_application);
            SendMessage(((WindowsFormsHostUserControl)((TabItem)applicationTabControl.SelectedItem).Content).handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            WindowsFormsHostUserControl windowsFormsHostUserControl = new WindowsFormsHostUserControl();
            TabItem windowsFormsHostTabItem = new TabItem();
            Label headerLabel = new Label();
            headerLabel.Content = "外程";
            headerLabel.MouseDoubleClick += headerLabel_MouseDoubleClick;
            windowsFormsHostTabItem.Header = headerLabel;
            windowsFormsHostTabItem.Content = windowsFormsHostUserControl;
            applicationTabControl.Items.Add(windowsFormsHostTabItem);
            applicationTabControl.SelectedItem = windowsFormsHostTabItem;

            //IntPtr handle_application = FindWindow(null, "Microsoft Edge");

            IntPtr handle_application = (IntPtr)Convert.ToInt32(appTextBox.Text, 16);
            windowsFormsHostUserControl.loadProcess2(handle_application);
        }

        private void btnInput2_Click(object sender, RoutedEventArgs e)
        {
            WindowsFormsHostUserControl windowsFormsHostUserControl = new WindowsFormsHostUserControl();
            TabItem windowsFormsHostTabItem = new TabItem();
            Label headerLabel = new Label();
            headerLabel.Content = "外程";
            headerLabel.MouseDoubleClick += headerLabel_MouseDoubleClick;
            windowsFormsHostTabItem.Header = headerLabel;
            windowsFormsHostTabItem.Content = windowsFormsHostUserControl;
            applicationTabControl.Items.Add(windowsFormsHostTabItem);
            applicationTabControl.SelectedItem = windowsFormsHostTabItem;

            IntPtr handle_application = FindWindow(appTextBox2.Text, null);

            //IntPtr handle_application = (IntPtr)Convert.ToInt32(appTextBox.Text, 16);
            windowsFormsHostUserControl.loadProcess2(handle_application);

        }

        private void btnInput3_Click(object sender, RoutedEventArgs e)
        {
            WindowsFormsHostUserControl windowsFormsHostUserControl = new WindowsFormsHostUserControl();
            TabItem windowsFormsHostTabItem = new TabItem();
            Label headerLabel = new Label();
            headerLabel.Content = "外程";
            headerLabel.MouseDoubleClick += headerLabel_MouseDoubleClick;
            windowsFormsHostTabItem.Header = headerLabel;
            windowsFormsHostTabItem.Content = windowsFormsHostUserControl;
            applicationTabControl.Items.Add(windowsFormsHostTabItem);
            applicationTabControl.SelectedItem = windowsFormsHostTabItem;

            IntPtr handle_application = FindWindow(null, appTextBox3.Text);

            //IntPtr handle_application = (IntPtr)Convert.ToInt32(appTextBox.Text, 16);
            windowsFormsHostUserControl.loadProcess2(handle_application);
        }
    }    
}
