using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Text.RegularExpressions;

namespace ScienceResearchWpfApplication.TextManage
{
    class TextProcessClass
    {
        //用于文字的排版处理
        public static void ChangeColor(Color color, RichTextBox richBox, string word,string type)
        {
            //设置文字指针为Document初始位置           
            //richBox.Document.FlowDirection           
            TextPointer position = richBox.Document.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text       
                var content = position.GetPointerContext(LogicalDirection.Forward);
                if (content == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找           
                    int index = 0;
                    index = text.IndexOf(word, 0);
                    if (index != -1)
                    {
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(word.Length);

                        int englishWordsCount=Regex.Matches(word, "[a-zA-Z]").Count;
                        if (englishWordsCount > 1)
                        {
                            TextPointer start1 = position.GetPositionAtOffset(index-1);
                            TextPointer end1 = start.GetPositionAtOffset(word.Length+1);
                            TextRange range1 = new TextRange(start1, start);
                            TextRange range2 = new TextRange(end, end1);
                            if (range1.Text != " " || range2.Text != " ")
                                goto cc;
                        }                        
                        position = selecta(color, richBox, word.Length, start, end,type);
                    }
                }
                //文字指针向前偏移   
                cc: position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static TextPointer selecta(Color color, RichTextBox richTextBox1, int selectLength, TextPointer tpStart, TextPointer tpEnd,string type)
        {
            TextRange range = richTextBox1.Selection;
            range.Select(tpStart, tpEnd);
            //高亮选择         
            if (type == "前景")
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
                range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(color));
            }

            return tpEnd.GetNextContextPosition(LogicalDirection.Forward);
        }

        public string[] GetBlueKeyWords()
        {
            string[] res = { "on", "is" };
            return res;
        }



        public static void XiaHuaXian(RichTextBox richBox, string keyword)
        {
            //设置文字指针为Document初始位置           
            //richBox.Document.FlowDirection           
            TextPointer position = richBox.Document.ContentStart;
            while (position != null)
            {
                //向前搜索,需要内容为Text       
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //拿出Run的Text        
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    //可能包含多个keyword,做遍历查找           
                    int index = 0;
                    index = text.IndexOf(keyword, 0);
                    if (index != -1)
                    {
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(keyword.Length);
                        position = selecta2(richBox, keyword.Length, start, end);
                    }
                }
                //文字指针向前偏移   
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static TextPointer selecta2(RichTextBox richTextBox1, int selectLength, TextPointer tpStart, TextPointer tpEnd)
        {
            //加下划线
            TextRange range = richTextBox1.Selection;
            range.Select(tpStart, tpEnd);
            range.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);

            return tpEnd.GetNextContextPosition(LogicalDirection.Forward);
        }

        public static void SelectionChangeColor(RichTextBox richBox,Color color)
        {
            //改变选中部分的颜色
            TextRange range = richBox.Selection;
            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            range.Select(richBox.Selection.Start, richBox.Selection.Start);
        }
    }
}
