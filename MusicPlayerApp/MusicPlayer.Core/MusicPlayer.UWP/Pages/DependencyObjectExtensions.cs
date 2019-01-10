using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MusicPlayer.UWP.Pages
{
    public static class DependencyObjectExtensions
    {
        public static bool IsAncestorOf(this DependencyObject parent, DependencyObject child)
        {
            DependencyObject current = child;
            bool isAncestor = false;

            while (current != null && !isAncestor)
            {
                if (current == parent)
                {
                    isAncestor = true;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return isAncestor;
        }
    }
}
