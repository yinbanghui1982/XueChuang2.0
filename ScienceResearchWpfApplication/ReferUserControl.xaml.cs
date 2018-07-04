using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.Data.OleDb;
using System.Data;


namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// ReferUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReferUserControl : UserControl
    {
        RegistryKey scienceResearchKey;
        public string type;    // yd---语段；wz——文章；refer——参考
        public static string type_show;

        ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        ScienceResearchDataSetNew.单词DataTable dc_dt;
        ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;
        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;


        int new_yd_id;

        Color color_mode, color_black;

        public ReferUserControl(string _type)
        {
            type = _type;
            InitializeComponent();
            scienceResearchKey = MainWindow.scienceResearchKey;      
            

            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;

            color_mode = ColorManager.color_mode_word;
            color_black = Colors.Black;

        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
                new_yd_id = newID;
            }
        }

  

       
    }
}
