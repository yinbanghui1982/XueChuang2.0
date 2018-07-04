using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;
using System.Linq;
using System.Windows;
using System.IO;
using System.Data;
using System.Windows.Controls;
using ScienceResearchWpfApplication.DatabaseManage;
using ScienceResearchWpfApplication.TextManage;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// 根据关键词或者语段编号生成段文、段图、参考等控件
    /// </summary>
    class ParagraphResourceClass
    {
        protected ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        protected ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;
        protected ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        protected ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        protected ScienceResearchDataSetNew.语段DataTable yd_dt;
        protected ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;

        public ParagraphResourceClass()
        {
            yd_gjc_dt = MainWindow.yd_gjc_dt;
            yd_gjc_ta = MainWindow.yd_gjc_ta;
            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;
        }
    }


    class ParaResourceClass_PaperDataGrid : ParagraphResourceClass
    {
        //点击文章创作标签页的语段选项，生成相应的段图和段文标签页
        DataGrid referDataGrid, referKeywordDataGrid;
        int paraReferID;



        public ParaResourceClass_PaperDataGrid(DataGrid _referDataGrid, DataGrid _referKeywordDataGrid, int _paraReferID)
        {
            //文章创作标签页
            referDataGrid = _referDataGrid;
            referKeywordDataGrid = _referKeywordDataGrid;
            paraReferID = _paraReferID;
        }

        public ParaResourceClass_PaperDataGrid()
        {
            //语段标签页
        }

        public void referDataGrid_MouseLeftButtonUp()
        {
            if (referDataGrid.CurrentItem != null)
            {
                //选择变化时，查询该语段对应的关键词               

                var data = (from yd_gjc in yd_gjc_dt
                            join gjc in gjc_dt on yd_gjc.关键词ID equals gjc.ID
                            where yd_gjc.语段ID == paraReferID
                            select new Keyword
                            {
                                关键词 = gjc.关键词
                            }).ToList();
                referKeywordDataGrid.ItemsSource = data;

                //更新参考图片
                string figure_path = "";
                string figure_path_isf = "";
                var data2 = from data_item in yd_dt
                            where data_item.ID == paraReferID
                            select data_item;
                foreach (var d2 in data2)
                {
                    try
                    {
                        figure_path = d2.图片.ToString();
                        figure_path_isf = d2.图片isf.ToString();
                    }
                    catch { }
                }
                figure_path = MainWindow.path_translate(figure_path);
                figure_path_isf = MainWindow.path_translate(figure_path_isf);

                //墨笔文件
                ScienceResearchDataSetNew.语段Row yd = (ScienceResearchDataSetNew.语段Row)yd_dt.Rows.Find(paraReferID);
                if (figure_path_isf != "")
                {
                    MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.mainWindow.paragraphFigureTabItem_r;
                    creat_figure( MainWindow.mainWindow.paragraphFigureUserControl_r,yd);
                }
                else
                {                    
                    creat_text(yd, MainWindow.referUserControl_duanwen_right);
                    MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.referTabItem_duanwen_right;
                }
            }
        }

        public void creat_figure(ParagraphFigureUserControl paragraphFigureUserControl, ScienceResearchDataSetNew.语段Row yd)
        {
            int paraReferID = yd.ID;
            string figure_path = yd.图片;
            string figure_path_isf = yd.图片isf;
            figure_path = MainWindow.path_translate(figure_path);
            figure_path_isf = MainWindow.path_translate(figure_path_isf);

            if (File.Exists(figure_path))
            {
                //加载图片
                ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                paragraphFigureUserControl.img.Source = img;

                //加载笔记
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
                    figure_path_isf = ".\\科学研究\\图片ISF\\" + paraReferID.ToString() + ".isf";

                    //更新数据库
                    var data3 = from data_item in yd_dt
                                where data_item.ID == paraReferID
                                select data_item;
                    foreach (var d3 in data3)
                    {
                        d3.图片isf = figure_path_isf;
                        yd_ta.Update(yd_dt);
                    }

                    //更新墨笔文件路径
                    figure_path_isf = MainWindow.path_translate(figure_path_isf);
                    paragraphFigureUserControl.figure_path_isf = figure_path_isf;
                }
            }
            else
            {
                MessageBox.Show("图片文件不存在");
            }
        }

        public void creat_text(ScienceResearchDataSetNew.语段Row yd,ReferUserControl referUserControl)
        {
            if (yd.语段 != "")
            {
                ReferTabItemUserControl referTabItemUserControl = new ReferTabItemUserControl(yd);
                TabItem referTabItemTabItem = new TabItem();
                referTabItemTabItem.Header = yd.ID.ToString();
                referTabItemTabItem.Content = referTabItemUserControl;
                referUserControl.referTabControl.Items.Clear();
                referUserControl.referTabControl.Items.Add(referTabItemTabItem);
                referUserControl.referTabControl.SelectedItem = referTabItemTabItem;                
            }
        }
    }

    class ParaResourceClass_Refer : ParagraphResourceClass
    {
        int paraReferID;
        public ParaResourceClass_Refer(int _paraReferID)
        {
            paraReferID = _paraReferID;
        }

        public void creat_figure()
        {
            //创建图片   

        }
    }
}
