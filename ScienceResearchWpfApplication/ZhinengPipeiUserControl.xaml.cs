using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ScienceResearchWpfApplication.TextManage;
using ScienceResearchWpfApplication.Xps;

#region ScienceResearchWpfApplication
namespace ScienceResearchWpfApplication.AI
{
    #region ZhinengPipeiUserControl
    /// <summary>
    /// ZhinengPipeiUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ZhinengPipeiUserControl : UserControl
    {
        #region 变量

        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;

        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;

        public static ScienceResearchDataSetNew.单词DataTable dc_dt;
        ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;

        public static ScienceResearchDataSetNew.短语DataTable dy_dt;
        ScienceResearchDataSetNewTableAdapters.短语TableAdapter dy_ta;

        ScienceResearchDataSetNew.短语_关键词DataTable dy_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.短语_关键词TableAdapter dy_gjc_ta;

        public static ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;

        public string type;     //类型，"paper"——文章，"refer"——参考
        //文章所用变量
        public StackPanel paperStackPanel;
        public TextboxInkcavasUserControl textboxInkcavasUserControl_using;
        public ReferUserControl referUserControl_using;
        public DataGrid keywordDataGrid2;


        //参考所用变量
        public TabControl referTabControl;
        public ReferTabItemUserControl referTabItemUserControl;

        private bool isTooLong = false;  
        int new_yd_id;

        /// <summary>
        /// 文件是否太长
        /// </summary>
        public bool IsTooLong
        {
            get
            {
                return isTooLong;
            }

            set
            {
                isTooLong = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 智能匹配控件的构造函数
        /// </summary>
        public ZhinengPipeiUserControl()
        {
            InitializeComponent();
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;

            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            dy_dt = MainWindow.dy_dt;
            dy_ta = MainWindow.dy_ta;
            dy_gjc_dt = MainWindow.dy_gjc_dt;
            dy_gjc_ta = MainWindow.dy_gjc_ta;

            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            //shoujiComboBox.SelectedItem = lblDc;

            allRadioButton.IsChecked = true;
        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
                new_yd_id = newID;
            }
        }
        #endregion

        #region 模式收集
        /// <summary>
        /// 标识单词，为收集做准备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBiaoshi_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox rtb = textboxInkcavasUserControl_using.paragraphRichTextBox;
            TextProcessClass.SelectionChangeColor(rtb, ColorManager.color_biaoshi);
        }

        /// <summary>
        /// 取消标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQingbiao_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox rtb = textboxInkcavasUserControl_using.paragraphRichTextBox;
            TextProcessClass.SelectionChangeColor(rtb, ColorManager.color_zhengwen);
        }

        private List<string> get_elements_in_string(TextPointer tp_start, TextPointer tp_end, Color color)
        {
            //获取选择区域内部指定颜色的所有单词和短语

            List<string> dc_list = new List<string>();
            TextPointer tp1 = tp_start;
            TextPointer tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
            string color_mode_str = color.ToString();
            string color_select_str;

            while (tp1 != tp_end)
            {
                if (tp2 == null)
                    break;
                TextRange range_char = new TextRange(tp1, tp2);
                color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                if (color_select_str == color_mode_str)
                {
                    TextPointer tp_dc_start = tp1;
                    TextPointer tp_dc_end;
                    while (color_select_str == color_mode_str)
                    {
                        tp1 = tp2;
                        tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                        if (tp2 == null)
                            break;
                        range_char = new TextRange(tp1, tp2);
                        color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                    }

                    tp_dc_end = tp1;
                    TextRange range_word = new TextRange(tp_dc_start, tp_dc_end);
                    string word = range_word.Text;
                    dc_list.Add(word);
                }
                else
                {
                    tp1 = tp2;
                    tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                }
            }

            return dc_list;
        }

        private string get_type(string input_str)
        {
            //获取输入字符串的类型
            string output_type = "";
            //------------------------------------------------------------------
            //有两个及以上的语段为篇章
            int index = input_str.IndexOf("\n");
            if (index != -1 || input_str == "")
                output_type = "文章";
            else
            {
                //有两个及以上句号、感叹号或者问号的字符串为段落
                Regex regImg = new Regex("[.?!。？！]", RegexOptions.IgnoreCase);
                MatchCollection matches = regImg.Matches(input_str);
                int count = matches.Count;
                if (count >= 2)
                    output_type = "语段";
                else if (count == 1)        ////有一个句号、感叹号或者问号的字符串为句型
                    output_type = "句型";
                else
                {
                    //有空格的是短语，没有空格的是单词
                    int index2 = input_str.IndexOf(" ");
                    if (index2 != -1)
                        output_type = "短语";
                    else
                        output_type = "单词";
                }
            }
            return output_type;
        }

