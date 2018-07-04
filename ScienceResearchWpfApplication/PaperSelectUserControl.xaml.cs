using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using System.Data.OleDb;
using System.Collections.Generic;
using ScienceResearchWpfApplication.TextManage;
using ScienceResearchWpfApplication.DatabaseManage;

namespace ScienceResearchWpfApplication
{
    class Paper2
    {
        public int ID { get; set; }
        public string 文章名 { get; set; }
    }

    /// <summary>
    /// PaperSelectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaperSelectUserControl : UserControl
    {

        static ScienceResearchDataSetNew.文章DataTable wz_dt;
        ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;
        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        int newDataId;


        public PaperSelectUserControl()
        {
            InitializeComponent();
            wz_dt = MainWindow.wz_dt;
            wz_ta = MainWindow.wz_ta;
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;

            var data = from wz in wz_dt
                       where wz["分类"] != DBNull.Value && (string)wz["分类"]=="创作"
                       orderby wz.ID
                       select new Paper2
                       {
                           ID = wz.ID,
                           文章名=wz.文章名
                       };
            paperDataGrid.ItemsSource = data;

            zhengRadioButton.IsChecked = true;
            wz_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            yd_ta.Adapter.RowUpdated += Adapter_RowUpdated;
        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                newDataId = newID;
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
            }
        }

