using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Data;
using MusicPlayer.UWP.AppStart;
using MusicPlayer.UWP.Controllers;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer.UWP
{
    /// <summary>
    /// Zapewnia zachowanie specyficzne dla aplikacji, aby uzupełnić domyślną klasę aplikacji.
    /// </summary>
    /// 

    //http://blog.rogatnev.net/2018/01/04/Specification-pattern.html?fbclid=IwAR2PxLelsTM5XiG9ntKjVWqnA2ULr5LsYqcM0ZaZU4CXMSlOwdQJC9DKTg4
    sealed partial class App : Application
    {
        public static IContainer ApplicationContainer { get; private set; }

        public static ICommandDispatcher CommandDispatcher { get; private set; }
        public static  IQueryDispatcher QueryDispatcher { get; private set; }


        public static GenreController GenreController { get; private set; }
       

        /// <summary>
        /// Inicjuje pojedynczy obiekt aplikacji. Jest to pierwszy wiersz napisanego kodu
        /// wykonywanego i jest logicznym odpowiednikiem metod main() lub WinMain().
        /// </summary>
        public App()
        {

            this.InitializeComponent();
            this.Suspending += OnSuspending;

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<DataContext>();

            ApplicationContainer = IocConfig.RegisterDependencies(services);

            IContainer container = IocConfig.RegisterDependencies(services);

            ILifetimeScope scop = container.BeginLifetimeScope();
            CommandDispatcher = scop.Resolve<ICommandDispatcher>();
            QueryDispatcher = scop.Resolve<IQueryDispatcher>();

            //sprawdzam
            using (var scope = container.BeginLifetimeScope())
            {
                //var commandDispatcher = scope.Resolve<ICommandDispatcher>();
                //var queryDispatcher = scope.Resolve<IQueryDispatcher>();
                var dataContextEntityContext = scope.Resolve<IEntitiesContext>();
                var dataContextDbContext = scope.Resolve<DbContext>();
                var uow = scope.Resolve<IUnitOfWork>();

                //var app = scope.Resolve<IQueryDispatcher>();
                //app.WriteInformation("injected!");
            }

            GenreController = new GenreController(App.QueryDispatcher, App.CommandDispatcher);



            //         ImageController ImgController = new ImageController(queryDispatcher, commandDispatcher);
            //ImgController.Create("sciezka4").Wait();
            //          AlbumController albumController = new AlbumController(queryDispatcher, commandDispatcher);
            //          albumController.Create("tytul", "opis", DateTime.UtcNow, 1).Wait();
            //ImgController.Get(0).Wait();

            //GenreController GenreController = new GenreController(queryDispatcher, commandDispatcher);
            //GenreController.Create("gatunek1", "opis").Wait();


            //test  --  nie mozna 2 szybko po sobie
            //BandController BandController = new BandController(queryDispatcher, commandDispatcher);
            //BandController.Create("band1", new DateTime(2018, 5, 5), null, "opis").Wait();
            //Console.WriteLine("bandyciiii:");
            //Console.WriteLine(BandController.GetBands().ToString());
            //       Console.WriteLine("usuwam");
            //       BandController.Delete(1).Wait();
        }

        /// <summary>
        /// Wywoływane, gdy aplikacja jest uruchamiana normalnie przez użytkownika końcowego. Inne punkty wejścia
        /// będą używane, kiedy aplikacja zostanie uruchomiona w celu otworzenia określonego pliku.
        /// </summary>
        /// <param name="e">Szczegóły dotyczące żądania uruchomienia i procesu.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            //ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Nie powtarzaj inicjowania aplikacji, gdy w oknie znajduje się już zawartość,
            // upewnij się tylko, że okno jest aktywne
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Utwórz ramkę, która będzie pełnić funkcję kontekstu nawigacji, i przejdź do pierwszej strony
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Załaduj stan z wstrzymanej wcześniej aplikacji
                }

                // Umieść ramkę w bieżącym oknie
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Kiedy stos nawigacji nie jest przywrócony, przejdź do pierwszej strony,
                    // konfigurując nową stronę przez przekazanie wymaganych informacji jako
                    // parametr
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Upewnij się, ze bieżące okno jest aktywne
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Wywoływane, gdy nawigacja do konkretnej strony nie powiedzie się
        /// </summary>
        /// <param name="sender">Ramka, do której nawigacja nie powiodła się</param>
        /// <param name="e">Szczegóły dotyczące niepowodzenia nawigacji</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Wywoływane, gdy wykonanie aplikacji jest wstrzymywane. Stan aplikacji jest zapisywany
        /// bez wiedzy o tym, czy aplikacja zostanie zakończona, czy wznowiona z niezmienioną zawartością
        /// pamięci.
        /// </summary>
        /// <param name="sender">Źródło żądania wstrzymania.</param>
        /// <param name="e">Szczegóły żądania wstrzymania.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Zapisz stan aplikacji i zatrzymaj wszelkie aktywności w tle
            deferral.Complete();
        }
    }
}
