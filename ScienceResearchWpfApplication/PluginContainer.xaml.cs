using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// PluginContainer.xaml 的交互逻辑
    /// </summary>
    public partial class PluginContainer : UserControl
    {
        public PluginContainer(string appFileName, IntPtr hostHandle)
        {
            InitializeComponent();
            _appFilename = appFileName;
            _hostWinHandle = hostHandle;
        }

        private void OpenExternProcess(int width, int height)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(_appFilename);
                info.UseShellExecute = true;
                //info.WindowStyle = ProcessWindowStyle.Minimized;
                info.WindowStyle = ProcessWindowStyle.Hidden;

                AppProcess = System.Diagnostics.Process.Start(info);
                // Wait for process to be created and enter idle condition
                AppProcess.WaitForInputIdle();
                while (AppProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EmbedProcess(int width, int height)
        {
            OpenExternProcess(width, height);
            try
            {
                var pluginWinHandle = AppProcess.MainWindowHandle;//Get the handle of main window.
                embedResult = Win32API.SetParent(pluginWinHandle, _hostWinHandle);//set parent window
                Win32API.SetWindowLong(new HandleRef(this, pluginWinHandle), Win32API.GWL_STYLE, Win32API.WS_VISIBLE);//Set window style to "None".
                var moveResult = Win32API.MoveWindow(pluginWinHandle, 0, 0, width, height, true);//Move window to fixed position(up-left is (0,0), and low-right is (width, height)).
                //embed failed, and tries again
                if (!moveResult || embedResult == 0)
                {
                    AppProcess.Kill();
                    if (MAXCOUNT-- > 0)
                    {
                        EmbedProcess(width, height);
                    }
                }
                else
                {
                    Win32API.ShowWindow(pluginWinHandle, (short)Win32API.SW_MAXIMIZE);
                }
            }
            catch (Exception ex)
            {
                var errorString = Win32API.GetLastError();
                MessageBox.Show(errorString + ex.Message);
            }
            return (embedResult != 0);
        }

        #region

        public int embedResult = 0;

        public Process AppProcess { get; set; }
        private IntPtr _hostWinHandle { get; set; }

        private string _appFilename = "";

        private int MAXCOUNT = 10;

        #endregion

    }
}
