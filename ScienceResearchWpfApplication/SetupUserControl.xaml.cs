using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// SetupUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class SetupUserControl : UserControl
    {
        string paperInkOrTextString;
        string dataBaseInkOrTextString;
        /// <summary>
        /// 文章写作标签页字体
        /// </summary>
        public FontFamily fontFamily_PaperUserControl_Scroll;
        /// <summary>
        /// 文章写作标签页字号
        /// </summary>
        public double fontsize_PaperUserControl_Scroll;

        RegistryKey scienceResearchKey;        

        /// <summary>
        /// 构造函数
        /// </summary>
        public SetupUserControl()
        {
            InitializeComponent();

            scienceResearchKey = MainWindow.scienceResearchKey;
            paperInkOrTextString = scienceResearchKey.GetValue("PaperInkOrText").ToString();
            if (paperInkOrTextString == "Text")
                textRadioButton.IsChecked = true;
            else
                inkRadioButton.IsChecked = true;

            dataBaseInkOrTextString = scienceResearchKey.GetValue("DataBaseInkOrText").ToString();
            if (dataBaseInkOrTextString == "Text")
                textRadioButton_db.IsChecked = true;
            else
                inkRadioButton_db.IsChecked = true;

            fontsize_PaperUserControl_Scroll = double.Parse(scienceResearchKey.GetValue("Fontsize_PaperUserControl_Scroll").ToString());
            fontFamily_PaperUserControl_Scroll = new FontFamily(scienceResearchKey.GetValue("FontFamily_PaperUserControl_Scroll").ToString());

            fontSelectUserControl.fontComboBox.SelectionChanged += FontComboBox_SelectionChanged;
            fontSelectUserControl.sizeComboBox.SelectionChanged += SizeComboBox_SelectionChanged;

            inkProperties1.NameInk = "Ink1";
            inkProperties2.NameInk = "Ink2";
            inkProperties3.NameInk = "Ink3";

            string left = scienceResearchKey.GetValue("ScreenShot_Left").ToString();
            string right = scienceResearchKey.GetValue("ScreenShot_Right").ToString();
            screenLeftTextBox.Text = left;
            screenRightTextBox.Text = right;

            //normalRadioButton.IsChecked = true;

            string yd_cz_width_str = scienceResearchKey.GetValue("yd_cz_width").ToString();
            yd_cz_width_TextBox.Text = yd_cz_width_str;
            string yd_ck_width_str = scienceResearchKey.GetValue("yd_ck_width").ToString();
            yd_ck_width_TextBox.Text = yd_ck_width_str;

            if (MainWindow.txt_file_encoding == "ASCII")
                ASCIIRadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "BigEndianUnicode")
                BigEndianUnicodeRadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "Unicode")
                UnicodeRadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "UTF32")
                UTF32RadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "UTF7")
                UTF7RadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "UTF8")
                UTF8RadioButton.IsChecked = true;
            if (MainWindow.txt_file_encoding == "Default")
                DefaultRadioButton.IsChecked = true;

            //模式匹配设置初值
            if (ModeSetup.shibieDictionary["isGjcShibie"] == "是")
                gjcShibieCheckBox.IsChecked = true;
            if (ModeSetup.shibieDictionary["isDcShibie"] == "是")
                dcShibieCheckBox.IsChecked = true;
            if (ModeSetup.shibieDictionary["isDyShibie"] == "是")
                dyShibieCheckBox.IsChecked = true;
            if (ModeSetup.shibieDictionary["isJxShibie"] == "是")
                jxShibieCheckBox.IsChecked = true;
            if (ModeSetup.shibieDictionary["isYdShibie"] == "是")
                ydShibieCheckBox.IsChecked = true;
            if (ModeSetup.shibieDictionary["isWzShibie"] == "是")
                wzShibieCheckBox.IsChecked = true;

            if (ModeSetup.pipeiDictionary["is_gjc_czwz_pipei"] == "是")
                gjc_czwz_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_dc_yd_pipei"] == "是")
                dc_yd_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_dc_ckwz_pipei"] == "是")
                dc_ckwz_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_dc_xps_pipei"] == "是")
                dc_xps_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_dy_yd_pipei"] == "是")
                dy_yd_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_dy_ckwz_pipei"] == "是")
                dy_ckwz_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_jx_ckwz_pipei"] == "是")
                jx_ckwz_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_yd_ckwz_pipei"] == "是")
                yd_ckwz_PipeiCheckBox.IsChecked = true;
            if (ModeSetup.pipeiDictionary["is_wz_ckwz_pipei"] == "是")
                wz_ckwz_PipeiCheckBox.IsChecked = true;


        }

        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fontFamily_PaperUserControl_Scroll = fontSelectUserControl.SelectedFontFamily;
            fontsize_PaperUserControl_Scroll = fontSelectUserControl.SelectedFontSize;
            scienceResearchKey.SetValue("FontFamily_PaperUserControl_Scroll", fontFamily_PaperUserControl_Scroll.Source);
            scienceResearchKey.SetValue("Fontsize_PaperUserControl_Scroll", fontsize_PaperUserControl_Scroll.ToString());
        }

        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fontFamily_PaperUserControl_Scroll = fontSelectUserControl.SelectedFontFamily;
            fontsize_PaperUserControl_Scroll = fontSelectUserControl.SelectedFontSize;
            scienceResearchKey.SetValue("FontFamily_PaperUserControl_Scroll", fontFamily_PaperUserControl_Scroll.Source);
            scienceResearchKey.SetValue("Fontsize_PaperUserControl_Scroll", fontsize_PaperUserControl_Scroll.ToString());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fontSelectUserControl.setSelected(fontsize_PaperUserControl_Scroll, fontFamily_PaperUserControl_Scroll);
        }
        //----------------------按钮--------------------------------------------------------------------------------

        private void textRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paperInkOrTextString = "Text";
            scienceResearchKey.SetValue("PaperInkOrText", paperInkOrTextString);
        }

        private void inkRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paperInkOrTextString = "Ink";
            scienceResearchKey.SetValue("PaperInkOrText", paperInkOrTextString);
        }

        private void textRadioButton_db_Checked(object sender, RoutedEventArgs e)
        {
            dataBaseInkOrTextString = "Text";
            scienceResearchKey.SetValue("DataBaseInkOrText", dataBaseInkOrTextString);
        }

        private void inkRadioButton_db_Checked(object sender, RoutedEventArgs e)
        {
            dataBaseInkOrTextString = "Ink";
            scienceResearchKey.SetValue("DataBaseInkOrText", dataBaseInkOrTextString);
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }

        private void fzCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_fz.pizuCheckBox_Checked();
        }

        private void fzCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_fz.pizuCheckBox_UnChecked();
        }

        private void wjwzCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_wjwz.pizuCheckBox_Checked();
        }

        private void wjwzCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_wjwz.pizuCheckBox_UnChecked();
        }

        private void gjcCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_gjc.pizuCheckBox_Checked();
        }

        private void gjcCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_gjc.pizuCheckBox_UnChecked();
        }

        private void dcCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_dc.pizuCheckBox_Checked();
        }

        private void dcCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_dc.pizuCheckBox_UnChecked();
        }

        private void dyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_dy.pizuCheckBox_Checked();
        }

        private void dyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_dy.pizuCheckBox_UnChecked();
        }

        private void jxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_jx.pizuCheckBox_Checked();
        }

        private void jxCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_jx.pizuCheckBox_UnChecked();
        }

        private void ydCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_yd.pizuCheckBox_Checked();
        }

        private void ydCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_yd.pizuCheckBox_UnChecked();
        }

        private void wzCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_wz.pizuCheckBox_Checked();
        }

        private void wzCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_wz.pizuCheckBox_UnChecked();
        }

        private void xmCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_xm.pizuCheckBox_Checked();
        }

        private void xmCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_xm.pizuCheckBox_UnChecked();
        }

        private void tpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_tp.pizuCheckBox_Checked();
        }

        private void tpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_tp.pizuCheckBox_UnChecked();

        }

        private void tpczCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_tpcz.pizuCheckBox_Checked();
        }

        private void tpczCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_tpcz.pizuCheckBox_UnChecked();
        }


        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            scienceResearchKey.SetValue("ScreenShot_Left", screenLeftTextBox.Text);
            scienceResearchKey.SetValue("ScreenShot_Right", screenRightTextBox.Text);

            string left = screenLeftTextBox.Text;
            string[] arrTemp = left.Split(',');
            MainWindow.left_l = int.Parse(arrTemp[0]);
            MainWindow.right_l = int.Parse(arrTemp[1]);
            MainWindow.top_l = int.Parse(arrTemp[2]);
            MainWindow.bottem_l = int.Parse(arrTemp[3]);

            string right = screenRightTextBox.Text;
            string[] arrTemp2 = right.Split(',');
            MainWindow.left_r = int.Parse(arrTemp2[0]);
            MainWindow.right_r = int.Parse(arrTemp2[1]);
            MainWindow.top_r = int.Parse(arrTemp2[2]);
            MainWindow.bottem_r = int.Parse(arrTemp2[3]);
        }

        private void leftTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (leftTextBox.Text != "")
            {
                MainWindow.mainWindow.grid.ColumnDefinitions[0].Width = new GridLength(double.Parse(leftTextBox.Text));
                MainWindow.mainWindow.grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private void rightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rightTextBox.Text != "")
            {
                MainWindow.mainWindow.grid.ColumnDefinitions[1].Width = new GridLength(double.Parse(rightTextBox.Text));
                MainWindow.mainWindow.grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private void yanZhenButton_Click(object sender, RoutedEventArgs e)
        {
            string mac_string = macTextBox.Text;
            string register_string = yanzhenTextBox.Text;

            string mac_recode = MainWindow.decode_sr(register_string);
            if (mac_string == mac_recode)
            {
                MessageBox.Show("注册成功！");
                registerCodeTextBlock.Text = "6 注册码（已注册）";
                scienceResearchKey.SetValue("SV001", yanzhenTextBox.Text);
                yanZhenButton.IsEnabled = false;

                MainWindow.mainWindow.jiechu_xianzhi_use();


            }
            else
            {
                MessageBox.Show("注册码不正确！");
                registerCodeTextBlock.Text = "6 注册码（还未进行注册）";
            }


        }

        private void yd_cz_width_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("yd_cz_width", yd_cz_width_TextBox.Text);
            MainWindow.yd_cz_width = double.Parse(yd_cz_width_TextBox.Text);

        }

        private void yd_ck_width_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("yd_ck_width", yd_ck_width_TextBox.Text);
            MainWindow.yd_ck_width = double.Parse(yd_ck_width_TextBox.Text);
        }

        private void ASCIIRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "ASCII");
            MainWindow.txt_file_encoding = "ASCII";
        }

        private void BigEndianUnicodeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "BigEndianUnicode");
            MainWindow.txt_file_encoding = "BigEndianUnicode";
        }

        private void UnicodeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "Unicode");
            MainWindow.txt_file_encoding = "Unicode";
        }

        private void UTF32RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "UTF32");
            MainWindow.txt_file_encoding = "UTF32";
        }

        private void UTF7RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "UTF7");
            MainWindow.txt_file_encoding = "UTF7";
        }

        private void UTF8RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "UTF8");
            MainWindow.txt_file_encoding = "UTF8";
        }

        private void DefaultRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            scienceResearchKey.SetValue("txt_file_encoding", "Default");
            MainWindow.txt_file_encoding = "Default";
        }

        private void gjcShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isGjcShibie", "是");
            ModeSetup.shibieDictionary["isGjcShibie"] = "是";
        }        

        private void dcShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isDcShibie", "是");
            ModeSetup.shibieDictionary["isDcShibie"] = "是";
        }

        private void dyShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isDyShibie", "是");
            ModeSetup.shibieDictionary["isDyShibie"] = "是";
        }

        private void jxShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isJxShibie", "是");
            ModeSetup.shibieDictionary["isJxShibie"] = "是";
        }

        private void ydShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isYdShibie", "是");
            ModeSetup.shibieDictionary["isYdShibie"] = "是";
        }

        private void wzShibieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isWzShibie", "是");
            ModeSetup.shibieDictionary["isWzShibie"] = "是";
        }

        private void gjcShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isGjcShibie", "否");
            ModeSetup.shibieDictionary["isGjcShibie"] = "否";
        }

        private void dcShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isDcShibie", "否");
            ModeSetup.shibieDictionary["isDcShibie"] = "否";
        }

        private void dyShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isDyShibie", "否");
            ModeSetup.shibieDictionary["isDyShibie"] = "否";
        }

        private void jxShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isJxShibie", "否");
            ModeSetup.shibieDictionary["isJxShibie"] = "否";
        }

        private void ydShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isYdShibie", "否");
            ModeSetup.shibieDictionary["isYdShibie"] = "否";
        }

        private void wzShibieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("isWzShibie", "否");
            ModeSetup.shibieDictionary["isWzShibie"] = "否";
        }

        private void gjc_czwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_gjc_czwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_gjc_czwz_pipei"] = "是";
        }

        private void dc_yd_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_dc_yd_pipei", "是");
            ModeSetup.pipeiDictionary["is_dc_yd_pipei"] = "是";
        }

        private void dc_ckwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_dc_ckwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_dc_ckwz_pipei"] = "是";
        }

        private void dc_xps_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_dc_xps_pipei", "是");
            ModeSetup.pipeiDictionary["is_dc_xps_pipei"] = "是";
        }

        private void dy_yd_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_dy_yd_pipei", "是");
            ModeSetup.pipeiDictionary["is_dy_yd_pipei"] = "是";
        }

        private void dy_ckwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_dy_ckwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_dy_ckwz_pipei"] = "是";
        }

        private void jx_ckwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_jx_ckwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_jx_ckwz_pipei"] = "是";
        }

        private void yd_ckwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_yd_ckwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_yd_ckwz_pipei"] = "是";
        }

        private void wz_ckwz_PipeiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.scienceResearchKey.SetValue("is_wz_ckwz_pipei", "是");
            ModeSetup.pipeiDictionary["is_wz_ckwz_pipei"] = "是";
        }

        
    }
}
