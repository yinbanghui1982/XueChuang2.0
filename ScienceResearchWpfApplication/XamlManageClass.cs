using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using System.Windows.Controls;

namespace ScienceResearchWpfApplication.TextManage
{
    public class XamlManageClass
    {

        public FlowDocument xaml_load(string yd_xaml)
        {
            StringReader sr = new StringReader(yd_xaml);
            var xmlReaderSettings = new XmlReaderSettings() { CheckCharacters = false };
            XmlReader xmlReader = XmlReader.Create(sr, xmlReaderSettings);

            FlowDocument fd=(FlowDocument)System.Windows.Markup.XamlReader.Load(xmlReader);
            sr.Close();
            return fd;
        }

        public string xaml_save(RichTextBox richTextBox)
        {
            FlowDocument fd = richTextBox.Document;
            string text2 = System.Windows.Markup.XamlWriter.Save(fd);
            return text2;
        }


        public string toxaml(RichTextBox rtb)
        {
            // Stream s = new MemoryStream();  // 其他的什么Stream类型都没问题
            //// XamlWriter.Save(
            MemoryStream s = new MemoryStream();
            TextRange documentTextRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            documentTextRange.Save(s, DataFormats.XamlPackage);
            //return Convert.ToBase64String(s.ToArray());
            return Convert.ToString(s.ToArray());
        }
    }




}
