using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Windows.Input;

namespace ScienceResearchWpfApplication
{
    class PaperResearchResult
    {
        public int 文章编号 { get; set; }
        public string 文章名 { get; set; }
        public int 行号 { get; set; }
    }

    /// <summary>
    /// PaperResearchUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaperResearchUserControl : UserControl
    {
        ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;
        DataTable resultDataTable;

        List<PaperResearchResult> paperResearchResultList;
        
        public PaperResearchUserControl()
        {
            InitializeComponent();
            resultDataTable = new DataTable();
            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;

            paperResearchResultList = new List<PaperResearchResult>();
        }

        private void btnResearch_Click(object sender, RoutedEventArgs e)
        {
            //清空数据表里面的行
            paperResearchResultList.Clear();
            var paperList = (from wz in wz_dt
                             where wz["text文件"].GetType().Name!="DBNull"
                             select wz).ToList();

            string keyword = keywordTextBox.Text;

            //按照文章编号循环
            for (int i = 1; i <= paperList.Count; i++)
            {
                int paperId = paperList[i - 1].ID;
                string paperName = paperList[i - 1].文章名;
                string paperPath = paperList[i - 1].text文件;
                paperPath = MainWindow.path_translate(paperPath);

                try
                {
                    StreamReader sr = new StreamReader(paperPath, Encoding.Default);
                }
                catch
                {
                    MessageBox.Show("下列文件不存在："+paperPath);
                    break;
                }

                string[] filelist = File.ReadAllLines(paperPath, Encoding.Default);
                for (int linenum = 0; linenum <= filelist.Length - 1; linenum++)
                {
                    if (filelist[linenum].IndexOf(keyword) > -1)
                    {
                        PaperResearchResult row = new PaperResearchResult();
                        row.文章编号 = paperId;
                        row.文章名 = paperPath;
                        row.行号 = linenum + 1;

                        paperResearchResultList.Add(row);
                    }
                }
            }

            resultDataGrid.ItemsSource = null;
            resultDataGrid.ItemsSource = paperResearchResultList;
            resultDataGrid.CanUserAddRows = false;
            resultDataGrid.CanUserDeleteRows = false;

            //MessageBox.Show("查询完成");

        }

        private void resultDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.leftTabControl.SelectedItem = MainWindow.applicationTabItem;

            var paperResearchResult = resultDataGrid.SelectedItem as PaperResearchResult;
            if (paperResearchResult != null)
            {
                MainWindow.applicationUserControl.headerStr = "查询";
                string s = paperResearchResult.文章名;
                MainWindow.applicationUserControl.loadProcess(s);
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnResearch_Click(sender, e);
            }

        }
    }
}
