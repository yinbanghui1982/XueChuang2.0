using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.ComponentModel;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// 属性更改委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="v"></param>
    public delegate void ChangedHandler(object sender,object v);

    /// <summary>
    /// InkPropertiesUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class InkPropertiesUserControl : UserControl, INotifyPropertyChanged
    {
        RegistryKey scienceResearchKey;

        Color color;
        bool isHighlighter;        
        double slider_double;
        InkCanvasEditingMode editingMode;

        public event PropertyChangedEventHandler PropertyChanged;
        public event ChangedHandler ChangeColorInk;
        public event ChangedHandler ChangeWidthInk;
        public event ChangedHandler ChangeIsHighlighterInk;
        public event ChangedHandler ChangeEditingModeInk;

        public bool isInitial;
        Color color1;
        string name;

        //-------------------------构造函数----------------------------------------------------
        public InkPropertiesUserControl()
        {
            InitializeComponent();
            isInitial = true;

            //===========设置控件初始值=================================================
            scienceResearchKey = MainWindow.scienceResearchKey;

            if (MainWindow.inkPropertiesUserControlIndex == 0)
            {
                string biSelct = scienceResearchKey.GetValue("Ink_Bi").ToString();
                if (biSelct == "Gangbi")
                {
                    gangbiRadioButton.IsChecked = true;
                }
                else
                {
                    yingguangbiRadioButton.IsChecked = true;
                }

                slider_double = Convert.ToDouble(scienceResearchKey.GetValue("Ink_Slider").ToString());
                slider.Value = slider_double;

                string colorStr = scienceResearchKey.GetValue("Ink_Color").ToString();
                color1 = (Color)ColorConverter.ConvertFromString(colorStr);
                lstbox.SelectedColor = color1;
                ColorInk = lstbox.SelectedColor;

                string editingModeStr = scienceResearchKey.GetValue("Ink_EditingMode").ToString();
                if (editingModeStr == "Write")
                {
                    writeRadioButton.IsChecked = true;
                }
                else
                {
                    clearRadioButton.IsChecked = true;
                }
            }
            else if (MainWindow.inkPropertiesUserControlIndex == 1)
            {
                string biSelct = scienceResearchKey.GetValue("Ink_Bi_1").ToString();
                if (biSelct == "Gangbi")
                {
                    gangbiRadioButton.IsChecked = true;
                }
                else
                {
                    yingguangbiRadioButton.IsChecked = true;
                }

                slider_double = Convert.ToDouble(scienceResearchKey.GetValue("Ink_Slider_1").ToString());
                slider.Value = slider_double;

                string colorStr = scienceResearchKey.GetValue("Ink_Color_1").ToString();
                color1 = (Color)ColorConverter.ConvertFromString(colorStr);
                lstbox.SelectedColor = color1;
                ColorInk = lstbox.SelectedColor;

                string editingModeStr = scienceResearchKey.GetValue("Ink_EditingMode_1").ToString();
                if (editingModeStr == "Write")
                {
                    writeRadioButton.IsChecked = true;
                }
                else
                {
                    clearRadioButton.IsChecked = true;
                }
            }
            else if (MainWindow.inkPropertiesUserControlIndex == 2)
            {
                string biSelct = scienceResearchKey.GetValue("Ink_Bi_2").ToString();
                if (biSelct == "Gangbi")
                {
                    gangbiRadioButton.IsChecked = true;
                }
                else
                {
                    yingguangbiRadioButton.IsChecked = true;
                }

                slider_double = Convert.ToDouble(scienceResearchKey.GetValue("Ink_Slider_2").ToString());
                slider.Value = slider_double;

                string colorStr = scienceResearchKey.GetValue("Ink_Color_2").ToString();
                color1 = (Color)ColorConverter.ConvertFromString(colorStr);
                lstbox.SelectedColor = color1;
                ColorInk = lstbox.SelectedColor;

                string editingModeStr = scienceResearchKey.GetValue("Ink_EditingMode_2").ToString();
                if (editingModeStr == "Write")
                {
                    writeRadioButton.IsChecked = true;
                }
                else
                {
                    clearRadioButton.IsChecked = true;
                }
            }
            else if (MainWindow.inkPropertiesUserControlIndex == 3)
            {
                string biSelct = scienceResearchKey.GetValue("Ink_Bi_3").ToString();
                if (biSelct == "Gangbi")
                {
                    gangbiRadioButton.IsChecked = true;
                }
                else
                {
                    yingguangbiRadioButton.IsChecked = true;
                }

                slider_double = Convert.ToDouble(scienceResearchKey.GetValue("Ink_Slider_3").ToString());
                slider.Value = slider_double;

                string colorStr = scienceResearchKey.GetValue("Ink_Color_3").ToString();
                color1 = (Color)ColorConverter.ConvertFromString(colorStr);
                lstbox.SelectedColor = color1;
                ColorInk = lstbox.SelectedColor;

                string editingModeStr = scienceResearchKey.GetValue("Ink_EditingMode_3").ToString();
                if (editingModeStr == "Write")
                {
                    writeRadioButton.IsChecked = true;
                }
                else
                {
                    clearRadioButton.IsChecked = true;
                }
            }
            MainWindow.inkPropertiesUserControlIndex += 1;

        }
        public string NameInk
        {
            set
            {
                name = value;
            }
        }
        
        //-------------------------控件操作处理----------------------------------------------------
        private void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorInk = lstbox.SelectedColor;
            
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WidthInk = slider.Value;
        }

        private void gangbiRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            IsHighlighterInk = false;
        }

        private void yingguangbiRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            IsHighlighterInk = true;
        }

        private void clearRadioButton_Click(object sender, RoutedEventArgs e)
        {
            EditingModeInk = InkCanvasEditingMode.EraseByStroke;
        }

        private void writeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            EditingModeInk = InkCanvasEditingMode.Ink;
        }

        private void clearRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            EditingModeInk = InkCanvasEditingMode.EraseByStroke;
        }

        private void writeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            EditingModeInk = InkCanvasEditingMode.Ink;
        }

        private void gangbiRadioButton_Click(object sender, RoutedEventArgs e)
        {
            IsHighlighterInk = false;
        }

        private void yingguangbiRadioButton_Click(object sender, RoutedEventArgs e)
        {
            IsHighlighterInk = true;
        }
        //-------------------------属性----------------------------------------------------

        public Color ColorInk
        {
            get { return color; }
            set
            {
                color = value;
                ChangeColorInk?.Invoke(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorInk"));
            }
        }

        public double WidthInk
        {
            get { return slider_double; }
            set
            {
                slider_double = value;
                ChangeWidthInk?.Invoke(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WidthInk"));
            }
        }

        public bool IsHighlighterInk
        {
            get { return isHighlighter; }
            set
            {
                
                isHighlighter = value;
                ChangeIsHighlighterInk?.Invoke(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsHighlighterInk"));
            }
        }

        public InkCanvasEditingMode EditingModeInk
        {
            get { return editingMode; }
            set
            {
                editingMode = value;
                ChangeEditingModeInk?.Invoke(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EditingMode"));
            }
        }

        //-------------------------关闭控件----------------------------------------------------

        public void UserControl_Closing()
        {
            //将画笔信息写入注册表
            if (name == "Ink0")
            {
                if (gangbiRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_Bi", "Gangbi");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_Bi", "Yinguangbi");
                }

                if (writeRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_EditingMode", "Write");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_EditingMode", "Clear");
                }

                string slider_string = slider.Value.ToString();
                scienceResearchKey.SetValue("Ink_Slider", slider_string);

                if (lstbox != null)
                {
                    color = lstbox.SelectedColor;
                    string color_string = color.ToString();
                    scienceResearchKey.SetValue("Ink_Color", color_string);
                }
            }
            else if (name == "Ink1")
            {
                if (gangbiRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_Bi_1", "Gangbi");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_Bi_1", "Yinguangbi");
                }

                if (writeRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_1", "Write");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_1", "Clear");
                }

                string slider_string = slider.Value.ToString();
                scienceResearchKey.SetValue("Ink_Slider_1", slider_string);

                if (lstbox != null)
                {
                    color = lstbox.SelectedColor;
                    string color_string = color.ToString();
                    scienceResearchKey.SetValue("Ink_Color_1", color_string);
                }
            }
            else if (name == "Ink2")
            {
                if (gangbiRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_Bi_2", "Gangbi");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_Bi_2", "Yinguangbi");
                }

                if (writeRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_2", "Write");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_2", "Clear");
                }

                string slider_string = slider.Value.ToString();
                scienceResearchKey.SetValue("Ink_Slider_2", slider_string);

                if (lstbox != null)
                {
                    color = lstbox.SelectedColor;
                    string color_string = color.ToString();
                    scienceResearchKey.SetValue("Ink_Color_2", color_string);
                }
            }
            else if (name == "Ink3")
            {
                if (gangbiRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_Bi_3", "Gangbi");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_Bi_3", "Yinguangbi");
                }

                if (writeRadioButton.IsChecked == true)
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_3", "Write");
                }
                else
                {
                    scienceResearchKey.SetValue("Ink_EditingMode_3", "Clear");
                }

                string slider_string = slider.Value.ToString();
                scienceResearchKey.SetValue("Ink_Slider_3", slider_string);

                if (lstbox != null)
                {
                    color = lstbox.SelectedColor;
                    string color_string = color.ToString();
                    scienceResearchKey.SetValue("Ink_Color_3", color_string);
                }
            }
        }
    }
}
