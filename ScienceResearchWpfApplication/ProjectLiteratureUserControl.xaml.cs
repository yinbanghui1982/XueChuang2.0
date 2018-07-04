using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ScienceResearchWpfApplication.DatabaseManage;

namespace ScienceResearchWpfApplication
{   

    /// <summary>
    /// ProjectLiteratureUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectLiteratureUserControl : UserControl
    {
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;
        ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;
        ScienceResearchDataSetNew.文章_关键词DataTable wz_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.文章_关键词TableAdapter wz_gjc_ta;

        public static string paperFanwei;         //确定智能搜索的文献范围
        public static ObservableCollection<Keyword> keywordList;

        public ProjectLiteratureUserControl()
        {
            InitializeComponent();

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            xm_gjc_dt = MainWindow.xm_gjc_dt;
            xm_gjc_ta = MainWindow.xm_gjc_ta;
            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;
            wz_gjc_dt = MainWindow.wz_gjc_dt;
            wz_gjc_ta = MainWindow.wz_gjc_ta;

            if (MainWindow.projectId != -1)
                SetKeyword();

            keywordList = new ObservableCollection<Keyword>();
            keywordSelectedDataGrid.ItemsSource = keywordList;

            projectPaperRadioButton.IsChecked = true;
        }

        public void SetKeyword()
        {
            var data = from gjc in gjc_dt
                       join xm_gjc in xm_gjc_dt on gjc.ID equals xm_gjc.关键词ID
                       where xm_gjc.项目ID == MainWindow.projectId
                       select new Keyword
                       {
                           关键词 = gjc.关键词
                       };
            keywordDataGrid.ItemsSource = data;
        }

        private void keywordDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var keyword = keywordDataGrid.CurrentItem as Keyword;
            if (keyword != null)
            {
                string keyword_kw = keyword.关键词;
                try
                {
                    var data = from gjc in gjc_dt
                               join wz_gjc in wz_gjc_dt on gjc.ID equals wz_gjc.关键词ID
                               join wz in wz_dt on wz_gjc.文章ID equals wz.ID
                               where gjc.关键词 == keyword_kw
                               select new WZ
                               {
                                   ID = wz.ID,
                                   文章名 = wz.文章名,
                                   文件 = wz.文件
                               };
                    paperDataGrid.ItemsSource = data;
                }
                catch { }                
            }

        }

        private void paperDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
            var wz = paperDataGrid.CurrentItem as WZ;
            if (wz != null)
            {
                MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;
                string processStr = wz.文件;
                processStr = MainWindow.path_translate(processStr);
                MainWindow.applicationUserControl.headerStr = "文献";
                MainWindow.applicationUserControl.loadProcess(processStr);
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

        private void btnAddKeyword_Click(object sender, RoutedEventArgs e)
        {
            Keyword kw= keywordDataGrid.CurrentItem as Keyword;
            keywordList.Add(kw);            
        }

        private void btnRemoveKeyword_Click(object sender, RoutedEventArgs e)
        {
            Keyword kw = keywordSelectedDataGrid.CurrentItem as Keyword;
            keywordList.Remove(kw);
        }

        private void allPaperRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paperFanwei = "全部";
        }

        private void projectPaperRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paperFanwei = "项目";
        }

        private void selectedKeywordPaperRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paperFanwei = "关键词";
        }
    }
}
