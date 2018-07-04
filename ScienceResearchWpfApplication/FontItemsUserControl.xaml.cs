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

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// ItemsUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class FontItemsUserControl : UserControl
    {
        //字体选择框中每一项的文字和字体

        public FontFamily Font;
        public FontItemsUserControl(FontFamily font)
        {
            InitializeComponent();
            Font = font;
            LanguageSpecificStringDictionary fontFamilyNames = font.FamilyNames;
            if (fontFamilyNames.ContainsKey(System.Windows.Markup.XmlLanguage.GetLanguage("zh-cn")))
            {
                string fontName = null;
                if (fontFamilyNames.TryGetValue(System.Windows.Markup.XmlLanguage.GetLanguage("zh-cn"), out fontName))
                {
                    text.FontFamily = font;
                    text.Text = fontName;
                }
            }
            else
            {
                text.FontFamily = font;
                text.Text = font.Source;
            }
        }
    }
}
