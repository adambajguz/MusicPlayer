using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using MusicPlayer.Data;
using MusicPlayer.Service.AppStart;

namespace MusicPlayer.Service
{
    public class Startup
    {
        public Startup()
        {
                Configuration = new ConfigurationBuilder()
                .Build();
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>();

            ApplicationContainer = IocConfig.RegisterDependencies(services);
			
            return new AutofacServiceProvider(ApplicationContainer);
        }

    }
}