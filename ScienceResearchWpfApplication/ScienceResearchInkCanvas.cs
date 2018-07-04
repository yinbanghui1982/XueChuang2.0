using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// 学创中的InkCanvas子类
    /// </summary>
    public class ScienceResearchInkCanvas : InkCanvas
    {
        InkPropertiesUserControl inkPropertiesUserControl;

        /// <summary>
        /// 手写笔属性
        /// </summary>
        public InkPropertiesUserControl InkPropertiesUserControl
        {
            set
            {
                inkPropertiesUserControl = value;

                //==========设置绘图板属性==================
                EditingMode = inkPropertiesUserControl.EditingModeInk;
                DynamicRenderer.DrawingAttributes.Color = inkPropertiesUserControl.ColorInk;
                DynamicRenderer.DrawingAttributes.Width = inkPropertiesUserControl.WidthInk;
                DynamicRenderer.DrawingAttributes.Height = inkPropertiesUserControl.WidthInk;
                DynamicRenderer.DrawingAttributes.IsHighlighter = inkPropertiesUserControl.IsHighlighterInk;

                //==========属性改变处理==================
                inkPropertiesUserControl.ChangeColorInk += InkPropertiesUserControl_ChangeColorInk;
                inkPropertiesUserControl.ChangeEditingModeInk += InkPropertiesUserControl_ChangeEditingModeInk;
                inkPropertiesUserControl.ChangeIsHighlighterInk += InkPropertiesUserControl_ChangeIsHighlighterInk;
                inkPropertiesUserControl.ChangeWidthInk += InkPropertiesUserControl_ChangeWidthInk;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScienceResearchInkCanvas()
            : base()
        {
            InkPropertiesUserControl = MainWindow.mainWindow.inkProperties;

            //InkPresenter.DetachVisuals(DynamicRenderer.RootVisual);
            //InkPresenter.AttachVisuals(DynamicRenderer.RootVisual, DynamicRenderer.DrawingAttributes);          
        }

        #region 属性更改处理
        private void InkPropertiesUserControl_ChangeColorInk(object sender, object v)
        {
            DynamicRenderer.DrawingAttributes.Color = (Color)v;
        }

        private void InkPropertiesUserControl_ChangeEditingModeInk(object sender, object v)
        {
            EditingMode =(InkCanvasEditingMode)v;
        }
        private void InkPropertiesUserControl_ChangeIsHighlighterInk(object sender, object v)
        {
            DynamicRenderer.DrawingAttributes.IsHighlighter = (bool)v;
        }
        private void InkPropertiesUserControl_ChangeWidthInk(object sender, object v)
        {
            DynamicRenderer.DrawingAttributes.Width = (double)v;
            DynamicRenderer.DrawingAttributes.Height = (double)v;
        }
        #endregion

        #region 触控笔操作
        /// <summary>
        /// 触控笔操作
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStylusDown(StylusDownEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                EditingMode = InkCanvasEditingMode.Ink;
                base.OnStylusDown(e);
            }
            else if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                EditingMode = InkCanvasEditingMode.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStylusMove(StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                EditingMode = InkCanvasEditingMode.Ink;
                base.OnStylusMove(e);
            }
            else if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                EditingMode = InkCanvasEditingMode.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStylusUp(StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
            }
            else if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
            }
        }
        #endregion

    }
}
