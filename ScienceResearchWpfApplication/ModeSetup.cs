using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// 模式识别和匹配的注册表管理以及相应的静态变量管理
    /// </summary>
    class ModeSetup
    {
        public static  Dictionary<string, string> shibieDictionary = new Dictionary<string, string>();
        static Dictionary<string, string> shibieInitialDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> pipeiDictionary = new Dictionary<string, string>();
        static Dictionary<string, string> pipeiInitialDictionary = new Dictionary<string, string>();

        /// <summary>
        /// 注册表设置初始值
        /// </summary>
        public static void set_initial_value()
        {
            //模式识别
            shibieInitialDictionary.Add("isGjcShibie", "否");
            shibieInitialDictionary.Add("isDcShibie","是");
            shibieInitialDictionary.Add("isDyShibie", "否");
            shibieInitialDictionary.Add("isJxShibie", "否");
            shibieInitialDictionary.Add("isYdShibie", "否");
            shibieInitialDictionary.Add("isWzShibie", "否");

            foreach (var shibieKey in shibieInitialDictionary)
            {
                //初始化注册表
                if (Array.IndexOf<string>(MainWindow.valueNames, shibieKey.Key) == -1)
                {
                    MainWindow.scienceResearchKey.SetValue(shibieKey.Key, shibieKey.Value);
                }
                //加载注册表
                shibieDictionary.Add(shibieKey.Key, MainWindow.scienceResearchKey.GetValue(shibieKey.Key).ToString());
            }

            //模式匹配
            pipeiInitialDictionary.Add("is_gjc_czwz_pipei", "否");
            pipeiInitialDictionary.Add("is_dc_yd_pipei", "是");
            pipeiInitialDictionary.Add("is_dc_ckwz_pipei", "是");
            pipeiInitialDictionary.Add("is_dc_xps_pipei", "否");
            pipeiInitialDictionary.Add("is_dy_yd_pipei", "否");
            pipeiInitialDictionary.Add("is_dy_ckwz_pipei", "否");
            pipeiInitialDictionary.Add("is_jx_ckwz_pipei", "否");
            pipeiInitialDictionary.Add("is_yd_ckwz_pipei", "否");
            pipeiInitialDictionary.Add("is_wz_ckwz_pipei", "否");

            foreach (var pipeiKey in pipeiInitialDictionary)
            {
                //初始化注册表
                if (Array.IndexOf<string>(MainWindow.valueNames, pipeiKey.Key) == -1)
                {
                    MainWindow.scienceResearchKey.SetValue(pipeiKey.Key, pipeiKey.Value);
                }
                //加载注册表
                pipeiDictionary.Add(pipeiKey.Key, MainWindow.scienceResearchKey.GetValue(pipeiKey.Key).ToString());
            }

        }


    }

    



}
