using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MusicPlayer.UWP.Pages
{
    public static class DependencyObjectExtension
    {
        public static T FindParent<T>(this DependencyObject dependencyObject) where T : class
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
    }
}
