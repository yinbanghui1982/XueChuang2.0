using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Xml;
//using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Text;
using System.IO.Packaging;
using System.Windows.Documents;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using ScienceResearchWpfApplication.TextManage;


namespace ScienceResearchWpfApplication.Xps
{
    /// <summary>
    /// XpsUserControl.xaml 的交互逻辑
    /// </summary>
    ///    

    public partial class XpsUserControl : UserControl
    {
        ScienceResearchDataSetNew.关键词Row gjc;
        string keyword;
        string title;

        int current_paper,current_page;
        List<ScienceResearchDataSetNew.文章Row> paperList;
        XpsReferTabItemUserControl xpsReferTabItemUserControl;

        public XpsUserControl(ScienceResearchDataSetNew.关键词Row _gjc, ScienceResearchDataSetNew.单词Row dc)
        {
            InitializeComponent();
            xpsReferTabItemUserControl = new XpsReferTabItemUserControl();
            xpsReferTabItemUserControl.type = "xps";
            referGrid.Children.Add(xpsReferTabItemUserControl);
            gjc = _gjc;
            
            if (gjc == null)
                title = "关键词=null" + "，单词=" + dc.单词;
            else
                title = "关键词=" + gjc.关键词 + "，单词=" + dc.单词;

            keyword = dc.单词;

            UsedPaper paperUsedClass = new UsedPaper();
            paperList = paperUsedClass.provide_paper("xps文件");
            current_paper = 0;
            current_page = 0;
            selection("next");

            //OpenFile(null);
        }

        public void Split(string originalDocument, string detinationDocument)
        {
            using (Package package = Package.Open(originalDocument, FileMode.Open, FileAccess.Read))
            {
                using (Package packageDest = Package.Open(detinationDocument))
                {
                    string inMemoryPackageName = "memorystream://miXps.xps";
                    Uri packageUri = new Uri(inMemoryPackageName);
                    PackageStore.AddPackage(packageUri, package);
                    XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Maximum, inMemoryPackageName);
                    XpsDocument xpsDocumentDest = new XpsDocument(packageDest, CompressionOption.Normal, detinationDocument);
                    var fixedDocumentSequence = xpsDocument.GetFixedDocumentSequence();
                    DocumentReference docReference = xpsDocument.GetFixedDocumentSequence().References.First();
                    FixedDocument doc = docReference.GetDocument(false);
                    var content = doc.Pages[0];
                    var fixedPage = content.GetPageRoot(false);
                    var writter = XpsDocument.CreateXpsDocumentWriter(xpsDocumentDest);
                    writter.Write(fixedPage);
                    xpsDocumentDest.Close();
                    xpsDocument.Close();
                }
            }
        }        

        int[] array = null;             // 用于存放目录文档各节点OutlineLevel值,并转化为int型
        string[] array1 = null;         // 用于存放目录文档各节点OutlineLevel值
        string[] arrayName = null;      // 用于存放目录文档各节点Description值,章节信息
        string[] pages = null;          // 用于存放目录文档各节点OutlineTarget值,页码信息    

