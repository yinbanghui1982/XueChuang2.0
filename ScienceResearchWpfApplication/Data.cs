

namespace ScienceResearchWpfApplication.DatabaseManage
{
    /// <summary>
    /// 关键词
    /// </summary>
    public class Keyword
    {
        public string 关键词 { get; set; }
    }

    /// <summary>
    /// 语段ID
    /// </summary>
    class ParagraphID
    {
        public int 语段ID { get; set; }
    }

    /// <summary>
    /// 语段_语段关键词_关键词
    /// </summary>
    class YD_YDGJC_GJC
    {
        public string 关键词 { get; set; }
        public int 语段_关键词ID { get; set; }
        public int 语段ID { get; set; }
        public int 关键词ID { get; set; }        
    }

    /// <summary>
    /// 文章
    /// </summary>
    class WZ
    {
        public int ID { get; set; }
        public string 文章名 { get; set; }
        public string 文件 { get; set; }
    }

    /// <summary>
    /// 图片创作
    /// </summary>
    class TPCZ
    {
        public int ID { get; set; }
        public string 图片文件 { get; set; }
        public string 解释 { get; set; }
    }

    /// <summary>
    /// 图片
    /// </summary>
    class TP
    {
        public int ID { get; set; }
        public string 图片 { get; set; }
        public int 文章ID { get; set; }
    }
}
