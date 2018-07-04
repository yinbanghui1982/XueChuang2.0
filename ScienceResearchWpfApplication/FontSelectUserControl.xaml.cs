using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// FontSelectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class FontSelectUserControl : UserControl, INotifyPropertyChanged
    {
        FontFamily fontFamily;
        double fontSize;

        public event PropertyChangedEventHandler PropertyChanged;
        public FontSelectUserControl()
        {
            InitializeComponent();

            foreach (FontFamily font in Fonts.SystemFontFamilies)

            {
                fontComboBox.Items.Add(new FontItemsUserControl(font));
            }

            for (double i = 20; i <= 40; i += 2)
            {
                sizeComboBox.Items.Add(i);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void fontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontItemsUserControl item = (FontItemsUserControl)fontComboBox.SelectedItem;
            SelectedFontFamily = item.Font;
        }

        public FontFamily SelectedFontFamily
        {
            get { return fontFamily; }
            set
            {
                fontFamily = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFontFamily"));
            }
        }

        private void sizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double item = (double)sizeComboBox.SelectedItem;
            SelectedFontSize = item;
        }

        public double SelectedFontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFontSize"));
            }
        }

        public void setSelected(double fontSize,FontFamily fontFamily)
        {
            foreach (FontItemsUserControl item in fontComboBox.Items)
            {
                if (item.Font.Source == fontFamily.Source)
                {
                    fontComboBox.SelectedItem = item;
                    break;
                }
            }
            for (int i = 0; i < sizeComboBox.Items.Count; i++)
            {
                if (fontSize == (double)sizeComboBox.Items[i])
                {
                    sizeComboBox.SelectedIndex = i;
                    break;
                }
            }           
        }


    }
}
