using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DragonKing.UI.Utils
{
    public class WPFUtil
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T) return (T)child;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }

            return null;
        }

        // 递归获取窗体中的所有子控件
        public static IEnumerable<FrameworkElement> GetChildControls(DependencyObject parent)
        {
            List<FrameworkElement> controls = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement frameworkElement)
                {
                    controls.Add(frameworkElement);

                    // 如果该子元素还包含子元素，则继续获取子元素的子元素
                    if (VisualTreeHelper.GetChildrenCount(child) > 0)
                    {
                        controls.AddRange(GetChildControls(child));
                    }
                }
            }

            return controls;
        }
    }
}
