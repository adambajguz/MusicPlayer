using MusicPlayer.Core.Extensions;
using MusicPlayer.UWP.Pages.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace MusicPlayer.UWP.Pages
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    /// 
    //https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
    //https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/navigationview
    //https://blogs.msdn.microsoft.com/appconsult/2018/05/06/using-the-navigationview-in-your-uwp-applications/
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //AudioPlayer.MediaPlayer.SeekCompleted
        }


        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }


        public const string LibraryTag = "library";
        public const string PlaylistsTag = "playlists";
        public const string GenresTag = "genres";
        public const string GenresAddEditTag = "genresAddEdit";


        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            (LibraryTag, typeof(LibraryPage)),
            (PlaylistsTag, typeof(LibraryPage)),
            (GenresTag, typeof(GenresPage)),
            (GenresAddEditTag, typeof(AddEditGenrePage)),

        };

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            //// You can also add items in code.
            //NavView.MenuItems.Add(new muxc.NavigationViewItemSeparator());
            //NavView.MenuItems.Add(new muxc.NavigationViewItem
            //{
            //    Content = "My content",
            //    Icon = new SymbolIcon((Symbol)0xF1AD),
            //    Tag = "content"
            //});
            //_pages.Add(("content", typeof(MyContentPage)));

            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate(LibraryTag, new EntranceNavigationTransitionInfo());

            // Add keyboard accelerators for backwards navigation.
            var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);
        }

        private void NavView_ItemInvoked(NavigationView sender,
                                         NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_SelectionChanged(NavigationView sender,
                                              NavigationViewSelectionChangedEventArgs args)
        {
            //if (args.IsSettingsSelected == true)
            //{
            //    NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            //}
            //else if (args.SelectedItemContainer != null)
            //{
            //    var navItemTag = args.SelectedItemContainer.Tag.ToString();
            //    NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            //}
        }

        public void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(SettingsPage);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }

        private void NavView_BackRequested(NavigationView sender,
                                           NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private void BackInvoked(KeyboardAccelerator sender,
                                 KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);


                object menuItemToSelect = NavView.MenuItems.OfType<NavigationViewItem>()
                                             .FirstOrDefault(n => n.Tag.Equals(item.Tag));

                if (menuItemToSelect.IsNotNull())
                {
                    NavView.SelectedItem = menuItemToSelect;

                    NavView.Header = ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
                }
            }
        }

    }
}
