using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Topshelf.Logging;

namespace TopShelf.Owin
{
    public class WebAppConfigurator
    {
        protected IDisposable WebApplication;

        protected readonly LogWriter Log;

        public string Scheme { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }

        protected IDependencyResolver DependencyResolver;
        protected Action<HttpConfiguration> HttpConfigurator;

        public WebAppConfigurator()
        {
            Scheme = "http";
            Domain = "localhost";
            Port = 8080;

            Log = HostLogger.Get(typeof(WebAppConfigurator));
            DependencyResolver = null;
            HttpConfigurator = httpConfiguration => httpConfiguration.MapHttpAttributeRoutes();
        }

        public WebAppConfigurator UseDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            return this;
        }

        public WebAppConfigurator ConfigureHttp(Action<HttpConfiguration> httpConfigurator)
        {
            HttpConfigurator = httpConfigurator;
            return this;
        }

        public void Start()
        {
            var options = new StartOptions();
            options.Urls.Add(new UriBuilder(Scheme, Domain, Port).ToString());
            options.Urls.Add(new UriBuilder(Scheme, Environment.MachineName, Port).ToString());

            Log.InfoFormat("[Topshelf.Owin] Starting OWIN self-host, listening on: {0}", string.Join(",", options.Urls));
            
            WebApplication = WebApp.Start(options, Startup);
        }

        private void Startup(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            HttpConfigurator(httpConfiguration);

            if (DependencyResolver != null)
                httpConfiguration.DependencyResolver = DependencyResolver;

            appBuilder.UseWebApi(httpConfiguration);
        }

        public void Stop()
        {
            Log.Info("[Topshelf.Owin] Stopping OWIN self-host");
            WebApplication.Dispose();
        }

    }

}