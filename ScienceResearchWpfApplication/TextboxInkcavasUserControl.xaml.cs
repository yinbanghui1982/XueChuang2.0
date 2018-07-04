using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Ink;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using ScienceResearchWpfApplication.AI;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// TextboxInkcavasUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TextboxInkcavasUserControl : UserControl
    {
        #region 变量
        public ScienceResearchDataSetNew.语段DataTable yd_dt;
        public ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;

        public string path_isf;                                 //注释文件
        public ScienceResearchDataSetNew.文章Row wz;            //文章

        private int _yd_id;
        RegistryKey scienceResearchKey;

        public string type="yd";            //类型：语段“yd”，参考“refer”，文章“wz”
        XamlManageClass xamlManageClass = MainWindow.xamlManageClass;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public TextboxInkcavasUserControl()
        {
            InitializeComponent();
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_path_isf"></param>
        public TextboxInkcavasUserControl(string _path_isf)
        {
            InitializeComponent();
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;

            //加载墨笔
            path_isf = _path_isf;
            path_isf = MainWindow.path_translate(path_isf);
            if (path_isf != "")
            {
                FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                if (file_ink.Length != 0)
                {
                    inkCanvas.Strokes = new StrokeCollection(file_ink);
                }
                file_ink.Close();
            }
        }        

        /// <summary>
        /// 语段ID属性
        /// </summary>
        public int yd_id
        {
            set
            {
                _yd_id = value;
            }
            get
            {
                return _yd_id;
            }
        }
        
        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (type == "yd")
            {
                //重新加载文件
                ScienceResearchDataSetNew.语段Row para = (ScienceResearchDataSetNew.语段Row)yd_dt.Rows.Find(yd_id);
                string yd_xaml = para.语段;
                if (yd_xaml!=""&&yd_xaml.Substring(0, 13) == "<FlowDocument")
                {                    
                    paragraphRichTextBox.Document = xamlManageClass.xaml_load(yd_xaml);
                }
                else
                {
                    FlowDocument doc = new FlowDocument();
                    doc.LineHeight = 10;
                    Paragraph p = new Paragraph();
                    p.LineHeight = 30;
                    Run r = new Run(para.语段);
                    //r.SetBinding(Run.TextProperty, binding);
                    p.Inlines.Add(r);
                    doc.Blocks.Add(p);
                    paragraphRichTextBox.Document = doc;
                }                

                //设置该控件与墨笔属性控件的关联
                //InkPropertiesUserControl.canvas = inkCanvas;

                //加载墨笔
                if (path_isf != null)
                {
                    path_isf = MainWindow.path_translate(path_isf);
                    FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                    if (file_ink.Length != 0)
                    {
                        inkCanvas.Strokes = new StrokeCollection(file_ink);
                    }
                    file_ink.Close();
                }
            }
            else if (type == "wz")
            {
                //不用进行任何操作                
            }

        }

        /// <summary>
        /// 失去焦点事件处理程序
        /// </summary>
        public void textboxLostFocus()
        {
            //保存文本内容和墨笔文件
            if (type == "yd")
            {
                if (path_isf != null)
                {
                    ScienceResearchDataSetNew.语段Row dr = (ScienceResearchDataSetNew.语段Row)yd_dt.Rows.Find(yd_id);
                    if (dr != null)
                    {
                        string text2 = xamlManageClass.xaml_save(paragraphRichTextBox);
                        dr["语段"] = text2;
                        dr["宽度"] = paragraphRichTextBox.Width;
                        //try
                        //{
                        yd_ta.Update(yd_dt);
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message);
                        //}


                        //保存墨笔文件         
                        if (File.Exists(path_isf))
                            File.Delete(path_isf);

                        FileStream file_ink = new FileStream(path_isf, FileMode.OpenOrCreate);
                        inkCanvas.Strokes.Save(file_ink);
                        file_ink.Close();
                    }
                }
            }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            textboxLostFocus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            scienceResearchKey = MainWindow.scienceResearchKey;
            string paperInkOrTextString = scienceResearchKey.GetValue("PaperInkOrText").ToString();
            
            if (paperInkOrTextString == "Text")
            {
                inkCanvas.SetValue(Grid.ZIndexProperty, 0);
                paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 1);
            }
            else
            {
                inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
            }
        }
    }
}
