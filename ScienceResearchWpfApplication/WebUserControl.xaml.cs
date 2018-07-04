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

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// WebUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WebUserControl : UserControl
    {
        public WebUserControl()
        {
            InitializeComponent();
        }

        private void btnHalf_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.mainWindow.left_zero();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDuli_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void ForwardButton_Click(object sender, RoutedEventArgs e)
        //{
        //    BrowserControl.GoForward();
        //}

        //private void BackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    BrowserControl.GoBack();
        //}

        //private void RefreshButton_Click(object sender, RoutedEventArgs e)
        //{
        //    BrowserControl.Refresh();
        //}

        //private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)
        //    {
        //        var url = "http://"+UrlTextBox.Text;
        //        BrowserControl.Navigate(url);
        //    }
        //}


    }


}