        private void paperDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paperDataGrid_MouseLeftButtonUp(null, null);
        }

        private void paperDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //选择变化时，查询该关键词对应的语段
            if (paperDataGrid.SelectedItem != null)
            {
                var paper = paperDataGrid.SelectedItem as Paper2;
                int paperId = paper.ID;

                ScienceResearchDataSetNew.文章Row wz_using = (ScienceResearchDataSetNew.文章Row)wz_dt.Rows.Find(paperId);
                string file_str = wz_using.文件;

                if (file_str != "")
                {
                    if (MainWindow.paperIdList_left.Contains(paperId) || MainWindow.paperIdList_right.Contains(paperId))
                    {
                        //MessageBox.Show("该Word文章已经打开");
                        return;
                    }
                }

                //查看是否已经打开                
                PaperUserControl paperUserControl = new PaperUserControl(paperId);
                TabItem paperTabItem = new TabItem();
                string paperNameString;
                if (paper.文章名.Length > 3)
                    paperNameString = "[" + paperId + "]" + paper.文章名.Substring(0, 3);
                else
                    paperNameString = "[" + paperId + "]" + paper.文章名;

                Label headerLabel = new Label();
                headerLabel.Content = paperNameString;
                paperTabItem.Header = headerLabel;
                paperTabItem.Content = paperUserControl;
                headerLabel.MouseLeftButtonUp += HeaderLabel_MouseLeftButtonUp;

                if (MainWindow.isZheng)
                {
                    if (!MainWindow.paperIdList_left.Contains(paperId))
                    {
                        paperUserControl.location = "left";
                        MainWindow.mainWindow.leftTabControl.Items.Add(paperTabItem);
                        headerLabel.MouseDoubleClick += HeaderLabelLeft_MouseDoubleClick;
                        MainWindow.paperIdList_left.Add(paperId);
                        MainWindow.paperUserControlList_left.Add(paperUserControl);
                        MainWindow.paperUserControlTabItemList_left.Add(paperTabItem);
                    }
                    else
                    {
                        //MessageBox.Show("该文章已经在左栏打开");
                    }
                }
                else
                {
                    if (!MainWindow.paperIdList_right.Contains(paperId))
                    {
                        paperUserControl.location = "right";
                        MainWindow.mainWindow.rightTabControl.Items.Add(paperTabItem);
                        headerLabel.MouseDoubleClick += HeaderLabelRight_MouseDoubleClick;
                        MainWindow.paperIdList_right.Add(paperId);
                    }
                    else
                    {
                        //MessageBox.Show("该文章已经在右栏打开");
                    }
                }

            }
        }

        private void HeaderLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string headerStr= (string)((Label)sender).Content;
            string paperIdStr = headerStr.Substring(headerStr.IndexOf("[") + 1, headerStr.IndexOf("]") - headerStr.IndexOf("[")-1);
            int paperId = int.Parse(paperIdStr);
            PaperHeaderClick(paperId);
        }

        /// <summary>
        /// 文章标题单击事件
        /// </summary>
        /// <param name="paperId"></param>
        public static void PaperHeaderClick(int paperId)
        {
            //改变状态栏文字
            var paper_list = (from wz in wz_dt
                              where wz.ID == paperId
                              select wz).ToList();
            int xiangmuId = 0;
            if (paper_list[0].项目Row != null)
                xiangmuId = paper_list[0].项目Row.ID;
            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "选中的文章为：ID=" + paperId + "，项目ID=" + xiangmuId;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            if (xiangmuId!=0)
                DataBaseRowManage.ProjectMapping(xiangmuId);
        }

        private void HeaderLabelLeft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.paperIdList_left.Remove(((PaperUserControl)(((TabItem)MainWindow.mainWindow.leftTabControl.SelectedItem).Content)).paperId);
            MainWindow.paperUserControlList_left.Remove((PaperUserControl)(((TabItem)MainWindow.mainWindow.leftTabControl.SelectedItem).Content));
            MainWindow.paperUserControlTabItemList_left.Remove((TabItem)MainWindow.mainWindow.leftTabControl.SelectedItem);
            MainWindow.mainWindow.leftTabControl.Items.Remove(MainWindow.mainWindow.leftTabControl.SelectedItem);
        }
        private void HeaderLabelRight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.paperIdList_right.Remove(((PaperUserControl)(((TabItem)MainWindow.mainWindow.rightTabControl.SelectedItem).Content)).paperId);
            MainWindow.mainWindow.rightTabControl.Items.Remove(MainWindow.mainWindow.rightTabControl.SelectedItem);
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }

        private void zhengRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.isZheng = true;
        }

        private void fanRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.isZheng = false;
        }


        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var data = from wz in wz_dt
                       where wz["分类"] != DBNull.Value && (string)wz["分类"] == "创作"
                       select new Paper2
                       {
                           ID = wz.ID,
                           文章名 = wz.文章名
                       };
            paperDataGrid.ItemsSource = data;
        }

        private void createPaperButton_Click(object sender, RoutedEventArgs e)
        {
            //创建文章和空白语段
            string projectId = projectIdTextBox.Text;
            string paperName = paperNameTextBox.Text;

            //新建文章
            DataRow newRow;
            newRow = wz_dt.NewRow();

            try
            {
                newRow["文章名"] = paperName;
                newRow["分类"] = "创作";
                newRow["项目ID"] = projectId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            wz_dt.Rows.Add(newRow);
            try
            {
                //新建空白语段
                wz_ta.Update(wz_dt);
                int paperId_white = newDataId;
                int duanshu=0;
                try
                {
                    duanshu = int.Parse(paraNumberTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    var wz_del_list = (from wz in wz_dt
                                  where wz.ID == paperId_white
                                  select wz).ToList();

                    wz_del_list[0].Delete();
                    wz_ta.Update(wz_dt);
                }

                for (int i = 0; i < duanshu; i++)
                {
                    newRow = yd_dt.NewRow();
                    newRow["排序"] = i + 1;
                    newRow["文章ID"] = paperId_white;
                    yd_dt.Rows.Add(newRow);
                    try
                    {
                        yd_ta.Update(yd_dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        yd_ta.Fill(yd_dt);
                    }
                }

                //刷新打开文章表
                var data = from wz in wz_dt
                           where wz["分类"] != DBNull.Value && (string)wz["分类"] == "创作"
                           select new Paper2
                           {
                               ID = wz.ID,
                               文章名 = wz.文章名
                           };
                paperDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                wz_ta.Fill(wz_dt);
            }

            MessageBox.Show("文章创建成功");       
        }

        private void addParaButton_Click(object sender, RoutedEventArgs e)
        {
            //为某一篇文章，批量添加参考语段
            int paperId, duanshu;

            try
            {
                paperId = int.Parse(paraIdTextBox.Text);
                duanshu = int.Parse(numberTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            DataRow newRow;

            for (int i = 0; i < duanshu; i++)
            {
                newRow = yd_dt.NewRow();
                newRow["文章ID"] = paperId;
                yd_dt.Rows.Add(newRow);
                try
                {
                    yd_ta.Update(yd_dt);
                    int paraId = newDataId;
                    var data = (from yd in yd_dt
                                where yd.ID == paraId
                                select yd).ToList();
                    data[0].图片 = ".\\科学研究\\图片\\"+paraId+".tif";
                    yd_ta.Update(yd_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    yd_ta.Fill(yd_dt);
                    return;
                }

            }
            MessageBox.Show("空白语段添加成功");
        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            string path;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择pdf所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;

                //导入所有文件
                InputDir(path);
            }
        }

        private void InputDir(string srcPath)
        {            
            string fullName,paperName,paperXiangduiPath,paperTextXiangduiPath;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);                
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        //subdir.Delete(true);          //删除子目录和文件
                        InputDir(subdir.FullName);
                    }
                    else
                    {
                        if (i.Extension == ".pdf" || i.Extension == ".PDF" || i.Extension == ".txt" || i.Extension == ".TXT")
                        {
                            fullName = i.FullName;
                            paperName = i.Name.Substring(0,i.Name.Length-4);

                            //判断文章表中是否有该名称的文件
                            var data = (from wz in wz_dt
                                       where wz.文章名==paperName
                                       select wz).ToList();
                            if (data.Count==0)
                            {
                                //如果文章表中没有，则在文章表中创建新的记录
                                DataRow newRow;
                                newRow = wz_dt.NewRow();
                                newRow["文章名"] = paperName;

                                //将绝对路径改为相对路径
                                MatchCollection mc;
                                List<Regex> regex_list = new List<Regex>();
                                regex_list.Add(new Regex("科学研究"));
                                regex_list.Add(new Regex("社会科学"));
                                regex_list.Add(new Regex("文学艺术"));
                                regex_list.Add(new Regex("应用科学"));
                                regex_list.Add(new Regex("哲学"));
                                regex_list.Add(new Regex("自然科学"));

                                for (int regex_int = 0; regex_int < regex_list.Count; regex_int++)
                                {
                                    Regex r = regex_list[regex_int];
                                    mc = r.Matches(fullName);
                                    if (mc.Count != 0)
                                    {
                                        int index = mc[0].Index;        //正则表达式位置

                                        int qian_location = 1;          //正则表达式的前一字符位置
                                        int hou_location;               //正则表达式的后一字符位置
                                        if (mc[0].Value=="哲学")
                                            hou_location = 2;   
                                        else
                                            hou_location = 4;

                                        int houZhui_length = 4;         //扩展名长度

                                        char qian = fullName[index - qian_location];
                                        char hou = fullName[index + hou_location];
                                        if ((qian == '/' || qian == '\\') & (hou == '/' || hou == '\\'))
                                        {
                                            if (i.Extension == ".pdf" || i.Extension == ".PDF")
                                            {
                                                paperXiangduiPath = ".\\" + fullName.Substring(index);
                                                newRow["文件"] = paperXiangduiPath;
                                            }

                                            string hh = fullName.Substring(index);
                                            string kk = hh.Substring(0, hh.Length - houZhui_length);
                                            paperTextXiangduiPath = ".\\" + kk + ".txt";
                                            newRow["text文件"] = paperTextXiangduiPath;

                                            wz_dt.Rows.Add(newRow);
                                            wz_ta.Update(wz_dt);

                                            //将pdf文件内容写入txt文件
                                            if (i.Extension == ".pdf" || i.Extension == ".PDF")
                                            {
                                                string text = "";
                                                try
                                                {
                                                    string pdffilename = fullName;
                                                    PdfReader pdfReader = new PdfReader(pdffilename);
                                                    int numberOfPages = pdfReader.NumberOfPages;
                                                    text = string.Empty;

                                                    for (int j = 1; j <= numberOfPages; ++j)
                                                    {
                                                        iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                                                        text += iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, j, strategy);
                                                    }
                                                    pdfReader.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    StreamWriter wlog = File.AppendText(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\mylog.log");
                                                    wlog.WriteLine("出错文件：" + "原因：" + ex.ToString());
                                                    wlog.Flush();
                                                    wlog.Close();
                                                }

                                                string ss = fullName.Substring(0, fullName.Length - houZhui_length) + ".txt";
                                                StreamWriter fileWriter = new StreamWriter(ss, true);
                                                fileWriter.Write(text);
                                                fileWriter.Close();
                                            }
                                        }
                                    }
                                }                                                                
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        
    }
}
