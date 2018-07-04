using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Ink;
using System.IO;
using System.Data.OleDb;
using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Win32;
using ScienceResearchWpfApplication.AI;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// ReferTabItemUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReferTabItemUserControl : UserControl
    {
        #region 变量
        protected RegistryKey scienceResearchKey;

        ScienceResearchDataSetNew.文章DataTable wz_dt;
        public ScienceResearchDataSetNew.语段DataTable yd_dt;
        public ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        public ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        public ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;
        public ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        public ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        public ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        public ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;
        public ScienceResearchDataSetNew.文章_关键词DataTable wz_gjc_dt;
        public ScienceResearchDataSetNewTableAdapters.文章_关键词TableAdapter wz_gjc_ta;


        public RichTextBox richTextBox;
        public string path_isf;

        
        int yd_id;
        int new_yd_id;

        protected ScienceResearchDataSetNew.关键词Row gjc;
        public string type;             //"yd"——语段,"xps"——xps
        protected string type_show;     //部分显示或者全部显示

        XamlManageClass xamlManageClass = MainWindow.xamlManageClass;

        string path_wz;                 //文章路径
        #endregion

        #region 构造函数
        /// <summary>
        /// 加载数据库
        /// </summary>
        protected void LoadDataBase()
        {
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
            yd_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;
            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            wz_gjc_dt = MainWindow.wz_gjc_dt;
            wz_gjc_ta = MainWindow.wz_gjc_ta;
            xm_gjc_dt = MainWindow.xm_gjc_dt;
            xm_gjc_ta = MainWindow.xm_gjc_ta;
            wz_dt = MainWindow.wz_dt;
        }

        /// <summary>
        /// 参考标签页
        /// </summary>
        public ReferTabItemUserControl()
        {
            //用于显示参考内容

            InitializeComponent();
            scienceResearchKey = MainWindow.scienceResearchKey;
            zhinengPipeiUserControl.referTabItemUserControl = this;
            zhinengPipeiUserControl.textboxInkcavasUserControl_using = textboxInkcavasUserControl;
            zhinengPipeiUserControl.type = "refer";
            richTextBox = textboxInkcavasUserControl.paragraphRichTextBox;
        }

        /// <summary>
        /// 段文标签页
        /// </summary>
        /// <param name="yd"></param>
        public ReferTabItemUserControl(ScienceResearchDataSetNew.语段Row yd)
        {
            //用于显示语段内容

            InitializeComponent();
            saveToolBar.Visibility = Visibility.Collapsed;
            partAllToolBar.Visibility = Visibility.Collapsed;

            scienceResearchKey = MainWindow.scienceResearchKey;
            partRadioButton.IsChecked = true;

            richTextBox = textboxInkcavasUserControl.paragraphRichTextBox;
            yd_id = yd.ID;
            LoadDataBase();
            textboxInkcavasUserControl.yd_id = yd_id;

            string yd_xaml = yd.语段;
            yd_xaml = TextFile.ReplaceLowOrderASCIICharacters(yd_xaml);
            //yd_xaml = yd_xaml.Replace(";", " ");
            string condition = yd_xaml.Substring(0, 13);
            if (condition == "<FlowDocument")
            {                
                richTextBox.Document = xamlManageClass.xaml_load(yd_xaml);            
            }
            else
            {
                FlowDocument doc = new FlowDocument();
                doc.LineHeight = 10;
                Paragraph p = new Paragraph();
                p.LineHeight = 30;
                Run r = new Run(yd.语段);
                //r.SetBinding(Run.TextProperty, binding);
                p.Inlines.Add(r);
                doc.Blocks.Add(p);
                richTextBox.Document = doc;
            }

            //加载墨笔
            path_isf = yd.语段isf;
            path_isf = MainWindow.path_translate(path_isf);
            if (path_isf != "")
            {
                FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                if (file_ink.Length != 0)
                {
                    textboxInkcavasUserControl.inkCanvas.Strokes = new StrokeCollection(file_ink);
                }
                file_ink.Close();
            }
            if (path_isf == "")
            {
                //保存数据库
                yd.语段isf = @".\科学研究\语段ISF\" + yd.ID + ".isf";
                yd_ta.Update(yd_dt);
                path_isf = yd.语段isf;
                path_isf = MainWindow.path_translate(path_isf);
            }

            type = "yd";
            textboxInkcavasUserControl.type = "yd";
            textboxInkcavasUserControl.path_isf = path_isf;

            //调整宽度
            double width = yd.宽度;

            if (width > 0)
            {
                textboxInkcavasUserControl.paragraphRichTextBox.Width = width;
                textboxInkcavasUserControl.inkCanvas.Width = width;
            }
            else
            {
                textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_ck_width;
                textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_ck_width;
            }

            zhinengPipeiUserControl.referTabItemUserControl = this;
            zhinengPipeiUserControl.textboxInkcavasUserControl_using = textboxInkcavasUserControl;
            zhinengPipeiUserControl.type = "refer";
        }

        /// <summary>
        /// 段文标签页
        /// </summary>
        /// <param name="wz"></param>
        public ReferTabItemUserControl(ScienceResearchDataSetNew.文章Row wz)
        {
            //用于显示文章内容

            InitializeComponent();
            //saveToolBar.Visibility = Visibility.Collapsed;
            partAllToolBar.Visibility = Visibility.Collapsed;

            scienceResearchKey = MainWindow.scienceResearchKey;
            partRadioButton.IsChecked = true;
            type = "wz";
            textboxInkcavasUserControl.type = "wz";

            richTextBox = textboxInkcavasUserControl.paragraphRichTextBox;
            //FlowDocument flowDocument = new FlowDocument();
            //flowDocument.LineHeight = 50;
            //Paragraph paragraph = new Paragraph();
            //string title = "文章=" + wz.文章名;
            //Run run = new Run(title);
            //paragraph.Inlines.Add(run);
            //flowDocument.Blocks.Add(paragraph);
            //richTextBox.Document = flowDocument;

            textboxInkcavasUserControl.wz = wz;

            //---------------------读取文章------------------------------------------------
            path_wz = wz.text文件;
            path_wz = MainWindow.path_translate(path_wz);

            //判断文章是否存在
            try
            {
                StreamReader sr = new StreamReader(path_wz, Encoding.Default);
                sr.Close();
            }
            catch
            {
                MessageBox.Show("下列文件不存在：" + path_wz);
            }

            //读取文章
            string line_paper_str =TextFile.GetFileString(path_wz);            

            //----------------------填充RichTextBox--------------------------------
            if (line_paper_str.Length>=13 && line_paper_str.Substring(0, 13) == "<FlowDocument")
            {
                richTextBox.Document = xamlManageClass.xaml_load(line_paper_str);
            }
            else
            {
                FlowDocument doc = new FlowDocument();
                doc.LineHeight = 10;
                Paragraph p = new Paragraph();
                p.LineHeight = 30;
                Run r = new Run(line_paper_str);
                //r.SetBinding(Run.TextProperty, binding);
                p.Inlines.Add(r);
                doc.Blocks.Add(p);
                richTextBox.Document = doc;
            }

            textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_ck_width;
            textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_ck_width;

            //--------------------加载墨笔--------------------------------------
            path_isf = path_wz.Substring(0, path_wz.Length - 3) + "isf";
            if (path_isf != "")
            {
                FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                if (file_ink.Length != 0)
                {
                    textboxInkcavasUserControl.inkCanvas.Strokes = new StrokeCollection(file_ink);
                }
                file_ink.Close();
            }

            //----------------------设置智能匹配--------------------------------
            zhinengPipeiUserControl.referTabItemUserControl = this;
            zhinengPipeiUserControl.textboxInkcavasUserControl_using = textboxInkcavasUserControl;
            zhinengPipeiUserControl.type = "refer";

        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                e.Row["ID"] = newID;
                new_yd_id = newID;
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
                
            }
        }
        
        #endregion

        private void scrolls_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void paperStackPanel_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            scrolls.DataContext = fontSelectUserControl;
        }

        private void paperStackPanel_LostFocus(object sender, RoutedEventArgs e)
        {
            //保存文本内容和墨笔文件
            if (type == "yd")
            {
                if (path_isf != "")
                {                 

                    ScienceResearchDataSetNew.语段Row dr = (ScienceResearchDataSetNew.语段Row)yd_dt.Rows.Find(yd_id);
                    //TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    //dr.语段 = textRange.Text;

                    string text2 = xamlManageClass.xaml_save(richTextBox);
                    dr["语段"] = text2;
                    dr["宽度"] = richTextBox.Width;

                    yd_ta.Update(yd_dt);

                    //保存墨笔文件         
                    if (File.Exists(path_isf))
                        File.Delete(path_isf);

                    FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                    textboxInkcavasUserControl.inkCanvas.Strokes.Save(file_ink);
                    file_ink.Close();
                }
            }
        }       

        private void textRadioButton_Click(object sender, RoutedEventArgs e)
        {
            TextboxInkcavasUserControl t = textboxInkcavasUserControl;
            t.inkCanvas.SetValue(Grid.ZIndexProperty, 0);
            t.paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 1);
        }

        private void inkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            TextboxInkcavasUserControl t = textboxInkcavasUserControl;
            t.inkCanvas.SetValue(Grid.ZIndexProperty, 1);
            t.paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
            t.inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        #region 保存工具条
        /// <summary>
        /// 保存参考资料以及文章
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (type == "refer")
            {
                DataRow dr = yd_dt.NewRow();
                string text2 = xamlManageClass.xaml_save(richTextBox);
                dr["语段"] = text2;
                dr["宽度"] = richTextBox.Width;
                yd_dt.Rows.Add(dr);
                yd_ta.Update(yd_dt);

                //保存墨笔文件   
                ScienceResearchDataSetNew.语段Row yd = (ScienceResearchDataSetNew.语段Row)yd_dt.Rows.Find(new_yd_id);
                yd.语段isf = @".\科学研究\语段ISF\" + yd.ID + ".isf";
                yd_ta.Update(yd_dt);
                path_isf = yd.语段isf;
                path_isf = MainWindow.path_translate(path_isf);

                if (File.Exists(path_isf))
                    File.Delete(path_isf);

                FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                textboxInkcavasUserControl.inkCanvas.Strokes.Save(file_ink);
                file_ink.Close();

                //为语段添加关键词

                if (gjc != null)
                {
                    dr = yd_gjc_dt.NewRow();
                    dr["语段ID"] = new_yd_id;
                    dr["关键词ID"] = gjc.ID;

                    yd_gjc_dt.Rows.Add(dr);
                    yd_gjc_ta.Update(yd_gjc_dt);
                }                
            }
            else if (type == "wz")
            {
                //保存文章
                //string text2 = xamlManageClass.xaml_save(richTextBox);
                string text2 = TextFile.GetStringOfRichTextBox(richTextBox);
                TextFile.SaveStringToFile(text2, path_wz);

                //保存墨笔文件
                if (File.Exists(path_isf))
                    File.Delete(path_isf);

                FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                textboxInkcavasUserControl.inkCanvas.Strokes.Save(file_ink);
                file_ink.Close();
            }

            MessageBox.Show("保存成功");
        }
        #endregion

        private void fontSelectUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            double fontsize_PaperUserControl_Scroll = double.Parse(scienceResearchKey.GetValue("Fontsize_PaperUserControl_Scroll").ToString());
            FontFamily fontFamily_PaperUserControl_Scroll = new FontFamily(scienceResearchKey.GetValue("FontFamily_PaperUserControl_Scroll").ToString());
            fontSelectUserControl.setSelected(fontsize_PaperUserControl_Scroll, fontFamily_PaperUserControl_Scroll);
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            type_show = "all";
        }

        private void btnPart_Click(object sender, RoutedEventArgs e)
        {
            type_show = "part";
        }
    }

    /// <summary>
    /// XpsUserControl下面的控件，用于显示当前页面上的文字
    /// </summary>
    public class XpsReferTabItemUserControl : ReferTabItemUserControl
    {
        public XpsReferTabItemUserControl()
        {
            textboxInkcavasUserControl.type = "xps";
            
        }

        public void SetText(string text)
        {
            richTextBox = textboxInkcavasUserControl.paragraphRichTextBox;
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.LineHeight = 50;
            Paragraph paragraph = new Paragraph();
            Run run = new Run(text);
            paragraph.Inlines.Add(run);
            flowDocument.Blocks.Add(paragraph);
            richTextBox.Document = flowDocument;

            zhinengPipeiUserControl.shibie_control();
        }

    }

    /// <summary>
    /// 匹配参考标签页控件
    /// </summary>
    public abstract class PipeiReferTabItemUserControl : ReferTabItemUserControl
    {
        protected FlowDocument flowDocument;
        protected Paragraph paragraph;
        protected int count = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PipeiReferTabItemUserControl()
        {
            scienceResearchKey = MainWindow.scienceResearchKey;
            partRadioButton.IsChecked = true;
            type = "refer";
            textboxInkcavasUserControl.type = "Yd";
            LoadDataBase();
            richTextBox = textboxInkcavasUserControl.paragraphRichTextBox;

            flowDocument = new FlowDocument();
            flowDocument.LineHeight = 50;
            paragraph = new Paragraph();
        }

        /// <summary>
        /// 获取匹配结果
        /// </summary>
        protected void get_pipei_result()
        {
            //获取匹配的文章
            UsedPaper paperUsedClass = new UsedPaper();
            List<ScienceResearchDataSetNew.文章Row> paperList = paperUsedClass.provide_paper("text文件");

            //按照文章编号循环
            
            for (int i = 1; i <= paperList.Count; i++)
            {
                ScienceResearchDataSetNew.文章Row wz = paperList[i - 1];
                int paperId = wz.ID;
                string paperName = wz.文章名;
                if (paperName == "再别康桥")
                {
                    //int x = 1;
                }

                string paperPath = wz.text文件;
                paperPath = MainWindow.path_translate(paperPath);

                try
                {
                    StreamReader sr = new StreamReader(paperPath, Encoding.Default);
                    sr.Close();
                }
                catch
                {
                    MessageBox.Show("下列文件不存在：" + paperPath);
                    break;
                }

                string[] filelist = TextFile.GetStringArrayInPaper(paperPath);
                get_pipei_result_paper(filelist, wz);

            }
        }        

        /// <summary>
        /// 获取某篇文章的匹配结果
        /// </summary>
        /// <param name="filelist"></param>
        /// <param name="wz"></param>
        public abstract void get_pipei_result_paper(string[] filelist, ScienceResearchDataSetNew.文章Row wz);


    }

    /// <summary>
    /// 用于显示根据单词模式匹配得到的参考资料
    /// </summary>
    public class DcPipeiReferTabItemUserControl : PipeiReferTabItemUserControl
    {
        string keyword;
        int dc_yujing = 7;      //单词语境长度

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_gjc"></param>
        /// <param name="dc"></param>
        public DcPipeiReferTabItemUserControl(ScienceResearchDataSetNew.关键词Row _gjc, ScienceResearchDataSetNew.单词Row dc)
        {
            gjc = _gjc;
                        
            string title;
            if (gjc == null)
                title = "关键词=null" + "，单词=" + dc.单词;
            else
                title = "关键词=" + gjc.关键词 + "，单词=" + dc.单词;
            Run run = new Run(title);
            paragraph.Inlines.Add(run);
            flowDocument.Blocks.Add(paragraph);
            richTextBox.Document = flowDocument;
            keyword = dc.单词;

            //进行匹配
            get_pipei_result();

            //着色
            TextProcessClass.ChangeColor(ColorManager.color_mode_word, richTextBox, keyword, "前景");

            //调整宽度
            textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_ck_width;
            textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_ck_width;

            zhinengPipeiUserControl.referTabItemUserControl = this;
            zhinengPipeiUserControl.type = "refer";
        }

        /// <summary>
        /// 获取某篇文章的匹配结果
        /// </summary>
        /// <param name="filelist"></param>
        /// <param name="wz"></param>
        public override void get_pipei_result_paper(string[] filelist, ScienceResearchDataSetNew.文章Row wz)
        {
            //对一篇文章进行匹配，将匹配到的结果添加到流文档里面

            for (int linenum = 0; linenum <= filelist.Length - 1; linenum++)
            {
                if (filelist[linenum].IndexOf(keyword) > -1)
                {
                    string paper_file_str = wz.文件;
                    string stringInPaper = "[" + wz.ID + "]";


                    if (paper_file_str != "")       //从pdf文件导入的text文件
                    {
                        for (int j = -dc_yujing/2; j <= dc_yujing/2; j++)
                        {
                            if (linenum + j >= 0 && linenum + j <= filelist.Length - 1)
                            {
                                stringInPaper = stringInPaper + filelist[linenum + j];
                            }
                        }
                    }
                    else                            //网上下载的文件
                        stringInPaper = stringInPaper + filelist[linenum];

                    paragraph = new Paragraph();
                    Run run = new Run(stringInPaper);
                    paragraph.Inlines.Add(run);
                    flowDocument.Blocks.Add(paragraph);

                    if (type_show == "part")
                        if (count >= 20)
                            return;

                    count = count + 1;
                }
            }
        }
        

    }

    /// <summary>
    /// 用于显示根据短语模式匹配得到的参考资料
    /// </summary>
    public class DyPipeiReferTabItemUserControl : PipeiReferTabItemUserControl
    {
        string keyword;
        int dy_yujing = 7;      //单词语境长度

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_gjc"></param>
        /// <param name="dy"></param>
        public DyPipeiReferTabItemUserControl(ScienceResearchDataSetNew.关键词Row _gjc, ScienceResearchDataSetNew.短语Row dy)
        {
            gjc = _gjc;

            string title;
            if (gjc == null)
                title = "关键词=null" + "，单词=" + dy.短语;
            else
                title = "关键词=" + gjc.关键词 + "，单词=" + dy.短语;
            Run run = new Run(title);
            paragraph.Inlines.Add(run);
            flowDocument.Blocks.Add(paragraph);
            richTextBox.Document = flowDocument;
            keyword = dy.短语;

            //进行匹配
            get_pipei_result();

            //调整宽度
            textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_ck_width;
            textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_ck_width;

            zhinengPipeiUserControl.referTabItemUserControl = this;
            zhinengPipeiUserControl.type = "refer";
        }

        /// <summary>
        /// 获取某篇文章的匹配结果
        /// </summary>
        /// <param name="filelist"></param>
        /// <param name="wz"></param>
        public override void get_pipei_result_paper(string[] filelist, ScienceResearchDataSetNew.文章Row wz)
        {
            //对一篇文章进行匹配，将匹配到的结果添加到流文档里面

            for (int linenum = 0; linenum <= filelist.Length - 1; linenum++)
            {
                if (filelist[linenum].IndexOf(keyword) > -1)
                {
                    string paper_file_str = wz.文件;
                    string stringInPaper = "[" + wz.ID + "]";


                    if (paper_file_str != "")       //从pdf文件导入的text文件
                    {
                        for (int j = -dy_yujing / 2; j <= dy_yujing / 2; j++)
                        {
                            if (linenum + j >= 0 && linenum + j <= filelist.Length - 1)
                            {
                                stringInPaper = stringInPaper + filelist[linenum + j];
                            }
                        }
                    }
                    else                            //网上下载的文件
                        stringInPaper = stringInPaper + filelist[linenum];

                    paragraph = new Paragraph();
                    Run run = new Run(stringInPaper);
                    paragraph.Inlines.Add(run);
                    flowDocument.Blocks.Add(paragraph);

                    if (type_show == "part")
                        if (count >= 20)
                            return;

                    count = count + 1;
                }
            }
        }


    }

    /// <summary>
    /// 用于显示根据语段模式匹配得到的参考资料
    /// </summary>
    public class YdPipeiReferTabItemUserControl : PipeiReferTabItemUserControl
    {
        List<string> dc_list;
        int pdf_yd_line = 7;         //pdf导入文件的语段长度    
        int yd_id;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_gjc"></param>
        /// <param name="_dc_list"></param>
        /// <param name="_yd_id"></param>
        public YdPipeiReferTabItemUserControl(ScienceResearchDataSetNew.关键词Row _gjc, List<string> _dc_list,int _yd_id)
        {
            gjc = _gjc;
            dc_list = _dc_list;
            int dc_count = dc_list.Count;
            yd_id = _yd_id;

            string title;
            if (gjc == null)
                title = "关键词=null" + "，单词={";
            else
                title = "关键词=" + gjc.关键词 + "，单词={";
            foreach (string dc in dc_list)
                title += dc + ",";
            title = title.Substring(0, title.Length - 1);
            title += "}";

            Run run = new Run(title);
            paragraph.Inlines.Add(run);
            flowDocument.Blocks.Add(paragraph);
            richTextBox.Document = flowDocument;

            get_pipei_result();
        }

        public override void get_pipei_result_paper(string[] filelist, ScienceResearchDataSetNew.文章Row wz)
        {
            //获取一篇文章中的匹配结果

            //判断该文章是从网上下载的text文件还是pdf文件
            string pdf_or_text;
            int num_paper_lines = filelist.Count();
            int juhao = 0;          //每一段话是不是以句号结尾
            foreach (string line_str in filelist)
            {
                if (line_str != "")
                {
                    string s = line_str.Substring(line_str.Length - 1, 1);
                    if (s == "." || s == "!" || s == "?" || s == "。" || s == "！" || s == "？")
                        juhao++;
                }
            }
            if ((double)juhao / num_paper_lines > .8)
                pdf_or_text = "text";
            else
                pdf_or_text = "pdf";

            //--------------匹配由pdf导出的text文件----------------------------------------------------
            //前后若干句称为一段，看看能不能匹配上
            if (pdf_or_text == "pdf")
            {
                for (int linenum = pdf_yd_line/2; linenum <= filelist.Length - 1-pdf_yd_line/2; linenum+= pdf_yd_line)
                {
                    string pdf_yd_str = "";
                    for (int j = -pdf_yd_line / 2; j <= pdf_yd_line / 2; j++)
                        pdf_yd_str = pdf_yd_str + filelist[linenum + j];

                    //判定该语段是否含有该模式
                    List<int> mode_int_list=ZhinengPipeiUserControl.shibie_para_yd(pdf_yd_str);
                    if (mode_int_list.Contains(yd_id))
                    {
                        string paper_file_str = wz.文件;
                        string stringInPaper = "[" + wz.ID + "]";
                        stringInPaper = stringInPaper + pdf_yd_str;

                        paragraph = new Paragraph();
                        Run run = new Run(stringInPaper);
                        paragraph.Inlines.Add(run);
                        flowDocument.Blocks.Add(paragraph);

                        if (type_show == "part")
                            if (count >= 20)
                                return;

                        count = count + 1;
                    }
                }
            }





            //--------------匹配text文件----------------------------------------------------
            //一段一段匹配，看看能不能匹配上
            if (pdf_or_text == "text")
            {


            }

        }


    }


}
