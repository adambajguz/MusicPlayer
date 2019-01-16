using MusicPlayer.Core.Extensions;
using MusicPlayer.UWP.Pages.Albums;
using MusicPlayer.UWP.Pages.Artists;
using MusicPlayer.UWP.Pages.Bands;
using MusicPlayer.UWP.Pages.Genres;
using MusicPlayer.UWP.Pages.Playlists;
using MusicPlayer.UWP.Pages.Songs;
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

        public const string SongsTag = "songs";
        public const string SongDetailsTag = "songDetails";
        public const string SongAddTag = "songAdd";
        public const string SongEditTag = "songEdit";

        public const string AlbumsTag = "albums";
        public const string AlbumDetailsTag = "albumDetails";
        public const string AlbumAddTag = "albumAdd";
        public const string AlbumEditTag = "albumEdit";


        public const string PlaylistsTag = "playlists";
        public const string PlaylistDetailsTag = "playlistDetails";
        public const string PlaylistAddTag = "playlistAdd";
        public const string PlaylistEditTag = "playlistEdit";

        public const string GenresTag = "genres";
        public const string GenreDetailsTag = "genreDetails";
        public const string GenreAddTag = "genreAdd";
        public const string GenreEditTag = "genreEdit";

        public const string BandsTag = "bands";
        public const string BandDetailsTag = "bandDetails";
        public const string BandAddTag = "bandAdd";
        public const string BandEditTag = "bandEdit";

        public const string ArtistsTag = "artists";
        public const string ArtistDetailsTag = "artistDetails";
        public const string ArtistAddTag = "artistAdd";
        public const string ArtistEditTag = "artistEdit";

        public const string PlayQueueTag = "queue";



        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            (SongsTag, typeof(SongsPage)),
            (SongDetailsTag, typeof(SongDetailsPage)),
            (SongAddTag, typeof(SongAddPage)),
            (SongEditTag, typeof(SongEditPage)),

            (AlbumsTag, typeof(AlbumsPage)),
            (AlbumDetailsTag, typeof(AlbumDetailsPage)),
            (AlbumAddTag, typeof(AlbumAddPage)),
            (AlbumEditTag, typeof(AlbumEditPage)),

            (PlaylistsTag, typeof(PlaylistsPage)),
            (PlaylistDetailsTag, typeof(PlaylistDetailsPage)),
            (PlaylistAddTag, typeof(PlaylistAddPage)),
            (PlaylistEditTag, typeof(PlaylistEditPage)),

            (BandsTag, typeof(BandsPage)),
            (BandDetailsTag, typeof(BandDetailsPage)),
            (BandAddTag, typeof(BandAddPage)),
            (BandEditTag, typeof(BandEditPage)),

            (ArtistsTag, typeof(ArtistsPage)),
            (ArtistDetailsTag, typeof(ArtistDetailsPage)),
            (ArtistAddTag, typeof(ArtistAddPage)),
            (ArtistEditTag, typeof(ArtistEditPage)),

            (GenresTag, typeof(GenresPage)),
            (GenreDetailsTag, typeof(GenreDetailsPage)),
            (GenreAddTag, typeof(GenreAddPage)),
            (GenreEditTag, typeof(GenreEditPage)),

            (PlayQueueTag, typeof(GenresPage)),

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
            NavView_Navigate(SongsTag, new EntranceNavigationTransitionInfo(), null);

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
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo, null);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo, null);
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

        public void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo, object parameter)
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
                ContentFrame.Navigate(_page, parameter, transitionInfo);
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

        public bool GoBack() => On_BackRequested();

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
