using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// ParagraphFigureUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ParagraphFigureUserControl : UserControl
    {
        ScienceResearchDataSetNew.语段DataTable yd_dt;
        ScienceResearchDataSetNewTableAdapters.语段TableAdapter yd_ta;

        RegistryKey scienceResearchKey;             //注册表项
        string figure_path;
        public string figure_path_isf;              //墨笔文件地址
        FileStream file_ink;
        public int paragraphId;
        public string left_right;

        public ParagraphFigureUserControl()
        {
            InitializeComponent();
            canvas.IsManipulationEnabled = true;

            RegistryKey key = Registry.CurrentUser;
            scienceResearchKey = key.OpenSubKey("software\\ScienceResearch", true);

            yd_dt = MainWindow.yd_dt;
            yd_ta = MainWindow.yd_ta;

        }

        public void ClearStrokes()
        {
            canvas.Strokes.Clear();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            //保存墨笔文件         
            if (File.Exists(figure_path_isf))
            {
                File.Delete(figure_path_isf);                
            }
            if (figure_path_isf != null)
            {
                file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
                canvas.Strokes.Save(file_ink);
                file_ink.Close();
            }
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            if (left_right=="left")
                MainWindow.mainWindow.right_zero();
            else
                MainWindow.mainWindow.left_zero();
        }

        public void HideToolBar()
        {
            grid.RowDefinitions[0].Height = new GridLength(0);
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            //保存墨笔文件
            int paragraphId = int.Parse(ydTextBox.Text);
            var yd_list = (from yd in yd_dt
                     where yd.ID == paragraphId
                     select yd).ToList();
            yd_list[0].图片 = @"./科学研究/图片/" + paragraphId + ".tiff";
            yd_list[0].图片isf= @"./科学研究/图片ISF/" + paragraphId + ".isf";
            yd_ta.Update(yd_dt);

            figure_path = MainWindow.path_translate(yd_list[0].图片);
            figure_path_isf= MainWindow.path_translate(yd_list[0].图片isf);

            BitmapSource BS = (BitmapSource)img.Source;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(BS));
            Stream stream = File.Create(figure_path);
            encoder.Save(stream);
            stream.Close();

            if (File.Exists(figure_path_isf))
                File.Delete(figure_path_isf);
            file_ink = new FileStream(figure_path_isf, FileMode.OpenOrCreate);
            canvas.Strokes.Save(file_ink);
            file_ink.Close();

            MessageBox.Show("语段保存成功");
        }
    }
}
