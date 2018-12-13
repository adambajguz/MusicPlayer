using System;
using Autofac;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using MusicPlayer.Data;
using MusicPlayer.Service;
using MusicPlayer.Service.AppStart;
using MusicPlayer.Service.Controllers;

namespace Service
{
    class Program
    {
        public static IContainer ApplicationContainer { get; private set; }


        static void Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<DataContext>();

            ApplicationContainer = IocConfig.RegisterDependencies(services);

            var container = IocConfig.RegisterDependencies(services);

            var scop = container.BeginLifetimeScope();
            var commandDispatcher = scop.Resolve<ICommandDispatcher>();
            var queryDispatcher = scop.Resolve<IQueryDispatcher>();

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

            
            ImageController ImgController = new ImageController(queryDispatcher, commandDispatcher);
            ImgController.Create("sciezka3").Wait();
            Console.WriteLine(ImgController.GetImages().ToString());
            foreach(var i in ImgController.GetImages().Result)
            {
                Console.WriteLine(i);
            }

            //test  --  nie mozna 2 szybko po sobie
            //         BandController BandController = new BandController(queryDispatcher, commandDispatcher);
            //BandController.Create("band1", new DateTime(2018, 5, 5), null, "opis").Wait();
            //Console.WriteLine("bandyciiii:");
            //Console.WriteLine(BandController.GetBands().ToString());
            //       Console.WriteLine("usuwam");
            //       BandController.Delete(1).Wait();


            Console.ReadKey();
        }

    }
}
