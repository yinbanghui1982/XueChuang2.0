using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace ScienceResearchWpfApplication.TextManage
{
    /// <summary>
    /// 用于读取和保存文本文档
    /// 涉及文本文档的编码、RichTextBox中的文本文档、数据库中的文本文档等方面
    /// </summary>
    class TextFile
    {
        /// <summary>
        /// 保存字符串到文本文档
        /// </summary>
        /// <param name="paper_string"></param>
        /// <param name="localFilePath"></param>
        public static void SaveStringToFile(string paper_string, string localFilePath)
        {
            StreamWriter fileWriter = new StreamWriter(localFilePath, false);
            fileWriter.Write(paper_string);
            fileWriter.Close();
        }

        /// <summary>
        /// 获取富文本框中的文本
        /// </summary>
        /// <param name="rtb"></param>
        /// <returns></returns>
        public static string GetStringOfRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string rtb_str = textRange.Text;
            return rtb_str;
        }        

        /// <summary>
        /// 替换低阶ASCII字符
        /// </summary>
        /// <param name="tmp">Xaml字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);//&#x{0:X}; 
                else info.Append(cc);
            }
            return info.ToString();
        }

        #region 编码格式
        /// <summary>
        /// 根据设置获取编码格式
        /// </summary>
        /// <returns></returns>
        private static Encoding GetEncodingBySetting()
        {
            Encoding targetEncoding = Encoding.Default;
            if (MainWindow.txt_file_encoding == "ASCII")
                targetEncoding = Encoding.ASCII;
            if (MainWindow.txt_file_encoding == "BigEndianUnicode")
                targetEncoding = Encoding.BigEndianUnicode;
            if (MainWindow.txt_file_encoding == "Unicode")
                targetEncoding = Encoding.Unicode;
            if (MainWindow.txt_file_encoding == "UTF32")
                targetEncoding = Encoding.UTF32;
            if (MainWindow.txt_file_encoding == "UTF7")
                targetEncoding = Encoding.UTF7;
            if (MainWindow.txt_file_encoding == "UTF8")
                targetEncoding = Encoding.UTF8;
            if (MainWindow.txt_file_encoding == "Default")
                targetEncoding = Encoding.Default;
            return targetEncoding;
        }

        /// <summary>
        /// 获取文件流的编码格式
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="defaultEncoding">默认格式</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe   
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                else if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode   
                {
                    targetEncoding = Encoding.Unicode;
                }
                else if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8   
                {
                    targetEncoding = Encoding.UTF8;
                }

                //恢复Seek位置         
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
        #endregion

        #region 读取文章
        /// <summary>
        /// 获取文章的字符串数组
        /// </summary>
        /// <param name="paperPath"></param>
        /// <returns></returns>
        public static string[] GetStringArrayInPaper(string paperPath)
        {
            //FileStream fs = new FileStream(paperPath, FileMode.Open);
            //Encoding targetEncoding = GetEncoding(fs, Encoding.Default);
            //fs.Close();

            Encoding targetEncoding = GetEncodingBySetting();
            string[] filelist = File.ReadAllLines(paperPath, targetEncoding);
            return filelist;
        }

        /// <summary>
        /// 获取文章的字符串
        /// 从文章表打开Text文件时，调用此函数
        /// </summary>
        /// <param name="path_wz">文章路径</param>
        /// <returns></returns>
        public static string GetFileString(string path_wz)
        {
            //FileStream fs = new FileStream(path_wz, FileMode.Open);
            //Encoding targetEncoding = GetEncoding(fs, Encoding.Default);
            //fs.Close();

            Encoding targetEncoding = GetEncodingBySetting();

            string[] filelist = File.ReadAllLines(path_wz, targetEncoding);
            string line_paper_str = "";

            for (int linenum = 0; linenum <= filelist.Length - 1; linenum++)
                line_paper_str = line_paper_str + filelist[linenum] + "\r\n";
            return line_paper_str;
        }
        #endregion

    }
}
