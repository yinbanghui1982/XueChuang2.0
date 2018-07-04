using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Data;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Linq;
using System.IO;
using System;
using System.Windows.Ink;
using System.ComponentModel;
using System.Data.OleDb;
using System.Text;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication.DatabaseManage
{
    /// <summary>
    /// DataBaseUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataBaseUserControl : UserControl
    {
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;

        ScienceResearchDataSetNew.单词DataTable dc_dt;
        ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;
        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;

        ScienceResearchDataSetNew.短语DataTable dy_dt;
        ScienceResearchDataSetNewTableAdapters.短语TableAdapter dy_ta;
        ScienceResearchDataSetNew.短语_关键词DataTable dy_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.短语_关键词TableAdapter dy_gjc_ta;

        ScienceResearchDataSetNew.句型DataTable jx_dt;
        ScienceResearchDataSetNewTableAdapters.句型TableAdapter jx_ta;
        ScienceResearchDataSetNew.句型_关键词DataTable jx_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.句型_关键词TableAdapter jx_gjc_ta;

        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;

        ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;
        ScienceResearchDataSetNew.文章_关键词DataTable wz_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.文章_关键词TableAdapter wz_gjc_ta;

        ScienceResearchDataSetNew.图片DataTable tp_dt;
        ScienceResearchDataSetNewTableAdapters.图片TableAdapter tp_ta;
        ScienceResearchDataSetNew.图片_关键词DataTable tp_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片_关键词TableAdapter tp_gjc_ta;

        ScienceResearchDataSetNew.图片创作DataTable tpcz_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter tpcz_ta;
        ScienceResearchDataSetNew.图片创作_关键词DataTable tpcz_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作_关键词TableAdapter tpcz_gjc_ta;

        ScienceResearchDataSetNew.项目DataTable xm_dt;
        ScienceResearchDataSetNewTableAdapters.项目TableAdapter xm_ta;
        ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;

        ScienceResearchDataSetNew.文件位置DataTable wjwz_dt;
        ScienceResearchDataSetNewTableAdapters.文件位置TableAdapter wjwz_ta;

        ScienceResearchDataSetNew.仿真DataTable fz_dt;
        ScienceResearchDataSetNewTableAdapters.仿真TableAdapter fz_ta;

        List<string> columnNameList;
        List<string> columnWidthList;
        ScrollViewer scroll_text;
        int rowHeight;
        string type;

        RegistryKey scienceResearchKey;

        bool canContentScroll;
        string figure_path_isf="";

        string keywordSource;      //关键词来源，all——数据表，shaixuan——筛选 

        bool is_contentDataGrid_GotFocus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_rowHeight"></param>
        /// <param name="_columnNameList"></param>
        /// <param name="_columnWidthList"></param>
        public DataBaseUserControl(string _type, int _rowHeight,List<string> _columnNameList, List<string> _columnWidthList)
        {
            InitializeComponent();

            canContentScroll = true;
            type = _type;

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;
            dc_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            dy_dt = MainWindow.dy_dt;
            dy_ta = MainWindow.dy_ta;
            dy_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            dy_gjc_dt = MainWindow.dy_gjc_dt;
            dy_gjc_ta = MainWindow.dy_gjc_ta;
            dy_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            jx_dt = MainWindow.jx_dt;
            jx_ta = MainWindow.jx_ta;
            jx_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            jx_gjc_dt = MainWindow.jx_gjc_dt;
            jx_gjc_ta = MainWindow.jx_gjc_ta;
            jx_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
            yd_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;
            yd_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;
            wz_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            wz_gjc_dt = MainWindow.wz_gjc_dt;
            wz_gjc_ta = MainWindow.wz_gjc_ta;
            wz_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            xm_dt = MainWindow.xm_dt;
            xm_ta = MainWindow.xm_ta;
            xm_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            xm_gjc_dt = MainWindow.xm_gjc_dt;
            xm_gjc_ta = MainWindow.xm_gjc_ta;
            xm_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            tp_dt = MainWindow.tp_dt;
            tp_ta = MainWindow.tp_ta;
            tp_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            tp_gjc_dt = MainWindow.tp_gjc_dt;
            tp_gjc_ta = MainWindow.tp_gjc_ta;
            //tp_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            tpcz_dt = MainWindow.tpcz_dt;
            tpcz_ta = MainWindow.tpcz_ta;
            tpcz_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            tpcz_gjc_dt = MainWindow.tpcz_gjc_dt;
            tpcz_gjc_ta = MainWindow.tpcz_gjc_ta;
            tpcz_gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated2;

            wjwz_dt = MainWindow.wjwz_dt;
            wjwz_ta = MainWindow.wjwz_ta;
            wjwz_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            fz_dt = MainWindow.fz_dt;
            fz_ta = MainWindow.fz_ta;
            fz_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            rowHeight = _rowHeight;
            contentDataGrid.RowHeight = rowHeight;
            columnNameList = _columnNameList;
            columnWidthList = _columnWidthList;
            //创建列
            for (int i = 0; i < columnNameList.Count; i++)
            {
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn() { Header = columnNameList[i], Binding = new Binding(columnNameList[i]) };
                if (columnWidthList[i].IndexOf('*') == -1)
                {
                    DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i]));
                    dataGridTextColumn.Width = dl;
                }
                else
                {
                    DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i].Substring(0, columnWidthList[i].Length - 1)), DataGridLengthUnitType.Star);
                    dataGridTextColumn.Width = dl;
                }

                contentDataGrid.Columns.Add(dataGridTextColumn);
            }

            //填充数据                  
            if (type == "仿真")
            {
                contentDataGrid.ItemsSource = fz_dt.DefaultView;
                inkCanvas.Height = rowHeight * (fz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\fz.isf";

                grid.ColumnDefinitions[1].Width = new GridLength(0);
                grid.ColumnDefinitions[2].Width = new GridLength(0);
                toolBar3.Visibility = Visibility.Collapsed;
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "位置")
            {
                contentDataGrid.ItemsSource = wjwz_dt;
                inkCanvas.Height = rowHeight * (wjwz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\wjwz.isf";

                grid.ColumnDefinitions[1].Width = new GridLength(0);
                grid.ColumnDefinitions[2].Width = new GridLength(0);
                toolBar3.Visibility = Visibility.Collapsed;
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "关键词")
            {
                contentDataGrid.ItemsSource = gjc_dt.DefaultView;
                //var data_gjc = from gjc in gjc_dt
                //           select gjc;
                //contentDataGrid.ItemsSource = data_gjc;

                inkCanvas.Height = rowHeight * (gjc_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\gjc.isf";

                grid.ColumnDefinitions[1].Width = new GridLength(0);
                grid.ColumnDefinitions[2].Width = new GridLength(0);
                //toolBar3.Items.Remove(btnAddKeyword);
                //toolBar3.Items.Remove(btnRemoveKeyword);
                //toolBar3.Items.Remove(keywordTextBox);
                //toolBar3.Items.Remove(btnResearch);
                toolBar3.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;

                for (int i = 0; i < columnNameList.Count; i++)
                {
                    DataGridTextColumn dataGridTextColumn = new DataGridTextColumn() { Header = columnNameList[i], Binding = new Binding(columnNameList[i]) };
                    if (columnWidthList[i].IndexOf('*') == -1)
                    {
                        DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i]));
                        dataGridTextColumn.Width = dl;
                    }
                    else
                    {
                        DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i].Substring(0, columnWidthList[i].Length - 1)), DataGridLengthUnitType.Star);
                        dataGridTextColumn.Width = dl;
                    }

                    contentSelectedDataGrid.Columns.Add(dataGridTextColumn);
                }
                contentSelectedDataGrid.RowHeight = rowHeight;
            }
            else if (type == "单词")
            {
                contentDataGrid.ItemsSource = dc_dt.DefaultView;
                inkCanvas.Height = rowHeight * (dc_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\dc.isf";
                toolBar4.Visibility = Visibility.Collapsed;
            }
            else if (type == "短语")
            {
                contentDataGrid.ItemsSource = dy_dt.DefaultView;
                inkCanvas.Height = rowHeight * (dy_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\dy.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "句型")
            {
                contentDataGrid.ItemsSource = jx_dt.DefaultView; 
                inkCanvas.Height = rowHeight * (jx_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\jx.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "语段")
            {
                contentDataGrid.ItemsSource = yd_dt.DefaultView;
                inkCanvas.Height = rowHeight * (yd_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\yd.isf";
                //toolBar4.Items.Remove(btnContentJump);
                //toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;

                for (int i = 0; i < columnNameList.Count; i++)
                {
                    DataGridTextColumn dataGridTextColumn = new DataGridTextColumn() { Header = columnNameList[i], Binding = new Binding(columnNameList[i]) };
                    if (columnWidthList[i].IndexOf('*') == -1)
                    {
                        DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i]));
                        dataGridTextColumn.Width = dl;
                    }
                    else
                    {
                        DataGridLength dl = new DataGridLength(double.Parse(columnWidthList[i].Substring(0, columnWidthList[i].Length - 1)), DataGridLengthUnitType.Star);
                        dataGridTextColumn.Width = dl;
                    }

                    contentSelectedDataGrid.Columns.Add(dataGridTextColumn);
                }
                contentSelectedDataGrid.RowHeight = rowHeight;
            }
            
            else if (type == "文章")
            {
                contentDataGrid.ItemsSource = wz_dt.DefaultView;
                inkCanvas.Height = rowHeight * (wz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\wz.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "项目")
            {
                contentDataGrid.ItemsSource = xm_dt.DefaultView;
                inkCanvas.Height = rowHeight * (xm_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\xm.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "图片")
            {
                contentDataGrid.ItemsSource = tp_dt.DefaultView;
                inkCanvas.Height = rowHeight * (tp_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\tp.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }
            else if (type == "图片创作")
            {
                contentDataGrid.ItemsSource = tpcz_dt.DefaultView;
                inkCanvas.Height = rowHeight * (tpcz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
                figure_path_isf = @".\科学研究\图片ISF\tpcz.isf";
                toolBar4.Visibility = Visibility.Collapsed;
                toolBar5.Visibility = Visibility.Collapsed;
            }

            //增加关键词
            //var data = from gjc in gjc_dt
            //           orderby gjc.关键词 ascending
            //           select gjc;
            //keywordAllDataGrid.ItemsSource = data;

            keywordAllDataGrid.ItemsSource = gjc_dt.DefaultView;
            keywordSource = "all";
            ICollectionView cvTasks = CollectionViewSource.GetDefaultView(keywordAllDataGrid.ItemsSource);
            if (cvTasks != null && cvTasks.CanSort == true)
            {
                cvTasks.SortDescriptions.Clear();
                cvTasks.SortDescriptions.Add(new SortDescription("关键词", ListSortDirection.Ascending));
            }

            scroll_text = GetVisualChild<ScrollViewer>(contentDataGrid);

            //contentDataGrid.Columns[0].SortDirection = System.ComponentModel.ListSortDirection.Ascending;

            ICollectionView view = CollectionViewSource.GetDefaultView(contentDataGrid.ItemsSource);
            view.SortDescriptions.Clear();
            SortDescription sd = new SortDescription("ID", ListSortDirection.Ascending);
            view.SortDescriptions.Add(sd);


            //加载墨笔
            figure_path_isf = MainWindow.path_translate(figure_path_isf);
            if (File.Exists(figure_path_isf))
            {
                FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                if (file_ink.Length != 0)
                {
                    inkCanvas.Strokes = new StrokeCollection(file_ink);
                }
                file_ink.Close();
            }
            contentDataGrid.CanUserAddRows = false;

            btnText.IsEnabled = false;
            btnInk.IsEnabled = false;
        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                e.Row["ID"] = newID;
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
                contentDataGrid.ScrollIntoView(contentDataGrid.Items[contentDataGrid.Items.Count - 1]);
            }
        }

        private void Adapter_RowUpdated2(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                e.Row["ID"] = newID;
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
            }
        }

        /// <summary>
        /// 批注单选框选中
        /// </summary>
        public void pizuCheckBox_Checked()
        {
            canContentScroll = false;
            if (!canContentScroll)
            {
                if (scroll_text != null)
                {
                    scroll_text.CanContentScroll = false;
                    scroll_ink.ScrollChanged += scroll_ink_ScrollChanged;
                    scroll_text.ScrollChanged += scrool_text_ScrollChanged;
                }
                contentDataGrid.Opacity = .8;
            }

            btnText.IsEnabled = true;
            btnInk.IsEnabled = true;
        }

        /// <summary>
        /// 批注选择框取消选择
        /// </summary>
        public void pizuCheckBox_UnChecked()
        {
            canContentScroll = true;
            scroll_text.CanContentScroll = true;
            scroll_ink.ScrollChanged -= scroll_ink_ScrollChanged;
            scroll_text.ScrollChanged -= scrool_text_ScrollChanged;
            contentDataGrid.Opacity = 1;
            scroll_ink.SetValue(Grid.ZIndexProperty, 0);
            contentDataGrid.SetValue(Grid.ZIndexProperty, 1);

            btnText.IsEnabled = false;
            btnInk.IsEnabled = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            scienceResearchKey = MainWindow.scienceResearchKey;
            string dataBaseInkOrTextString = scienceResearchKey.GetValue("DataBaseInkOrText").ToString();

            if (dataBaseInkOrTextString == "Text")
            {
                scroll_ink.SetValue(Grid.ZIndexProperty, 0);
                contentDataGrid.SetValue(Grid.ZIndexProperty, 1);
            }
            else
            {
                scroll_ink.SetValue(Grid.ZIndexProperty, 1);
                contentDataGrid.SetValue(Grid.ZIndexProperty, 0);
            }

            if (canContentScroll)
            {
                if (scroll_text == null)
                {
                    scroll_text = GetVisualChild<ScrollViewer>(contentDataGrid);
                }
                contentDataGrid.Opacity = 1;
            }

            if (!canContentScroll)
            {
                if (scroll_text == null)
                {
                    scroll_text = GetVisualChild<ScrollViewer>(contentDataGrid);
                    if (scroll_text != null)
                    {
                        scroll_text.CanContentScroll = false;
                        scroll_ink.ScrollChanged += scroll_ink_ScrollChanged;
                        scroll_text.ScrollChanged += scrool_text_ScrollChanged;
                    }
                }
            }
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.left_zero();
        }

        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            scroll_ink.SetValue(Grid.ZIndexProperty, 0);
            contentDataGrid.SetValue(Grid.ZIndexProperty, 1);
        }

        private void btnInk_Click(object sender, RoutedEventArgs e)
        {
            if (!canContentScroll)
            {
                scroll_ink.SetValue(Grid.ZIndexProperty, 1);
                contentDataGrid.SetValue(Grid.ZIndexProperty, 0);
            }
        }

        private void scrolls_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void scrool_text_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {       
            if (scroll_ink != null)
            {
                double a = e.VerticalOffset;
                scroll_ink.ScrollToVerticalOffset(a);
            }
        }

        private void scroll_ink_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (scroll_text != null)
            {
                double a = e.VerticalOffset;
                scroll_text.ScrollToVerticalOffset(a);
            }
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void contentDataGrid_StylusEnter(object sender, System.Windows.Input.StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                if (!canContentScroll)
                {
                    scroll_ink.SetValue(Grid.ZIndexProperty, 1);
                    contentDataGrid.SetValue(Grid.ZIndexProperty, 0);
                }
            }
            else if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (scroll_ink.GetValue(Grid.ZIndexProperty).ToString() =="0")
                {
                    scroll_ink.SetValue(Grid.ZIndexProperty, 1);
                    contentDataGrid.SetValue(Grid.ZIndexProperty, 0);
                }
                else
                {
                    scroll_ink.SetValue(Grid.ZIndexProperty, 0);
                    contentDataGrid.SetValue(Grid.ZIndexProperty, 1);
                }
            }
        }

        #region 数据库表选中一行
        private void contentDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            content_changed();
        }

        private void contentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (type != "文章")
                content_changed();
        }

        private void content_changed()
        {
            DataRowView currentItem;

            try
            {
                currentItem = (DataRowView)contentDataGrid.SelectedItem;
            }
            catch { return; }
            if (currentItem != null)
            {
                if (type == "位置")
                {
                    if (contentDataGrid.CurrentColumn != null && contentDataGrid.CurrentColumn.Header.ToString() == "位置" && (int)currentItem["ID"] > 0)
                    {
                        string locationStr = currentItem["位置"].ToString();
                        locationStr = MainWindow.path_translate(locationStr);
                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
                        MainWindow.applicationUserControl.headerStr = "位置";
                        MainWindow.applicationUserControl.loadProcess(locationStr);
                    }
                }
                else if (type == "单词")
                {
                    var dc_gjc_list = (from dc_gjc in dc_gjc_dt
                                       join gjc in gjc_dt on dc_gjc.关键词ID equals gjc.ID
                                       where dc_gjc.单词ID.ToString() == currentItem["ID"].ToString()
                                       select dc_gjc).ToList();
                    keywordDataGrid.ItemsSource = dc_gjc_list;
                }
                else if (type == "短语")
                {
                    var dy_gjc_list = (from dy_gjc in dy_gjc_dt
                                       join gjc in gjc_dt on dy_gjc.关键词ID equals gjc.ID
                                       where dy_gjc.短语ID.ToString() == currentItem["ID"].ToString()
                                       select dy_gjc).ToList();
                    keywordDataGrid.ItemsSource = dy_gjc_list;
                }
                else if (type == "句型")
                {
                    var jx_gjc_list = (from jx_gjc in jx_gjc_dt
                                       join gjc in gjc_dt on jx_gjc.关键词ID equals gjc.ID
                                       where jx_gjc.句型ID.ToString() == currentItem["ID"].ToString()
                                       select jx_gjc).ToList();
                    keywordDataGrid.ItemsSource = jx_gjc_list;
                }
                else if (type == "语段")
                {
                    var yd_gjc_list = (from yd_gjc in yd_gjc_dt
                                       join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                                       where yd_gjc.语段ID.ToString() == currentItem["ID"].ToString()
                                       select yd_gjc).ToList();
                    keywordDataGrid.ItemsSource = yd_gjc_list;                   

                    //墨笔文件
                    if (contentDataGrid.CurrentColumn != null && contentDataGrid.CurrentColumn.Header.ToString() == "图片" && (int)currentItem["ID"] > 0 && currentItem["图片"].GetType().ToString() != "System.DBNull")
                    {
                        ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.paragraphFigureTabItem_l;
                        paraResourceClass_PaperDataGrid.creat_figure( MainWindow.paragraphFigureUserControl_l, (ScienceResearchDataSetNew.语段Row)currentItem.Row);
                
                    }
                    if (contentDataGrid.CurrentColumn != null && contentDataGrid.CurrentColumn.Header.ToString() == "语段" && (int)currentItem["ID"] > 0 && currentItem["语段"].GetType().ToString() != "System.DBNull")
                    {
                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.referTabItem_duanwen_left;
                        ScienceResearchDataSetNew.语段Row yd = (ScienceResearchDataSetNew.语段Row)currentItem.Row;

                        for (int i = 0; i < MainWindow.referUserControl_duanwen_left.referTabControl.Items.Count; i++)
                        {
                            if (((TabItem)MainWindow.referUserControl_duanwen_left.referTabControl.Items[i]).Header.ToString() == yd.ID.ToString())
                            {
                                return;
                            }
                        }

                        ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                        paraResourceClass_PaperDataGrid.creat_text(yd, MainWindow.referUserControl_duanwen_left);

                    }
                }
                else if (type == "文章")
                {
                    //加载关键词
                    List<ScienceResearchDataSetNew.文章_关键词Row> wz_gjc_list = DataBaseRowManage.GetWzGjc((ScienceResearchDataSetNew.文章Row)currentItem.Row);
                    keywordDataGrid.ItemsSource = wz_gjc_list;

                    //加载文件
                    if (contentDataGrid.CurrentColumn != null && contentDataGrid.CurrentColumn.Header.ToString() == "文件" && (int)currentItem["ID"] > 0)
                    {
                        string locationStr = currentItem["文件"].ToString();
                        locationStr = MainWindow.path_translate(locationStr);
                        if (MainWindow.app_included)
                            MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
                        MainWindow.applicationUserControl.headerStr = "文章";
                        MainWindow.applicationUserControl.loadProcess(locationStr);
                    }

                    //加载text文件
                    if (contentDataGrid.CurrentColumn != null && contentDataGrid.CurrentColumn.Header.ToString() == "text文件" && (int)currentItem["ID"] > 0)
                    {
                        string locationStr = currentItem["text文件"].ToString();
                        if (locationStr == "")
                            return;

                        locationStr = MainWindow.path_translate(locationStr);
                        //if (MainWindow.app_included)
                        //    MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
                        //MainWindow.applicationUserControl.headerStr = "文章";
                        //MainWindow.applicationUserControl.loadProcess(locationStr);

                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.referTabItem_wenzhang_left;
                        ScienceResearchDataSetNew.文章Row wz = (ScienceResearchDataSetNew.文章Row)currentItem.Row;

                        for (int i = 0; i < MainWindow.referUserControl_wenzhang_left.referTabControl.Items.Count; i++)
                        {
                            if (((TabItem)MainWindow.referUserControl_wenzhang_left.referTabControl.Items[i]).Header.ToString() == wz.ID.ToString())
                            {
                                return;
                            }
                        }

                        ReferTabItemUserControl referTabItemUserControl = new ReferTabItemUserControl(wz);
                        TabItem referTabItemTabItem = new TabItem();
                        referTabItemTabItem.Header = wz.ID.ToString();
                        referTabItemTabItem.Content = referTabItemUserControl;
                        MainWindow.referUserControl_wenzhang_left.referTabControl.Items.Clear();
                        MainWindow.referUserControl_wenzhang_left.referTabControl.Items.Add(referTabItemTabItem);
                        MainWindow.referUserControl_wenzhang_left.referTabControl.SelectedItem = referTabItemTabItem;
                    }

                }
                else if (type == "项目")
                {
                    var xm_gjc_list = (from xm_gjc in xm_gjc_dt
                                       join gjc in gjc_dt on xm_gjc.关键词ID equals gjc.ID
                                       where xm_gjc.项目ID.ToString() == currentItem["ID"].ToString()
                                       select xm_gjc).ToList();
                    keywordDataGrid.ItemsSource = xm_gjc_list;
                }
                else if (type == "图片")
                {
                    int figureId = (int)currentItem["ID"];

                    var tp_gjc_list = (from tp_gjc in tp_gjc_dt
                                       join gjc in gjc_dt on tp_gjc.关键词ID equals gjc.ID
                                       where tp_gjc.图片ID.ToString() == currentItem["ID"].ToString()
                                       select tp_gjc).ToList();
                    keywordDataGrid.ItemsSource = tp_gjc_list;

                    //更新参考图片
                    if ((int)currentItem["ID"] > 0)
                    {
                        string figure_path = "";
                        string figure_path_isf = "";
                        var data2 = from tp in tp_dt
                                    where tp.ID == figureId
                                    select tp;
                        foreach (var d2 in data2)
                        {
                            try
                            {
                                figure_path = d2.图片.ToString();
                                figure_path_isf = d2.图片isf.ToString();
                            }
                            catch { }
                            finally { }

                        }
                        figure_path = MainWindow.path_translate(figure_path);
                        figure_path_isf = MainWindow.path_translate(figure_path_isf);

                        //墨笔文件
                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.paragraphFigureTabItem_l;
                        if (File.Exists(figure_path))
                        {
                            ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                            MainWindow.paragraphImage_l.Source = img;

                            if (File.Exists(figure_path_isf))
                            {
                                FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                                if (file_ink.Length != 0)
                                {
                                    MainWindow.scienceResearchInkCanvas_l.Strokes = new StrokeCollection(file_ink);
                                }
                                file_ink.Close();
                                MainWindow.paragraphFigureUserControl_l.figure_path_isf = figure_path_isf;
                            }
                            else
                            {
                                MainWindow.scienceResearchInkCanvas_l.Strokes.Clear();
                                figure_path_isf = ".\\科学研究\\图片ISF\\图片_" + figureId.ToString() + ".isf";

                                //更新数据库
                                var tp_data = from tp in tp_dt
                                              where tp.ID == figureId
                                              select tp;
                                foreach (var tp in tp_data)
                                {
                                    tp.图片isf = figure_path_isf;
                                    tp_ta.Update(tp_dt);
                                }

                                //更新墨笔文件路径
                                figure_path_isf = MainWindow.path_translate(figure_path_isf);
                                MainWindow.paragraphFigureUserControl_l.figure_path_isf = figure_path_isf;
                            }
                        }
                        else
                        {
                            MessageBox.Show("图片文件不存在");
                        }
                    }
                }
                else if (type == "图片创作")
                {
                    var tpcz_gjc_list = (from tpcz_gjc in tpcz_gjc_dt
                                         join gjc in gjc_dt on tpcz_gjc.关键词ID equals gjc.ID
                                         where tpcz_gjc.图片创作ID.ToString() == currentItem["ID"].ToString()
                                         select tpcz_gjc).ToList();
                    keywordDataGrid.ItemsSource = tpcz_gjc_list;
                }
            }
        }

        
        #endregion

        #region 关键词操作
        /// <summary>
        /// 增加关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddKeyword_Click(object sender, RoutedEventArgs e)
        {
            if (type == "单词")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow dc_gjc_dr = dc_gjc_dt.NewRow();
                    
                    var dc = (DataRowView)contentDataGrid.SelectedItem;

                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (keywordSource == "all")
                    {
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    }
                    else

                        gjc = keywordAllDataGrid.SelectedItem as ScienceResearchDataSetNew.关键词Row;


                    dc_gjc_dr["关键词ID"] = gjc["ID"];
                    dc_gjc_dr["单词ID"] = dc["ID"];
                    dc_gjc_dt.Rows.Add(dc_gjc_dr);
                    dc_gjc_ta.Update(dc_gjc_dt);
                    //dc_gjc_ta.Fill(dc_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var dc_gjc_list = (from dc_gjc in dc_gjc_dt
                                       join gjc2 in gjc_dt on dc_gjc.关键词ID equals gjc2.ID
                                       where dc_gjc.单词ID.ToString() == word["ID"].ToString()
                                       select dc_gjc).ToList();
                    keywordDataGrid.ItemsSource = dc_gjc_list;
                }
                else
                {
                    MessageBox.Show("单词或者关键词没有选中");
                }
            }
            else if (type == "短语")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow dy_gjc_dr = dy_gjc_dt.NewRow();

                    var dy = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    dy_gjc_dr["关键词ID"] = gjc["ID"];
                    dy_gjc_dr["短语ID"] = dy["ID"];
                    dy_gjc_dt.Rows.Add(dy_gjc_dr);
                    dy_gjc_ta.Update(dy_gjc_dt);
                    //dy_gjc_ta.Fill(dy_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var dy_gjc_list = (from dy_gjc in dy_gjc_dt
                                       join gjc2 in gjc_dt on dy_gjc.关键词ID equals gjc2.ID
                                       where dy_gjc.短语ID.ToString() == word["ID"].ToString()
                                       select dy_gjc).ToList();
                    keywordDataGrid.ItemsSource = dy_gjc_list;
                }
                else
                {
                    MessageBox.Show("短语或者关键词没有选中");
                }
            }
            else if (type == "句型")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow jx_gjc_dr = jx_gjc_dt.NewRow();

                    var jx = (DataRowView)contentDataGrid.SelectedItem;

                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    
                    jx_gjc_dr["关键词ID"] = gjc["ID"];
                    jx_gjc_dr["句型ID"] = jx["ID"];
                    jx_gjc_dt.Rows.Add(jx_gjc_dr);
                    jx_gjc_ta.Update(jx_gjc_dt);
                    //jx_gjc_ta.Fill(jx_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var jx_gjc_list = (from jx_gjc in jx_gjc_dt
                                       join gjc2 in gjc_dt on jx_gjc.关键词ID equals gjc2.ID
                                       where jx_gjc.句型ID.ToString() == word["ID"].ToString()
                                       select jx_gjc).ToList();
                    keywordDataGrid.ItemsSource =jx_gjc_list;
                }
                else
                {
                    MessageBox.Show("句型或者关键词没有选中");
                }
            }
            else if (type == "语段")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow yd_gjc_dr = yd_gjc_dt.NewRow();

                    var yd = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    yd_gjc_dr["关键词ID"] = gjc["ID"];
                    yd_gjc_dr["语段ID"] = yd["ID"];
                    yd_gjc_dt.Rows.Add(yd_gjc_dr);
                    yd_gjc_ta.Update(yd_gjc_dt);
                    //yd_gjc_ta.Fill(yd_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var yd_gjc_list = (from yd_gjc in yd_gjc_dt
                                       join gjc2 in gjc_dt on yd_gjc.关键词ID equals gjc2.ID
                                       where yd_gjc.语段ID.ToString() == word["ID"].ToString()
                                       select yd_gjc).ToList();
                    keywordDataGrid.ItemsSource = yd_gjc_list;
                }
                else
                {
                    MessageBox.Show("语段或者关键词没有选中");
                }
            }
            else if (type == "文章")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow wz_gjc_dr = wz_gjc_dt.NewRow();

                    var wz = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    wz_gjc_dr["关键词ID"] = gjc.ID;
                    wz_gjc_dr["文章ID"] = wz["ID"];
                    wz_gjc_dt.Rows.Add(wz_gjc_dr);
                    wz_gjc_ta.Update(wz_gjc_dt);
                    //wz_gjc_ta.Fill(wz_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var wz_gjc_list = (from wz_gjc in wz_gjc_dt
                                       join gjc2 in gjc_dt on wz_gjc.关键词ID equals gjc2.ID
                                       where wz_gjc.文章ID.ToString() == word["ID"].ToString()
                                       select wz_gjc).ToList();
                    keywordDataGrid.ItemsSource = wz_gjc_list;
                }
                else
                {
                    MessageBox.Show("文章或者关键词没有选中");
                }
            }
            else if (type == "项目")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow xm_gjc_dr = xm_gjc_dt.NewRow();

                    var xm = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    xm_gjc_dr["关键词ID"] = gjc["ID"];
                    xm_gjc_dr["项目ID"] = xm["ID"];
                    xm_gjc_dt.Rows.Add(xm_gjc_dr);
                    xm_gjc_ta.Update(xm_gjc_dt);
                    //xm_gjc_ta.Fill(xm_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var xm_gjc_list = (from xm_gjc in xm_gjc_dt
                                       join gjc2 in gjc_dt on xm_gjc.关键词ID equals gjc2.ID
                                       where xm_gjc.项目ID.ToString() == word["ID"].ToString()
                                       select xm_gjc).ToList();
                    keywordDataGrid.ItemsSource = xm_gjc_list;
                }
                else
                {
                    MessageBox.Show("项目或者关键词没有选中");
                }
            }
            else if (type == "图片")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow tp_gjc_dr = tp_gjc_dt.NewRow();

                    var tp = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    tp_gjc_dr["关键词ID"] = gjc["ID"];
                    tp_gjc_dr["图片ID"] = tp["ID"];
                    tp_gjc_dt.Rows.Add(tp_gjc_dr);
                    tp_gjc_ta.Update(tp_gjc_dt);
                    //tp_gjc_ta.Fill(tp_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var tp_gjc_list = (from tp_gjc in tp_gjc_dt
                                       join gjc2 in gjc_dt on tp_gjc.关键词ID equals gjc2.ID
                                       where tp_gjc.图片ID.ToString() == word["ID"].ToString()
                                       select tp_gjc).ToList();
                    keywordDataGrid.ItemsSource = tp_gjc_list;
                }
                else
                {
                    MessageBox.Show("图片或者关键词没有选中");
                }
            }
            else if (type == "图片创作")
            {
                if (contentDataGrid.SelectedItem != null && keywordAllDataGrid.SelectedItem != null)
                {
                    DataRow tpcz_gjc_dr = tpcz_gjc_dt.NewRow();

                    var tpcz = (DataRowView)contentDataGrid.SelectedItem;
                    string dataRowType = keywordAllDataGrid.SelectedItem.GetType().ToString();
                    ScienceResearchDataSetNew.关键词Row gjc;
                    if (dataRowType == "System.Data.DataRowView")
                        gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
                    else
                        gjc = (ScienceResearchDataSetNew.关键词Row)keywordAllDataGrid.SelectedItem;
                    tpcz_gjc_dr["关键词ID"] = gjc["ID"];
                    tpcz_gjc_dr["图片创作ID"] = tpcz["ID"];
                    tpcz_gjc_dt.Rows.Add(tpcz_gjc_dr);
                    tpcz_gjc_ta.Update(tpcz_gjc_dt);
                    //tpcz_gjc_ta.Fill(tpcz_gjc_dt);

                    var word = (DataRowView)contentDataGrid.SelectedItem;
                    var tpcz_gjc_list = (from tpcz_gjc in tpcz_gjc_dt
                                       join gjc2 in gjc_dt on tpcz_gjc.关键词ID equals gjc2.ID
                                       where tpcz_gjc.图片创作ID.ToString() == word["ID"].ToString()
                                       select tpcz_gjc).ToList();
                    keywordDataGrid.ItemsSource = tpcz_gjc_list;
                }
                else
                {
                    MessageBox.Show("图片创作或者关键词没有选中");
                }
            }
        }
        
        /// <summary>
        /// 减少关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveKeyword_Click(object sender, RoutedEventArgs e)
        {
            if (type == "单词")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.单词_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                dc_gjc_ta.Update(dc_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("单词没有选中");
                    return;
                }
                var dc_gjc_list = (from dc_gjc in dc_gjc_dt
                                   join gjc in gjc_dt on dc_gjc.关键词ID equals gjc.ID
                                   where dc_gjc.单词ID.ToString() == word["ID"].ToString()
                                   select dc_gjc).ToList();
                keywordDataGrid.ItemsSource = dc_gjc_list;

            }
            else if (type == "短语")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.短语_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                dy_gjc_ta.Update(dy_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("短语没有选中");
                    return;
                }
                var dy_gjc_list = (from dy_gjc in dy_gjc_dt
                                   join gjc in gjc_dt on dy_gjc.关键词ID equals gjc.ID
                                   where dy_gjc.短语ID.ToString() == word["ID"].ToString()
                                   select dy_gjc).ToList();
                keywordDataGrid.ItemsSource = dy_gjc_list;
            }
            else if (type == "句型")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.句型_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                jx_gjc_ta.Update(jx_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("句型没有选中");
                    return;
                }
                var jx_gjc_list = (from jx_gjc in jx_gjc_dt
                                   join gjc in gjc_dt on jx_gjc.关键词ID equals gjc.ID
                                   where jx_gjc.句型ID.ToString() == word["ID"].ToString()
                                   select jx_gjc).ToList();
                keywordDataGrid.ItemsSource = jx_gjc_list;
            }
            else if (type == "语段")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.语段_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();                        
                }

                yd_gjc_ta.Update(yd_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("语段没有选中");
                    return;
                }
                var yd_gjc_list = (from yd_gjc in yd_gjc_dt
                                   join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                                   where yd_gjc.语段ID.ToString() == word["ID"].ToString()
                                   select yd_gjc).ToList();
                keywordDataGrid.ItemsSource = yd_gjc_list;
            }
            else if (type == "文章")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.文章_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                wz_gjc_ta.Update(wz_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("文章没有选中");
                    return;
                }
                var wz_gjc_list = (from wz_gjc in wz_gjc_dt
                                   join gjc in gjc_dt on wz_gjc.关键词ID equals gjc.ID
                                   where wz_gjc.文章ID.ToString() == word["ID"].ToString()
                                   select wz_gjc).ToList();
                keywordDataGrid.ItemsSource = wz_gjc_list;
            }
            else if (type == "项目")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.项目_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                xm_gjc_ta.Update(xm_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("项目没有选中");
                    return;
                }
                var xm_gjc_list = (from xm_gjc in xm_gjc_dt
                                   join gjc in gjc_dt on xm_gjc.关键词ID equals gjc.ID
                                   where xm_gjc.项目ID.ToString() == word["ID"].ToString()
                                   select xm_gjc).ToList();
                keywordDataGrid.ItemsSource = xm_gjc_list;
            }           
            else if (type == "图片创作")
            {
                int count = keywordDataGrid.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("关键词没有选中");
                    return;
                }
                for (int i = 0; i < count; i++)
                {
                    var x = (ScienceResearchDataSetNew.图片创作_关键词Row)keywordDataGrid.SelectedItems[i];
                    x.Delete();
                }

                tpcz_gjc_ta.Update(tpcz_gjc_dt);
                var word = (DataRowView)contentDataGrid.SelectedItem;
                if (word == null)
                {
                    MessageBox.Show("图片创作没有选中");
                    return;
                }
                var tpcz_gjc_list = (from tpcz_gjc in tpcz_gjc_dt
                                   join gjc in gjc_dt on tpcz_gjc.关键词ID equals gjc.ID
                                   where tpcz_gjc.图片创作ID.ToString() == word["ID"].ToString()
                                   select tpcz_gjc).ToList();
                keywordDataGrid.ItemsSource = tpcz_gjc_list;
            }
        }
        #endregion

        #region 数据表操作
        /// <summary>
        /// 跳到最前面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQian_Click(object sender, RoutedEventArgs e)
        {
            contentDataGrid.ScrollIntoView(contentDataGrid.Items[0]);
        }

        /// <summary>
        /// 跳到最后面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHou_Click(object sender, RoutedEventArgs e)
        {
            contentDataGrid.ScrollIntoView(contentDataGrid.Items[contentDataGrid.Items.Count-1]);
        }

        /// <summary>
        /// 增行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddingNewItem_Click(object sender, RoutedEventArgs e)
        {

            if (type == "仿真")
            {
                DataRow newRow;
                newRow = fz_dt.NewRow();
                //int maxID = fz_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                fz_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (fz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "位置")
            {
                DataRow newRow;
                newRow = wjwz_dt.NewRow();
                //int maxID = wjwz_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                wjwz_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (wjwz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "关键词")
            {
                DataRow newRow;
                newRow = gjc_dt.NewRow();
                //int maxID = gjc_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                gjc_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (gjc_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "单词")
            {
                DataRow newRow;
                newRow = dc_dt.NewRow();
                //int maxID = dc_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                dc_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (dc_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "短语")
            {
                DataRow newRow;
                newRow = dy_dt.NewRow();
                //int maxID = dy_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                dy_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (dy_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "句型")
            {
                DataRow newRow;
                newRow = jx_dt.NewRow();
                //int maxID = jx_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                jx_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (jx_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "语段")
            {
                DataRow newRow;
                newRow = yd_dt.NewRow();
                //int maxID = yd_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                yd_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (yd_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "文章")
            {
                DataRow newRow;
                newRow = wz_dt.NewRow();
                //int maxID = wz_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                wz_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (wz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "项目")
            {
                DataRow newRow;
                newRow = xm_dt.NewRow();
                //int maxID = xm_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                xm_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (xm_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "图片")
            {
                DataRow newRow;
                newRow = tp_dt.NewRow();
                //int maxID = tp_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                tp_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (tp_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }
            else if (type == "图片创作")
            {
                DataRow newRow;
                newRow = tpcz_dt.NewRow();
                //int maxID = tpcz_dt.AsEnumerable().Select(t => t.Field<int>("ID")).Max();
                //newRow["ID"] = maxID + 1;
                tpcz_dt.Rows.Add(newRow);
                inkCanvas.Height = rowHeight * (tpcz_dt.Rows.Count + 1) + contentDataGrid.ColumnHeaderHeight;
            }

            //contentDataGrid.ScrollIntoView(contentDataGrid.Items[contentDataGrid.Items.Count - 1]);
            contentDataGrid.ScrollIntoView(contentDataGrid.Items[0]);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (type == "仿真")
            {
                fz_ta.Update(fz_dt);
            }
            else if (type == "位置")
            {
                wjwz_ta.Update(wjwz_dt);
            }
            else if (type == "关键词")
            {
                gjc_ta.Update(gjc_dt);
            }
            else if (type == "单词")
            {
                try
                {
                    dc_ta.Update(dc_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dc_ta.Fill(dc_dt);
                }
            }
            else if (type == "短语")
            {
                dy_ta.Update(dy_dt);
            }
            else if (type == "句型")
            {
                jx_ta.Update(jx_dt);
            }
            else if (type == "语段")
            {
                try
                {
                    yd_ta.Update(yd_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库保存出现错误: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    yd_ta.Fill(yd_dt);
                }
            }
            else if (type == "文章")
            {
                try
                {
                    wz_ta.Update(wz_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库保存出现错误: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    wz_ta.Fill(wz_dt);
                }
            }
            else if (type == "项目")
            {
                xm_ta.Update(xm_dt);
            }
            else if (type == "图片")
            {
                tp_ta.Update(tp_dt);
            }
            else if (type == "图片创作")
            {
                tpcz_ta.Update(tpcz_dt);
            }
        }

        /// <summary>
        /// 删行数据库的一行并保存
        /// 如果是文章的一行，需要先删除该语段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveNewItem_Click(object sender, RoutedEventArgs e)
        {
            if (type == "文章")
            {
                //删除该文章的所有关键词、语段和图片
                if (contentDataGrid.SelectedItems.Count > 0)
                {
                    if (contentDataGrid.SelectedItems.Count > 0)
                    {
                        var item = contentDataGrid.SelectedItems[0] as DataRowView;
                        ScienceResearchDataSetNew.文章Row wz= item.Row as ScienceResearchDataSetNew.文章Row;
                        DataBaseRowManage.DeleteWz(wz);
                    }
                    
                }
            }
            else
            {
                var dataView = contentDataGrid.ItemsSource as DataView;
                if (contentDataGrid.SelectedItems.Count > 0)
                {
                    while (contentDataGrid.SelectedItems.Count > 0)
                    {
                        var item = contentDataGrid.SelectedItems[0] as DataRowView;
                        int index = contentDataGrid.Items.IndexOf(item);    //获取选中项的主键，可以根据这个主键来删除数据库的记录
                        dataView.Delete(index);                             //把数据从DataGrid中移除
                    }
                    contentDataGrid.ItemsSource = dataView;
                }                
            }

            btnSave_Click(null, null);                                  //保存
        }
        #endregion

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            //保存墨笔文件         
            if (File.Exists(figure_path_isf))
                File.Delete(figure_path_isf);

            FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
            inkCanvas.Strokes.Save(file_ink);
            file_ink.Close();
        }

        private void pizuCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pizuCheckBox_Checked();
        }

        private void pizuCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pizuCheckBox_UnChecked();
        }

        private void btnResearch_Click(object sender, RoutedEventArgs e)
        {
            if (keywordTextBox.Text != "")
            {               

                var data = from gjc in gjc_dt
                           where gjc.关键词.Substring(0, 1) == keywordTextBox.Text.Substring(0, 1)
                           orderby gjc.关键词 ascending
                           select gjc;
                keywordAllDataGrid.ItemsSource = data;

                keywordSource = "shaixuan";
            }
        }

        private void btnAllKeyword_Click(object sender, RoutedEventArgs e)
        {
            //var data = from gjc in gjc_dt
            //           orderby gjc.关键词 ascending
            //           select gjc;
            //keywordAllDataGrid.ItemsSource = data;

            keywordAllDataGrid.ItemsSource = gjc_dt.DefaultView;
            keywordSource = "all";
            ICollectionView cvTasks = CollectionViewSource.GetDefaultView(keywordAllDataGrid.ItemsSource);
            if (cvTasks != null && cvTasks.CanSort == true)
            {
                cvTasks.SortDescriptions.Clear();
                cvTasks.SortDescriptions.Add(new SortDescription("关键词", ListSortDirection.Ascending));
            }
        }

        private void btnContent_Click(object sender, RoutedEventArgs e)
        {
            if (type == "关键词")
            {
                try
                {
                    if (contentTextBox.Text != "")
                    {
                        grid1.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                        var data_gjc = from gjc in gjc_dt
                                       where gjc.关键词.Substring(0, 1) == contentTextBox.Text.Substring(0, 1)
                                       orderby gjc.关键词 ascending
                                       select gjc;
                        contentSelectedDataGrid.ItemsSource = data_gjc;
                    }
                    else
                        MessageBox.Show("请输入一个字符");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("出现错误: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else if (type == "语段")
            {
                try
                {
                    if (contentTextBox.Text != "")
                    {
                        grid1.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                        var data_yd = from yd in yd_dt
                                      where yd.文章ID == int.Parse(contentTextBox.Text)
                                      orderby yd.排序 ascending
                                      select yd;
                        contentSelectedDataGrid.ItemsSource = data_yd;
                    }
                    else
                        MessageBox.Show("请输入一个语段编号");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("出现错误: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }            

        private void btnAllContent_Click(object sender, RoutedEventArgs e)
        {
            if (type == "关键词")
            {
                grid1.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
            }
            else if (type == "语段")
            {
                grid1.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
            }
        }

        private void btnContentJump_Click(object sender, RoutedEventArgs e)
        {
            if (type == "关键词")
            {
                MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.keywordTreeTabItem;
                int kwId;

                if (is_contentDataGrid_GotFocus == true)
                {
                    DataRowView kw = (DataRowView)contentDataGrid.SelectedItem;

                    if (kw != null)
                        kwId = (int)kw["ID"];
                    else
                    {
                        MessageBox.Show("请选中某一个关键词");
                        return;
                    }
                }
                else
                {
                    ScienceResearchDataSetNew.关键词Row kw = (ScienceResearchDataSetNew.关键词Row)contentSelectedDataGrid.SelectedItem;
                    if (kw != null)
                        kwId = kw.ID;
                    else
                    {
                        MessageBox.Show("请选中某一个关键词");
                        return;
                    }
                }

                try
                {
                    TreeView tv = MainWindow.keywordTreeUserControl.keywordTreeView;
                    MainWindow.keywordTreeUserControl.linkComboBox.SelectedIndex = 2;
                    //Node item = ((Node)tv.Items[0]).Children[2].Children[2];
                    Node item = FindItem(tv, kwId);


                    TreeViewItem tvi = FindTreeViewItem(tv, item);
                    tvi.IsSelected = true;
                    tvi.BringIntoView();
                }
                catch
                {
                    MessageBox.Show("请点击\"展开\"按钮，展开所有关键词");
                }
            }
            else if (type == "语段")
            {
                DataRowView yd = (DataRowView)contentDataGrid.SelectedItem;
                if (yd != null)
                {
                    string isChuangzuo = ((ScienceResearchDataSetNew.语段Row)yd.Row).文章Row.分类;
                    if (isChuangzuo == "创作")
                    {
                        //判断该文章是否打开
                        int paperId = ((ScienceResearchDataSetNew.语段Row)yd.Row).文章ID;
                        if (MainWindow.paperIdList_left.Contains(paperId))
                        {
                            //如果包含那么就跳转到相关文章的相应语段
                            for (int i = 0; i < MainWindow.paperIdList_left.Count; i++)
                            {
                                if (MainWindow.paperIdList_left[i] == paperId)
                                {
                                    MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.paperUserControlTabItemList_left[i];

                                    //判断跳转到第几段
                                    float paraLocation = ((ScienceResearchDataSetNew.语段Row)yd.Row).排序;
                                    var paixu_list = (from yd2 in yd_dt
                                                      where yd2.文章ID == paperId
                                                      orderby yd2.排序 ascending
                                                      select yd2.排序).ToList();
                                    int location = paixu_list.IndexOf(paraLocation);
                                    MainWindow.paperUserControlList_left[i].scrollToParagraph(location);
                                }
                            }
                        }
                        else
                        {
                            //如果不包含则需要先打开相应文章
                            MessageBox.Show("文章未打开");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("请选择创作文章的语段");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请选中某一个语段");
                    return;
                }

                
            }
        }

        private Node FindItem(ItemsControl container, int keywordId)
        {
            int itemsCount = container.Items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                Node nd = (Node)container.Items[i];
                Node result = FindNode(nd, keywordId);
                if (result != null)
                    return result;
            }

            return null;
        }

        private Node FindNode(Node root, int keywordId)
        {
            if (root.Data.ID == keywordId)
                return root;
            else
            {
                int childrenCount = root.Children.Count;
                for (int i = 0; i < childrenCount; i++)
                {
                    Node nd = root.Children[i];
                    Node resltNd = FindNode(nd, keywordId);
                    if (resltNd != null)
                        return resltNd;
                }
            }

            return null;
        }

        private TreeViewItem FindTreeViewItem(ItemsControl container, object item)
        {
            if (null == container || null == item)
            {
                return null;
            }

            if (container.DataContext == item)
            {
                return container as TreeViewItem;
            }

            int count = container.Items.Count;
            for (int i = 0; i < count; i++)
            {
                TreeViewItem subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);

                if (null == subContainer)
                {
                    continue;
                }

                // Search the next level for the object.
                TreeViewItem resultContainer = FindTreeViewItem(subContainer, item);
                if (null != resultContainer)
                {
                    return resultContainer;
                }
            }

            return null;
        }
        
        private void contentDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (type == "关键词")
            {
                is_contentDataGrid_GotFocus = true;
            }
        }

        private void contentSelectedDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (type == "关键词")
            {
                is_contentDataGrid_GotFocus = false;
            }
        }

        private void contentSelectedDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScienceResearchDataSetNew.语段Row currentItem;
            try
            {
                currentItem = (ScienceResearchDataSetNew.语段Row)contentSelectedDataGrid.SelectedItem;
            }
            catch
            {
                return;
            }
            if (currentItem != null)
            {
                if (type == "语段")
                {
                    string figure_path = currentItem["图片"].ToString();
                    string figure_path_isf = currentItem["图片isf"].ToString();
                    figure_path = MainWindow.path_translate(figure_path);
                    figure_path_isf = MainWindow.path_translate(figure_path_isf);

                    //墨笔文件

                    if (contentSelectedDataGrid.CurrentColumn != null && contentSelectedDataGrid.CurrentColumn.Header.ToString() == "图片" && (int)currentItem["ID"] > 0 && currentItem["图片"].GetType().ToString() != "System.DBNull")
                    {
                        MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.paragraphFigureTabItem_l;
                        if (File.Exists(figure_path))
                        {
                            ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                            MainWindow.paragraphImage_l.Source = img;

                            if (File.Exists(figure_path_isf))
                            {
                                FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                                if (file_ink.Length != 0)
                                {
                                    MainWindow.scienceResearchInkCanvas_l.Strokes = new StrokeCollection(file_ink);
                                }
                                file_ink.Close();
                                MainWindow.paragraphFigureUserControl_l.figure_path_isf = figure_path_isf;
                            }
                            else
                            {
                                MainWindow.scienceResearchInkCanvas_l.Strokes.Clear();
                                figure_path_isf = ".\\科学研究\\图片ISF\\" + currentItem["ID"].ToString() + ".isf";

                                //更新数据库
                                var data3 = from data_item in yd_dt
                                            where data_item.ID == (int)currentItem["ID"]
                                            select data_item;
                                foreach (var d3 in data3)
                                {
                                    d3.图片isf = figure_path_isf;
                                    yd_ta.Update(yd_dt);
                                }

                                //更新墨笔文件路径
                                figure_path_isf = MainWindow.path_translate(figure_path_isf);
                                MainWindow.paragraphFigureUserControl_l.figure_path_isf = figure_path_isf;
                            }
                        }
                        else
                        {
                            MessageBox.Show("图片文件不存在");
                        }
                    }

                    MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValuePath = "ID";
                    MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValue = currentItem.ID;
                    MainWindow.dataBaseUserControl_yd.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedItem);
                }                
            }
        }

        private void keywordAllDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            ScienceResearchDataSetNew.关键词Row gjc;
            if (keywordSource == "all")
                gjc = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordAllDataGrid.SelectedItem).Row;
            else
                gjc = keywordAllDataGrid.SelectedItem as ScienceResearchDataSetNew.关键词Row;

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "选中的关键词为：ID=" + gjc.ID + "，关键词=" + gjc.关键词;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);
        }

        private void btnMode_Click(object sender, RoutedEventArgs e)
        {
            //建立模式
            var dc_list = (from dc in dc_dt                            
                            select dc).ToList();

            string dc_strs = "";
            for (int i=0;i<dc_list.Count;i++)
            {
                string dc = dc_list[i].单词;
                dc_strs = dc_strs + dc + ",";
            }
            dc_strs = dc_strs.Substring(0, dc_strs.Length - 1);

            //保存文件
            string localFilePath= MainWindow.path_database+"\\Mode_Word.txt";
            StreamWriter fileWriter = new StreamWriter(localFilePath, true, Encoding.Default);
            fileWriter.Write(dc_strs);
            fileWriter.Close();

            MessageBox.Show("建模成功");
        }

        
    }

    #region 单词数据表控件子类


    #endregion


}
