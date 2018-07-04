using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// ConnectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectUserControl : UserControl
    {
        TextboxInkcavasUserControl textboxInkcavasUserControl;
        public ConnectUserControl()
        {
            InitializeComponent();

            textboxInkcavasUserControl = new TextboxInkcavasUserControl();
            textboxInkcavasUserControl.paragraphRichTextBox.Width = 580;
            textboxInkcavasUserControl.inkCanvas.Width = 580;
            textboxInkcavasUserControl.type = "refer";
            chateStackPanel.Children.Add(textboxInkcavasUserControl);

            textboxInkcavasUserControl = new TextboxInkcavasUserControl();
            textboxInkcavasUserControl.paragraphRichTextBox.Width = 580;
            textboxInkcavasUserControl.inkCanvas.Width = 580;
            textboxInkcavasUserControl.type = "refer";
            sendStackPanel.Children.Add(textboxInkcavasUserControl);
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private List<string> GetMacByIPConfig()
        {
            List<string> macs = new List<string>();

            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            //截取输出流
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("物理地址") || line.StartsWith("Physical Address"))
                    {
                        int start_location = line.IndexOf(":") + 2;
                        line = line.Substring(start_location);

                        line = line.Replace("-", "");
                        if (line != "00000000000000E0")
                            macs.Add(line);
                    }
                }

                line = reader.ReadLine();
            }

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();

            return macs;
        }

        

        private void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                byte[] arrRecMsg = new byte[1024 * 1024];
                int length = MainWindow.socketClient.Receive(arrRecMsg);
                string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            RichTextBox richTextBox = ((TextboxInkcavasUserControl)sendStackPanel.Children[0]).paragraphRichTextBox;
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            ClientSendMsg(textRange.Text.Trim());

        }

        private void ClientSendMsg(string sendMsg)
        {
            RichTextBox richTextBox = ((TextboxInkcavasUserControl)chateStackPanel.Children[0]).paragraphRichTextBox;

            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            MainWindow.socketClient.Send(arrClientSendMsg);
            richTextBox.AppendText("天之涯:" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
        }

        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
    }
}
