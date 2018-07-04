using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XYDES;
using ScienceResearchWpfApplication.ApplicationProgram;
using ScienceResearchWpfApplication.DatabaseManage;
using ScienceResearchWpfApplication.FigureManage;
using ScienceResearchWpfApplication.TextManage;
using ScienceResearchWpfApplication.Share;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 变量
        public static MainWindow mainWindow;

        //----------------数据库----------------------------------------------
        public static ScienceResearchDataSetNew scienceResearchDataSetNew;

        public static ScienceResearchDataSetNew.关键词DataTable gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;

        public static ScienceResearchDataSetNew.单词DataTable dc_dt;
        public static ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;
        public static ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;

        public static ScienceResearchDataSetNew.短语DataTable dy_dt;
        public static ScienceResearchDataSetNewTableAdapters.短语TableAdapter dy_ta;
        public static ScienceResearchDataSetNew.短语_关键词DataTable dy_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.短语_关键词TableAdapter dy_gjc_ta;

        public static ScienceResearchDataSetNew.句型DataTable jx_dt;
        public static ScienceResearchDataSetNewTableAdapters.句型TableAdapter jx_ta;
        public static ScienceResearchDataSetNew.句型_关键词DataTable jx_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.句型_关键词TableAdapter jx_gjc_ta;

        public static ScienceResearchDataSetNew.语段DataTable yd_dt;
        public static ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;
        public static ScienceResearchDataSetNew.语段_关键词DataTable yd_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter yd_gjc_ta;

        public static ScienceResearchDataSetNew.文章DataTable wz_dt;
        public static ScienceResearchDataSetNewTableAdapters.文章TableAdapter wz_ta;
        public static ScienceResearchDataSetNew.文章_关键词DataTable wz_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.文章_关键词TableAdapter wz_gjc_ta;

        public static ScienceResearchDataSetNew.图片DataTable tp_dt;
        public static ScienceResearchDataSetNewTableAdapters.图片TableAdapter tp_ta;
        public static ScienceResearchDataSetNew.图片_关键词DataTable tp_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.图片_关键词TableAdapter tp_gjc_ta;

        public static ScienceResearchDataSetNew.图片创作DataTable tpcz_dt;
        public static ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter tpcz_ta;
        public static ScienceResearchDataSetNew.图片创作_关键词DataTable tpcz_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.图片创作_关键词TableAdapter tpcz_gjc_ta;

        public static ScienceResearchDataSetNew.项目DataTable xm_dt;
        public static ScienceResearchDataSetNewTableAdapters.项目TableAdapter xm_ta;
        public static ScienceResearchDataSetNew.项目_关键词DataTable xm_gjc_dt;
        public static ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter xm_gjc_ta;

        public static ScienceResearchDataSetNew.仿真DataTable fz_dt;
        public static ScienceResearchDataSetNewTableAdapters.仿真TableAdapter fz_ta;

        public static ScienceResearchDataSetNew.文件位置DataTable wjwz_dt;
        public static ScienceResearchDataSetNewTableAdapters.文件位置TableAdapter wjwz_ta;

        //----------------用户控件----------------------------------------------
        //===============左侧=================================
        public static PaperSelectUserControl paperSelectUserControl;
        public static AppUserControl appUserControl;
        public static ApplicationUserControl applicationUserControl;
        public static TabItem applicationTabItem;
        public static ApplicationUserControl applicationUserControl_r;
        public static TabItem applicationTabItem_r;
        public static FigureCreateUserControl figureCreateUserControl;
        ScreenShotUserControl screenShotUserControl;
        TabItem screenShotTabItem;
        public static SetupUserControl setupUserControl;
        public static ParagraphFigureUserControl paragraphFigureUserControl_l;
        public static TabItem paragraphFigureTabItem_l;
        public static ScienceResearchInkCanvas scienceResearchInkCanvas_l;
        public static System.Windows.Controls.Image paragraphImage_l;
        public static KeywordTreeUserControl keywordTreeUserControl;
        public static TabItem keywordTreeTabItem;
        public static PaperUserControl paperUserControl;

        public static ReferUserControl referUserControl_wenzhang_left;
        public static TabItem referTabItem_wenzhang_left;

        public static ProjectMappingUserControl projectMappingUserControl;

        //===============右侧=================================
        public static ScienceResearchInkCanvas scienceResearchInkCanvas_r;
        public static System.Windows.Controls.Image paragraphImage_r;

        public static ReferUserControl referUserControl;
        public static TabItem referTabItem;
        public static ReferUserControl referUserControl_duanwen_left;
        public static TabItem referTabItem_duanwen_left;
        public static ReferUserControl referUserControl_duanwen_right;
        public static TabItem referTabItem_duanwen_right;

        public static ProjectLiteratureUserControl projectLiteratureUserControl;

        public static WebUserControl webUserControl;
        public static TabItem webTabItem;

        public static KeywordMappingUserControl keywordMappingUserControl;
        public static TabItem keywordMappingTabItem;

        public static DataBaseUserControl dataBaseUserControl_fz;
        public static DataBaseUserControl dataBaseUserControl_wjwz;
        public static DataBaseUserControl dataBaseUserControl_gjc;
        public static DataBaseUserControl dataBaseUserControl_dc;
        public static DataBaseUserControl dataBaseUserControl_dy;
        public static DataBaseUserControl dataBaseUserControl_jx;
        public static DataBaseUserControl dataBaseUserControl_yd;
        public static DataBaseUserControl dataBaseUserControl_wz;
        public static DataBaseUserControl dataBaseUserControl_xm;
        public static DataBaseUserControl dataBaseUserControl_tp;
        public static DataBaseUserControl dataBaseUserControl_tpcz;
        public static TabItem dataBaseTabItem_fz;
        public static TabItem dataBaseTabItem_wjwz;
        public static TabItem dataBaseTabItem_gjc;
        public static TabItem dataBaseTabItem_dc;
        public static TabItem dataBaseTabItem_dy;
        public static TabItem dataBaseTabItem_jx;
        public static TabItem dataBaseTabItem_yd;
        public static TabItem dataBaseTabItem_wz;
        public static TabItem dataBaseTabItem_xm;
        public static TabItem dataBaseTabItem_tp;
        public static TabItem dataBaseTabItem_tpcz;

        public static ConnectUserControl connectUserControl;
        public static TabItem connectTabItem;

        public static ShareUserControl shareUserControl;
        public static TabItem shareTabItem;


        public static string tabControlInUse;
        public static PaperUserControl wordPaperUserControl;

        public static Socket socketClient;
        public static Thread threadClient;

        //----------------Windows API----------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        const int WM_CLOSE = 0x0010;
        const int WM_USER = 0x0400;
        const int WM_APP = 0x8000;
        //==================截图========================
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT rect);
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        public static Bitmap bitBmp;

        //----------------变量----------------------------------------------
        public static RegistryKey scienceResearchKey;               //注册表项
        public static string path_database;         //工作文件夹地址
        public static OleDbConnection conn;         //数据库连接
        public static string conn_string;           //数据库连接字符串
        public static int paperId;                  //文章编号
        public static int projectId;             //项目编号
        public static string mac_string;            //mac地址
        public static string key_string;            //密钥
        List<string> macs;                        //所有mac地址
        public static List<IntPtr> intPtrs;         //所有打开的进程的句柄

        public static bool isZheng;                 //是否按照常规排布窗     
        public static bool app_included;            //应用程序是否包含在窗体内部
        public static int inkPropertiesUserControlIndex = 0;

        public static int left_l;
        public static int right_l;
        public static int top_l;
        public static int bottem_l;

        public static int left_r;
        public static int right_r;
        public static int top_r;
        public static int bottem_r;

        public static List<int> paperIdList_left;
        public static List<PaperUserControl> paperUserControlList_left;
        public static List<TabItem> paperUserControlTabItemList_left;
        public static List<int> paperIdList_right;
        public static List<int> hongguanList;

        string dataBaseFilePath;

        public static double lineHeight = 20;

        public static double yd_cz_width;
        public static double yd_ck_width;

        public static string txt_file_encoding;

        public static XamlManageClass xamlManageClass = new XamlManageClass();

        public static double yd_mode_rate = .8;         //判断语段是否拥有一个语段模式的比率

        public static string[] valueNames;              //注册表所有子项的名称


        #endregion

        #region 构造函数

        private static string GetConnectionStringsConfig(string connectionName)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
            return connectionString;
        }

        private static void UpdateConnectionStringsConfig(string newName,string newConString,string newProviderName)
        {
            bool isModified = false;    
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        /// <summary>
        /// 删除指定文件夹里面的所有pdf文件
        /// </summary>
        /// <param name="srcPath"></param>
        public static void DelectDir(string srcPath)
        {
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
                        DelectDir(subdir.FullName);
                    }
                    else
                    {
                        if (i.Extension==".pdf"|| i.Extension == ".PDF" || i.Extension == ".caj" || i.Extension == ".pdz")
                            File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            #region 清除文件夹
            //string srcPath = @"C:\Users\surface\Documents\学创\自然科学";
            //DelectDir(srcPath);
            #endregion

            #region 注册表初始化            
            RegistryKey key = Registry.CurrentUser;
            RegistryKey key1 = key.OpenSubKey("Software");
            string[] valueNames1;
            valueNames1 = key1.GetValueNames();

            if (Array.IndexOf<string>(valueNames1, "ScienceResearch") == -1)
            {
                key.CreateSubKey("Software\\ScienceResearch");
                scienceResearchKey = key1.OpenSubKey("ScienceResearch", true);
            }
            else
            {
                scienceResearchKey = key1.OpenSubKey("ScienceResearch", true);
            }
            
            valueNames = scienceResearchKey.GetValueNames();
            //===========注册码============
            if (Array.IndexOf<string>(valueNames, "SV001") == -1)
            {
                scienceResearchKey.SetValue("SV001", "-1");
            }

            if (Array.IndexOf<string>(valueNames, "SV002") == -1)           //表示系统变量：开始使用时间
            {
                scienceResearchKey.SetValue("SV002", "-1");
            }
            //===========图片===========
            if (Array.IndexOf<string>(valueNames, "FigureFileName") == -1)
            {
                scienceResearchKey.SetValue("FigureFileName", "-1");
            }
            if (Array.IndexOf<string>(valueNames, "FigureIsfFileName") == -1)
            {
                scienceResearchKey.SetValue("FigureIsfFileName", "-1");
            }
            //===========墨笔===========
            //0
            if (Array.IndexOf<string>(valueNames, "Ink_Bi") == -1)
            {
                scienceResearchKey.SetValue("Ink_Bi", "Gangbi");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Slider") == -1)
            {
                scienceResearchKey.SetValue("Ink_Slider", "3");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Color") == -1)
            {
                scienceResearchKey.SetValue("Ink_Color", "#FFFF0000");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_EditingMode") == -1)
            {
                scienceResearchKey.SetValue("Ink_EditingMode", "Write");
            }
            //1
            if (Array.IndexOf<string>(valueNames, "Ink_Bi_1") == -1)
            {
                scienceResearchKey.SetValue("Ink_Bi_1", "Gangbi");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Slider_1") == -1)
            {
                scienceResearchKey.SetValue("Ink_Slider_1", "3");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Color_1") == -1)
            {
                scienceResearchKey.SetValue("Ink_Color_1", "#FFFF0000");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_EditingMode_1") == -1)
            {
                scienceResearchKey.SetValue("Ink_EditingMode_1", "Write");
            }
            //2
            if (Array.IndexOf<string>(valueNames, "Ink_Bi_2") == -1)
            {
                scienceResearchKey.SetValue("Ink_Bi_2", "Gangbi");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Slider_2") == -1)
            {
                scienceResearchKey.SetValue("Ink_Slider_2", "3.86330935251799");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Color_2") == -1)
            {
                scienceResearchKey.SetValue("Ink_Color_2", "#FF000000");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_EditingMode_2") == -1)
            {
                scienceResearchKey.SetValue("Ink_EditingMode_2", "Write");
            }
            //3
            if (Array.IndexOf<string>(valueNames, "Ink_Bi_3") == -1)
            {
                scienceResearchKey.SetValue("Ink_Bi_3", "Yingguangbi");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Slider_3") == -1)
            {
                scienceResearchKey.SetValue("Ink_Slider_3", "30");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_Color_3") == -1)
            {
                scienceResearchKey.SetValue("Ink_Color_3", "#FFFFFF00");
            }
            if (Array.IndexOf<string>(valueNames, "Ink_EditingMode_3") == -1)
            {
                scienceResearchKey.SetValue("Ink_EditingMode_3", "Write");
            }

            //===========数据库============

            if (Array.IndexOf<string>(valueNames, "PathDataBase") == -1)
            {
                scienceResearchKey.SetValue("PathDataBase", "-1");
            }

            if (Array.IndexOf<string>(valueNames, "DataBaseVersion") == -1)
            {
                scienceResearchKey.SetValue("DataBaseVersion", "1");
            }

            //===========设置============
            if (Array.IndexOf<string>(valueNames, "PaperInkOrText") == -1)
            {
                scienceResearchKey.SetValue("PaperInkOrText", "Text");
            }
            if (Array.IndexOf<string>(valueNames, "DataBaseInkOrText") == -1)
            {
                scienceResearchKey.SetValue("DataBaseInkOrText", "Text");
            }
            if (Array.IndexOf<string>(valueNames, "FontFamily_PaperUserControl_Scroll") == -1)
            {
                scienceResearchKey.SetValue("FontFamily_PaperUserControl_Scroll", "SimSun");
            }
            if (Array.IndexOf<string>(valueNames, "Fontsize_PaperUserControl_Scroll") == -1)
            {
                scienceResearchKey.SetValue("Fontsize_PaperUserControl_Scroll", "26");
            }
            if (Array.IndexOf<string>(valueNames, "ScreenShot_Left") == -1)
            {
                scienceResearchKey.SetValue("ScreenShot_Left", "0,20,110,60");
            }
            if (Array.IndexOf<string>(valueNames, "ScreenShot_Right") == -1)
            {
                scienceResearchKey.SetValue("ScreenShot_Right", "10,50,170,50");
            }
            //===========截图============
            if (Array.IndexOf<string>(valueNames, "ScreenShotWindowHandle") == -1)
            {
                scienceResearchKey.SetValue("ScreenShotWindowHandle", "0x00470C72");
            }
            //===========当前文档和项目============
            if (Array.IndexOf<string>(valueNames, "PaperId") == -1)
            {
                scienceResearchKey.SetValue("PaperId", "-1");
            }
            if (Array.IndexOf<string>(valueNames, "ProjectId") == -1)
            {
                scienceResearchKey.SetValue("ProjectId", "-1");
            }
            //===========语段宽度============
            if (Array.IndexOf<string>(valueNames, "yd_cz_width") == -1)
            {
                scienceResearchKey.SetValue("yd_cz_width", "580");
            }
            if (Array.IndexOf<string>(valueNames, "yd_ck_width") == -1)
            {
                scienceResearchKey.SetValue("yd_ck_width", "700");
            }
            string yd_cz_width_str = scienceResearchKey.GetValue("yd_cz_width").ToString();
            yd_cz_width = double.Parse(yd_cz_width_str);
            string yd_ck_width_str = scienceResearchKey.GetValue("yd_ck_width").ToString();
            yd_ck_width = double.Parse(yd_ck_width_str);
            //===========文本格式============
            if (Array.IndexOf<string>(valueNames, "txt_file_encoding") == -1)
            {
                scienceResearchKey.SetValue("txt_file_encoding", "Default");
            }
            txt_file_encoding = scienceResearchKey.GetValue("txt_file_encoding").ToString();
            //===========模式识别和匹配============
            ModeSetup.set_initial_value();

            #endregion

            #region 数据库
            //string conn = GetConnectionStringsConfig("ScienceResearchWpfApplication.Properties.Settings.科学研究_NewConnectionString2");
            //int loc1 = conn.LastIndexOf('=');
            //int loc2 = conn.LastIndexOf('\\');

            //path_database = conn.Substring(loc1 + 1, loc2 - loc1 - 1);

            //path_database = Environment.GetEnvironmentVariable("ScienceResearchPath");                                      //数据库所在文件夹
            //conn_string = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path_database + "\\科学研究_New.accdb";        //连接字符串


            //while(conn!=conn_string)
            //    UpdateConnectionStringsConfig("ScienceResearchWpfApplication.Properties.Settings.科学研究_NewConnectionString", conn_string, "System.Data.OleDb");


            //string str = this.GetType().Assembly.Location;

            //调试时候使用
            DirectoryInfo topDir = Directory.GetParent(Environment.CurrentDirectory);
            path_database = topDir.Parent.Parent.Parent.FullName;


            //发行时候使用
            //path_database = Environment.CurrentDirectory;

            //是指数据库文件路径
            scienceResearchKey.SetValue("PathDataBase", path_database);

            //数据库所在文件夹
            conn_string = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path_database + "\\科学研究2.accdb";        //连接字符串
            //MessageBox.Show(conn_string);            
            dataBaseFilePath = path_database + "\\科学研究2.accdb";

            //数据库升级
            database_update(); 
            conn = new OleDbConnection(conn_string);
            try
            {
                conn.Open();
            }
            catch
            {
                MessageBox.Show("数据库未能正确连接！");
                return;
            }

            try
            {
                scienceResearchDataSetNew = new ScienceResearchDataSetNew();

                gjc_dt = scienceResearchDataSetNew.关键词;
                gjc_ta = new ScienceResearchDataSetNewTableAdapters.关键词TableAdapter();
                gjc_ta.Fill(gjc_dt);

                dc_dt = scienceResearchDataSetNew.单词;
                dc_ta = new ScienceResearchDataSetNewTableAdapters.单词TableAdapter();
                dc_ta.Fill(dc_dt);
                dc_gjc_dt = scienceResearchDataSetNew.单词_关键词;
                dc_gjc_ta = new ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter();
                dc_gjc_ta.Fill(dc_gjc_dt);

                dy_dt = scienceResearchDataSetNew.短语;
                dy_ta = new ScienceResearchDataSetNewTableAdapters.短语TableAdapter();
                dy_ta.Fill(dy_dt);
                dy_gjc_dt = scienceResearchDataSetNew.短语_关键词;
                dy_gjc_ta = new ScienceResearchDataSetNewTableAdapters.短语_关键词TableAdapter();
                dy_gjc_ta.Fill(dy_gjc_dt);

                jx_dt = scienceResearchDataSetNew.句型;
                jx_ta = new ScienceResearchDataSetNewTableAdapters.句型TableAdapter();
                jx_ta.Fill(jx_dt);
                jx_gjc_dt = scienceResearchDataSetNew.句型_关键词;
                jx_gjc_ta = new ScienceResearchDataSetNewTableAdapters.句型_关键词TableAdapter();
                jx_gjc_ta.Fill(jx_gjc_dt);

                yd_dt = scienceResearchDataSetNew.语段;
                yd_ta = new ScienceResearchDataSetNewTableAdapters.语段TableAdapter();
                yd_ta.Fill(yd_dt);
                yd_gjc_dt = scienceResearchDataSetNew.语段_关键词;
                yd_gjc_ta = new ScienceResearchDataSetNewTableAdapters.语段_关键词TableAdapter();
                yd_gjc_ta.Fill(yd_gjc_dt);

                wz_dt = scienceResearchDataSetNew.文章;
                wz_ta = new ScienceResearchDataSetNewTableAdapters.文章TableAdapter();
                wz_ta.Fill(wz_dt);
                wz_gjc_dt = scienceResearchDataSetNew.文章_关键词;
                wz_gjc_ta = new ScienceResearchDataSetNewTableAdapters.文章_关键词TableAdapter();
                wz_gjc_ta.Fill(wz_gjc_dt);

                tp_dt = scienceResearchDataSetNew.图片;
                tp_ta = new ScienceResearchDataSetNewTableAdapters.图片TableAdapter();
                tp_ta.Fill(tp_dt);
                tp_gjc_dt = scienceResearchDataSetNew.图片_关键词;
                tp_gjc_ta = new ScienceResearchDataSetNewTableAdapters.图片_关键词TableAdapter();
                tp_gjc_ta.Fill(tp_gjc_dt);

                tpcz_dt = scienceResearchDataSetNew.图片创作;
                tpcz_ta = new ScienceResearchDataSetNewTableAdapters.图片创作TableAdapter();
                tpcz_ta.Fill(tpcz_dt);
                tpcz_gjc_dt = scienceResearchDataSetNew.图片创作_关键词;
                tpcz_gjc_ta = new ScienceResearchDataSetNewTableAdapters.图片创作_关键词TableAdapter();
                tpcz_gjc_ta.Fill(tpcz_gjc_dt);

                xm_dt = scienceResearchDataSetNew.项目;
                xm_ta = new ScienceResearchDataSetNewTableAdapters.项目TableAdapter();
                xm_ta.Fill(xm_dt);
                xm_gjc_dt = scienceResearchDataSetNew.项目_关键词;
                xm_gjc_ta = new ScienceResearchDataSetNewTableAdapters.项目_关键词TableAdapter();
                xm_gjc_ta.Fill(xm_gjc_dt);

                fz_dt = scienceResearchDataSetNew.仿真;
                fz_ta = new ScienceResearchDataSetNewTableAdapters.仿真TableAdapter();
                fz_ta.Fill(fz_dt);

                wjwz_dt = scienceResearchDataSetNew.文件位置;
                wjwz_ta = new ScienceResearchDataSetNewTableAdapters.文件位置TableAdapter();
                wjwz_ta.Fill(wjwz_dt);
            }
            catch
            {
                MessageBox.Show("数据库未能正确加载！");
                return;
            }
            #endregion

            #region 验证
            paperIdList_left = new List<int>();
            paperUserControlList_left = new List<PaperUserControl>();
            paperUserControlTabItemList_left = new List<TabItem>();
            paperIdList_right = new List<int>();
            hongguanList = new List<int>();
            open();

            //读取Mac地址和注册表进行解锁
            macs = new List<string>();
            GetMacByIPConfig();
            mac_string = macs[0];
            setupUserControl.macTextBox.Text = mac_string;

            //macs = GetMacByWMI();
            //mac_string = macs[0];

            //对Mac地址编码
            string mac_encode = encode_sr(mac_string);
            string mac_decode2 = decode_sr(mac_encode);


            //已经验证，直接进行使用
            //对注册码进行解码
            string register_code_string = scienceResearchKey.GetValue("SV001").ToString();
            //string mac_decode = EncryptUtil.Md532(mac_string).ToLower();
            string mac_decode = decode_sr(register_code_string);

            if (macs.Contains(mac_decode))
            {
                //验证成功，不加限制
                setupUserControl.registerCodeTextBlock.Text = "6 注册码（已注册）";
                setupUserControl.yanzhenTextBox.Text = register_code_string;
                setupUserControl.yanZhenButton.IsEnabled = false;

                //MainWindow.mainWindow.statusBar.Items.Clear();
                //TextBlock txtb = new TextBlock();
                //txtb.Text = "欢迎使用学创软件，软件已注册。如有问题请联系软件作者：尹帮辉，微信和手机号均为：17623579427。" ;
                //MainWindow.mainWindow.statusBar.Items.Add(txtb);
            }
            else
            {
                //验证不成功                
                string startTime = scienceResearchKey.GetValue("SV002").ToString();

                //如果是第一次使用则需要写入注册表
                if (startTime == "-1")
                {
                    DateTime now = DateTime.Now;
                    string start_time_now = now.ToFileTime().ToString();
                    //string start_time_now = DateTime.Now.ToString("G");

                    string start_time_encode = encode_sr(start_time_now);
                    string start_time_encode2 = decode_sr(start_time_encode);

                    scienceResearchKey.SetValue("SV002", start_time_encode);
                    setupUserControl.registerCodeTextBlock.Text = "6 注册码（还未进行注册，试用时间还有30天）";

                    //MainWindow.mainWindow.statusBar.Items.Clear();
                    //TextBlock txtb = new TextBlock();
                    //txtb.Text = "欢迎使用学创软件，软件还未进行注册，试用时间还有30天。如有问题请联系软件作者：尹帮辉，微信和手机号均为：17623579427。";
                    //MainWindow.mainWindow.statusBar.Items.Add(txtb);
                }
                else
                {
                    //试用时间还没结束
                    startTime = decode_sr(startTime);

                    //DateTime dt_start = Convert.ToDateTime(startTime);
                    DateTime dt_start = DateTime.FromFileTimeUtc(long.Parse(startTime));

                    TimeSpan timespan = DateTime.Now.Subtract(dt_start);

                    int shiyongDays = 30;

                    if (timespan.Days <= shiyongDays)
                    {
                        setupUserControl.registerCodeTextBlock.Text = "6 注册码（还未进行注册，试用时间还有"+ (shiyongDays - timespan.Days).ToString() + "天）";

                        //MainWindow.mainWindow.statusBar.Items.Clear();
                        //TextBlock txtb = new TextBlock();
                        //txtb.Text = "欢迎使用学创软件，软件还未进行注册，试用时间还有"+ (shiyongDays - timespan.Days).ToString() + "天。如有问题请联系软件作者：尹帮辉，微信和手机号均为：17623579427。";
                        //MainWindow.mainWindow.statusBar.Items.Add(txtb);
                    }

                    //没有验证，试用时间结束，提示进行验证，所有菜单栏都不可用
                    if (timespan.Days > shiyongDays)
                    {
                        MessageBox.Show("试用时间结束，请注册使用");
                        //限制使用
                        xianzhi_use();

                        //MainWindow.mainWindow.statusBar.Items.Clear();
                        //TextBlock txtb = new TextBlock();
                        //txtb.Text = "试用时间结束，请注册使用。购买注册码请联系软件作者：尹帮辉，微信和手机号均为：17623579427。";
                        //MainWindow.mainWindow.statusBar.Items.Add(txtb);
                    }             
                }
            }

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "欢迎使用学创！有问题请联系：17623579427（手机、微信），QQ：364399924，邮箱：yinbanghui2007@163.com";
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            #endregion
        }

        #region 编码解码

        public static string encode_sr(string sentence)
        {
            byte[] code_input_bytes = Encoding.ASCII.GetBytes(sentence);
            //byte[] code_input_bytes = Encoding.BigEndianUnicode.GetBytes(sentence);
            //byte[] code_input_bytes = Encoding.Unicode.GetBytes(sentence);
            byte[] code_output_bytes = new byte[code_input_bytes.Length];
            

            //Byte[] sendByte = Encoding.BigEndianUnicode.GetBytes(sentence);
            //string receiveMessage = Encoding.BigEndianUnicode.GetString(sendByte);

            string shuiji_code_string =    "10101001010111100101010111001010101110011010100101"
                                        +  "01100010100010111100010110001101000011110001010011"
                                        +  "10011010011100001111001001000111100010101101110110"
                                        +  "00011000011101010110010101100100001110000010001010"
                                        +  "01000010010100011110010001110001111000110100101001";

            for (int i = 0; i < code_input_bytes.Length; i++)
            {
                string byte_input_string = Convert.ToString(code_input_bytes[i], 2);
                string byte_output_string = byte_input_string;
                for (int k=0;k< 8 - byte_input_string.Length; k++)
                    byte_output_string=byte_output_string.Insert(0, "0");

                for (int j = 0; j < 8; j++)
                {
                    char encode_char = shuiji_code_string[i*8+j];
                    if (encode_char == '0')
                    {
                        if (byte_output_string[j] == '0')
                            byte_output_string=byte_output_string.Remove(j, 1).Insert(j, "1");
                        else
                            byte_output_string=byte_output_string.Remove(j, 1).Insert(j, "0");
                    }
                }
                code_output_bytes[i]=Convert.ToByte(byte_output_string, 2);                
            }

            string code_string = "";
            for (int i = 0; i < code_output_bytes.Length; i++)
            {
                if (i< code_output_bytes.Length-1)
                    code_string = code_string + code_output_bytes[i] + "-";
                if (i == code_output_bytes.Length - 1)
                    code_string = code_string + code_output_bytes[i];
            }

            //string code_string = Encoding.ASCII.GetString(code_output_bytes);
            return code_string;
        }

        public static string decode_sr(string code)
        {     
            string[] code_strings=code.Split('-');
            byte[] code_output_bytes = new byte[code_strings.Length];

            string shuiji_code_string = "10101001010111100101010111001010101110011010100101"
                                        + "01100010100010111100010110001101000011110001010011"
                                        + "10011010011100001111001001000111100010101101110110"
                                        + "00011000011101010110010101100100001110000010001010"
                                        + "01000010010100011110010001110001111000110100101001";

            for (int i = 0; i < code_strings.Length; i++)
            {
                byte char_byte;
                try
                {
                    char_byte = Convert.ToByte(code_strings[i]);
                }
                catch
                {
                    return "-1";
                }
                string char_input_bits = Convert.ToString(char_byte, 2);
                string byte_output_bits = char_input_bits;

                for (int k = 0; k < 8 - char_input_bits.Length; k++)
                    byte_output_bits = byte_output_bits.Insert(0, "0");

                for (int j = 0; j < 8; j++)
                {
                    char encode_char = shuiji_code_string[i * 8 + j];
                    if (encode_char == '0')
                    {
                        if (byte_output_bits[j] == '0')
                            byte_output_bits = byte_output_bits.Remove(j, 1).Insert(j, "1");
                        else
                            byte_output_bits = byte_output_bits.Remove(j, 1).Insert(j, "0");
                    }
                }
                code_output_bytes[i] = Convert.ToByte(byte_output_bits, 2);
            }

            string sentence = Encoding.ASCII.GetString(code_output_bytes);
            //string sentence = Encoding.BigEndianUnicode.GetString(code_output_bytes);
            //string sentence = Encoding.Unicode.GetString(code_output_bytes);
            return sentence;
        }

        #endregion

        #region Mac地址获取
        private void GetMacByIPConfig()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            //截取输出流
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("物理地址")||line.StartsWith("Physical Address"))
                    {
                        int start_location = line.IndexOf(":")+2;
                        line = line.Substring(start_location);

                        line = line.Replace("-", "");
                        if (line!="00000000000000E0")
                            macs.Add(line);
                    }
                }

                line = reader.ReadLine();
            }

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();
        }

        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }

        #endregion

        /// <summary>
        /// 当超过试用日期时，限制一些控件的使用
        /// </summary>
        public void xianzhi_use()
        {
            paperSelectUserControl.IsEnabled = false;
            keywordTreeUserControl.IsEnabled = false;
            //paperUserControl.IsEnabled = false;
        }

        /// <summary>
        /// 解除限制
        /// </summary>
        public void jiechu_xianzhi_use()
        {
            paperSelectUserControl.IsEnabled = true;
            keywordTreeUserControl.IsEnabled = true;
            //paperUserControl.IsEnabled = true;
        }

        private void open()
        {
            #region 初始化
            mainWindow = this;
            paperId = int.Parse(scienceResearchKey.GetValue("PaperId").ToString());
            projectId = int.Parse(scienceResearchKey.GetValue("ProjectId").ToString());
            InitializeComponent();
            inkProperties.NameInk = "Ink0";
            #endregion

            #region 用户控件
            Label headerLabel = AddLeftTabItems();               
            headerLabel = AddRightTabItems();
            #endregion

            #region 变量         
            SourceInitialized += new EventHandler(win_SourceInitialized);
            intPtrs = new List<IntPtr>();

            //includeRadioButton.IsChecked = true;
            excludeButton.IsChecked = true;
            app_included = false;

            string left = scienceResearchKey.GetValue("ScreenShot_Left").ToString();
            string[] arrTemp = left.Split(',');
            left_l = int.Parse(arrTemp[0]);
            right_l = int.Parse(arrTemp[1]);
            top_l = int.Parse(arrTemp[2]);
            bottem_l = int.Parse(arrTemp[3]);

            string right = scienceResearchKey.GetValue("ScreenShot_Right").ToString();
            string[] arrTemp2 = right.Split(',');
            left_r = int.Parse(arrTemp2[0]);
            right_r = int.Parse(arrTemp2[1]);
            top_r = int.Parse(arrTemp2[2]);
            bottem_r = int.Parse(arrTemp2[3]);
            #endregion
        }

        private Label AddRightTabItems()
        {
            Label headerLabel;
            paragraphFigureUserControl_r.left_right = "right";

            referUserControl_duanwen_right = new ReferUserControl("yd");
            referTabItem_duanwen_right = new TabItem();
            referTabItem_duanwen_right.Header = "段文";
            referTabItem_duanwen_right.Content = referUserControl_duanwen_right;
            rightTabControl.Items.Add(referTabItem_duanwen_right);

            referUserControl = new ReferUserControl("refer");
            referTabItem = new TabItem();
            referTabItem.Header = "参考";
            referTabItem.Content = referUserControl;
            rightTabControl.Items.Add(referTabItem);

            projectLiteratureUserControl = new ProjectLiteratureUserControl();
            TabItem projectLiteratureTabItem = new TabItem();
            headerLabel = new Label();
            headerLabel.Content = "文献";
            projectLiteratureTabItem.Header = headerLabel;
            projectLiteratureTabItem.Content = projectLiteratureUserControl;
            rightTabControl.Items.Add(projectLiteratureTabItem);

            PaperResearchUserControl paperResearchUserControl = new PaperResearchUserControl();
            TabItem paperResearchTabItem = new TabItem();
            paperResearchTabItem.Header = "查询";
            paperResearchTabItem.Content = paperResearchUserControl;
            rightTabControl.Items.Add(paperResearchTabItem);

            //webUserControl = new WebUserControl();
            //webTabItem = new TabItem();
            //webTabItem.Header = "WEB";
            //webTabItem.Content = webUserControl;
            //rightTabControl.Items.Add(webTabItem);

            applicationUserControl_r = new ApplicationUserControl();
            applicationTabItem_r = new TabItem();
            headerLabel = new Label();
            headerLabel.Content = "外程";
            applicationTabItem_r.Header = headerLabel;
            applicationTabItem_r.Content = applicationUserControl_r;
            rightTabControl.Items.Add(applicationTabItem_r);


            projectMappingUserControl = new ProjectMappingUserControl();
            TabItem projectMappingTabItem = new TabItem();
            projectMappingTabItem.Header = "项映";
            projectMappingTabItem.Foreground = new SolidColorBrush(Colors.Blue);
            //projectMappingTabItem.FontWeight = FontWeights.Bold;
            projectMappingTabItem.Content = projectMappingUserControl;
            rightTabControl.Items.Add(projectMappingTabItem);


            string[] columnNames_fz = { "ID", "仿真文件", "结果文件夹", "结果文件", "后处理文件", "后处理结果1", "后处理结果2", "说明", "项目ID" };
            string[] columnWidths_fz = { "40", "1*", "1*", "1*", "1*", "1*", "1*", "5*", "40" };
            dataBaseUserControl_fz = new DataBaseUserControl("仿真", 20, new List<string>(columnNames_fz), new List<string>(columnWidths_fz));
            dataBaseTabItem_fz = new TabItem();
            dataBaseTabItem_fz.Header = "仿真";
            dataBaseTabItem_fz.Content = dataBaseUserControl_fz;
            rightTabControl.Items.Add(dataBaseTabItem_fz);

            string[] columnNames_wjwz = { "ID", "名称", "位置", "项目ID" };
            string[] columnWidths_wjwz = { "40", "1*", "1*", "60" };
            dataBaseUserControl_wjwz = new DataBaseUserControl("位置", 20, new List<string>(columnNames_wjwz), new List<string>(columnWidths_wjwz));
            dataBaseTabItem_wjwz = new TabItem();
            dataBaseTabItem_wjwz.Header = "位置";
            dataBaseTabItem_wjwz.Content = dataBaseUserControl_wjwz;
            rightTabControl.Items.Add(dataBaseTabItem_wjwz);

            string[] columnNames_gjc = { "ID", "关键词", "父节点" };
            string[] columnWidths_gjc = { "40", "1*", "1*" };
            dataBaseUserControl_gjc = new DataBaseUserControl("关键词", 20, new List<string>(columnNames_gjc), new List<string>(columnWidths_gjc));
            dataBaseTabItem_gjc = new TabItem();
            dataBaseTabItem_gjc.Header = "关词";
            dataBaseTabItem_gjc.Content = dataBaseUserControl_gjc;
            rightTabControl.Items.Add(dataBaseTabItem_gjc);

            keywordMappingUserControl = new KeywordMappingUserControl();
            keywordMappingTabItem = new TabItem();
            keywordMappingTabItem.Header = "关映";
            keywordMappingTabItem.Foreground = new SolidColorBrush(Colors.Blue);
            //keywordMappingTabItem.FontWeight = FontWeights.Bold;
            keywordMappingTabItem.Content = keywordMappingUserControl;
            rightTabControl.Items.Add(keywordMappingTabItem);

            string[] columnNames_dc = { "ID", "单词", "意义", "分类" };
            string[] columnWidths_dc = { "40", "1*", "1*", "1*" };
            dataBaseUserControl_dc = new DataBaseUserControl("单词", 20, new List<string>(columnNames_dc), new List<string>(columnWidths_dc));
            dataBaseTabItem_dc = new TabItem();
            dataBaseTabItem_dc.Header = "单词";
            dataBaseTabItem_dc.Content = dataBaseUserControl_dc;
            rightTabControl.Items.Add(dataBaseTabItem_dc);

            string[] columnNames_dy = { "ID", "短语", "意义", "分类" };
            string[] columnWidths_dy = { "40", "1*", "1*", "1*" };
            dataBaseUserControl_dy = new DataBaseUserControl("短语", 20, new List<string>(columnNames_dy), new List<string>(columnWidths_dy));
            dataBaseTabItem_dy = new TabItem();
            dataBaseTabItem_dy.Header = "短语";
            dataBaseTabItem_dy.Content = dataBaseUserControl_dy;
            rightTabControl.Items.Add(dataBaseTabItem_dy);

            string[] columnNames_jx = { "ID", "句型", "分类" };
            string[] columnWidths_jx = { "40", "1*", "1*" };
            dataBaseUserControl_jx = new DataBaseUserControl("句型", 20, new List<string>(columnNames_jx), new List<string>(columnWidths_jx));
            dataBaseTabItem_jx = new TabItem();
            dataBaseTabItem_jx.Header = "句型";
            dataBaseTabItem_jx.Content = dataBaseUserControl_jx;
            rightTabControl.Items.Add(dataBaseTabItem_jx);

            string[] columnNames_yd = { "ID", "语段", "语段isf", "排序", "分类", "图片", "图片isf", "文章ID" };
            string[] columnWidths_yd = { "40", "2*", "1*", "40", "60", "1*", "1*", "50" };
            dataBaseUserControl_yd = new DataBaseUserControl("语段", 20, new List<string>(columnNames_yd), new List<string>(columnWidths_yd));
            dataBaseTabItem_yd = new TabItem();
            dataBaseTabItem_yd.Header = "语段";
            dataBaseTabItem_yd.Content = dataBaseUserControl_yd;
            rightTabControl.Items.Add(dataBaseTabItem_yd);

            //string[] columnNames_tp = { "ID", "图片", "文章ID", "图片isf" };
            //string[] columnWidths_tp = { "40", "1*", "50", "1*" };
            //dataBaseUserControl_tp = new DataBaseUserControl("图片", 20, new List<string>(columnNames_tp), new List<string>(columnWidths_tp));
            //dataBaseTabItem_tp = new TabItem();
            //dataBaseTabItem_tp.Header = "图片";
            //dataBaseTabItem_tp.Content = dataBaseUserControl_tp;
            //rightTabControl.Items.Add(dataBaseTabItem_tp);

            string[] columnNames_wz = { "ID", "文章名", "文件", "text文件", "xps文件", "分类", "项目ID" };
            string[] columnWidths_wz = { "40", "2*", "1*", "1*", "1*", "60", "50" };
            dataBaseUserControl_wz = new DataBaseUserControl("文章", 20, new List<string>(columnNames_wz), new List<string>(columnWidths_wz));
            dataBaseTabItem_wz = new TabItem();
            dataBaseTabItem_wz.Header = "文章";
            dataBaseTabItem_wz.Content = dataBaseUserControl_wz;
            rightTabControl.Items.Add(dataBaseTabItem_wz);

            string[] columnNames_tpcz = { "ID", "图片文件", "解释", "项目ID" };
            string[] columnWidths_tpcz = { "40", "1*", "1*", "1*" };
            dataBaseUserControl_tpcz = new DataBaseUserControl("图片创作", 20, new List<string>(columnNames_tpcz), new List<string>(columnWidths_tpcz));
            dataBaseTabItem_tpcz = new TabItem();
            dataBaseTabItem_tpcz.Header = "图创";
            dataBaseTabItem_tpcz.Content = dataBaseUserControl_tpcz;
            rightTabControl.Items.Add(dataBaseTabItem_tpcz);

            string[] columnNames_xm = { "ID", "项目名称" };
            string[] columnWidths_xm = { "40", "3*" };
            dataBaseUserControl_xm = new DataBaseUserControl("项目", 20, new List<string>(columnNames_xm), new List<string>(columnWidths_xm));
            dataBaseTabItem_xm = new TabItem();
            dataBaseTabItem_xm.Header = "项目";
            dataBaseTabItem_xm.Content = dataBaseUserControl_xm;
            rightTabControl.Items.Add(dataBaseTabItem_xm);

            //WriteReferUserControl writeReferUserControl = new WriteReferUserControl(paperId);
            //TabItem writeReferTabItem = new TabItem();
            //headerLabel = new Label();
            //headerLabel.Content = "宏:" + paperId;
            //headerLabel.MouseDoubleClick += headerLabel_hongguan_MouseDoubleClick;
            //writeReferTabItem.Header = headerLabel;
            //writeReferTabItem.Content = writeReferUserControl;
            //referTabControl.Items.Add(writeReferTabItem);
            //hongguanList.Add(paperId);
            return headerLabel;
        }

        private Label AddLeftTabItems()
        {
            //左边窗体  
            paperSelectUserControl = new PaperSelectUserControl();
            TabItem paperSelectTabItem = new TabItem();
            Label headerLabel = new Label();
            headerLabel.Content = "打开";
            paperSelectTabItem.Header = headerLabel;
            paperSelectTabItem.Content = paperSelectUserControl;
            leftTabControl.Items.Add(paperSelectTabItem);

            //appUserControl = new AppUserControl();
            //TabItem appTabItem = new TabItem();
            //appTabItem.Header = "应用程序";
            //appTabItem.Content = appUserControl;
            //paperTabControl.Items.Add(appTabItem);

            applicationUserControl = new ApplicationUserControl();
            applicationTabItem = new TabItem();
            headerLabel = new Label();
            headerLabel.Content = "外程";
            applicationTabItem.Header = headerLabel;
            applicationTabItem.Content = applicationUserControl;
            leftTabControl.Items.Add(applicationTabItem);

            figureCreateUserControl = new FigureCreateUserControl();
            TabItem figureCreateTabItem = new TabItem();
            figureCreateTabItem.Header = "作图";
            figureCreateTabItem.Foreground = new SolidColorBrush(Colors.Blue);
            //figureCreateTabItem.FontWeight = FontWeights.Bold;
            figureCreateTabItem.Content = figureCreateUserControl;
            leftTabControl.Items.Add(figureCreateTabItem);

            screenShotUserControl = new ScreenShotUserControl();
            screenShotTabItem = new TabItem();
            screenShotTabItem.Header = "截图";
            screenShotTabItem.Content = screenShotUserControl;
            leftTabControl.Items.Add(screenShotTabItem);

            setupUserControl = new SetupUserControl();
            TabItem setupTabItem = new TabItem();
            setupTabItem.Header = "设置";
            setupTabItem.Content = setupUserControl;
            leftTabControl.Items.Add(setupTabItem);

            paragraphFigureUserControl_l = new ParagraphFigureUserControl();
            paragraphFigureTabItem_l = new TabItem();
            paragraphFigureTabItem_l.Header = "段图";
            paragraphFigureTabItem_l.Content = paragraphFigureUserControl_l;
            leftTabControl.Items.Add(paragraphFigureTabItem_l);
            paragraphFigureUserControl_l.left_right = "left";

            referUserControl_duanwen_left = new ReferUserControl("yd");
            referTabItem_duanwen_left = new TabItem();
            referTabItem_duanwen_left.Header = "段文";
            referTabItem_duanwen_left.Content = referUserControl_duanwen_left;
            leftTabControl.Items.Add(referTabItem_duanwen_left);

            referUserControl_wenzhang_left = new ReferUserControl("wz");
            referTabItem_wenzhang_left = new TabItem();
            referTabItem_wenzhang_left.Header = "章文";
            referTabItem_wenzhang_left.Content = referUserControl_wenzhang_left;
            leftTabControl.Items.Add(referTabItem_wenzhang_left);

            keywordTreeUserControl = new KeywordTreeUserControl();
            keywordTreeTabItem = new TabItem();
            keywordTreeTabItem.Header = "关树";
            keywordTreeTabItem.Foreground = new SolidColorBrush(Colors.Blue);
            //keywordTreeTabItem.FontWeight = FontWeights.Bold;
            keywordTreeTabItem.Content = keywordTreeUserControl;
            leftTabControl.Items.Add(keywordTreeTabItem);

            //shareUserControl = new ShareUserControl();
            //shareTabItem = new TabItem();
            //shareTabItem.Header = "共享";
            //shareTabItem.Content = shareUserControl;
            //leftTabControl.Items.Add(shareTabItem);

            //connectUserControl = new ConnectUserControl();
            //connectTabItem = new TabItem();
            //connectTabItem.Header = "沟通";
            //connectTabItem.Content = connectUserControl;
            //leftTabControl.Items.Add(connectTabItem);

            paragraphImage_l = paragraphFigureUserControl_l.img;
            paragraphImage_r = paragraphFigureUserControl_r.img;
            scienceResearchInkCanvas_l = paragraphFigureUserControl_l.canvas;
            scienceResearchInkCanvas_r = paragraphFigureUserControl_r.canvas;

            if (paperId != -1)
            {
                ScienceResearchDataSetNew.文章Row wz_using = (ScienceResearchDataSetNew.文章Row)wz_dt.Rows.Find(paperId);
                if (wz_using != null && wz_using.文件 == "")
                {
                    string paperNameString = "";
                    var paperNameList = (from paper in wz_dt
                                         where paper.ID == paperId
                                         select paper.文章名).ToList();

                    if (paperNameList.Count > 0)
                    {
                        if (paperNameList[0].Length > 3)
                            paperNameString = "[" + paperId + "]" + paperNameList[0].Substring(0, 3);
                        else
                            paperNameString = "[" + paperId + "]" + paperNameList[0];

                        paperUserControl = new PaperUserControl(paperId);
                        TabItem paperTabItem = new TabItem();
                        headerLabel = new Label();
                        headerLabel.Content = paperNameString;
                        headerLabel.MouseDoubleClick += headerLabel_MouseDoubleClick;
                        headerLabel.MouseLeftButtonUp += HeaderLabel_MouseLeftButtonUp;
                        paperTabItem.Header = headerLabel;
                        paperTabItem.Content = paperUserControl;
                        leftTabControl.Items.Add(paperTabItem);
                        leftTabControl.SelectedItem = paperTabItem;

                        paperIdList_left.Add(paperId);
                        paperUserControlList_left.Add(paperUserControl);
                        paperUserControlTabItemList_left.Add(paperTabItem);
                    }
                }
            }

            return headerLabel;
        }

        private void HeaderLabel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string headerStr = (string)((Label)sender).Content;
            string paperIdStr = headerStr.Substring(headerStr.IndexOf("[") + 1, headerStr.IndexOf("]") - headerStr.IndexOf("[") - 1);
            int paperId = int.Parse(paperIdStr);
            PaperSelectUserControl.PaperHeaderClick(paperId);
        }
        #endregion

        #region 标签页双击
        private void headerLabel_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            paperIdList_left.Remove(((PaperUserControl)(((TabItem)leftTabControl.SelectedItem).Content)).paperId);
            paperUserControlList_left.Remove((PaperUserControl)(((TabItem)leftTabControl.SelectedItem).Content));
            paperUserControlTabItemList_left.Remove((TabItem)leftTabControl.SelectedItem);
            leftTabControl.Items.Remove(leftTabControl.SelectedItem);
            
        }

        private void headerLabel_hongguan_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            hongguanList.Remove(((WriteReferUserControl)(((TabItem)rightTabControl.SelectedItem).Content)).paperId);
            rightTabControl.Items.Remove(rightTabControl.SelectedItem);
        }
        #endregion

        #region 关闭窗体
        public static string path_translate(string path)
        {
            if (path != "")
            {
                if (path[0] == '#')
                {
                    path = path.Substring(1, path.Length - 2);
                }

                if (path[0] == '.' && path[path.Length - 1] == '#')
                {
                    path = path.Substring(0, path.IndexOf('#'));
                }

                if (path[0] == '.')
                {
                    path = path.Substring(1, path.Length - 1);
                    path = path_database + path;
                }
            }
            return path;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //将画笔信息写入注册表
            inkProperties.UserControl_Closing();
            setupUserControl.inkProperties1.UserControl_Closing();
            setupUserControl.inkProperties2.UserControl_Closing();
            setupUserControl.inkProperties3.UserControl_Closing();

            //关闭所有打开的应用程序
            for (int i = 0; i < intPtrs.Count; i++)
            {
                SendMessage(intPtrs[i], WM_CLOSE, 0, 0);
            }

            //将当前文章编号和项目编号写入注册表
            scienceResearchKey.SetValue("PaperId", paperId.ToString());
            scienceResearchKey.SetValue("ProjectId", projectId.ToString());
        }

        #endregion

        #region 主工具条
        private void zhengRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            isZheng = true;
        }

        private void fanRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            isZheng = false;
        }

        #region 截图
        private void jietuButton_Click(object sender, RoutedEventArgs e)
        {
            //使用外部应用程序进行截图，并在二者之间建立通信，合作完成截图操作
            //Process process = Process.Start(path_database+@"\科学研究\重庆大学\ScienceResearch\ScienceResearch_New\ScreenShotWindowsFormsApplication\bin\Debug\ScreenShotWindowsFormsApplication.exe");
            //process.EnableRaisingEvents = true;
            //process.Exited += App_Exited;           

            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            bitBmp = PrtWindow(handle);
            screenShotUserControl.SetImageSource();
            leftTabControl.SelectedItem = screenShotTabItem;
        }

        private void jieZuoButton_Click(object sender, RoutedEventArgs e)
        {

            //IntPtr handle = (new WindowInteropHelper(this)).Handle;
            //IntPtr handle = IntPtr.Zero;
            //Bitmap bitBmp2 = PrtWindow(handle);
            float ScaleX = PrimaryScreen.ScaleX;
            float ScaleY = PrimaryScreen.ScaleY;
            int width = Convert.ToInt32(System.Windows.Forms.Screen.AllScreens[0].Bounds.Width);
            int height = Convert.ToInt32(System.Windows.Forms.Screen.AllScreens[0].Bounds.Height);

            Bitmap bitBmp2 = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics1 = Graphics.FromImage(bitBmp2);
            graphics1.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);

            bitBmp = new Bitmap(bitBmp2.Width / 2 - left_l - right_l, bitBmp2.Height - top_l - bottem_l);
            Graphics graphic = Graphics.FromImage(bitBmp);
            graphic.DrawImage(bitBmp2, 0, 0, new Rectangle(left_l, top_l, bitBmp2.Width / 2 - left_l - right_l, bitBmp2.Height - top_l - bottem_l), GraphicsUnit.Pixel);

            rightTabControl.SelectedIndex = 0;
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bitBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource img = bs;
            paragraphImage_r.Source = img;

            paragraphFigureUserControl_r.ClearStrokes();

        }
        private void jieZuoButton_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            float ScaleX = PrimaryScreen.ScaleX;
            float ScaleY = PrimaryScreen.ScaleY;
            int width = Convert.ToInt32(System.Windows.Forms.Screen.AllScreens[0].Bounds.Width);
            int height = Convert.ToInt32(System.Windows.Forms.Screen.AllScreens[0].Bounds.Height);

            Bitmap bitBmp2 = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics1 = Graphics.FromImage(bitBmp2);
            graphics1.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);

            bitBmp = new Bitmap(bitBmp2.Width / 2 - left_l - right_l, bitBmp2.Height - top_l - bottem_l);
            Graphics graphic = Graphics.FromImage(bitBmp);
            graphic.DrawImage(bitBmp2, 0, 0, new Rectangle(left_l, top_l, bitBmp2.Width / 2 - left_l - right_l, bitBmp2.Height - top_l - bottem_l), GraphicsUnit.Pixel);

            screenShotUserControl.SetImageSource();
            leftTabControl.SelectedItem = screenShotTabItem;
        }

        private void jieYouButton_Click(object sender, RoutedEventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            Bitmap bitBmp2 = PrtWindow(handle);
            bitBmp = new Bitmap(bitBmp2.Width / 2 - left_r - right_r, bitBmp2.Height - top_r - bottem_r);
            Graphics graphic = Graphics.FromImage(bitBmp);
            graphic.DrawImage(bitBmp2, 0, 0, new Rectangle(bitBmp2.Width / 2 + left_r, top_r, bitBmp2.Width / 2 - left_r - right_r, bitBmp2.Height - top_r - bottem_r), GraphicsUnit.Pixel);

            leftTabControl.SelectedItem = paragraphFigureTabItem_l;
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bitBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource img = bs;
            paragraphImage_l.Source = img;

            paragraphFigureUserControl_l.ClearStrokes();
        }
        private void jieYouButton_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            Bitmap bitBmp2 = PrtWindow(handle);
            bitBmp = new Bitmap(bitBmp2.Width / 2 - left_r - right_r, bitBmp2.Height - top_r - bottem_r);
            Graphics graphic = Graphics.FromImage(bitBmp);
            graphic.DrawImage(bitBmp2, 0, 0, new Rectangle(bitBmp2.Width / 2 + left_r, top_r, bitBmp2.Width / 2 - left_r - right_r, bitBmp2.Height - top_r - bottem_r), GraphicsUnit.Pixel);

            screenShotUserControl.SetImageSource();
            leftTabControl.SelectedItem = screenShotTabItem;
        }

        private void jieQuanButton_Click(object sender, RoutedEventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            Bitmap bitBmp2 = PrtWindow(handle);

            rightTabControl.SelectedIndex = 0;
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bitBmp2.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource img = bs;
            paragraphImage_r.Source = img;

            paragraphFigureUserControl_r.ClearStrokes();
        }

        private void jieQuanButton_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            bitBmp = PrtWindow(handle);

            screenShotUserControl.SetImageSource();
            leftTabControl.SelectedItem = screenShotTabItem;

        }

        /// <summary>
        /// 打印窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Bitmap PrtWindow(IntPtr hWnd)
        {
            //============获取缩放比例==================================

            float ScaleX = PrimaryScreen.ScaleX;
            float ScaleY = PrimaryScreen.ScaleY;

            //============获取截图==================================
            IntPtr hscrdc = GetWindowDC(hWnd);
            RECT rect;
            GetWindowRect(hWnd, out rect);
            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, Convert.ToInt32((rect.right - rect.left)), Convert.ToInt32((rect.bottom - rect.top)));
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            DeleteDC(hscrdc);
            DeleteDC(hmemdc);
            return bmp;
        }
        #endregion

        #region 退出处理
        /// <summary>
        /// 应用程序退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void App_Exited(object sender, EventArgs e)
        {
            //加载图片
            string figure_path = @".\科学研究\图片\jietu.tif";
            string figure_path_isf = @".\科学研究\图片ISF\jietu.isf";
            figure_path = path_translate(figure_path);
            figure_path_isf = path_translate(figure_path_isf);

            //墨笔文件
            if (File.Exists(figure_path))
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(ScreenShotUserControl.bitMap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    ImageSource img = bs;
                    //ImageSource img = new BitmapImage(new Uri(figure_path, UriKind.RelativeOrAbsolute));
                    paragraphImage_r.Source = img;

                    //if (File.Exists(figure_path_isf))
                    //{
                    //    FileStream file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                    //    if (file_ink.Length != 0)
                    //    {
                    //        scienceResearchInkCanvas.Strokes = new StrokeCollection(file_ink);
                    //    }
                    //    file_ink.Close();
                    //    paragraphFigure.figure_path_isf = figure_path_isf;
                    //}
                    //else
                    //{
                    //    scienceResearchInkCanvas.Strokes.Clear();
                    //    paragraphFigure.figure_path_isf = figure_path_isf;
                    //}
                }));
            }
        }

        private void paperTabControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //paperTabControl.Items.Remove(paperTabControl.SelectedItem);
        }

        private void figureTabControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //figureTabControl.Items.Remove(figureTabControl.SelectedItem);
        }
        #endregion

        private void inkRadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            inkProperties.lstbox.SelectedColor = setupUserControl.inkProperties1.ColorInk;
            inkProperties.ColorInk = inkProperties.lstbox.SelectedColor;

            inkProperties.slider.Value = setupUserControl.inkProperties1.slider.Value;

            if (setupUserControl.inkProperties1.gangbiRadioButton.IsChecked == true)
                inkProperties.gangbiRadioButton.IsChecked = true;
            else
                inkProperties.yingguangbiRadioButton.IsChecked = true;

            if (setupUserControl.inkProperties1.clearRadioButton.IsChecked == true)
                inkProperties.clearRadioButton.IsChecked = true;
            else
                inkProperties.writeRadioButton.IsChecked = true;
        }

        private void inkRadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            inkProperties.lstbox.SelectedColor = setupUserControl.inkProperties2.ColorInk;
            inkProperties.ColorInk = inkProperties.lstbox.SelectedColor;

            inkProperties.slider.Value = setupUserControl.inkProperties2.slider.Value;

            if (setupUserControl.inkProperties2.gangbiRadioButton.IsChecked == true)
                inkProperties.gangbiRadioButton.IsChecked = true;
            else
                inkProperties.yingguangbiRadioButton.IsChecked = true;

            if (setupUserControl.inkProperties2.clearRadioButton.IsChecked == true)
                inkProperties.clearRadioButton.IsChecked = true;
            else
                inkProperties.writeRadioButton.IsChecked = true;
        }

        private void inkRadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            inkProperties.lstbox.SelectedColor = setupUserControl.inkProperties3.ColorInk;
            inkProperties.ColorInk = inkProperties.lstbox.SelectedColor;

            inkProperties.slider.Value = setupUserControl.inkProperties3.slider.Value;

            if (setupUserControl.inkProperties3.gangbiRadioButton.IsChecked == true)
                inkProperties.gangbiRadioButton.IsChecked = true;
            else
                inkProperties.yingguangbiRadioButton.IsChecked = true;

            if (setupUserControl.inkProperties3.clearRadioButton.IsChecked == true)
                inkProperties.clearRadioButton.IsChecked = true;
            else
                inkProperties.writeRadioButton.IsChecked = true;
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            if (app_included)
                mainWindow.leftTabControl.SelectedItem = applicationTabItem;
            string processStr = @".\学创软件帮助2.0-2.pdf";
            processStr = path_translate(processStr);
            applicationUserControl.headerStr = "帮助";
            applicationUserControl.loadProcess(processStr);

            //paperId = 269;
            //string paperNameString = "";
            //var paperNameList = (from paper in wz_dt
            //                     where paper.ID == paperId
            //                     select paper.文章名).ToList();

            //if (paperNameList[0].Length > 3)
            //    paperNameString = "[" + paperId + "]" + paperNameList[0].Substring(0, 3);
            //else
            //    paperNameString = "[" + paperId + "]" + paperNameList[0];
            //PaperUserControl paperUserControl = new PaperUserControl(paperId);
            //TabItem paperTabItem = new TabItem();
            //paperTabItem.Header = paperNameString;
            //paperTabItem.Content = paperUserControl;
            //leftTabControl.Items.Add(paperTabItem);

        }

        public void half()
        {
            grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
        }

        public void left_zero()
        {
            if (leftTabControl.SelectedIndex >= 7)
                leftTabControl.SelectedItem = paragraphFigureTabItem_l;
            grid.ColumnDefinitions[0].Width = new GridLength(0);
            grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
        }

        public void right_zero()
        {
            if (rightTabControl.SelectedIndex >= 15)
                rightTabControl.SelectedItem = paragraphFigureTabItem_r;
            grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions[1].Width = new GridLength(0);
        }

        private void includeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            app_included = true;
        }

        private void excludeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            app_included = false;
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            rightButton_Click2();
        }

        public void rightButton_Click2()
        {
            WindowState = WindowState.Normal;
            Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point(0, 0));
            double scaleX = PrimaryScreen.ScaleX;
            double scaleY = PrimaryScreen.ScaleY;
            Top = 0;
            Left = rect.Width / 2 / scaleX;
            Width = rect.Width / 2 / scaleX;
            Height = rect.Height / scaleY;
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point(0, 0));
            double scaleX = PrimaryScreen.ScaleX;
            double scaleY = PrimaryScreen.ScaleY;
            Top = 0;
            Left = 0;
            Width = rect.Width / 2 / scaleX;
            Height = rect.Height / scaleY;
        }

        private void pingbanButton_Click(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            Hide();
            Show();
        }

        private void pcButton_Click(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Maximized;
        }

        private void refreshKeywordButton_Click(object sender, RoutedEventArgs e)
        {
            leftTabControl.Items.Remove(keywordTreeTabItem);

            keywordTreeUserControl = new KeywordTreeUserControl();
            keywordTreeTabItem = new TabItem();
            keywordTreeTabItem.Header = "关树";
            keywordTreeTabItem.Content = keywordTreeUserControl;
            leftTabControl.Items.Insert(6, keywordTreeTabItem);
            leftTabControl.SelectedItem = keywordTreeTabItem;

        }

        private void leftTabControl_GotFocus(object sender, RoutedEventArgs e)
        {
            tabControlInUse = "left";
        }

        private void rightTabControl_GotFocus(object sender, RoutedEventArgs e)
        {
            tabControlInUse = "right";
        }
        #endregion

        #region 信息处理

        void win_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WndProc));
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0003:
                    break;
                case WM_APP+1:
                    //模式识别
                    if (tabControlInUse == "left")
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                    else
                    {
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                    }
                    wordPaperUserControl.WordProcess_shibie();
                    break;
                case WM_APP + 2:
                    //跳转参考
                    if (tabControlInUse == "left")
                    {
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                    }
                    else
                    {
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                    }
                    wordPaperUserControl.WordProcess_pipei();
                    break;

                case WM_APP + 3:
                    //收集关键词
                    if (tabControlInUse == "left")
                    {
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                    }
                    else
                    {
                        try
                        {
                            wordPaperUserControl = (PaperUserControl)rightTabControl.SelectedContent;
                        }
                        catch
                        {
                            wordPaperUserControl = (PaperUserControl)leftTabControl.SelectedContent;
                        }
                    }
                    wordPaperUserControl.WordProcess_shouji();
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region 数据库升级
        private void database_update()
        {
            //单词，分类——Empty
            //单词_关键词，没有什么特别的

            //短语，分类——Empty
            //短语_关键词，没有什么特别的

            //句型，没有什么特别的
            //句型_关键词，没有什么特别的

            //语段，语段——Empty，语段isf——Empty，图片——Empty，图片isf——Empty，文章ID——0，排序——0，分类——Empty，文件——Empty，宽度——0
            //语段_关键词，没有什么特别的

            //图片，没有什么特别的
            //图片_关键词，没有什么特别的

            //文章，文件——Empty，分类——"n"，项目ID——0
            //文章_关键词，没有什么特别的

            //图片创作，没有什么特别的
            //图片创作_关键词，没有什么特别的

            //仿真，没有什么特别的
            //文件位置，项目ID——0
            //关键词，分类——Empty，父节点——0，翻译——Empty

            //项目，没有什么特别的
            //项目_关键词，没有什么特别的

            //---------------------------------------------------------------------------------
            //Data Source=.\..\..\..\..\科学研究_New.accdb


            int database_version = int.Parse(scienceResearchKey.GetValue("DataBaseVersion").ToString());
            if (database_version == 1)
            {
                if (File.Exists(dataBaseFilePath))
                {
                    scienceResearchKey.SetValue("DataBaseVersion", "2");
                }
                else
                {
                    database_one_two();
                    scienceResearchKey.SetValue("DataBaseVersion", "2");
                    MessageBox.Show("数据库转换成功");
                }
                
            }
        }

        private void database_one_two()
        {
            //从数据库版本1升级到2
            string sourceFile = path_database + "\\科学研究_New.accdb";
            string destinationFile = path_database + "\\科学研究2.accdb";
            FileInfo file = new FileInfo(sourceFile);
            if (file.Exists)
            {
                file.CopyTo(destinationFile, true);
            }

            //数据库所在文件夹
            conn_string = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path_database + "\\科学研究2.accdb";        //连接字符串
            //MessageBox.Show(conn_string);            

            conn = new OleDbConnection(conn_string);
            try
            {
                conn.Open();
            }
            catch
            {
                MessageBox.Show("数据库未能正确连接！");
            }

            //修改文章表结构
            string dbstr = "ALTER TABLE 文章 ADD COLUMN 项目ID Long";
            OleDbCommand oleDbCom = new OleDbCommand(dbstr, conn);
            oleDbCom.ExecuteNonQuery();

            //添加约束
            dbstr = "ALTER TABLE 文章 ADD CONSTRAINT fk_ProjectPapers FOREIGN KEY (项目ID) REFERENCES 项目(ID)";
            oleDbCom = new OleDbCommand(dbstr, conn);
            oleDbCom.ExecuteNonQuery();

            conn.Close();
        }
        #endregion
    }

    /// <summary>
    /// 加密解密类
    /// </summary>
    public static class EncryptUtil
    {
        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string Md532(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }

            var encoding = Encoding.UTF8;
            MD5 md5 = MD5.Create();
            return HashAlgorithmBase(md5, value, encoding);
        }

        /// <summary>
        /// 加权MD5加密
        /// </summary>
        public static string Md532(this string value, string salt)
        {
            return salt == null ? value.Md532() : (value + "『" + salt + "』").Md532();
        }

        #endregion

        #region SHA 加密

        /// <summary>
        /// SHA1 加密
        /// </summary>
        public static string Sha1(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }

            var encoding = Encoding.UTF8;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return HashAlgorithmBase(sha1, value, encoding);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        public static string Sha256(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }

            var encoding = Encoding.UTF8;
            SHA256 sha256 = new SHA256Managed();
            return HashAlgorithmBase(sha256, value, encoding);
        }

        /// <summary>
        /// SHA512 加密
        /// </summary>
        public static string Sha512(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            SHA512 sha512 = new SHA512Managed();
            return HashAlgorithmBase(sha512, value, encoding);
        }

        #endregion

        #region HMAC 加密

        /// <summary>
        /// HmacSha1 加密
        /// </summary>
        public static string HmacSha1(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA1 hmacSha1 = new HMACSHA1(keyStr);
            return HashAlgorithmBase(hmacSha1, value, encoding);
        }

        /// <summary>
        /// HmacSha256 加密
        /// </summary>
        public static string HmacSha256(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA256 hmacSha256 = new HMACSHA256(keyStr);
            return HashAlgorithmBase(hmacSha256, value, encoding);
        }

        /// <summary>
        /// HmacSha384 加密
        /// </summary>
        public static string HmacSha384(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA384 hmacSha384 = new HMACSHA384(keyStr);
            return HashAlgorithmBase(hmacSha384, value, encoding);
        }

        /// <summary>
        /// HmacSha512 加密
        /// </summary>
        public static string HmacSha512(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA512 hmacSha512 = new HMACSHA512(keyStr);
            return HashAlgorithmBase(hmacSha512, value, encoding);
        }

        /// <summary>
        /// HmacMd5 加密
        /// </summary>
        public static string HmacMd5(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACMD5 hmacMd5 = new HMACMD5(keyStr);
            return HashAlgorithmBase(hmacMd5, value, encoding);
        }

        /// <summary>
        /// HmacRipeMd160 加密
        /// </summary>
        public static string HmacRipeMd160(this string value, string keyVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACRIPEMD160 hmacRipeMd160 = new HMACRIPEMD160(keyStr);
            return HashAlgorithmBase(hmacRipeMd160, value, encoding);
        }

        #endregion

        #region AES 加密解密

        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="value">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param> 
        /// <returns></returns>  
        public static string AesStr(this string value, string keyVal, string ivVal)
        {
            if (value == null)
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }

            var encoding = Encoding.UTF8;
            byte[] btKey = keyVal.FormatByte(encoding);
            byte[] btIv = ivVal.FormatByte(encoding);
            byte[] byteArray = encoding.GetBytes(value);
            string encrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="value">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param>  
        /// <returns></returns>  
        public static string UnAesStr(this string value, string keyVal, string ivVal)
        {
            var encoding = Encoding.UTF8;
            byte[] btKey = keyVal.FormatByte(encoding);
            byte[] btIv = ivVal.FormatByte(encoding);
            byte[] byteArray = Convert.FromBase64String(value);
            string decrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = encoding.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        /// <summary>  
        /// AES Byte类型 加密  
        /// </summary>  
        /// <param name="data">待加密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param>  
        /// <returns></returns>  
        public static byte[] AesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] cryptograph;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        cStream.Write(data, 0, data.Length);
                        cStream.FlushFinalBlock();
                        cryptograph = mStream.ToArray();
                    }
                }
            }
            catch
            {
                cryptograph = null;
            }
            return cryptograph;
        }

        /// <summary>  
        /// AES Byte类型 解密  
        /// </summary>  
        /// <param name="data">待解密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param> 
        /// <returns></returns>  
        public static byte[] UnAesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] original;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream(data))
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    {
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int readBytes;
                            while ((readBytes = cStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                originalMemory.Write(buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return original;
        }

        #endregion

        #region DES 加密解密

        /// <summary>
        /// DES 加密
        /// </summary>
        public static string Des(this string value, string keyVal, string ivVal)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
                var desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch { return "转换出错！"; }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        public static string UnDes(this string value, string keyVal, string ivVal)
        {
            try
            {
                string[] sInput = value.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }
                var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
                var desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch { return "解密出错！"; }
        }

        #endregion

        #region BASE64 加密解密

        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="value">待加密字段</param>
        /// <returns></returns>
        public static string Base64(this string value)
        {
            var btArray = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="value">待解密字段</param>
        /// <returns></returns>
        public static string UnBase64(this string value)
        {
            var btArray = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(btArray);
        }

        #endregion

        #region Base64加密解密
        /// <summary>
        /// Base64加密 可逆
        /// </summary>
        /// <param name="value">待加密文本</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value)
        {
            return Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="ciphervalue">密文</param>
        /// <returns></returns>
        public static string Base64Decrypt(string ciphervalue)
        {
            return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(ciphervalue));
        }
        #endregion

        #region 内部方法

        /// <summary>
        /// 转成数组
        /// </summary>
        private static byte[] Str2Bytes(this string source)
        {
            source = source.Replace(" ", "");
            byte[] buffer = new byte[source.Length / 2];
            for (int i = 0; i < source.Length; i += 2) buffer[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            StringBuilder pwd = new StringBuilder();
            foreach (byte btStr in source) { pwd.AppendFormat(formatStr, btStr); }
            return pwd.ToString();
        }

        private static byte[] FormatByte(this string strVal, Encoding encoding)
        {
            return encoding.GetBytes(strVal.Base64().Substring(0, 16).ToUpper());
        }

        /// <summary>
        /// HashAlgorithm 加密统一方法
        /// </summary>
        private static string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
        {
            byte[] btStr = encoding.GetBytes(source);
            byte[] hashStr = hashAlgorithmObj.ComputeHash(btStr);
            return hashStr.Bytes2Str();
        }

        #endregion

    }




}