        /// <summary>
        /// 收集单词、短语、句型、语段、文章等语言要素作为模式。可以使用这些模式，进行模式匹配，从而寻找到参考资料。
        /// 对单词和短语进行精准识别和匹配，对句型、语段、文章进行模糊识别和匹配。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShouji_Click(object sender, RoutedEventArgs e)
        {            
            RichTextBox rtb = new RichTextBox();
            if (type == "paper")
                rtb = textboxInkcavasUserControl_using.paragraphRichTextBox;
            else if (type == "refer")
                rtb = referTabItemUserControl.richTextBox;

            TextSelection textSelection = rtb.Selection;
            string selected_str = textSelection.Text;

            string str_type = get_type(selected_str);

            //----------------------保存语言要素到数据库-------------------------------------------
            if (str_type == "单词")
            {
                //shouji_dc(selected_str);
                save_element_in_database(dc_dt, dc_ta.Adapter, "单词", selected_str);

            }
            else if (str_type == "短语")
            {
                save_element_in_database(dy_dt, dy_ta.Adapter, "短语", selected_str);
            }
            else if (str_type == "句型")
            {



            }
            else if (str_type == "语段")
            {
                //获取选中区域所有单词和短语；创建新的单词或短语；创建新的语段，分类为模式
                TextPointer tpStart = textSelection.Start;
                TextPointer tpEnd = textSelection.End;
                List<string> dc_list = get_elements_in_string(tpStart, tpEnd, ColorManager.color_biaoshi);
                string output = selected_str + "{";
                for (int i = 0; i < dc_list.Count; i++)
                    output = output + dc_list[i] + ",";
                output = output.Remove(output.Length - 1, 1);
                output = output + "}";
                save_element_in_database(yd_dt, yd_ta.Adapter, "语段", output);
            }
            else if (str_type == "文章")
            {

            }

            //----------------------模式识别-------------------------------------------
            if (type == "paper")
                shibie_control();
            //shibie_rtb(rtb);
            else if (type == "refer")
                shibie_rtb(rtb);

        }

        /// <summary>
        /// 将单词、短语、句型、语段、文章等各种语言要素保存到数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="da"></param>
        /// <param name="jilu">记录，为单词、短语、句型、语段、文章等</param>
        /// <param name="selected_str"></param>
        public void save_element_in_database(DataTable dt, OleDbDataAdapter da, string jilu, string selected_str)
        {           
            DataRow dr2 = dt.NewRow();
            dr2[jilu] = selected_str;
            dr2["分类"] = modeFenleiTextBox.Text;
            dt.Rows.Add(dr2);
            try
            {
                da.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dt.Rows.Remove(dr2);
                return;
            }
        }
        #endregion

        #region 模式识别
        /// <summary>  
        /// 对富文本框中的单词进行识别
        /// </summary>  
        private void shibie_rtb_dc(RichTextBox rtb)
        {
            //TextRange range = rtb.Selection;
            //TextPointer tp1 = range.Start;
            //TextPointer tp2 = range.End;

            for (int i = 0; i < dc_dt.Rows.Count; i++)
            {
                ScienceResearchDataSetNew.单词Row dc = (ScienceResearchDataSetNew.单词Row)dc_dt.Rows[i];
                string dc_str = dc.单词;
                string fenlei_str = dc.分类;
                if (allRadioButton.IsChecked == true)
                    TextProcessClass.ChangeColor(ColorManager.color_mode_word, rtb, dc_str, "前景");
                else if (fenleiRadioButton.IsChecked == true && modeFenleiTextBox.Text == fenlei_str)
                    TextProcessClass.ChangeColor(ColorManager.color_mode_word, rtb, dc_str, "前景");
                else if (keywordRadioButton.IsChecked == true)
                {
                    List<ScienceResearchDataSetNew.关键词Row> currentGjcAndSons = MainWindow.keywordTreeUserControl.currentGjcAndSons;
                    foreach (ScienceResearchDataSetNew.单词_关键词Row dc_gjc in dc.Get单词_关键词Rows())
                    {
                        if (currentGjcAndSons.Contains(dc_gjc.关键词Row))
                        {
                            TextProcessClass.ChangeColor(ColorManager.color_mode_word, rtb, dc_str, "前景");
                            break;
                        }
                    }
                }
            }                        
            
            //range.Select(tp1, tp2);
        }

