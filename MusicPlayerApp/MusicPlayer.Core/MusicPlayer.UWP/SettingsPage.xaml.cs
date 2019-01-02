using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MusicPlayer.UWP.Controllers;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        GenreController GenreController;

        public SettingsPage()
        {
            this.InitializeComponent();

            GenreController = App.GenreController;

        
            Test();

        }

        private async void Test()
        {
            string test = GenreController.GetAll().ToString();
            Controllers.Genre.Result genre = await GenreController.Get(1);
            List<Controllers.Genre.Result>  genres = await GenreController.GetAll();

            progress.IsActive = false;
        

            string c = genres.Count.ToString();
            MessageDialog message = new MessageDialog(c + genre.Name, "OUTPUT:");
            await message.ShowAsync();


        }
    }
}
