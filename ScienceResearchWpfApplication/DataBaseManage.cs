using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace ScienceResearchWpfApplication.DatabaseManage
{
    /// <summary>
    /// 数据库管理
    /// *删除语段、文章
    /// *关键词映射
    /// *项目映射
    /// </summary>
    class DataBaseRowManage
    {
        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="wz"></param>
        public static void DeleteWz(ScienceResearchDataSetNew.文章Row wz)
        {
            if (MessageBox.Show("确认删除该文章及其所有关键词和语段", "重要", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //删除所有关键词
                List<ScienceResearchDataSetNew.文章_关键词Row> wz_gjc_list = GetWzGjc(wz);
                foreach (ScienceResearchDataSetNew.文章_关键词Row wz_gjc in wz_gjc_list)
                    wz_gjc.Delete();
                MainWindow.wz_gjc_ta.Update(MainWindow.wz_gjc_dt);

                //删除所有语段
                OrderedEnumerableRowCollection<ScienceResearchDataSetNew.语段Row> ydCollection = GetYd(wz);
                foreach (ScienceResearchDataSetNew.语段Row yd in ydCollection)
                    DeleteYd(yd);

                //删除该文章
                wz.Delete();
                MainWindow.wz_ta.Update(MainWindow.wz_dt);
            }
            else
            {
                return;
            }            
        }

        /// <summary>
        /// 获取文章的关键词列表
        /// </summary>
        /// <param name="wz">文章</param>
        /// <returns>关键词列表</returns>
        public static List<ScienceResearchDataSetNew.文章_关键词Row> GetWzGjc(ScienceResearchDataSetNew.文章Row wz)
        {
            return (from wz_gjc in MainWindow.wz_gjc_dt
                    join gjc in MainWindow.gjc_dt on wz_gjc.关键词ID equals gjc.ID
                    where wz_gjc.文章ID == wz.ID
                    select wz_gjc).ToList();
        }

        /// <summary>
        /// 获取文章的所有语段
        /// </summary>
        /// <param name="wz">文章</param>
        /// <returns>语段有序集合</returns>
        public static OrderedEnumerableRowCollection<ScienceResearchDataSetNew.语段Row> GetYd(ScienceResearchDataSetNew.文章Row wz)
        {
            return from data_item in MainWindow.yd_dt
                   where data_item.文章ID == wz.ID
                   orderby data_item.排序 ascending
                   select data_item;
        }

        /// <summary>
        /// 删除某一语段
        /// </summary>
        /// <param name="yd">语段</param>
        public static void DeleteYd(ScienceResearchDataSetNew.语段Row yd)
        {
            //删除关键词
            var keyword_list = (from data_item in MainWindow.yd_gjc_dt
                                where data_item.语段ID == yd.ID
                                select data_item).ToList();
            foreach (ScienceResearchDataSetNew.语段_关键词Row yd_gjc in keyword_list)
                yd_gjc.Delete();
            MainWindow.yd_gjc_ta.Update(MainWindow.yd_gjc_dt);

            //删除语段
            yd.Delete();
            MainWindow.yd_ta.Update(MainWindow.yd_dt);
            
        }

        /// <summary>
        /// 给定关键词ID，打开关键词映射
        /// </summary>
        /// <param name="gjcId"></param>
        public static void KeywordMapping(int gjcId)
        {
            var gjc_list = (from gjc in MainWindow.gjc_dt
                            where gjc.ID == gjcId
                            select gjc).ToList();
            int id = gjc_list[0].ID;
            MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedValuePath = "ID";
            MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedValue = id;
            MainWindow.keywordMappingUserControl.keywordDataGrid.ScrollIntoView(MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedItem);
        }

        /// <summary>
        /// 给定关键词ID，打开项目映射
        /// </summary>
        /// <param name="xmId"></param>
        public static void ProjectMapping(int xmId)
        {
            var xm_list = (from xm in MainWindow.xm_dt
                            where xm.ID == xmId
                            select xm).ToList();
            int id = xm_list[0].ID;

            MainWindow.projectMappingUserControl.projectDataGrid.SelectedValuePath = "ID";
            MainWindow.projectMappingUserControl.projectDataGrid.SelectedValue = id;
            MainWindow.projectMappingUserControl.projectDataGrid.ScrollIntoView(MainWindow.projectMappingUserControl.projectDataGrid.SelectedItem);
        }

    }
}
