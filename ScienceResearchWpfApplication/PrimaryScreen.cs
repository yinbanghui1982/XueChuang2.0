using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace XYDES
{
    public class PrimaryScreen
    {
        #region Win32 API
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        #endregion
        #region DeviceCaps常量
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int SCALINGFACTORX = 114;
        const int SCALINGFACTORY = 115;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        #endregion

        #region 属性
        /// <summary>
        /// 获取屏幕分辨率当前物理大小
        /// </summary>
        public static Size WorkingArea
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, HORZRES);
                size.Height = GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }
        /// <summary>
        /// 当前系统DPI_X 大小 一般为96
        /// </summary>
        public static int DpiX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 当前系统DPI_Y 大小 一般为96
        /// </summary>
        public static int DpiY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSY);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size DESKTOP
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获取宽度缩放百分比
        /// </summary>
        public static float ScaleX
        {
            get
            {
                //IntPtr hdc = GetDC(IntPtr.Zero);
                //int t = GetDeviceCaps(hdc, DESKTOPHORZRES);
                //int d = GetDeviceCaps(hdc, HORZRES);
                //float ScaleX = (float)GetDeviceCaps(hdc, DESKTOPHORZRES) / (float)GetDeviceCaps(hdc, HORZRES);
                //ReleaseDC(IntPtr.Zero, hdc);
                float ScaleX = 0;
                if (DpiX == 96)
                    ScaleX = 1;
                else if (DpiX == 120)
                    ScaleX = 1.25f;
                else if (DpiX == 144)
                    ScaleX = 1.5f;
                else if (DpiX == 192)
                    ScaleX = 2;
                else if (DpiX == 240)
                    ScaleX = 2.5f;

                return ScaleX;
            }
        }
        /// <summary>
        /// 获取高度缩放百分比
        /// </summary>
        public static float ScaleY
        {
            get
            {
                //IntPtr hdc = GetDC(IntPtr.Zero);
                //float ScaleY = (float)(float)GetDeviceCaps(hdc, DESKTOPVERTRES) / (float)GetDeviceCaps(hdc, VERTRES);
                //ReleaseDC(IntPtr.Zero, hdc);

                float ScaleY = 0;
                if (DpiY == 96)
                    ScaleY = 1;
                else if (DpiY == 120)
                    ScaleY = 1.25f;
                else if (DpiY == 144)
                    ScaleY = 1.5f;
                else if (DpiY == 192)
                    ScaleY = 2;
                else if (DpiY == 240)
                    ScaleY = 2.5f;

                return ScaleY;
            }
        }
        #endregion
    }
}