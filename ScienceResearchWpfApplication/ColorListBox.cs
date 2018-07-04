using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Reflection;

namespace ScienceResearchWpfApplication
{

    public class ColorListBoxItem : ListBoxItem
    {
        string str;
        Rectangle rect;      //左侧的颜色方块
        TextBlock text;      //对应的文字

        public ColorListBoxItem()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            Content = stack;

            //添加颜色方块
            rect = new Rectangle();
            rect.Width = 16;
            rect.Height = 16;
            rect.Margin = new System.Windows.Thickness(2);
            rect.Stroke = SystemColors.WindowTextBrush;
            stack.Children.Add(rect);

            //添加TextBlock
            text = new TextBlock();
            text.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(text);
        }


        public string Text
        {
            set
            {
                //在组合词中间添加空格
                str = value;
                string strSpaced = str[0].ToString();
                for (int i = 1; i < str.Length; i++)
                {
                    strSpaced += (Char.IsUpper(str[i]) ? " " : "") + str[i].ToString();
                }
                text.Text = strSpaced;
                //text.Text = value;
            }
            get { return str; }
        }

        //设置方块颜色
        public Color Color
        {
            set { rect.Fill = new SolidColorBrush(value); }
            get
            {
                SolidColorBrush brush = rect.Fill as SolidColorBrush;
                return brush == null ? Colors.Transparent : brush.Color;
            }
        }

        //当选中某一项的时候出发
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            text.FontWeight = FontWeights.Bold;
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            text.FontWeight = FontWeights.Regular;
        }
        public override string ToString()
        {
            return str;
        }

    }

    public class ColorListBox : ListBox
    {
        public ColorListBox()
        {
            PropertyInfo[] props = typeof(Colors).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                //根据Colors内颜色的个数创建ColorListBoxItem
                ColorListBoxItem item = new ColorListBoxItem();
                item.Text = prop.Name;
                item.Color = (Color)prop.GetValue(null, null);
                Items.Add(item);
            }

            SelectedValuePath = "Color";
        }

        //获取或设置当前选中的颜色
        public Color SelectedColor
        {
            set { SelectedValue = value; }
            get { return (null != SelectedValue) ? (Color)SelectedValue : Colors.Transparent; }
        }
    }

    public class ColorComboBox : ComboBox
    {
        public ColorComboBox()
        {
            PropertyInfo[] props = typeof(Colors).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                //根据Colors内颜色的个数创建ColorListBoxItem
                ColorListBoxItem item = new ColorListBoxItem();
                item.Text = prop.Name;
                item.Color = (Color)prop.GetValue(null, null);
                Items.Add(item);
            }

            SelectedValuePath = "Color";
        }

        //获取或设置当前选中的颜色
        public Color SelectedColor
        {
            set { SelectedValue = value; }
            get { return (null != SelectedValue) ? (Color)SelectedValue : Colors.Transparent; }
        }

    }

}