        // 读取导航目录
        private void ReadDoc(XpsDocument xpsDoc)
        {
            IXpsFixedDocumentSequenceReader docSeq = xpsDoc.FixedDocumentSequenceReader;
            IXpsFixedDocumentReader docReader = docSeq.FixedDocuments[0];
            XpsStructure xpsStructure = docReader.DocumentStructure;
            Stream stream = xpsStructure.GetStream();
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            //获取节点列表
            XmlNodeList nodeList = doc.ChildNodes.Item(0).FirstChild.FirstChild.ChildNodes;
            if (nodeList.Count <= 0)//判断是否存在目录节点
            {
                //tvTree.Visibility = System.Windows.Visibility.Hidden;
                tvTree.Items.Add(new TreeViewItem { Header = "没有导航目录" });
                return;
            }
            tvTree.Visibility = System.Windows.Visibility.Visible;
            array = new int[nodeList.Count];
            array1 = new string[nodeList.Count];
            arrayName = new string[nodeList.Count];
            pages = new string[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                array[i] = Convert.ToInt32(nodeList[i].Attributes["OutlineLevel"].Value);
                array1[i] = nodeList[i].Attributes["OutlineLevel"].Value.ToString();
                arrayName[i] = nodeList[i].Attributes["Description"].Value.ToString();
                pages[i] = nodeList[i].Attributes["OutlineTarget"].Value.ToString();
            }
            for (int i = 0; i < array.Length - 1; i++)
            {
                //对array进行转换组装成可读的树形结构，通过ASCII值进行增加、转换
                array1[0] = "A";
                if (array[i + 1] - array[i] == 1)
                {
                    array1[i + 1] = array1[i] + 'A';
                }
                if (array[i + 1] == array[i])
                {
                    char s = Convert.ToChar(array1[i].Substring((array1[i].Length - 1), 1));
                    array1[i + 1] = array1[i].Substring(0, array1[i].Length - 1) + (char)(s + 1);
                }
                if (array[i + 1] < array[i])
                {
                    int m = array[i + 1];
                    char s = Convert.ToChar(array1[i].Substring(0, m).Substring(m - 1, 1));
                    array1[i + 1] = array1[i].Substring(0, m - 1) + (char)(s + 1);
                }
            }
            //添加一个节点作为根节点
            TreeViewItem parent = new TreeViewItem();
            TreeViewItem parent1 = null;
            parent.Header = "目录导航";
            Boolean flag = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 1)
                {
                    flag = true;
                }
                if (flag) //如果找到实际根节点，加载树
                {
                    parent1 = new TreeViewItem();
                    parent1.Header = arrayName[i];
                    parent1.Tag = array1[i];
                    parent.Items.Add(parent1);
                    parent.IsExpanded = true;
                    parent1.IsExpanded = true;
                    FillTree(parent1, array1, arrayName);
                    flag = false;
                }
            }
            tvTree.Items.Clear();
            tvTree.Items.Add(parent);
        }

        //填充树的方法
        public void FillTree(TreeViewItem parentItem, string[] str1, string[] str2)
        {
            string parentID = parentItem.Tag as string;
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i].IndexOf(parentID) == 0 && str1[i].Length == (parentID.Length + 1) && str1[i].ElementAt(0).Equals(parentID.ElementAt(0)))
                {
                    TreeViewItem childItem = new TreeViewItem();
                    childItem.Header = str2[i];
                    childItem.Tag = str1[i];
                    parentItem.Items.Add(childItem);
                    FillTree(childItem, str1, str2);
                }
            }
        }

        //打开文件-如果传入路径为空则在此打开选择文件对话框
        private void OpenFile(string strFilepath)
        {
            if (string.IsNullOrEmpty(strFilepath))
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.DefaultExt = ".doc|.txt|.xps";
                openFileDialog.Filter = "*(.xps)|*.xps|Word documents (.doc)|*.doc|Word(2007-2010)(.docx)|*.docx|*(.txt)|*.txt";
                Nullable<bool> result = openFileDialog.ShowDialog();
                strFilepath = openFileDialog.FileName;
                if (result != true)
                {
                    return;
                }
            }
            //this.Title = strFilepath.Substring(strFilepath.LastIndexOf("\\") + 1);
            if (strFilepath.Length > 0)
            {
                XpsDocument xpsDoc = null;
                //如果是xps文件直接打开，否则需转换格式
                if (!strFilepath.EndsWith(".xps"))
                {
                    string newXPSdocName = String.Concat(System.IO.Path.GetDirectoryName(strFilepath), "\\", System.IO.Path.GetFileNameWithoutExtension(strFilepath), ".xps");
                    xpsDoc = ConvertWordToXPS(strFilepath, newXPSdocName);
                }
                else
                {
                    xpsDoc = new XpsDocument(strFilepath, System.IO.FileAccess.Read);
                }
                if (xpsDoc != null)
                {
                    documentViewer.Document = xpsDoc.GetFixedDocumentSequence();
                    //读取文档目录
                    //ReadDoc(xpsDoc);
                    xpsDoc.Close();
                }
            }

            documentViewer.GoToPage(5);
        }

        // 将word文档转换为xps文档
        private XpsDocument ConvertWordToXPS(string wordDocName, string xpsDocName)
        {
            XpsDocument result = null;
            //创建一个word文档，并将要转换的文档添加到新创建的对象
            //Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            //try
            //{
            //    wordApplication.Documents.Add(wordDocName);
            //    Document doc = wordApplication.ActiveDocument;
            //    doc.ExportAsFixedFormat(xpsDocName, WdExportFormat.wdExportFormatXPS, false, WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportAllDocument, 0, 0, WdExportItem.wdExportDocumentContent, true, true, WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
            //    result = new XpsDocument(xpsDocName, System.IO.FileAccess.ReadWrite);
            //}
            //catch (Exception ex)
            //{
            //    string error = ex.Message;
            //    wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            //}
            //wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            return result;
        }


        // 导航树跳转事件
        private void tvTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int x = 0;
            TreeViewItem selectTV = this.tvTree.SelectedItem as TreeViewItem;
            if (null == selectTV)
                return;
            if (null == selectTV.Tag)
                return;
            string page = selectTV.Tag.ToString();
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Equals(page))
                {
                    x = i;
                }
            }
            string[] strPages = pages[x].Split('_');
            documentViewer.GoToPage(int.Parse(strPages[1]));
        }
        private void cbNav_Click(object sender, RoutedEventArgs e)
        {
            cdTree.Width = cbNav.IsChecked == true ? new GridLength(300) : new GridLength(0);
        }
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            documentViewer.PreviousPage();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            documentViewer.NextPage();
        }
        
        private void dvShow_PageViewsChanged(object sender, EventArgs e)
        {
            current_page = documentViewer.MasterPageNumber;
            yeshuTextbox.Text = current_page.ToString();
        }

        private void btnPrevSelection_Click(object sender, RoutedEventArgs e)
        {
            //前一项
            selection("prev");
        }

        private void btnNextSelection_Click(object sender, RoutedEventArgs e)
        {
            //后一项
            selection("next");
        }

        private void selection(string prev_next )
        {            
            //按照文章编号循环
            //FixedDocument fixedDocumentPipei = new FixedDocument();
            //var pageContent1 = new PageContent();

            int i = current_paper;
            while (i>=0 && i < paperList.Count)
            {
                string strFilepath = MainWindow.path_translate(paperList[i].xps文件);
                //string strOutput = "c://1.xps";
                //Split(strFilepath, strOutput);

                XpsDocument xpsDocument = new XpsDocument(strFilepath, System.IO.FileAccess.Read);

                //查找文件里面的内容
                StringBuilder sb = new StringBuilder();
                var reader = xpsDocument.FixedDocumentSequenceReader;
                foreach (var document in reader.FixedDocuments)
                {                    
                    int pageCount = document.FixedPages.Count;
                    int j = current_page;

                    if (current_page == 0)
                        j = 1;
                    
                    while (j <= pageCount-1 && j>=1)
                    {
                        //if (current_page != 0)
                        //{
                            if (prev_next == "next")
                                j++;
                            else
                                j--;
                        //}

                        var page = document.FixedPages[j-1];

                        //提取一页文字
                        XmlReader xrdr = page.XmlReader;
                        while (xrdr.Read())
                        {
                            switch (xrdr.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (xrdr.Name == "Glyphs")
                                        sb.Append(xrdr["UnicodeString"]);
                                    break;
                                default: break;
                            }
                        }

                        //关键词匹配
                        if (sb.ToString().IndexOf(keyword) > -1)
                        {
                            int pageNumber = page.PageNumber;
                            documentViewer.Document = xpsDocument.GetFixedDocumentSequence();
                            documentViewer.GoToPage(j);
                            xpsDocument.Close();

                            //更新文本框数据
                            string text = title + "\r\n" + sb.ToString();
                            xpsReferTabItemUserControl.SetText(text);                            
                            return;

                            //var fixedDocumentSequence = xpsDoc.GetFixedDocumentSequence();
                            //DocumentReference docReference = xpsDoc.GetFixedDocumentSequence().References[documentNumber-1];
                            //FixedDocument doc = docReference.GetDocument(false);
                            //PageContent content = doc.Pages[pageNumber];

                            //JsonSerializerSettings settings = new JsonSerializerSettings();
                            //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                            //PageContent ss = JsonConvert.DeserializeObject<PageContent>(JsonConvert.SerializeObject(content, settings));

                            //MemoryStream ms = new MemoryStream();
                            //BinaryFormatter bf = new BinaryFormatter();
                            //bf.Serialize(ms, content);
                            //ms.Position = 0;
                            //PageContent ss = bf.Deserialize(ms) as PageContent;

                            //fixedDocumentPipei.Pages.Add(ss);
                        }
                        sb.Clear();                        

                        //current_page = j;
                    }
                }
                               
                if (prev_next == "next")
                    i++;
                else
                    i--;
                current_paper = i;
            }           
        }



    }
}
