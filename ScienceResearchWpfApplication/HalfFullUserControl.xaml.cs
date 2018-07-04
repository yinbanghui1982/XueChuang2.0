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
    /// HarfFullUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class HalfFullUserControl : UserControl
    {
        public HalfFullUserControl()
        {
            InitializeComponent();
        }


        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            bool inLeft = GetParentObject_leftTab(this);
            if (inLeft)
                MainWindow.mainWindow.right_zero();
            else
                MainWindow.mainWindow.left_zero();
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        public bool GetParentObject_leftTab(DependencyObject obj)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {                
                if (parent==MainWindow.mainWindow.leftTabControl)
                {
                    return true;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return false;
        }
    }



}
