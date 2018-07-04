using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace ScienceResearchWpfApplication.ApplicationProgram
{
    class AppButton:Button
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);

        public IntPtr handle_application;
        public AppButton(IntPtr _handle_application)
            : base()
        {
            handle_application = _handle_application;
            this.Click += AppButton_Click;
        }

        private void AppButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetForegroundWindow((int)handle_application);
        }
    }
}




