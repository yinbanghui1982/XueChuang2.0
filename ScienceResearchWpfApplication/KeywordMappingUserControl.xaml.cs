using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;
using System.Windows.Ink;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// KeywordUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class KeywordMappingUserControl : UserControl
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

        ScienceResearchDataSetNew.项目DataTable xm_dt;
        ScienceResearchDataSetNewTableAdapters.项目TableAdapter xm_ta;
        ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;

        ScienceResearchDataSetNew.图片DataTable tp_dt;
        ScienceResearchDataSetNewTableAdapters.图片TableAdapter tp_ta;
        ScienceResearchDataSetNew.图片_关键词DataTable tp_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片_关键词TableAdapter tp_gjc_ta;

        ScienceResearchDataSetNew.图片创作DataTable tpcz_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter tpcz_ta;
        ScienceResearchDataSetNew.图片创作_关键词DataTable tpcz_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作_关键词TableAdapter tpcz_gjc_ta;

        string keywordSource;      //关键词来源，all——数据表，shaixuan——筛选

        public KeywordMappingUserControl()
        {
            InitializeComponent();

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;

            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;

            dy_dt = MainWindow.dy_dt;
            dy_ta = MainWindow.dy_ta;
            dy_gjc_dt = MainWindow.dy_gjc_dt;
            dy_gjc_ta = MainWindow.dy_gjc_ta;

            jx_dt = MainWindow.jx_dt;
            jx_ta = MainWindow.jx_ta;
            jx_gjc_dt = MainWindow.jx_gjc_dt;
            jx_gjc_ta = MainWindow.jx_gjc_ta;

            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;

            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;
            wz_gjc_dt = MainWindow.wz_gjc_dt;
            wz_gjc_ta = MainWindow.wz_gjc_ta;

            xm_dt = MainWindow.xm_dt;
            xm_ta = MainWindow.xm_ta;
            xm_gjc_dt = MainWindow.xm_gjc_dt;
            xm_gjc_ta = MainWindow.xm_gjc_ta;

            tp_dt = MainWindow.tp_dt;
            tp_ta = MainWindow.tp_ta;
            tp_gjc_dt = MainWindow.tp_gjc_dt;
            tp_gjc_ta = MainWindow.tp_gjc_ta;

            tpcz_dt = MainWindow.tpcz_dt;
            tpcz_ta = MainWindow.tpcz_ta;
            tpcz_gjc_dt = MainWindow.tpcz_gjc_dt;
            tpcz_gjc_ta = MainWindow.tpcz_gjc_ta;

            //var data = from gjc in gjc_dt
            //           select gjc;
            //keywordDataGrid.ItemsSource = data;

            keywordDataGrid.ItemsSource = gjc_dt.DefaultView;
            keywordSource = "all";
        }
        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();            
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.left_zero();
        }


        private void keywordDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择变化时，查询该关键词对应的语段
            
            if (keywordDataGrid.SelectedItem != null)
            {
                ScienceResearchDataSetNew.关键词Row keyword;

                if (keywordSource == "all")
                {
                    keyword = (ScienceResearchDataSetNew.关键词Row)((DataRowView)keywordDataGrid.SelectedItem).Row;
                }
                else

                    keyword = keywordDataGrid.SelectedItem as ScienceResearchDataSetNew.关键词Row;

                MainWindow.mainWindow.statusBar.Items.Clear();
                TextBlock txtb = new TextBlock();
                txtb.Text = "选中的关键词为：ID=" + keyword.ID + "，关键词=" + keyword.关键词;
                MainWindow.mainWindow.statusBar.Items.Add(txtb);

                if (keyword != null)
                {
                    string kw = keyword.关键词.ToString();
                    //单词
                    var data = from gjc in gjc_dt
                               join dc_gjc in dc_gjc_dt on gjc.ID equals dc_gjc.关键词ID
                               join dc in dc_dt on dc_gjc.单词ID equals dc.ID
                               where gjc.关键词 == kw
                               select dc;
                    dc_DataGrid.ItemsSource = data;
                    //短语
                    var dy_data = from gjc in gjc_dt
                                  join dy_gjc in dy_gjc_dt on gjc.ID equals dy_gjc.关键词ID
                                  join dy in dy_dt on dy_gjc.短语ID equals dy.ID
                                  where gjc.关键词 == kw
                                  select dy;
                    dy_DataGrid.ItemsSource = dy_data;

                    //句型
                    var jx_data = from gjc in gjc_dt
                                  join jx_gjc in jx_gjc_dt on gjc.ID equals jx_gjc.关键词ID
                                  join jx in jx_dt on jx_gjc.句型ID equals jx.ID
                                  where gjc.关键词 == kw
                                  select jx;
                    jx_DataGrid.ItemsSource = jx_data;

                    //语段
                    var yd_data = from gjc in gjc_dt
                                  join yd_gjc in yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                                  join yd in yd_dt on yd_gjc.语段ID equals yd.ID
                                  where gjc.关键词 == kw
                                  select yd;
                    yd_DataGrid.ItemsSource = yd_data;

                    //文章
                    var wz_data = from gjc in gjc_dt
                                  join wz_gjc in wz_gjc_dt on gjc.ID equals wz_gjc.关键词ID
                                  join wz in wz_dt on wz_gjc.文章ID equals wz.ID
                                  where gjc.关键词 == kw
                                  select wz;
                    wz_DataGrid.ItemsSource = wz_data;

                    //项目
                    var xm_data = from gjc in gjc_dt
                                  join xm_gjc in xm_gjc_dt on gjc.ID equals xm_gjc.关键词ID
                                  join xm in xm_dt on xm_gjc.项目ID equals xm.ID
                                  where gjc.关键词 == kw
                                  select xm;
                    xm_DataGrid.ItemsSource = xm_data;

                    //图片
                    //var tp_data = from gjc in gjc_dt
                    //              join tp_gjc in tp_gjc_dt on gjc.ID equals tp_gjc.关键词ID
                    //              join tp in tp_dt on tp_gjc.图片ID equals tp.ID
                    //              where gjc.关键词 == kw
                    //              select tp;
                    //tp_DataGrid.ItemsSource = tp_data;

                    //图片创作
                    var tpcz_data = from gjc in gjc_dt
                                    join tpcz_gjc in tpcz_gjc_dt on gjc.ID equals tpcz_gjc.关键词ID
                                    join tpcz in tpcz_dt on tpcz_gjc.图片创作ID equals tpcz.ID
                                    where gjc.关键词 == kw
                                    select tpcz;
                    tpcz_DataGrid.ItemsSource = tpcz_data;
                }
            }
        }
        

        private void dc_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dc_row = dc_DataGrid.CurrentItem as ScienceResearchDataSetNew.单词Row;
            if (dc_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_dc;
                MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedValue = dc_row.ID;
                MainWindow.dataBaseUserControl_dc.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedItem);
            }
        }

        private void dy_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dy_row = dy_DataGrid.CurrentItem as ScienceResearchDataSetNew.短语Row;
            if (dy_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_dy;
                MainWindow.dataBaseUserControl_dy.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_dy.contentDataGrid.SelectedValue = dy_row.ID;
                MainWindow.dataBaseUserControl_dy.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_dy.contentDataGrid.SelectedItem);
            }
        }

        private void jx_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var jx_row = jx_DataGrid.CurrentItem as ScienceResearchDataSetNew.句型Row;
            if (jx_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_jx;
                MainWindow.dataBaseUserControl_jx.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_jx.contentDataGrid.SelectedValue = jx_row.ID;
                MainWindow.dataBaseUserControl_jx.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_jx.contentDataGrid.SelectedItem);
            }
        }

        private void yd_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var yd_row = yd_DataGrid.CurrentItem as ScienceResearchDataSetNew.语段Row;
            if (yd_row != null && yd_DataGrid.CurrentColumn.Header.ToString() == "语段ID")
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_yd;
                MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedValue = yd_row.ID;
                MainWindow.dataBaseUserControl_yd.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_yd.contentDataGrid.SelectedItem);
            }
            else if (yd_row != null && yd_DataGrid.CurrentColumn.Header.ToString() == "文章ID")
            {
                var yd_gjc_list = (from yd_gjc in yd_gjc_dt
                                   join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                                   where yd_gjc.语段ID == yd_row.ID
                                   select yd_gjc).ToList();
                string figure_path = yd_row.图片;
                string figure_path_isf = yd_row.图片isf;
                figure_path = MainWindow.path_translate(figure_path);
                figure_path_isf = MainWindow.path_translate(figure_path_isf);

                ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid();
                MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.paragraphFigureTabItem_l;
                paraResourceClass_PaperDataGrid.creat_figure( MainWindow.paragraphFigureUserControl_l,yd_row);
                
            }
        }

        private void wz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var wz = wz_DataGrid.CurrentItem as ScienceResearchDataSetNew.文章Row;
            if (wz != null && wz_DataGrid.CurrentColumn.Header.ToString() == "文章ID")
            {
                //单击文章ID，跳转到文章表的相应记录
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_wz;
                MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedValue = wz.ID;
                MainWindow.dataBaseUserControl_wz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedItem);
            }
            else if (wz != null && wz_DataGrid.CurrentColumn.Header.ToString() == "文章名")
            {
                //单击文章名跳转到相关文件
                string fenlei = wz.分类;
                if (fenlei != "创作")         //如果是参考文献则打开相关pdf文件
                {
                    string locationStr = wz.文件;
                    locationStr = MainWindow.path_translate(locationStr);
                    MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
                    MainWindow.applicationUserControl.headerStr = "文章";
                    MainWindow.applicationUserControl.loadProcess(locationStr);
                }
                else                        //如果是创作文章则打开文件
                {
                    MainWindow.paperSelectUserControl.zhengRadioButton.IsChecked = true;
                    MainWindow.paperSelectUserControl.paperDataGrid.SelectedValuePath = "ID";
                    MainWindow.paperSelectUserControl.paperDataGrid.SelectedValue = wz.ID;
                }
            }
        }

        private void xm_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var xm = xm_DataGrid.CurrentItem as ScienceResearchDataSetNew.项目Row;
            if (xm != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_xm;
                MainWindow.dataBaseUserControl_xm.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_xm.contentDataGrid.SelectedValue = xm.ID;
                MainWindow.dataBaseUserControl_xm.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_xm.contentDataGrid.SelectedItem);
            }
        }

        //private void tp_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var tp = tp_DataGrid.CurrentItem as ScienceResearchDataSetNew.图片Row;
        //    if (tp != null)
        //    {
        //        MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_tp;
        //        MainWindow.dataBaseUserControl_tp.contentDataGrid.SelectedValuePath = "ID";
        //        MainWindow.dataBaseUserControl_tp.contentDataGrid.SelectedValue = tp.ID;
        //        MainWindow.dataBaseUserControl_tp.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_tp.contentDataGrid.SelectedItem);
        //    }
        //}

        private void tpcz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tpcz = tpcz_DataGrid.CurrentItem as ScienceResearchDataSetNew.图片创作Row;
            if (tpcz != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_tpcz;
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedValue = tpcz.ID;
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedItem);
            }
        }

        private void btnResearch_Click(object sender, RoutedEventArgs e)
        {
            if (keywordTextBox.Text != "")
            {

                var data = from gjc in gjc_dt
                           where gjc.关键词.Substring(0, 1) == keywordTextBox.Text.Substring(0, 1)
                           orderby gjc.关键词 ascending
                           select gjc;
                keywordDataGrid.ItemsSource = data;
                keywordSource = "shaixuan";
            }
        }

        private void btnAllKeyword_Click(object sender, RoutedEventArgs e)
        {
            //var data = from gjc in gjc_dt
            //           orderby gjc.关键词 ascending
            //           select gjc;
            //keywordDataGrid.ItemsSource = data;
            //keywordSource = "shaixuan";

            keywordDataGrid.ItemsSource = gjc_dt.DefaultView;
            keywordSource = "all";
        }
    }

    
}
