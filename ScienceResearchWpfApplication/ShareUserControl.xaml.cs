using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication.Share
{
    /// <summary>
    /// ShareUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShareUserControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShareUserControl()
        {
            InitializeComponent();

        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {

        }

        private string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        private void dengluButton_Click(object sender, RoutedEventArgs e)
        {
            //登录服务器
            //List<string> macs = GetMacByIPConfig();
            //string mac_string = macs[0];

            MainWindow.socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(GetAddressIP());
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse("1"));
            MainWindow.socketClient.Connect(endpoint);
            MainWindow.threadClient = new Thread(RecMsg);
            MainWindow.threadClient.IsBackground = true;
            MainWindow.threadClient.Start();

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "连接成功!";
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            //((TextboxInkcavasUserControl)chateStackPanel.Children[0]).paragraphRichTextBox.AppendText("连接成功!" + "\r\n");
        }

        private void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                byte[] arrRecMsg = new byte[1024 * 1024];
                int length = MainWindow.socketClient.Receive(arrRecMsg);
                string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);

                MainWindow.connectUserControl.chateStackPanel.Dispatcher.Invoke(new Action(() => { ((TextboxInkcavasUserControl)MainWindow.connectUserControl.chateStackPanel.Children[0]).paragraphRichTextBox.AppendText("So-flash:" + GetCurrentTime() + "\r\n" + strRecMsg + "\r\n"); }));

            }
        }

        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
    }
}
