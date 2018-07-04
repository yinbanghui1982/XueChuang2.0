using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceResearchWpfApplication.TextManage
{
    class UsedPaper
    {
        ScienceResearchDataSetNew.文章DataTable wz_dt;
        public ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        public ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        public ScienceResearchDataSetNew.文章_关键词DataTable wz_gjc_dt;

        public UsedPaper()
        {
            wz_dt = MainWindow.wz_dt;
            gjc_dt = MainWindow.gjc_dt;
            xm_gjc_dt = MainWindow.xm_gjc_dt;
            wz_gjc_dt = MainWindow.wz_gjc_dt;
        }


        public List<ScienceResearchDataSetNew.文章Row>  provide_paper(string paperTypeStr)
        {
            List<ScienceResearchDataSetNew.文章Row> paperList = new List<ScienceResearchDataSetNew.文章Row>();
            if (ProjectLiteratureUserControl.paperFanwei == "全部")
            {
                paperList = (from wz in wz_dt
                             where wz[paperTypeStr].GetType().Name != "DBNull" && (string)wz[paperTypeStr]!=""
                             select wz).ToList();
            }
            else if (ProjectLiteratureUserControl.paperFanwei == "项目")
            {
                paperList = (from gjc in gjc_dt
                             join xm_gjc in xm_gjc_dt on gjc.ID equals xm_gjc.关键词ID
                             join wz_gjc in wz_gjc_dt on gjc.ID equals wz_gjc.关键词ID
                             join wz in wz_dt on wz_gjc.文章ID equals wz.ID
                             where xm_gjc.项目ID == MainWindow.projectId && wz[paperTypeStr].GetType().Name != "DBNull"
                             select wz).ToList();

            }
            else
            {
                paperList = (from gjc in ProjectLiteratureUserControl.keywordList
                             join wz_gjc in wz_gjc_dt on gjc.关键词 equals wz_gjc.关键词Row.关键词
                             join wz in wz_dt on wz_gjc.文章ID equals wz.ID
                             where wz[paperTypeStr].GetType().Name != "DBNull"
                             select wz).ToList();
            }

            return paperList;

        }

    }

    

}
