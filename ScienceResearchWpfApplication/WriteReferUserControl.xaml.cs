using System;
using System.Data;
using System.IO;
using System.Windows.Ink;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Collections.Generic;
using ScienceResearchWpfApplication.DatabaseManage;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// Writing_Refer_UserControl.xaml 的交互逻辑
    /// </summary>
    /// 

    class ParagraphIdKeyword
    {
        public int paragraphId;
        public string keyword;
        public ParagraphIdKeyword(int _paragraphId, string _keyword)
        {
            paragraphId = _paragraphId;
            keyword = _keyword;
        }


    }

    public partial class WriteReferUserControl : UserControl
    {
        TextboxInkcavasUserControl textboxInkcavasUserControl, textboxInkcavasUserControl_using;

        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;

        TextBlock textBlock;

        List<ParagraphIdKeyword> paragraphIdKeywords;
        int startPoint;         //载入的开始位置
        int sectionLength;      //页面长度
        int length;             //总长度

        public int paperId;

        XamlManageClass xamlManageClass = MainWindow.xamlManageClass;

        public WriteReferUserControl(int _paperId)
        {
            InitializeComponent();

            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;

            yd_gjc_dt = MainWindow.scienceResearchDataSetNew.语段_关键词;
            yd_gjc_ta = new ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter();

            gjc_dt = MainWindow.scienceResearchDataSetNew.关键词;
            gjc_ta = new ScienceResearchDataSetNewTableAdapters.关键词TableAdapter();

            //每个语段对应一个文本框
            paperId = _paperId;

            if (paperId != -1)
            {
                //存储语段和关键词数据
                paragraphIdKeywords = new List<ParagraphIdKeyword>();
                var yd_paper = from yd in yd_dt
                               where yd.文章ID == paperId
                               orderby yd.排序 ascending
                               select yd;
                foreach (var yd in yd_paper)
                {
                    int yd_id = yd.ID;
                    var gjc_yd = from yd_gjc in yd_gjc_dt
                                 join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                                 where yd_gjc.语段ID == yd.ID
                                 select new Keyword
                                 {
                                     关键词 = gjc.关键词
                                 };
                    if (gjc_yd.Count() == 0)
                    {
                        paragraphIdKeywords.Add(new ParagraphIdKeyword(yd.ID, null));
                    }
                    else
                    {
                        foreach (var gjc in gjc_yd)
                        {
                            paragraphIdKeywords.Add(new ParagraphIdKeyword(yd.ID, gjc.关键词));
                        }
                    }
                }

                startPoint = 0;
                sectionLength = 2;
                length = paragraphIdKeywords.Count;

                int page = length / sectionLength + 1;
                pageTextBlock.Text = "/"+page;
                pageTextBox.Text = 1.ToString();

                //载入初始页面
                if (length!=0)
                    pageLoad(startPoint, sectionLength);
            }
        }

        private void pageLoad(int start, int sectionLength)
        {
            pageTextBox.Text = (start / sectionLength + 1).ToString();
            //按语段+关键字循环
            for (int i = 0; i < sectionLength; i++)
            {
                if (startPoint + i<length)
                { 
                    int paraId = paragraphIdKeywords[startPoint + i].paragraphId;
                    string gjc = paragraphIdKeywords[startPoint + i].keyword;

                    var paragraph = from data_item in yd_dt
                                where data_item.ID == paraId
                                select data_item;
                    foreach (var para in paragraph)
                    {
                        //语段编号
                        textBlock = new TextBlock();
                        textBlock.Inlines.Add(new Run(para.ID.ToString() + ":" + gjc) { Foreground = Brushes.Red });
                        paperStackPanel.Children.Add(textBlock);

                        //文本框
                        if (para["分类"].ToString() != "图片语段")
                        {               
                            string path_isf = "";
                            path_isf = para.语段isf;

                            if (path_isf == "")
                            {
                                //保存数据库
                                para.语段isf = @".\科学研究\语段ISF\" + para.ID + ".isf";
                                yd_ta.Update(yd_dt);
                                path_isf = para.语段isf;
                            }
                            textboxInkcavasUserControl = new TextboxInkcavasUserControl(path_isf);
                            textboxInkcavasUserControl.GotFocus += textboxInkcavasUserControl_GotFocus;
                            textboxInkcavasUserControl.StylusDown += TextboxInkcavasUserControl_StylusDown;
                            textboxInkcavasUserControl.PreviewMouseLeftButtonDown += TextboxInkcavasUserControl_PreviewMouseLeftButtonDown;
                            textboxInkcavasUserControl.yd_dt = yd_dt;
                            textboxInkcavasUserControl.yd_ta = yd_ta;

                            double width = para.宽度;

                            if (width > 0)
                            {
                                textboxInkcavasUserControl.paragraphRichTextBox.Width = width;
                                textboxInkcavasUserControl.inkCanvas.Width = width;
                            }
                            else
                            {
                                textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_cz_width;
                                textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_cz_width;
                            }

                            //Binding binding = new Binding();
                            //binding.Source = para;
                            //binding.Path = new PropertyPath("语段");
                            //binding.Mode = BindingMode.TwoWay;

                            //textboxInkcavasUserControl.paragraphRichTextBox.SetBinding(TextBox.TextProperty, binding);

                            string yd_xaml = para.语段;
                            string condition = yd_xaml.Substring(0, 13);
                            if (condition == "<FlowDocument")
                            {
                                textboxInkcavasUserControl.paragraphRichTextBox.Document = xamlManageClass.xaml_load(yd_xaml);
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
                                textboxInkcavasUserControl.paragraphRichTextBox.Document = doc;
                            }

                            paperStackPanel.Children.Add(textboxInkcavasUserControl);
                            textboxInkcavasUserControl.yd_id = para.ID;
                        }
                        else
                        {
                            string figure_path = "";
                            string figure_path_isf = "";
                            var data4 = from data_item in yd_dt
                                        where data_item.ID == para.ID
                                        select data_item;
                            foreach (var d4 in data4)
                            {
                                figure_path = d4.图片.ToString();
                                figure_path_isf = d4.图片isf.ToString();
                            }
                            figure_path = MainWindow.path_translate(figure_path);
                            figure_path_isf = MainWindow.path_translate(figure_path_isf);

                            ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
                            if (File.Exists(figure_path))
                            {
                                //存在图片
                                //BitmapImage img = new BitmapImage();
                                //img.BeginInit(); 
                                //img.CacheOption = BitmapCacheOption.OnLoad;
                                //img.UriSource = new Uri(figure_path);
                                //img.EndInit();

                                //ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                                //ImageSource img2=img.Clone();

                                BitmapImage bitmapImage;
                                BinaryReader reader = new BinaryReader(File.Open(figure_path, FileMode.Open));
                                FileInfo fi = new FileInfo(figure_path);
                                byte[] bytes = reader.ReadBytes((int)fi.Length);
                                reader.Close();

                                bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.StreamSource = new MemoryStream(bytes);
                                bitmapImage.EndInit();
                                paragraphFigureUserControl.img.Source = bitmapImage;
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                            }
                            else
                            {
                                //不存在图片的时候                        
                            }
                            paragraphFigureUserControl.grid.RowDefinitions[0].Height = new GridLength(0);
                            paragraphFigureUserControl.paragraphId = para.ID;
                            paragraphFigureUserControl.GotFocus += ParagraphFigureUserControl_GotFocus;
                            paragraphFigureUserControl.StylusDown += ParagraphFigureUserControl_StylusDown;
   

                            if (File.Exists(figure_path_isf))
                            {
                                FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                                if (file_ink.Length != 0)
                                {
                                    paragraphFigureUserControl.canvas.Strokes = new StrokeCollection(file_ink);
                                }
                                file_ink.Close();
                                paragraphFigureUserControl.figure_path_isf = figure_path_isf;
                            }
                            else
                            {
                                paragraphFigureUserControl.canvas.Strokes.Clear();
                                figure_path_isf = ".\\科学研究\\图片ISF\\" + para.ID + ".isf";

                                //更新数据库
                                var data5 = from data_item in yd_dt
                                            where data_item.ID == para.ID
                                            select data_item;
                                foreach (var d5 in data5)
                                {
                                    d5.图片isf = figure_path_isf;
                                    yd_ta.Update(yd_dt);
                                }

                                //更新墨笔文件路径
                                figure_path_isf = MainWindow.path_translate(figure_path_isf);
                                paragraphFigureUserControl.figure_path_isf = figure_path_isf;
                            }
                            paperStackPanel.Children.Add(paragraphFigureUserControl);
                        }

                        //所有参考语段的图片
                        var data3 = from gjc2 in gjc_dt
                                    join yd_gjc in yd_gjc_dt on gjc2.ID equals yd_gjc.关键词ID
                                    join yd in yd_dt on yd_gjc.语段ID equals yd.ID
                                    where gjc2.关键词 == gjc
                                    select new ParagraphID
                                    {
                                        语段ID = yd.ID
                                    };

                        foreach (var d3 in data3)
                        {
                            textBlock = new TextBlock();
                            textBlock.Text = d3.语段ID.ToString();
                            paperStackPanel.Children.Add(textBlock);


                            string figure_path = "";
                            string figure_path_isf = "";
                            var data4 = from data_item in yd_dt
                                        where data_item.ID == d3.语段ID
                                        select data_item;
                            foreach (var d4 in data4)
                            {
                                try
                                {
                                    figure_path = d4.图片.ToString();
                                    figure_path_isf = d4.图片isf.ToString();
                                }
                                catch { }
                                finally { }
                            }
                            figure_path = MainWindow.path_translate(figure_path);
                            figure_path_isf = MainWindow.path_translate(figure_path_isf);

                            if (File.Exists(figure_path))
                            {
                                ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                                ParagraphFigureUserControl p = new ParagraphFigureUserControl();
                                p.img.Source = img;
                                p.grid.RowDefinitions[0].Height = new GridLength(0);

                                if (File.Exists(figure_path_isf))
                                {
                                    FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                                    if (file_ink.Length != 0)
                                    {
                                        p.canvas.Strokes = new StrokeCollection(file_ink);
                                    }
                                    file_ink.Close();
                                    p.figure_path_isf = figure_path_isf;
                                }
                                else
                                {
                                    p.canvas.Strokes.Clear();
                                    figure_path_isf = ".\\科学研究\\图片ISF\\" + d3.语段ID.ToString() + ".isf";

                                    //更新数据库
                                    var data5 = from data_item in yd_dt
                                                where data_item.ID == d3.语段ID
                                                select data_item;
                                    foreach (var d5 in data5)
                                    {
                                        d5.图片isf = figure_path_isf;
                                        yd_ta.Update(yd_dt);
                                    }

                                    //更新墨笔文件路径
                                    figure_path_isf = MainWindow.path_translate(figure_path_isf);
                                    p.figure_path_isf = figure_path_isf;
                                }
                                paperStackPanel.Children.Add(p);
                            }
                        }
                    }
                }
            }

            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
                }
                
            }
        }


        private void textboxInkcavasUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //btnText.IsEnabled = true;
            //btnInk.IsEnabled = true;

            textboxInkcavasUserControl_using = (TextboxInkcavasUserControl)sender;

            //Ink属性控制器获得cavas
            //InkPropertiesUserControl.canvas = ((TextboxInkcavasUserControl)sender).inkCanvas;
        }

        private void TextboxInkcavasUserControl_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            textboxInkcavasUserControl_GotFocus(sender, e);
        }

        private void TextboxInkcavasUserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textboxInkcavasUserControl_GotFocus(sender, e);
        }

        private void ParagraphFigureUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //btnText.IsEnabled = false;
            //btnInk.IsEnabled = false;
        }

        private void ParagraphFigureUserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ParagraphFigureUserControl_GotFocus(sender, e);
        }

        private void ParagraphFigureUserControl_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            ParagraphFigureUserControl_GotFocus(sender, e);
        }

        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 0);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 1);
                }
                else if (name == "System.Windows.Controls.TextBlock")
                {
                    //不做处理
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.None;
                }
            }
        }

        private void btnInk_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
                }
                else if (name == "System.Windows.Controls.TextBlock")
                {
                    //不做处理
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.Ink;
                }
            }
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
                    ((TextboxInkcavasUserControl)t).inkCanvas.EditingMode = InkCanvasEditingMode.None;
                }
                else if (name == "System.Windows.Controls.TextBlock")
                {
                    //不做处理
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.None;
                }
            }
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            grid.ColumnDefinitions[1].Width = new GridLength(0);
            grid.ColumnDefinitions[2].Width = new GridLength(0);
            grid.ColumnDefinitions[3].Width = new GridLength(0);
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.left_zero();
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            paperStackPanel.Children.Clear();
            startPoint += sectionLength;
            if (startPoint > length)
            {
                MessageBox.Show("已到文章末尾");
                startPoint -= sectionLength;
                pageLoad(startPoint, sectionLength);
            }
            else
            {
                pageLoad(startPoint, sectionLength);
            }

            scrolls.ScrollToHome();
        }

        private void scrolls_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void paperStackPanel_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void btnChangePage_Click(object sender, RoutedEventArgs e)
        {
            paperStackPanel.Children.Clear();
            startPoint =(int.Parse(pageTextBox.Text)-1)*sectionLength;
            pageLoad(startPoint, sectionLength);
            scrolls.ScrollToHome();
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            paperStackPanel.Children.Clear();
            startPoint -= sectionLength;
            if (startPoint < 0)
            {
                MessageBox.Show("已到文章开始");
                startPoint += sectionLength;
                pageLoad(startPoint, sectionLength);
            }
            else
            {
                pageLoad(startPoint, sectionLength);
            }
            scrolls.ScrollToHome();
        }

        private void hideScrollMunuItem_Click(object sender, RoutedEventArgs e)
        {
            scrolls.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private void viewScrollMunuItem_Click(object sender, RoutedEventArgs e)
        {
            scrolls.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
    }
}