        /// <summary>  
        /// 对富文本框中的短语进行识别
        /// </summary>
        private void shibie_rtb_dy(RichTextBox rtb)
        {
            for (int i = 0; i < dy_dt.Rows.Count; i++)
            {
                ScienceResearchDataSetNew.短语Row dy = (ScienceResearchDataSetNew.短语Row)dy_dt.Rows[i];
                string dy_str = dy.短语;

                string fenlei_str = dy.分类;
                if (allRadioButton.IsChecked == true)
                    TextProcessClass.ChangeColor(ColorManager.color_mode_duanyu, rtb, dy_str, "背景");
                else if (fenleiRadioButton.IsChecked == true && modeFenleiTextBox.Text == fenlei_str)
                    TextProcessClass.ChangeColor(ColorManager.color_mode_duanyu, rtb, dy_str, "背景");
                else if (keywordRadioButton.IsChecked == true)
                {
                    List<ScienceResearchDataSetNew.关键词Row> currentGjcAndSons = MainWindow.keywordTreeUserControl.currentGjcAndSons;
                    foreach (ScienceResearchDataSetNew.短语_关键词Row dy_gjc in dy.Get短语_关键词Rows())
                    {
                        if (currentGjcAndSons.Contains(dy_gjc.关键词Row))
                        {
                            TextProcessClass.ChangeColor(ColorManager.color_mode_duanyu, rtb, dy_str, "背景");
                            break;
                        }
                    }
                }
                
                //TextProcessClass.XiaHuaXian(rtb, dy_str);
            }
        }

        /// <summary>  
        /// 对一段话的语段模式进行识别，并且返回相应的模式序列
        /// </summary>
        /// <param name="para_str">输入语段的字符串</param>  
        public static List<int> shibie_para_yd(string para_str)
        {
            var yd_mode_list = (from yd in yd_dt
                                where yd.分类 == "模式"
                                select yd).ToList();
            int yd_mode_count = yd_mode_list.Count;
            List<int> yd_mode_int_list = new List<int>();

            //-----------------对每一语段模式进行判断------------------------------
            for (int i = 0; i < yd_mode_count; i++)
            {
                ScienceResearchDataSetNew.语段Row yd = yd_mode_list[i];
                string yd_str = yd.语段;
                Regex regImg = new Regex("{.{1,10000}}", RegexOptions.IgnoreCase);
                MatchCollection matches = regImg.Matches(yd_str);
                string yd_mode_array_str = matches[matches.Count - 1].Value;
                yd_mode_array_str = yd_mode_array_str.Substring(1, yd_mode_array_str.Length - 2);
                string[] yd_mode_array = yd_mode_array_str.Split(new char[1] { ',' });

                //如果para_str包含yd_mode_array中的单词达到一定比例，则可称该语段拥有该模式
                int count = 0;
                int yd_mode_array_count = yd_mode_array.Count();
                for (int j = 0; j < yd_mode_array_count; j++)
                    if (para_str.IndexOf(yd_mode_array[j]) != -1)
                        count++;

                if ((double)count / yd_mode_array_count >MainWindow.yd_mode_rate)
                    yd_mode_int_list.Add(yd.ID);

            }

            return yd_mode_int_list;
        }
        /// <summary>
        /// 获取第n个zhifu的结束位置
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="zhifu"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static TextPointer get_tp(RichTextBox rtb, string zhifu, int n)
        {           
            TextPointer tpStart = rtb.Document.ContentStart;
            if (n == 0)
                return tpStart;
            TextPointer tpEnd = rtb.Document.ContentEnd;
            TextPointer tp1 = tpStart;
            TextPointer tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
            TextRange range_char = new TextRange(tp1, tp2);
            int count = 0;

            while (tp2 != tpEnd)
            {
                tp1 = tp2;
                tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                range_char = new TextRange(tp1, tp2);

                if (zhifu == "\n")
                {
                    if (range_char.Text == "\n"|| range_char.Text == "\r\n")
                        count++;
                    if (count == n)
                        break;
                }
                else
                {
                    if (range_char.Text == zhifu)
                        count++;
                    if (count == n)
                        break;
                }

            }

            return tp2;
        }

