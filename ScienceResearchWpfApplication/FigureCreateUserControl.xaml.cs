using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Ink;
using ScienceResearchWpfApplication.DatabaseManage;

namespace ScienceResearchWpfApplication.FigureManage
{
    /// <summary>
    /// FigureCreateUserControl.xaml 的交互逻辑
    /// 图片创作用户控件，用于根据参考语段（主要是图片语段）进行图片创作
    /// </summary>
    /// 
    public partial class FigureCreateUserControl : UserControl
    {
        ScienceResearchDataSetNew.图片创作DataTable tpcz_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter tpcz_ta;
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        ScienceResearchDataSetNew.图片创作_关键词DataTable tpcz_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作_关键词TableAdapter tpcz_gjc_ta;
        ScienceResearchDataSetNew.图片DataTable tp_dt;
        ScienceResearchDataSetNewTableAdapters.图片TableAdapter tp_ta;
        ScienceResearchDataSetNew.图片_关键词DataTable tp_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.图片_关键词TableAdapter tp_gjc_ta;
        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;

        public FigureCreateUserControl()
        {
            InitializeComponent();

            tpcz_dt = MainWindow.tpcz_dt;
            tpcz_ta = MainWindow.tpcz_ta;
            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            tpcz_gjc_dt = MainWindow.tpcz_gjc_dt;
            tpcz_gjc_ta = MainWindow.tpcz_gjc_ta;
            tp_dt = MainWindow.tp_dt;
            tp_ta = MainWindow.tp_ta;
            tp_gjc_dt = MainWindow.tp_gjc_dt;
            tp_gjc_ta = MainWindow.tp_gjc_ta;
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;

            if (MainWindow.projectId != -1)
                SetFigureWrite();

        }

        public void SetFigureWrite()
        {
            var data = from tpcz in tpcz_dt
                       where tpcz.项目ID == MainWindow.projectId
                       select new TPCZ
                       {
                           ID = tpcz.ID,
                           图片文件 = tpcz.图片文件,
                           解释 = tpcz.解释
                       };
            figureWriteDataGrid.ItemsSource = data;
        }

        private void figureWriteDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (figureWriteDataGrid.CurrentItem != null)
            {
                var tpcz = figureWriteDataGrid.CurrentItem as TPCZ;
                int tpczId = tpcz.ID;

                var data = from gjc in gjc_dt
                           join tpcz_gjc in tpcz_gjc_dt on gjc.ID equals tpcz_gjc.关键词ID
                           where tpcz_gjc.图片创作ID == tpczId
                           select new Keyword
                           {
                               关键词 = gjc.关键词
                           };
                keywordWriteDataGrid.ItemsSource = data;

                //打开文件
                if (figureWriteDataGrid.CurrentColumn.Header.ToString() == "图片文件")
                {
                    string processStr2 = tpcz.图片文件;
                    processStr2 = MainWindow.path_translate(processStr2);
                    MainWindow.applicationUserControl.loadProcess(processStr2);
                }
            }
        }

        private void keywordWriteDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var keyword = keywordWriteDataGrid.CurrentItem as Keyword;
            if (keyword != null)
            {
                string keyword_kw = keyword.关键词;
                var data = from gjc in gjc_dt
                           join yd_gjc in yd_gjc_dt on gjc.ID equals yd_gjc.关键词ID
                           join yd in yd_dt on yd_gjc.语段ID equals yd.ID
                           where gjc.关键词 == keyword_kw && yd.文章Row.分类 != "创作"
                           select yd;

                figureReferDataGrid.ItemsSource = data;
            }
        }

        private void figureReferDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (figureReferDataGrid.CurrentItem != null)
            {
                var paragraphID = figureReferDataGrid.CurrentItem as ScienceResearchDataSetNew.语段Row;
                int paraReferID = paragraphID.ID;
                ParaResourceClass_PaperDataGrid paraResourceClass_PaperDataGrid = new ParaResourceClass_PaperDataGrid(figureReferDataGrid, keywordReferDataGrid, paraReferID);
                paraResourceClass_PaperDataGrid.referDataGrid_MouseLeftButtonUp();
            }            
        }

        private void btnHalf_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }

        
    }
}
