using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// ProjectMappingUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectMappingUserControl : UserControl
    {
        ScienceResearchDataSetNew.项目DataTable xm_dt;
        ScienceResearchDataSetNewTableAdapters.项目TableAdapter xm_ta;

        ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;

        ScienceResearchDataSetNew.仿真DataTable fz_dt;
        ScienceResearchDataSetNewTableAdapters.仿真TableAdapter fz_ta;

        ScienceResearchDataSetNew.文件位置DataTable wjwz_dt;
        ScienceResearchDataSetNewTableAdapters.文件位置TableAdapter wjwz_ta;

        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;

        ScienceResearchDataSetNew.图片创作DataTable tpcz_dt;
        ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter tpcz_ta;

        ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;

        public ProjectMappingUserControl()
        {
            InitializeComponent();
            xm_dt = MainWindow.xm_dt;
            xm_ta = MainWindow.xm_ta;

            xm_gjc_dt = MainWindow.xm_gjc_dt;
            xm_gjc_ta = MainWindow.xm_gjc_ta;

            fz_dt = MainWindow.fz_dt;
            fz_ta = MainWindow.fz_ta;

            wjwz_dt = MainWindow.wjwz_dt;
            wjwz_ta = MainWindow.wjwz_ta;

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;

            tpcz_dt = MainWindow.tpcz_dt;
            tpcz_ta = MainWindow.tpcz_ta;

            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;

            var data = from xm in xm_dt
                       select xm;

            projectDataGrid.ItemsSource = data;
            projectDataGrid.CanUserAddRows = false;
            projectDataGrid.CanUserDeleteRows = false;
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var data = from xm in xm_dt
                       select xm;

            projectDataGrid.ItemsSource = data;
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.left_zero();
        }

        private void projectDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (projectDataGrid.SelectedItem != null)
            {
                var project = projectDataGrid.SelectedItem as ScienceResearchDataSetNew.项目Row;
                if (project != null)
                {
                    int projectId = project.ID;
                    
                    //仿真
                    var data = from fz in fz_dt
                               where fz.项目ID==projectId
                               select fz;
                    fz_DataGrid.ItemsSource = data;
                    //文件位置
                    var data_wjwz = from wjwz in wjwz_dt
                               where wjwz.项目ID == projectId
                               select wjwz;
                    wjwz_DataGrid.ItemsSource = data_wjwz;
                    //关键词
                    var data_gjc = from gjc in gjc_dt
                                   join xm_gjc in xm_gjc_dt on gjc.ID equals xm_gjc.关键词ID
                                   where xm_gjc.项目ID == projectId
                                   select gjc;
                    gjc_DataGrid.ItemsSource = data_gjc;
                    //图片创作
                    var data_tpcz = from tpcz in tpcz_dt
                                    where tpcz.项目ID == projectId
                                    select tpcz;
                    tpcz_DataGrid.ItemsSource = data_tpcz;
                    //文章
                    var data_wz = from wz in wz_dt
                                    where wz.项目ID == projectId
                                    select wz;
                    wz_DataGrid.ItemsSource = data_wz;

                    //作图和文献窗口改变
                    MainWindow.projectId = projectId;
                    MainWindow.figureCreateUserControl.SetFigureWrite();
                    MainWindow.projectLiteratureUserControl.SetKeyword();
                }
            }
        }

        private void projectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            projectDataGrid_MouseLeftButtonUp(null, null);
        }

        private void fz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var fz_row = fz_DataGrid.CurrentItem as ScienceResearchDataSetNew.仿真Row;
            if (fz_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_fz;
                MainWindow.dataBaseUserControl_fz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_fz.contentDataGrid.SelectedValue = fz_row.ID;
                MainWindow.dataBaseUserControl_fz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_fz.contentDataGrid.SelectedItem);
            }
        }

        private void wjwz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var wjwz_row = wjwz_DataGrid.CurrentItem as ScienceResearchDataSetNew.文件位置Row;
            if (wjwz_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_wjwz;
                MainWindow.dataBaseUserControl_wjwz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_wjwz.contentDataGrid.SelectedValue = wjwz_row.ID;
                MainWindow.dataBaseUserControl_wjwz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_wjwz.contentDataGrid.SelectedItem);
            }
        }

        private void gjc_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var gjc_row = gjc_DataGrid.CurrentItem as ScienceResearchDataSetNew.关键词Row;
            if (gjc_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_gjc;
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedValue = gjc_row.ID;
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedItem);
            }
        }

        private void tpcz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tpcz_row = tpcz_DataGrid.CurrentItem as ScienceResearchDataSetNew.图片创作Row;
            if (tpcz_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_tpcz;
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedValue = tpcz_row.ID;
                MainWindow.dataBaseUserControl_tpcz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_tpcz.contentDataGrid.SelectedItem);
            }
        }

        private void wz_DataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var wz_row = wz_DataGrid.CurrentItem as ScienceResearchDataSetNew.文章Row;
            if (wz_row != null)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_wz;
                MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedValue = wz_row.ID;
                MainWindow.dataBaseUserControl_wz.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_wz.contentDataGrid.SelectedItem);
            }
        }

        
    }
}
