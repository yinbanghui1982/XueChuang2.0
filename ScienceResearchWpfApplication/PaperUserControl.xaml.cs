using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ScienceResearchWpfApplication.ApplicationProgram;
using ScienceResearchWpfApplication.DatabaseManage;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// PaperUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaperUserControl : UserControl
    {
        #region 变量
        TextboxInkcavasUserControl textboxInkcavasUserControl, textboxInkcavasUserControl_using;

        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;
        ScienceResearchDataSetNew.单词DataTable dc_dt;
        ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;
        List<int> paragraphIds;         //所有语段的编号集合
        int paraWriteId;                //当前正在使用的段落编号
        double columnWidth1, columnWidth2, columnWidth3;
        int first;

        /// <summary>
        /// 文章编号
        /// </summary>
        public int paperId;
        IList<Keyword> keywordList;
        IList<Keyword> keywordList2;

        RegistryKey scienceResearchKey;

        /// <summary>
        /// 文章是在左侧还是在右侧显示
        /// </summary>
        public string location;

        private UIElement currentChild;

        string sql;
        OleDbCommand comm;

        int new_yd_id;

        ScienceResearchDataSetNew.语段_关键词Row yd_gjc_write;
        ScienceResearchDataSetNew.文章Row wz_using;

        Color color_mode,color_black;

        XamlManageClass xamlManageClass = MainWindow.xamlManageClass;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        const int WM_CLOSE = 0x0010;

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;

        string paper_type="xuechuang";            //文章是学创还是word  
        WindowsFormsHostUserControl windowsFormsHostUserControl;        //外部应用程序放置窗体
        ReferUserControl referUserControl, referUserControl_using;

        string goutongType="weixin";

        ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid;
        #endregion

        #region 构造函数
        /// <summary>
        /// 文章创作标签页的构造函数
        /// </summary>
        /// <param name="_paperId">文章ID</param>
        public PaperUserControl(int _paperId)
        {        
            InitializeComponent();
            //=====================================================
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
            yd_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;
            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            wz_dt = MainWindow.wz_dt;
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;
            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            //=====================================================
            //每个语段对应一个文本框
            paperId = _paperId;
            color_mode = ColorManager.color_mode_word;
            color_black = Colors.Black;
            referUserControl_using = MainWindow.referUserControl;
            zhinengPipeiUserControl.referUserControl_using = referUserControl_using;
            loadPaper();
            //=====================================================
            first = 1;
            scienceResearchKey = MainWindow.scienceResearchKey;
            location = "left";

            //scienceResearchKey = MainWindow.scienceResearchKey;
            //string paperInkOrTextString = scienceResearchKey.GetValue("PaperInkOrText").ToString();
            //if (paperInkOrTextString == "Text")
            //    textRadioButton.IsChecked = true;
            //else
            //    inkRadioButton.IsChecked = true;  

            paperGrid.RowDefinitions[1].Height = new GridLength(0);
            paperGrid.RowDefinitions[2].Height = new GridLength(0);

            ZYRadioButton.IsChecked = true;

            //智能匹配工具条属性设置
            zhinengPipeiUserControl.type = "paper";
            zhinengPipeiUserControl.paperStackPanel = paperStackPanel;
            zhinengPipeiUserControl.keywordDataGrid2 = keywordDataGrid2;
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
       
        private void loadPaper()
        {
            wz_using = (ScienceResearchDataSetNew.文章Row)wz_dt.Rows.Find(paperId);
            string file_str = wz_using.文件;
            paperGrid.RowDefinitions[1].Height = new GridLength(0);
            if (file_str == "")         //非word文档
            {
                wordToolBar.Visibility = Visibility.Collapsed;
                referToolBar.Visibility = Visibility.Collapsed;
                scrollToolBar.Visibility = Visibility.Collapsed;
                goutongToolBar.Visibility = Visibility.Collapsed;

                paragraphIds = new List<int>();
                OrderedEnumerableRowCollection<ScienceResearchDataSetNew.语段Row> ydCollection = DataBaseRowManage.GetYd(wz_using);
                foreach (var para in ydCollection)
                {
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
                        //textboxInkcavasUserControl.paragraphRichTextBox.PreviewMouseLeftButtonUp += ParagraphRichTextBox_PreviewMouseLeftButtonUp;


                        //ContextMenu aMenu = new ContextMenu();
                        //MenuItem deleteMenu = new MenuItem();
                        //deleteMenu.Header = "前插文字语段";
                        //deleteMenu.Click += textParagraphInsertMenuItem_Click;
                        //aMenu.Items.Add(deleteMenu);
                        //textboxInkcavasUserControl.paragraphTextBox.ContextMenu = new ContextMenu();
                        //textboxInkcavasUserControl.paragraphTextBox.ContextMenu.Items.Add( deleteMenu);

                        //Binding binding = new Binding();
                        //binding.Source = para;
                        //binding.Path = new PropertyPath("语段");
                        //binding.Mode = BindingMode.TwoWay;

                        string yd_xaml = para.语段;
                        if (yd_xaml != "" && yd_xaml.Substring(0, 13) == "<FlowDocument")
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

                        //textboxInkcavasUserControl.paragraphTextBox.SetBinding(TextBox.TextProperty, binding);

                        paperStackPanel.Children.Add(textboxInkcavasUserControl);
                        textboxInkcavasUserControl.yd_id = para.ID;
                        //paperStackPanel.RegisterName("Para" + d.ID.ToString(), textboxInkcavasUserControl);
                        //textboxInkcavasUserControl.Name = "Para" + d.ID.ToString();
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
                        paragraphFigureUserControl.PreviewMouseLeftButtonDown += ParagraphFigureUserControl_PreviewMouseLeftButtonDown;

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
                    paragraphIds.Add(para.ID);
                }
                foreach (object t in paperStackPanel.Children)
                {
                    string name = t.GetType().FullName;
                    if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                    {
                        RichTextBox rtb = ((TextboxInkcavasUserControl)t).paragraphRichTextBox;
                        ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                        ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);

                        //关键词着色
                        //ZhinengPipeiUserControl.Shibie(rtb);
                    }
                }
            }
            else
            {
                //加载word时，不做处理
                paragraphToolBar.Visibility = Visibility.Collapsed;
                fontSelectUserControl.Visibility= Visibility.Collapsed;
                hongguanToolBar.Visibility = Visibility.Collapsed;
                insertYuduanToolBar.Visibility= Visibility.Collapsed;
                scrollToolBar.Visibility = Visibility.Collapsed;
                goutongToolBar.Visibility = Visibility.Collapsed;
                zhinengPipeiUserControl.Visibility = Visibility.Collapsed;

                referUserControl = new ReferUserControl("refer");
                referGrid.Children.Add(referUserControl);

                referUserControl_using = referUserControl;
                zhinengPipeiUserControl.referUserControl_using = referUserControl_using;
            }
        }                        

        private void ParagraphFigureUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //btnText.IsEnabled = false;
            //btnInk.IsEnabled = false;

            ParagraphFigureUserControl paragraphFigureUserControl = (ParagraphFigureUserControl)sender;
            //paragraphIdTextBox.Text = paragraphFigureUserControl.paragraphId.ToString();
            //更新关键字
            paraWriteId = paragraphFigureUserControl.paragraphId;

            var x = (from yd_gjc in yd_gjc_dt
                     join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                     where yd_gjc.语段ID == paraWriteId
                     select yd_gjc).ToList();

            keywordList = (from yd_gjc in yd_gjc_dt
                           join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                           where yd_gjc.语段ID == paraWriteId
                           select new Keyword
                           {
                               关键词 = gjc.关键词,
                           }).ToList();

            keywordDataGrid.ItemsSource = x;
            keywordList2 = new List<Keyword>(keywordList);

            keywordDataGrid.CanUserAddRows = true;
            keywordDataGrid.CanUserDeleteRows = true;
            currentChild = paragraphFigureUserControl;

            //改变状态栏文字
            var paper_list = (from wz in wz_dt
                              where wz.ID == paperId
                              select wz).ToList();
            int xiangmuId = 0;
            if (paper_list[0].项目Row != null)
                xiangmuId = paper_list[0].项目Row.ID;
            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "选中的语段为：ID=" + paraWriteId + "，文章ID=" + paperId + "，项目ID=" + xiangmuId;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);
        }

        private void ParagraphFigureUserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ParagraphFigureUserControl_GotFocus(sender, e);
        }

        private void ParagraphFigureUserControl_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            ParagraphFigureUserControl_GotFocus(sender, e);
        }

        private void TextboxInkcavasUserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textboxInkcavasUserControl_GotFocus(sender, e);
        }

        private void TextboxInkcavasUserControl_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            textboxInkcavasUserControl_GotFocus(sender, e);

            //如果是触控笔接触屏幕的话，改变“手写”和“擦除”状态
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                //暂时不加该功能                
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            scrolls.DataContext = fontSelectUserControl;
            MainWindow.paperId = paperId;
        }

        private void fontSelectUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            double fontsize_PaperUserControl_Scroll = double.Parse(scienceResearchKey.GetValue("Fontsize_PaperUserControl_Scroll").ToString());
            FontFamily fontFamily_PaperUserControl_Scroll = new FontFamily(scienceResearchKey.GetValue("FontFamily_PaperUserControl_Scroll").ToString());
            fontSelectUserControl.setSelected(fontsize_PaperUserControl_Scroll, fontFamily_PaperUserControl_Scroll);
        }

        private void textboxInkcavasUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //btnText.IsEnabled = true;
            //btnInk.IsEnabled = true;

            textboxInkcavasUserControl_using = (TextboxInkcavasUserControl)sender;
            zhinengPipeiUserControl.textboxInkcavasUserControl_using = textboxInkcavasUserControl_using;

            //paragraphIdTextBox.Text = ((TextboxInkcavasUserControl)sender).Name.Substring(4);
            //paragraphIdTextBox.Text = textboxInkcavasUserControl_using.yd_id.ToString();
            //更新关键字
            //paraWriteId = Convert.ToInt32(((TextboxInkcavasUserControl)sender).Name.Substring(4));
            paraWriteId = textboxInkcavasUserControl_using.yd_id;

            var x = (from yd_gjc in yd_gjc_dt
                     join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                     where yd_gjc.语段ID == paraWriteId
                     select yd_gjc).ToList();

            keywordList = (from yd_gjc in yd_gjc_dt
                           join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                           where yd_gjc.语段ID == paraWriteId
                           select new Keyword
                           {
                               关键词 = gjc.关键词,
                           }).ToList();

            keywordDataGrid.ItemsSource = x;
            keywordList2 = new List<Keyword>(keywordList);
            keywordDataGrid.CanUserAddRows = true;
            keywordDataGrid.CanUserDeleteRows = true;

            //更新匹配关键词
            List<ScienceResearchDataSetNew.关键词Row> gjc_list = new List<ScienceResearchDataSetNew.关键词Row>();
            for (int i = 0; i < dc_gjc_dt.Rows.Count; i++)
            {
                ScienceResearchDataSetNew.单词_关键词Row dc_gjc = (ScienceResearchDataSetNew.单词_关键词Row)dc_gjc_dt.Rows[i];
                string dc = dc_gjc.单词Row.单词;
                //ChangeColor(Colors.Blue, textboxInkcavasUserControl_using.paragraphRichTextBox, dc);

                TextRange textRange = new TextRange(textboxInkcavasUserControl_using.paragraphRichTextBox.Document.ContentStart, textboxInkcavasUserControl_using.paragraphRichTextBox.Document.ContentEnd);
                string yd_str = textRange.Text;

                if (yd_str.IndexOf(dc) > -1)
                {
                    gjc_list.Add(dc_gjc.关键词Row);

                }
            }
            keywordDataGrid2.ItemsSource = gjc_list;

            currentChild = textboxInkcavasUserControl_using;

            //改变状态栏文字
            var paper_list = (from wz in wz_dt
                              where wz.ID == paperId
                              select wz).ToList();
            int xiangmuId = 0;
            if (paper_list[0].项目Row != null)
                xiangmuId = paper_list[0].项目Row.ID;
            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "选中的语段为：ID=" + paraWriteId + "，文章ID=" + paperId + "，项目ID=" + xiangmuId;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            DataBaseRowManage.ProjectMapping(xiangmuId);

            //模式识别            
            zhinengPipeiUserControl.shibie_rtb(textboxInkcavasUserControl_using.paragraphRichTextBox);

        }
        #endregion

        #region 参考数据表操作
        private void keywordDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (keywordDataGrid.SelectedItem != null)
            {
                yd_gjc_write = keywordDataGrid.SelectedItem as ScienceResearchDataSetNew.语段_关键词Row;
                if (yd_gjc_write != null)
                {
                    //改变状态栏文字
                    MainWindow.mainWindow.statusBar.Items.Clear();
                    TextBlock txtb = new TextBlock();
                    txtb.Text = "选中的关键词为：ID=" + yd_gjc_write.关键词Row.ID + "，关键词=" + yd_gjc_write.关键词Row.关键词;
                    MainWindow.mainWindow.statusBar.Items.Add(txtb);

                    //改变参考语段表的内容
                    var data = from gjc in gjc_dt
                               join yd_gjc in yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                               join yd in yd_dt on yd_gjc.语段ID equals yd.ID
                               where gjc.ID == yd_gjc_write.关键词ID && yd.文章ID != paperId && (yd.文章Row == null || yd.文章Row.分类 != "创作")
                               select new ParagraphID
                               {
                                   语段ID = yd.ID
                               };
                    referDataGrid.ItemsSource = data;

                    //打开关键词映射表
                    btnZhiliao_Click(null, null);


                }
            }
        }

        private void keywordDataGrid_MouseLeftButtonUp2(object sender, MouseButtonEventArgs e)
        {
            if (keywordDataGrid2.SelectedItem != null)
            {
                ScienceResearchDataSetNew.关键词Row yd_gjc_write2 = keywordDataGrid2.SelectedItem as ScienceResearchDataSetNew.关键词Row;
                if (yd_gjc_write2 != null)
                {
                    //改变状态栏文字
                    MainWindow.mainWindow.statusBar.Items.Clear();
                    TextBlock txtb = new TextBlock();
                    txtb.Text = "选中的关键词为：ID=" + yd_gjc_write2.ID + "，关键词=" + yd_gjc_write2.关键词;
                    MainWindow.mainWindow.statusBar.Items.Add(txtb);

                    //改变参考语段表的内容

                    var data = from gjc in gjc_dt
                               join yd_gjc in yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                               join yd in yd_dt on yd_gjc.语段ID equals yd.ID
                               where gjc.ID == yd_gjc_write2.ID && yd.文章ID != paperId && (yd.文章Row == null || yd.文章Row.分类 != "创作")
                               select new ParagraphID
                               {
                                   语段ID = yd.ID
                               };
                    referDataGrid2.ItemsSource = data;
                }
            }
        }

        private void referDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var paragraphID = referDataGrid.CurrentItem as ParagraphID;
            int paraReferID = paragraphID.语段ID;
            paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid(referDataGrid, referKeywordDataGrid, paraReferID);
            paraResourceClass_PaperDataGrid.referDataGrid_MouseLeftButtonUp();
        }

        private void referDataGrid_MouseLeftButtonUp2(object sender, MouseButtonEventArgs e)
        {
            var paragraphID = referDataGrid2.CurrentItem as ParagraphID;
            int paraReferID = paragraphID.语段ID;
            paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid(referDataGrid2, referKeywordDataGrid2, paraReferID);
            paraResourceClass_PaperDataGrid.referDataGrid_MouseLeftButtonUp();
        }
        #endregion

        #region 窗体大小工具条
        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            if (first == 1)
            {
                columnWidth1 = grid.ColumnDefinitions[1].ActualWidth;
                columnWidth2 = grid.ColumnDefinitions[2].ActualWidth;
                columnWidth3 = grid.ColumnDefinitions[3].ActualWidth;
                first++;
            }
            else
            {
                grid.ColumnDefinitions[1].Width = new GridLength(columnWidth1);
                grid.ColumnDefinitions[2].Width = new GridLength(columnWidth2);
                grid.ColumnDefinitions[3].Width = new GridLength(columnWidth3);
            }
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            if (first == 1)
            {
                columnWidth1 = grid.ColumnDefinitions[1].ActualWidth;
                columnWidth2 = grid.ColumnDefinitions[2].ActualWidth;
                columnWidth3 = grid.ColumnDefinitions[3].ActualWidth;
                first++;
            }
            grid.ColumnDefinitions[1].Width = new GridLength(0);
            grid.ColumnDefinitions[2].Width = new GridLength(0);
            grid.ColumnDefinitions[3].Width = new GridLength(0);


            if (paper_type == "word")
            {
                SetParent(windowsFormsHostUserControl.handle_application, windowsFormsHostUserControl.handle_panel);
                SetForegroundWindow(windowsFormsHostUserControl.handle_application);
                SendMessage(windowsFormsHostUserControl.handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
            }
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            if (location == "left")
                MainWindow.mainWindow.right_zero();
            else
                MainWindow.mainWindow.left_zero();
        }
        #endregion        

        #region 文本、笔迹、刷新、另存工具条
        private void textRadioButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 0);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 1);
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.None;
                }
            }
        }

        private void inkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    ((TextboxInkcavasUserControl)t).inkCanvas.SetValue(Grid.ZIndexProperty, 1);
                    ((TextboxInkcavasUserControl)t).paragraphRichTextBox.SetValue(Grid.ZIndexProperty, 0);
                    ((TextboxInkcavasUserControl)t).inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.Ink;
                }
            }
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
                    ((TextboxInkcavasUserControl)t).inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                }
                else
                {
                    ((ParagraphFigureUserControl)t).canvas.EditingMode = InkCanvasEditingMode.Ink;
                }
            }
        }

        /// <summary>
        /// 保存文本文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTextSave_Click(object sender, RoutedEventArgs e)
        {
            string paper_string = "";

            var data = from data_item in yd_dt
                       where data_item.文章ID == paperId
                       orderby data_item.排序 ascending
                       select data_item;
            //foreach (var d in data)
            //{
            //    if (d["分类"].ToString() != "图片语段")
            //    {
            //        string yuduan = d.语段;
            //        paper_string = paper_string + yuduan + Environment.NewLine + Environment.NewLine;
            //    }
            //}

            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    TextboxInkcavasUserControl tb = (TextboxInkcavasUserControl)t;
                    RichTextBox rtb = tb.paragraphRichTextBox;
                    string rtb_str = TextFile.GetStringOfRichTextBox(rtb);
                    paper_string = paper_string + rtb_str;
                }
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string localFilePath = "";
            saveFileDialog.Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                localFilePath = saveFileDialog.FileName.ToString();
                TextFile.SaveStringToFile(paper_string, localFilePath);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //重新生成
            paperStackPanel.Children.Clear();
            loadPaper();
        }
        #endregion

        #region 语段管理工具条
        

        private void textParagraphInsertMenuItem_Click(object sender, RoutedEventArgs e)
        {            
            int currentChildInt=paperStackPanel.Children.IndexOf(currentChild);

            DataRow newRow;
            newRow = yd_dt.NewRow();
            newRow["文章Id"] = paperId;

            var para_list = (from data_item in yd_dt
                       where data_item.文章ID == paperId
                       orderby data_item.排序 ascending
                       select data_item).ToList();

            if (currentChildInt < 0)
            {
                MessageBox.Show("请选中一个语段");
                return;
            }

            if (currentChildInt == 0)
            {
                double paixu1 = para_list[currentChildInt].排序;
                newRow["排序"] = paixu1 / 2;
            }
            else
            {
                double paixu1 = para_list[currentChildInt].排序;
                double paixu2 = para_list[currentChildInt - 1].排序;
                newRow["排序"] = (paixu1 + paixu2) / 2;
            }

            yd_dt.Rows.Add(newRow);
            yd_ta.Update(yd_dt);
            
            sql = "select max(ID) from 语段";
            comm = new OleDbCommand(sql, MainWindow.conn);
            int maxID = (int)comm.ExecuteScalar();
            //newRow["ID"] = maxID;
            string path_isf = "";
            path_isf = newRow["语段isf"].ToString();
            if (path_isf == "")
            {
                //保存数据库
                string isfstr=@".\科学研究\语段ISF\" + maxID + ".isf";
                sql = "UPDATE 语段 SET 语段isf =\"" + isfstr + "\" WHERE ID =" + maxID;
                comm = new OleDbCommand(sql, MainWindow.conn);
                comm.ExecuteNonQuery();
                //newRow["语段isf"] = isfstr;
                //yd_ta.Update(yd_dt);
                path_isf = isfstr;
            }

            textboxInkcavasUserControl = new TextboxInkcavasUserControl(path_isf);
            textboxInkcavasUserControl.GotFocus += textboxInkcavasUserControl_GotFocus;
            textboxInkcavasUserControl.StylusDown += TextboxInkcavasUserControl_StylusDown;
            textboxInkcavasUserControl.PreviewMouseLeftButtonDown += TextboxInkcavasUserControl_PreviewMouseLeftButtonDown;

            textboxInkcavasUserControl.yd_dt = yd_dt;
            textboxInkcavasUserControl.yd_ta = yd_ta;

            textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_cz_width;
            textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_cz_width;

            //yd_ta.Fill(yd_dt);
            var data = from data_item in yd_dt
                       where data_item.ID == maxID
                       select data_item;
            foreach (var d in data)
            {
                Binding binding = new Binding();
                binding.Source = d;
                binding.Path = new PropertyPath("语段");
                binding.Mode = BindingMode.TwoWay;
                textboxInkcavasUserControl.paragraphRichTextBox.SetBinding(TextBox.TextProperty, binding);
                paperStackPanel.Children.Insert(currentChildInt, textboxInkcavasUserControl);
                paperStackPanel.UpdateLayout();
                textboxInkcavasUserControl.yd_id = maxID;
            }                
        }

        private void figureParagraphInsertMenuItem_Click(object sender, RoutedEventArgs e)
        {
            int currentChildInt = paperStackPanel.Children.IndexOf(currentChild);

            DataRow newRow;
            newRow = yd_dt.NewRow();
            newRow["文章Id"] = paperId;

            var para_list = (from data_item in yd_dt
                             where data_item.文章ID == paperId
                             orderby data_item.排序 ascending
                             select data_item).ToList();

            if (currentChildInt < 0)
            {
                MessageBox.Show("请选中一个语段");
                return;
            }

            if (currentChildInt == 0)
            {
                double paixu1 = para_list[currentChildInt].排序;
                newRow["排序"] = paixu1 / 2;
            }
            else
            {
                double paixu1 = para_list[currentChildInt].排序;
                double paixu2 = para_list[currentChildInt - 1].排序;
                newRow["排序"] = (paixu1 + paixu2) / 2;
            }
            newRow["分类"] = "图片语段";

            yd_dt.Rows.Add(newRow);
            yd_ta.Update(yd_dt);

            sql = "select max(ID) from 语段";
            comm = new OleDbCommand(sql, MainWindow.conn);
            int maxID = (int)comm.ExecuteScalar();
            //newRow["ID"] = maxID;
            string path_isf = "";
            path_isf = newRow["语段isf"].ToString();
            if (path_isf == "")
            {
                //保存数据库
                string isfstr = @".\科学研究\图片ISF\" + maxID + ".isf";
                sql = "UPDATE 语段 SET 图片isf =\"" + isfstr + "\" WHERE ID =" + maxID;
                comm = new OleDbCommand(sql, MainWindow.conn);
                comm.ExecuteNonQuery();
                //newRow["语段isf"] = isfstr;
                //yd_ta.Update(yd_dt);
                path_isf = isfstr;
            }


            string figure_path = "";
            string figure_path_isf = "";
            figure_path_isf = path_isf;

            figure_path = MainWindow.path_translate(figure_path);
            figure_path_isf = MainWindow.path_translate(figure_path_isf);


            ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
            if (File.Exists(figure_path))
            {
                //存在图片
                ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                paragraphFigureUserControl.img.Source = img;
            }
            else
            {
                //不存在图片的时候                        
            }
            paragraphFigureUserControl.grid.RowDefinitions[0].Height = new GridLength(0);
            paragraphFigureUserControl.paragraphId = maxID;
            paragraphFigureUserControl.GotFocus += ParagraphFigureUserControl_GotFocus;
            paragraphFigureUserControl.StylusDown += ParagraphFigureUserControl_StylusDown;
            paragraphFigureUserControl.PreviewMouseLeftButtonDown += ParagraphFigureUserControl_PreviewMouseLeftButtonDown;

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
                figure_path_isf = ".\\科学研究\\图片ISF\\" + maxID + ".isf";

                //更新数据库
                var data5 = from data_item in yd_dt
                            where data_item.ID == maxID
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
            paperStackPanel.Children.Insert(currentChildInt, paragraphFigureUserControl);
            paperStackPanel.UpdateLayout();
            //yd_ta.Fill(yd_dt);
        }

        private void textParagraphInsertHouMenuItem_Click(object sender, RoutedEventArgs e)
        {
            int currentChildInt = paperStackPanel.Children.IndexOf(currentChild);

            DataRow newRow;
            newRow = yd_dt.NewRow();
            newRow["文章Id"] = paperId;

            var para_list = (from data_item in yd_dt
                             where data_item.文章ID == paperId
                             orderby data_item.排序 ascending
                             select data_item).ToList();

            if (currentChildInt < 0)
            {
                MessageBox.Show("请选中一个语段");
                return;
            }

            if (currentChildInt == paperStackPanel.Children.Count-1)
            {
                double paixu1 = para_list[currentChildInt].排序;
                newRow["排序"] = paixu1+1;
            }
            else
            {
                double paixu1 = para_list[currentChildInt].排序;
                double paixu2 = para_list[currentChildInt + 1].排序;
                newRow["排序"] = (paixu1 + paixu2) / 2;
            }

            yd_dt.Rows.Add(newRow);
            yd_ta.Update(yd_dt);

            sql = "select max(ID) from 语段";
            //comm = new OleDbCommand(sql, MainWindow.conn);
            //int maxID = (int)comm.ExecuteScalar();
            ////newRow["ID"] = maxID;
            int maxID = new_yd_id;

            string path_isf = "";
            path_isf = newRow["语段isf"].ToString();
            if (path_isf == "")
            {
                //保存数据库
                string isfstr = @".\科学研究\语段ISF\" + maxID + ".isf";
                sql = "UPDATE 语段 SET 语段isf =\"" + isfstr + "\" WHERE ID =" + maxID;
                comm = new OleDbCommand(sql, MainWindow.conn);
                comm.ExecuteNonQuery();
                //newRow["语段isf"] = isfstr;
                //yd_ta.Update(yd_dt);
                path_isf = isfstr;
            }

            textboxInkcavasUserControl = new TextboxInkcavasUserControl(path_isf);
            textboxInkcavasUserControl.GotFocus += textboxInkcavasUserControl_GotFocus;
            textboxInkcavasUserControl.StylusDown += TextboxInkcavasUserControl_StylusDown;
            textboxInkcavasUserControl.PreviewMouseLeftButtonDown += TextboxInkcavasUserControl_PreviewMouseLeftButtonDown;

            textboxInkcavasUserControl.yd_dt = yd_dt;
            textboxInkcavasUserControl.yd_ta = yd_ta;

            textboxInkcavasUserControl.paragraphRichTextBox.Width = MainWindow.yd_cz_width;
            textboxInkcavasUserControl.inkCanvas.Width = MainWindow.yd_cz_width;

            //yd_ta.Fill(yd_dt);
            var data = from data_item in yd_dt
                       where data_item.ID == maxID
                       select data_item;
            foreach (var d in data)
            {
                Binding binding = new Binding();
                binding.Source = d;
                binding.Path = new PropertyPath("语段");
                binding.Mode = BindingMode.TwoWay;
                textboxInkcavasUserControl.paragraphRichTextBox.SetBinding(TextBox.TextProperty, binding);
                paperStackPanel.Children.Insert(currentChildInt+1, textboxInkcavasUserControl);
                paperStackPanel.UpdateLayout();
                textboxInkcavasUserControl.yd_id = maxID;
            }
        }

        private void figureParagraphInsertHouMenuItem_Click(object sender, RoutedEventArgs e)
        {
            int currentChildInt = paperStackPanel.Children.IndexOf(currentChild);

            DataRow newRow;
            newRow = yd_dt.NewRow();
            newRow["文章Id"] = paperId;

            var para_list = (from data_item in yd_dt
                             where data_item.文章ID == paperId
                             orderby data_item.排序 ascending
                             select data_item).ToList();

            if (currentChildInt < 0)
            {
                MessageBox.Show("请选中一个语段");
                return;
            }
            
            if (currentChildInt == paperStackPanel.Children.Count - 1)
            {
                double paixu1 = para_list[currentChildInt].排序;
                newRow["排序"] = paixu1 + 1;
            }
            else
            {
                double paixu1 = para_list[currentChildInt].排序;
                double paixu2 = para_list[currentChildInt + 1].排序;
                newRow["排序"] = (paixu1 + paixu2) / 2;
            }
            newRow["分类"] = "图片语段";

            yd_dt.Rows.Add(newRow);
            yd_ta.Update(yd_dt);

            sql = "select max(ID) from 语段";
            comm = new OleDbCommand(sql, MainWindow.conn);
            int maxID = (int)comm.ExecuteScalar();
            //newRow["ID"] = maxID;
            string path_isf = "";
            path_isf = newRow["语段isf"].ToString();
            if (path_isf == "")
            {
                //保存数据库
                string isfstr = @".\科学研究\图片ISF\" + maxID + ".isf";
                sql = "UPDATE 语段 SET 图片isf =\"" + isfstr + "\" WHERE ID =" + maxID;
                comm = new OleDbCommand(sql, MainWindow.conn);
                comm.ExecuteNonQuery();
                //newRow["语段isf"] = isfstr;
                //yd_ta.Update(yd_dt);
                path_isf = isfstr;
            }


            string figure_path = "";
            string figure_path_isf = "";
            figure_path_isf = path_isf;

            figure_path = MainWindow.path_translate(figure_path);
            figure_path_isf = MainWindow.path_translate(figure_path_isf);


            ParagraphFigureUserControl paragraphFigureUserControl = new ParagraphFigureUserControl();
            if (File.Exists(figure_path))
            {
                //存在图片
                ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                paragraphFigureUserControl.img.Source = img;
            }
            else
            {
                //不存在图片的时候                        
            }
            paragraphFigureUserControl.grid.RowDefinitions[0].Height = new GridLength(0);
            paragraphFigureUserControl.paragraphId = maxID;
            paragraphFigureUserControl.GotFocus += ParagraphFigureUserControl_GotFocus;
            paragraphFigureUserControl.StylusDown += ParagraphFigureUserControl_StylusDown;
            paragraphFigureUserControl.PreviewMouseLeftButtonDown += ParagraphFigureUserControl_PreviewMouseLeftButtonDown;

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
                figure_path_isf = ".\\科学研究\\图片ISF\\" + maxID + ".isf";

                //更新数据库
                var data5 = from data_item in yd_dt
                            where data_item.ID == maxID
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
            paperStackPanel.Children.Insert(currentChildInt+1, paragraphFigureUserControl);
            paperStackPanel.UpdateLayout();
            //yd_ta.Fill(yd_dt);

        }

        private void paragraphDeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            int currentChildInt = paperStackPanel.Children.IndexOf(currentChild);
            var para_list = (from data_item in yd_dt
                             where data_item.文章ID == paperId
                             orderby data_item.排序 ascending
                             select data_item).ToList();

            if (currentChildInt != -1)
            {
                int yd_selcted = para_list[currentChildInt].ID;

                DataBaseRowManage.DeleteYd(para_list[currentChildInt]);
                paperStackPanel.Children.RemoveAt(currentChildInt);
                paperStackPanel.UpdateLayout();

            }
            else
            {
                MessageBox.Show("请选中要删除的语段！");
            }
        }        

        private void tiaozhuanMunuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValuePath = "ID";
            MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValue = paraWriteId;

            if (MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedItem != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_yd;
                MainWindow.dataBaseUserControl_yd.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedItem);
            }
            else
                MessageBox.Show("请选中一个语段，再点击跳转按钮");
        }
        #endregion

        #region 宏观工具条
        /// <summary>
        /// 宏观视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHongguan_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.hongguanList.Contains(paperId))
            {
                WriteReferUserControl writeReferUserControl = new WriteReferUserControl(paperId);
                TabItem writeReferTabItem = new TabItem();
                Label headerLabel = new Label();
                headerLabel.Content = "宏:" + paperId;
                headerLabel.MouseDoubleClick += headerLabel_hongguan_MouseDoubleClick;
                writeReferTabItem.Header = headerLabel;
                writeReferTabItem.Content = writeReferUserControl;
                MainWindow.mainWindow.rightTabControl.Items.Add(writeReferTabItem);
                MainWindow.hongguanList.Add(paperId);
            }
        }

        /// <summary>
        /// 双击宏观视图标签页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerLabel_hongguan_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.hongguanList.Remove(((WriteReferUserControl)(((TabItem)MainWindow.mainWindow.rightTabControl.SelectedItem).Content)).paperId);
            MainWindow.mainWindow.rightTabControl.Items.Remove(MainWindow.mainWindow.rightTabControl.SelectedItem);

        }

        /// <summary>
        /// 选择变化时，查询该关键词对应的关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZhiliao_Click(object sender, RoutedEventArgs e)
        {
            if (yd_gjc_write != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.keywordMappingTabItem;
                DataBaseRowManage.KeywordMapping(yd_gjc_write.关键词ID);
            }
            else
            {
                MessageBox.Show("请选择关键词");
            }
        }

        /// <summary>
        /// 让文档的背景颜色变为透明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTouming_Click(object sender, RoutedEventArgs e)
        {
            foreach (object t in paperStackPanel.Children)
            {
                string name = t.GetType().FullName;
                if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
                {
                    TextboxInkcavasUserControl tb = (TextboxInkcavasUserControl)t;
                    RichTextBox rtb = tb.paragraphRichTextBox;
                    FlowDocument doc = rtb.Document;

                    foreach (Paragraph p in doc.Blocks)
                    {
                        foreach (Run r in p.Inlines)
                        {
                            r.Background = new SolidColorBrush(Colors.Transparent);
                        }
                    }
                    tb.textboxLostFocus();
                }
            }
        }
        #endregion

        #region 沟通工具条
        private void goutongRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //沟通
            paperGrid.RowDefinitions[1].Height = new GridLength(0);
            paperGrid.RowDefinitions[2].Height = new GridLength(2, GridUnitType.Star);

            MainWindow.mainWindow.includeRadioButton.IsChecked = true;


            windowsFormsHostUserControl = new WindowsFormsHostUserControl();
            TabItem windowsFormsHostTabItem = new TabItem();
            windowsFormsHostTabItem.Header = "外部应用程序";
            windowsFormsHostTabItem.Content = windowsFormsHostUserControl;
            scrolls2.Content = windowsFormsHostUserControl;

            IntPtr handle_application;
            if (goutongType=="weixin")
                //handle_application = FindWindow(null, "微信");
                handle_application = FindWindow("BRMainFrameGUI", null);
            //handle_application = FindWindow(null, "Microsoft Edge");
            else
                handle_application = FindWindow(null, "QQ");

            if (handle_application == IntPtr.Zero)
                MessageBox.Show("微信没有打开！");
            else
                windowsFormsHostUserControl.loadProcess2(handle_application);


        }

        private void btnQugou_Click(object sender, RoutedEventArgs e)
        {
            paperGrid.RowDefinitions[2].Height = new GridLength(0);
        }

        /// <summary>
        /// 沟通独立
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoutongDuli_Click(object sender, RoutedEventArgs e)
        {
            windowsFormsHostUserControl.Duli();
        }

        private void weixinRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            goutongType = "weixin";
        }

        private void qqRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            goutongType = "QQ";
        }
        #endregion

        #region 滚动条处理 
        private void hideScrollMunuItem_Click(object sender, RoutedEventArgs e)
        {
            scrolls.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private void viewScrollMunuItem_Click(object sender, RoutedEventArgs e)
        {
            scrolls.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private double GetVerticalOffset(FrameworkElement child, ScrollViewer scrollViewer)
        {
            // Ensure the control is scrolled into view in the ScrollViewer. 
            GeneralTransform focusedVisualTransform = child.TransformToVisual(scrollViewer);
            Point topLeft = focusedVisualTransform.Transform(new Point(child.Margin.Left, child.Margin.Top));
            Rect rectangle = new Rect(topLeft, child.RenderSize);
            //If the control is taller than the viewport, don't scroll down further than the top of the control. 
            double controlRectangleBottom = rectangle.Bottom - scrollViewer.ViewportHeight > scrollViewer.ViewportHeight ? scrollViewer.ViewportHeight : rectangle.Bottom;
            double newOffset = scrollViewer.VerticalOffset + (controlRectangleBottom - scrollViewer.ViewportHeight);
            return newOffset < 0 ? 0 : newOffset; // no use returning negative offset 
        }

        /// <summary>
        /// 打开同该语段相关的应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileMunuItem_Click(object sender, RoutedEventArgs e)
        {
            var para_list = (from data_item in yd_dt
                             where data_item.ID == paraWriteId
                             select data_item).ToList();

            string file_name = para_list[0].文件;

            MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
            MainWindow.applicationUserControl.headerStr = "语段";
            string s = MainWindow.path_translate(file_name);
            MainWindow.applicationUserControl.loadProcess(s);
        }

        private void paperStackPanel_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void scrolls_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 滚动到某一语段
        /// </summary>
        /// <param name="paraId">语段ID</param>
        public void scrollToParagraph(int paraId)
        {

            var child = paperStackPanel.Children[paraId];
            string name = child.GetType().FullName;

            if (name == "ScienceResearchWpfApplication.TextManage.TextboxInkcavasUserControl")
            {
                var myListBoxItem = (TextboxInkcavasUserControl)(child);

                //FrameworkElement element = myListBoxItem as FrameworkElement;
                //double distance = GetVerticalOffset(element, scrolls);
                //scrolls.ScrollToVerticalOffset(distance);


                // 获取要定位之前 ScrollViewer 目前的滚动位置
                var currentScrollPosition = scrolls.VerticalOffset;
                var point = new Point(0, currentScrollPosition);

                // 计算出目标位置并滚动
                var targetPosition = myListBoxItem.TransformToVisual(scrolls).Transform(point);
                scrolls.ScrollToVerticalOffset(targetPosition.Y);
            }
            else
            {
                //var myListBoxItem = (ParagraphFigureUserControl)(child);

                //// 获取要定位之前 ScrollViewer 目前的滚动位置
                //var currentScrollPosition = scrolls.VerticalOffset;
                //var point = new Point(0, currentScrollPosition);

                //// 计算出目标位置并滚动
                //var targetPosition = myListBoxItem.TransformToVisual(scrolls).Transform(point);
                //scrolls.ScrollToVerticalOffset(targetPosition.Y);
                MessageBox.Show("暂不支持图片语段的跳转");
            }
        }
        #endregion

        #region Word信息处理

        /// <summary>
        /// 打开word文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadWord_Click(object sender, RoutedEventArgs e)
        {
            paper_type = "word";
            MainWindow.mainWindow.includeRadioButton.IsChecked = false;

            string locationStr = wz_using.文件;
            locationStr = MainWindow.path_translate(locationStr);

            Process process = null;
            try
            {
                process = Process.Start(locationStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            SetParent(windowsFormsHostUserControl.handle_application, windowsFormsHostUserControl.handle_panel);
            SetForegroundWindow(windowsFormsHostUserControl.handle_application);
            SendMessage(windowsFormsHostUserControl.handle_application, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //关闭文件
            SendMessage(windowsFormsHostUserControl.handle_application, WM_CLOSE, 0, 0);
        }

        /// <summary>
        /// 嵌入word文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQianruWord_Click(object sender, RoutedEventArgs e)
        {
            paper_type = "word";
            MainWindow.mainWindow.includeRadioButton.IsChecked = true;

            string locationStr = wz_using.文件;
            locationStr = MainWindow.path_translate(locationStr);

            string fileName1 = locationStr.Substring(locationStr.LastIndexOf("\\")+1)+ "  -  兼容性模式 - Word";
            string fileName2 = locationStr.Substring(locationStr.LastIndexOf("\\") + 1) + " - Word";

            if (MainWindow.app_included)
            {
                windowsFormsHostUserControl = new WindowsFormsHostUserControl();
                scrolls.Content = windowsFormsHostUserControl;
                windowsFormsHostUserControl.wordPaperUserControl = this;          
                IntPtr handle_application = FindWindow(null, fileName1);
                if (handle_application==null)
                    handle_application = FindWindow(null, fileName2);

                try
                {
                    windowsFormsHostUserControl.loadProcess2(handle_application);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 使word文件保存独立
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDuliWord_Click(object sender, RoutedEventArgs e)
        {
            windowsFormsHostUserControl.Duli();
        }


        private void referXiaRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            referUserControl_using = referUserControl;
            paperGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            MainWindow.mainWindow.right_zero();
        }

        private void referYouRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            referUserControl_using = MainWindow.referUserControl;
            paperGrid.RowDefinitions[1].Height = new GridLength(0);
            //MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.referTabItem;
            MainWindow.mainWindow.half();
        }

        /// <summary>
        /// Word识别
        /// </summary>
        public void WordProcess_shibie()
        {
            //读文件
            string path = MainWindow.path_database;
            path = path + "\\Mode_Word_Using.txt";

            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            string yd_str = "";
            string content;
            while (( content = sr.ReadLine()) != null)
            {
                yd_str = yd_str + content;
            }
            sr.Close();

            List<ScienceResearchDataSetNew.关键词Row> gjc_list = new List<ScienceResearchDataSetNew.关键词Row>();
            for (int i = 0; i < dc_gjc_dt.Rows.Count; i++)
            {
                ScienceResearchDataSetNew.单词_关键词Row dc_gjc = (ScienceResearchDataSetNew.单词_关键词Row)dc_gjc_dt.Rows[i];
                string dc = dc_gjc.单词Row.单词;
                //ChangeColor(Colors.Blue, textboxInkcavasUserControl_using.paragraphRichTextBox, dc);
                
                if (yd_str.IndexOf(dc) > -1)
                {
                    gjc_list.Add(dc_gjc.关键词Row);
                }
            }
            keywordDataGrid2.ItemsSource = gjc_list;
        }

        /// <summary>
        /// Word匹配
        /// </summary>
        public void WordProcess_pipei()
        {
            //自动匹配相关信息
            string path = MainWindow.path_database;
            path = path + "\\Word.txt";

            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            string word_str = "";
            string content;
            while ((content = sr.ReadLine()) != null)
            {
                word_str = word_str + content;
            }
            sr.Close();

            word_str = word_str.Substring(1, word_str.Length - 2);
            zhinengPipeiUserControl.pipei_dc(word_str);

            MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.referTabItem;
        }

        /// <summary>
        /// Word收集
        /// </summary>
        public void WordProcess_shouji()
        {
            string path = MainWindow.path_database;
            path = path + "\\mode_str.txt";

            StreamReader sr = new StreamReader(path,System.Text.Encoding.Default);
            string word_str = "";
            string content;
            while ((content = sr.ReadLine()) != null)
            {
                word_str = word_str + content;
            }
            sr.Close();

            word_str = word_str.Substring(1, word_str.Length - 2);
            //zhinengPipeiUserControl.shouji_dc(word_str);
            zhinengPipeiUserControl.save_element_in_database(dc_dt, dc_ta.Adapter, "单词", word_str);
        }
        #endregion
    }
}
