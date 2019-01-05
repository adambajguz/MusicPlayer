using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.UWP.Pages.Genre
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenreEditPage : Page
    {
        private WriteOnce<int> elementID = new WriteOnce<int>();

        public GenreEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            elementID.Value = (int)e.Parameter;
            NameTextBox.Text = elementID.Value.ToString();
        }
    }
}
