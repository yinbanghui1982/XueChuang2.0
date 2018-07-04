using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using XYDES;
using System.Windows.Threading;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// ScreenShotUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenShotUserControl : UserControl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        public static System.Drawing.Bitmap bitMap;
        bool drawFlag = false;
        Shape insertShape;
        Point startPosition;
        double x_start, y_start;

        public ScreenShotUserControl()
        {
            InitializeComponent();  
        }

        public void SetImageSource()
        {
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(MainWindow.bitBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource img = bs;
            image.Source = img;
        }
        //-----------------------屏幕区域--------------------------------------------
        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }


        //-----------------------截图操作--------------------------------------------
        public static Shape CreateShape()
        {
            return new Rectangle() { Fill = null, Stroke = Brushes.Red, StrokeThickness = 1 };
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            insertShape = CreateShape();
            if (insertShape != null)
            {
                drawFlag = true;
                Canvas canvas = sender as Canvas;
                canvas.Children.Clear();
                startPosition = e.GetPosition(canvas);
                insertShape.Opacity = 1;
                Canvas.SetLeft(insertShape, e.GetPosition(canvas).X);
                Canvas.SetTop(insertShape, e.GetPosition(canvas).Y);
                canvas.Children.Add(insertShape);

                //Point p = e.MouseDevice.GetPosition(image);
                //x_start = p.X;
                //y_start = p.Y;

                POINT pit = new POINT();
                GetCursorPos(out pit);
                x_start = pit.X;
                y_start = pit.Y;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (drawFlag && insertShape != null)
            {
                if (e.GetPosition(canvas).X > startPosition.X)
                {
                    insertShape.Width = e.GetPosition(canvas).X - startPosition.X;

                }
                else
                {
                    insertShape.Width = startPosition.X - e.GetPosition(canvas).X;
                    Canvas.SetLeft(insertShape, e.GetPosition(canvas).X);
                }
                if (e.GetPosition(canvas).Y > startPosition.Y)
                {
                    insertShape.Height = e.GetPosition(canvas).Y - startPosition.Y;
                }
                else
                {
                    insertShape.Height = startPosition.Y - e.GetPosition(canvas).Y;
                    Canvas.SetTop(insertShape, e.GetPosition(canvas).Y);
                }
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;            
            drawFlag = false;

            canvas.Children.Clear();
        }

        private void btnJietu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.rightTabControl.SelectedIndex = 0;

            float ScaleX = PrimaryScreen.ScaleX;
            float ScaleY = PrimaryScreen.ScaleY;

            bitMap = new System.Drawing.Bitmap(Convert.ToInt32(insertShape.Width * ScaleX), Convert.ToInt32(insertShape.Height * ScaleY), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitMap);
            graphics.CopyFromScreen(new System.Drawing.Point(Convert.ToInt32(x_start), Convert.ToInt32(y_start)), new System.Drawing.Point(0, 0), new System.Drawing.Size(Convert.ToInt32(insertShape.Width * ScaleX), Convert.ToInt32(insertShape.Height * ScaleY)), System.Drawing.CopyPixelOperation.SourceCopy);
            //graphics.DrawImage(MainWindow.bitBmp, new System.Drawing.Rectangle(0, 0, Convert.ToInt32(insertShape.Width * ScaleX), Convert.ToInt32(insertShape.Height * ScaleY)), new System.Drawing.Rectangle(Convert.ToInt32(x_start*ScaleX*2), Convert.ToInt32(y_start*ScaleY*2), Convert.ToInt32(insertShape.Width * ScaleX*2), Convert.ToInt32(insertShape.Height * ScaleY*2)), System.Drawing.GraphicsUnit.Pixel);

            MainWindow.mainWindow.App_Exited(sender, e);
        }


        
    }
}