        /// <summary>
        /// 清除语段模式标记
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="tp_biaoji_start"></param>
        private static void clear_yd_biaoji(RichTextBox rtb,TextPointer tp_biaoji_start)
        {
            TextPointer tp1 = tp_biaoji_start;
            TextPointer tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);

            Inline run=(Inline)tp2.Parent;
            tp2.Paragraph.Inlines.Remove(run);
        }

        /// <summary>
        /// 对富文本框中的语段进行识别
        /// </summary>
        /// <param name="rtb"></param>
        private void shibie_rtb_yd(RichTextBox rtb)
        {
            TextPointer tpStart = rtb.Document.ContentStart;
            TextPointer tpEnd = rtb.Document.ContentEnd;
            TextRange rang_rtb= new TextRange(tpStart, tpEnd);
            string string_rtb = rang_rtb.Text;

            //------------获取段落及其数目-------------------------------
            Regex regImg = new Regex("\n", RegexOptions.IgnoreCase);
            MatchCollection matches = regImg.Matches(string_rtb);
            int para_count = matches.Count;
            string[] para_array = string_rtb.Split(new char[1] {'\n'});

            //------------对每一段落进行识别-------------------------------
            List<List<int>> para_list=new List<List<int>>();
            for (int i = 0; i < para_count; i++)
            {
                string string_para = para_array[i];
                //para_list.Add(shibie_para_yd(string_para));
                List<int> mode_list = shibie_para_yd(string_para);

                //做标记
                if (mode_list.Count != 0)
                {
                    string mode_str = "[";
                    for (int j = 0; j < mode_list.Count; j++)
                        mode_str = mode_str + mode_list[j] + ",";
                    mode_str = mode_str.Substring(0, mode_str.Length - 1);
                    mode_str = mode_str + "]";

                    //清除原有标记
                    TextPointer tp_biaoji_start = get_tp(rtb, "\n", i);
                    clear_yd_biaoji(rtb,tp_biaoji_start);

                    //在语段前面插入标识字符串
                    Run r = new Run(mode_str, get_tp(rtb,"\n",i));
                    r.Background= new SolidColorBrush(ColorManager.color_yd_background);
                    //var xx=rtb.Selection.Start.Parent;
                }
            }
        }

        /// <summary>
        /// 关键词识别
        /// </summary>
        /// <param name="rtb"></param>
        private void shibie_rtb_gjc(RichTextBox rtb)
        {


        }

        /// <summary>
        /// 句型识别
        /// </summary>
        /// <param name="rtb"></param>
        private void shibie_rtb_jx(RichTextBox rtb)
        {


        }

        /// <summary>
        /// 文章识别
        /// </summary>
        /// <param name="rtb"></param>
        private void shibie_rtb_wz(RichTextBox rtb)
        {


        }

        /// <summary>
        /// 对富文本框进行识别
        /// </summary>
        /// <param name="rtb"></param>
        public void shibie_rtb(RichTextBox rtb)
        {
            if (ModeSetup.shibieDictionary["isGjcShibie"] == "是")
                shibie_rtb_gjc(rtb);
            if (ModeSetup.shibieDictionary["isDcShibie"]=="是")
                shibie_rtb_dc(rtb);
            if (ModeSetup.shibieDictionary["isDyShibie"] == "是")
                shibie_rtb_dy(rtb);
            if (ModeSetup.shibieDictionary["isJxShibie"] == "是")
                shibie_rtb_jx(rtb);
            if (ModeSetup.shibieDictionary["isYdShibie"] == "是")
                shibie_rtb_yd(rtb);
            if (ModeSetup.shibieDictionary["isWzShibie"] == "是")
                shibie_rtb_wz(rtb);
        }

        /// <summary>
        /// 对文章创作或者参考控件进行识别
        /// </summary>
        public void shibie_control()
        {
            if (type == "paper")
            {
                foreach (object t in paperStackPanel.Children)
                {
                    string name = t.GetType().FullName;
                    if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                    {
                        RichTextBox rtb = ((TextboxInkcavasUserControl)t).paragraphRichTextBox;
                        shibie_rtb(rtb);

                        //保存语段
                        TextboxInkcavasUserControl tb = (TextboxInkcavasUserControl)t;
                        tb.textboxLostFocus();
                    }
                }
            }
            else if (type == "refer")
            {
                RichTextBox rtb = referTabItemUserControl.richTextBox;
                shibie_rtb(rtb);
            }
        }

        private void btnShibie_Click(object sender, RoutedEventArgs e)
        {
            shibie_control();
        }
        #endregion

        #region 模式匹配
        private string get_pipei_type(RichTextBox rtb)
        {
            TextPointer tp_caret = rtb.CaretPosition;
            TextPointer tp1, tp2;
            tp1 = tp_caret;
            tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Backward);
            if (tp2 == null)
                return null;
            TextRange range_char = new TextRange(tp1, tp2);
            string color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
            string color_back_select_str = range_char.GetPropertyValue(TextElement.BackgroundProperty).ToString();
            string color_mode_dc_str = ColorManager.color_mode_word.ToString();
            string color_mode_dy_str = ColorManager.color_mode_duanyu.ToString();
            if (color_select_str == color_mode_dc_str)
                return "单词";
            else if (color_back_select_str == color_mode_dy_str)
                return "短语";
            else
                return "语段";
        }

        private void btnPipei_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox rtb = new RichTextBox();
            if (type == "paper")
                rtb = textboxInkcavasUserControl_using.paragraphRichTextBox;
            else if (type == "refer")
                rtb = referTabItemUserControl.richTextBox;

            string pipei_type = get_pipei_type(rtb);

            //----------------------单词-------------------------------------------
            if (pipei_type=="单词")
            {
                TextPointer tp_caret = rtb.CaretPosition;
                TextPointer tp1, tp2;


                tp1 = tp_caret;
                tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Backward);
                if (tp2 == null)
                    return;
                TextRange range_char = new TextRange(tp1, tp2);

                string color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                string color_mode_dc_str = ColorManager.color_mode_word.ToString();

                while (color_select_str == color_mode_dc_str)
                {
                    tp1 = tp2;
                    tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Backward);
                    if (tp2 == null)
                        break;
                    range_char = new TextRange(tp1, tp2);
                    color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                }
                TextPointer tp_start = tp1;

                tp1 = tp_caret;
                tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                if (tp2 == null)
                    return;
                range_char = new TextRange(tp1, tp2);
                color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                while (color_select_str == color_mode_dc_str)
                {
                    tp1 = tp2;
                    tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                    if (tp2 == null)
                        break;
                    range_char = new TextRange(tp1, tp2);
                    color_select_str = range_char.GetPropertyValue(TextElement.ForegroundProperty).ToString();
                }
                TextPointer tp_end = tp1;

                TextRange range_word = new TextRange(tp_start, tp_end);
                string danci = range_word.Text;

                pipei_dc(danci);
            }

            //----------------------短语-------------------------------------------
            //需要进行模式识别，寻找相似短语
            if (pipei_type == "短语")
            {
                if (ModeSetup.pipeiDictionary["is_dy_ckwz_pipei"] == "是")
                {
                    TextPointer tp_caret = rtb.CaretPosition;
                    TextPointer tp1, tp2;


                    tp1 = tp_caret;
                    tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Backward);
                    if (tp2 == null)
                        return;
                    TextRange range_char = new TextRange(tp1, tp2);

                    string color_select_str = range_char.GetPropertyValue(TextElement.BackgroundProperty).ToString();
                    string color_mode_dy_str = ColorManager.color_mode_duanyu.ToString();

                    while (color_select_str == color_mode_dy_str)
                    {
                        tp1 = tp2;
                        tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Backward);
                        if (tp2 == null)
                            break;
                        range_char = new TextRange(tp1, tp2);
                        color_select_str = range_char.GetPropertyValue(TextElement.BackgroundProperty).ToString();
                    }
                    TextPointer tp_start = tp1;

                    tp1 = tp_caret;
                    tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                    if (tp2 == null)
                        return;
                    range_char = new TextRange(tp1, tp2);
                    color_select_str = range_char.GetPropertyValue(TextElement.BackgroundProperty).ToString();
                    while (color_select_str == color_mode_dy_str)
                    {
                        tp1 = tp2;
                        tp2 = tp1.GetNextInsertionPosition(LogicalDirection.Forward);
                        if (tp2 == null)
                            break;
                        range_char = new TextRange(tp1, tp2);
                        color_select_str = range_char.GetPropertyValue(TextElement.BackgroundProperty).ToString();
                    }
                    TextPointer tp_end = tp1;

                    TextRange range_duanyu = new TextRange(tp_start, tp_end);
                    string duanyu = range_duanyu.Text;

                    pipei_dy(duanyu);
                }
            }




            //----------------------句型-------------------------------------------
            //需要进行模式识别，寻找相似句型
            if (pipei_type == "句型")
            {
                if (ModeSetup.pipeiDictionary["is_jx_ckwz_pipei"] == "是")
                {
                    //还未完成
                }
            }


            //----------------------语段-------------------------------------------
            //根据关键词，寻找同义模式
            //采用该部分中的单词和短语的一部分进行模糊匹配

            if (pipei_type == "语段")
            {
                if (ModeSetup.pipeiDictionary["is_yd_ckwz_pipei"] == "是")
                {
                    pipei_yd(2869);
                }
            }

            //----------------------文章-------------------------------------------
            //采用该部分中的单词和短语的一部分进行模糊匹配，不进行模式识别
            if (pipei_type == "文章")
            {
                if (ModeSetup.pipeiDictionary["is_wz_ckwz_pipei"] == "是")
                {
                    //还未完成
                }
            }
        }

        private void set_refer()
        {
            //用于设置referUserControl_using

            //if (referUserControl_using != referUserControl)
            MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.referTabItem;
            referUserControl_using = MainWindow.referUserControl;
            referUserControl_using.referTabControl.Items.Clear();
        }

        /// <summary>
        /// 根据yd_id寻找其关键词，根据关键词寻找其他同义模式，为每一模式进行匹配
        /// </summary>
        /// <param name="yd_id"></param>
        public void pipei_yd(int yd_id)
        {
            set_refer();

            //-----------------------------------------------------------------------
            //根据yd_id寻找其关键词，根据关键词寻找其他同义模式

            



            //-----------------------------------------------------------------------
            //获取模式对应的语言要素集合
            List<string> dc_list = new List<string>();
            dc_list.Add("semi-analytical");
            dc_list.Add("mobilities");

            YdPipeiReferTabItemUserControl ydReferTabItemUserControl = new YdPipeiReferTabItemUserControl(null, dc_list, yd_id);
            TabItem referTabItemTabItem = new TabItem();
            referTabItemTabItem.Header = yd_id;
            referTabItemTabItem.Content = ydReferTabItemUserControl;
            referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
            //ydReferTabItemUserControl.zhinengPipeiUserControl.shibie_control();
        }

        /// <summary>
        /// 获取同单词word对应的段图、段文和Xps文件
        /// </summary>
        /// <param name="danci"></param>
        public void pipei_dc(string danci)
        {
            if (danci != "")
            {
                set_refer();

                danci = danci.Replace("\r", "");
                danci = danci.Replace("\n", "");

                var dc_list = (from dc in dc_dt
                               where dc.单词 == danci
                               select dc).ToList();
                if (dc_list.Count == 0)
                {
                    MessageBox.Show("不存在单词:" + danci);
                    return;
                }

                var gjc_list = (from dc_gjc in dc_gjc_dt
                                where dc_gjc.单词ID == dc_list[0].ID
                                select dc_gjc.关键词Row).ToList();

                if (gjc_list.Count == 0)
                {
                    createReferTextXps(null, dc_list[0]);
                }
                else
                {
                    string gjc_output_str = "选中的单词含有以下关键词：";

                    for (int i = 0; i < gjc_list.Count; i++)
                    {
                        //对关键词对应的语段生成段图和段文
                        if (ModeSetup.pipeiDictionary["is_dc_yd_pipei"] == "是")
                        {
                            var yd_list = (from gjc in gjc_dt
                                           join yd_gjc in MainWindow.yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                                           join yd in MainWindow.yd_dt on yd_gjc.语段ID equals yd.ID
                                           where gjc.ID == gjc_list[i].ID
                                           select yd).ToList();
                            for (int j = 0; j < yd_list.Count; j++)
                            {
                                //段图
                                if (yd_list[j].图片 != "")
                                {
                                    ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
                                    ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                                    paraResourceClass_PaperDataGrid.creat_figure(paragraphFigureUserControl, yd_list[j]);

                                    TabItem referTabItemTabItem = new TabItem();
                                    referTabItemTabItem.Header = yd_list[j].ID;
                                    referTabItemTabItem.Content = paragraphFigureUserControl;
                                    referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                                }

                                //段文
                                if (yd_list[j].语段 != "")
                                {
                                    ReferUserControl referUserControl = new ReferUserControl("yd");
                                    ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                                    paraResourceClass_PaperDataGrid.creat_text(yd_list[j], referUserControl);

                                    TabItem referTabItemTabItem = new TabItem();
                                    referTabItemTabItem.Header = yd_list[j].ID;
                                    referTabItemTabItem.Content = referUserControl;
                                    referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                                }
                            }
                        }

                        //ReferText和Xps
                        gjc_output_str += gjc_list[i].关键词 + "，";
                        var dc_list2 = (from dc_gjc in dc_gjc_dt
                                        where dc_gjc.关键词ID == gjc_list[i].ID
                                        select dc_gjc.单词Row).ToList();
                        for (int j = 0; j < dc_list2.Count; j++)
                        {
                            //对该单词生成一个ReferText和Xps标签页
                            ScienceResearchDataSetNew.关键词Row gjc_page = gjc_list[i];
                            ScienceResearchDataSetNew.单词Row dc_page = dc_list2[j];
                            createReferTextXps(gjc_page, dc_page);
                        }

                        //对于文章创作标签页，跳参后要将关键词标注
                        if (type == "paper")
                        { 
                            //对keywordDataGrid2中的选项做标识
                            for (int j = 0; j < keywordDataGrid2.Items.Count; j++)
                            {
                                ScienceResearchDataSetNew.关键词Row drv = keywordDataGrid2.Items[j] as ScienceResearchDataSetNew.关键词Row;
                                string gjc = drv.关键词;
                                if (gjc == gjc_list[i].关键词)
                                {
                                    DataGridRow row = (DataGridRow)keywordDataGrid2.ItemContainerGenerator.ContainerFromIndex(j);
                                    row.Foreground = new SolidColorBrush(ColorManager.color_mode_word);
                                }
                            }
                        }
                    }
                }
                if (referUserControl_using.referTabControl.Items.Count > 0)
                    referUserControl_using.referTabControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 获取同短语对应的段图、段文
        /// </summary>
        /// <param name="duanyu"></param>
        public void pipei_dy(string duanyu)
        {
            if (duanyu != "")
            {
                set_refer();

                duanyu = duanyu.Replace("\r", "");
                duanyu = duanyu.Replace("\n", "");

                var dy_list = (from dy in dy_dt
                               where dy.短语 == duanyu
                               select dy).ToList();
                if (dy_list.Count == 0)
                {
                    MessageBox.Show("不存在单词:" + duanyu);
                    return;
                }

                var gjc_list = (from dy_gjc in dy_gjc_dt
                                where dy_gjc.短语ID == dy_list[0].ID
                                select dy_gjc.关键词Row).ToList();

                if (gjc_list.Count == 0)
                {
                    createReferText_dy(null, dy_list[0]);
                }
                else
                {
                    string gjc_output_str = "选中的短语含有以下关键词：";

                    for (int i = 0; i < gjc_list.Count; i++)
                    {
                        //对关键词对应的语段生成段图和段文
                        if (ModeSetup.pipeiDictionary["is_dy_yd_pipei"] == "是")
                        {
                            var yd_list = (from gjc in gjc_dt
                                           join yd_gjc in MainWindow.yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                                           join yd in MainWindow.yd_dt on yd_gjc.语段ID equals yd.ID
                                           where gjc.ID == gjc_list[i].ID
                                           select yd).ToList();
                            for (int j = 0; j < yd_list.Count; j++)
                            {
                                //段图
                                if (yd_list[j].图片 != "")
                                {
                                    ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
                                    ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                                    paraResourceClass_PaperDataGrid.creat_figure(paragraphFigureUserControl, yd_list[j]);

                                    TabItem referTabItemTabItem = new TabItem();
                                    referTabItemTabItem.Header = yd_list[j].ID;
                                    referTabItemTabItem.Content = paragraphFigureUserControl;
                                    referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                                }

                                //段文
                                if (yd_list[j].语段 != "")
                                {
                                    ReferUserControl referUserControl = new ReferUserControl("yd");
                                    ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                                    paraResourceClass_PaperDataGrid.creat_text(yd_list[j], referUserControl);

                                    TabItem referTabItemTabItem = new TabItem();
                                    referTabItemTabItem.Header = yd_list[j].ID;
                                    referTabItemTabItem.Content = referUserControl;
                                    referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                                }
                            }
                        }

                        //ReferText和Xps
                        gjc_output_str += gjc_list[i].关键词 + "，";
                        var dy_list2 = (from dy_gjc in dy_gjc_dt
                                        where dy_gjc.关键词ID == gjc_list[i].ID
                                        select dy_gjc.短语Row).ToList();
                        for (int j = 0; j < dy_list2.Count; j++)
                        {
                            //对该单词生成一个ReferText和Xps标签页
                            ScienceResearchDataSetNew.关键词Row gjc_page = gjc_list[i];
                            ScienceResearchDataSetNew.短语Row dy_page = dy_list2[j];
                            createReferText_dy(gjc_page, dy_page);
                        }

                        //对于文章创作标签页，跳参后要将关键词标注
                        if (type == "paper")
                        {
                            //对keywordDataGrid2中的选项做标识
                            for (int j = 0; j < keywordDataGrid2.Items.Count; j++)
                            {
                                ScienceResearchDataSetNew.关键词Row drv = keywordDataGrid2.Items[j] as ScienceResearchDataSetNew.关键词Row;
                                string gjc = drv.关键词;
                                if (gjc == gjc_list[i].关键词)
                                {
                                    DataGridRow row = (DataGridRow)keywordDataGrid2.ItemContainerGenerator.ContainerFromIndex(j);
                                    row.Foreground = new SolidColorBrush(ColorManager.color_mode_word);
                                }
                            }
                        }
                    }
                }
                if (referUserControl_using.referTabControl.Items.Count > 0)
                    referUserControl_using.referTabControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 生成ReferText和Xps标签页
        /// </summary>
        /// <param name="gjc_page"></param>
        /// <param name="dc_page"></param>
        private void createReferTextXps(ScienceResearchDataSetNew.关键词Row gjc_page, ScienceResearchDataSetNew.单词Row dc_page)
        {
            //ReferTabItem
            TabItem referTabItemTabItem;
            if (ModeSetup.pipeiDictionary["is_dc_ckwz_pipei"] == "是")
            {
                ReferTabItemUserControl referTabItemUserControl = new DcPipeiReferTabItemUserControl(gjc_page, dc_page);
                referTabItemTabItem = new TabItem();
                referTabItemTabItem.Header = dc_page.单词;
                referTabItemTabItem.Content = referTabItemUserControl;
                referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                referTabItemUserControl.zhinengPipeiUserControl.shibie_control();
            }


            //对该单词生成一个Xps控件
            if (ModeSetup.pipeiDictionary["is_dc_xps_pipei"] == "是")
            {
                XpsUserControl xpsUserControl = new XpsUserControl(gjc_page, dc_page);
                referTabItemTabItem = new TabItem();
                referTabItemTabItem.Header = dc_page.单词;
                referTabItemTabItem.Content = xpsUserControl;
                referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
            }
        }

        private void createReferText_dy(ScienceResearchDataSetNew.关键词Row gjc_page, ScienceResearchDataSetNew.短语Row dy_page)
        {
            TabItem referTabItemTabItem;
            if (ModeSetup.pipeiDictionary["is_dy_ckwz_pipei"] == "是")
            {
                ReferTabItemUserControl referTabItemUserControl = new DyPipeiReferTabItemUserControl(gjc_page, dy_page);
                referTabItemTabItem = new TabItem();
                referTabItemTabItem.Header = dy_page.短语;
                referTabItemTabItem.Content = referTabItemUserControl;
                referUserControl_using.referTabControl.Items.Add(referTabItemTabItem);
                referTabItemUserControl.zhinengPipeiUserControl.shibie_control();
            }
        }
        #endregion

        #region 模式清除
        private void btnQingchu_Click(object sender, RoutedEventArgs e)
        {
            //清除所有标志
            if (type == "paper")
            {
                foreach (object t in paperStackPanel.Children)
                {
                    string name = t.GetType().FullName;
                    if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                    {
                        RichTextBox rtb = ((TextboxInkcavasUserControl)t).paragraphRichTextBox;
                        TextRange range = rtb.Selection;
                        range.Select(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        qingchu(range);

                        //保存语段
                        TextboxInkcavasUserControl tb = (TextboxInkcavasUserControl)t;
                        tb.textboxLostFocus();
                    }
                }
            }
            else if (type == "refer")
            {
                if (isTooLong == false)
                {
                    RichTextBox rtb = referTabItemUserControl.richTextBox;
                    TextRange range = rtb.Selection;
                    range.Select(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                    qingchu(range);
                    range.Select(rtb.Document.ContentStart, rtb.Document.ContentStart);
                }
                else
                    MessageBox.Show("文件太大，不能清除");
            }
        }

        
        private void qingchu(TextRange range)
        {
            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
            range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Light);
            range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            //range.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
        }


        #endregion

        
    }

    #endregion

}
#endregion