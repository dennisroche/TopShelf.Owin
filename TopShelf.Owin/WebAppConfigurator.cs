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

        public WebAppConfigurator()
        {
            Scheme = "http";
            Domain = "localhost";
            Port = 8080;

            Log = HostLogger.Get(typeof(WebAppConfigurator));
        }

        public WebAppConfigurator UseDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            return this;
        }

        public void Start()
        {
            var baseAddress = new UriBuilder(Scheme, Domain, Port);
            Log.InfoFormat("[Topshelf.Owin] Starting OWIN self-host, listening on: {0}", baseAddress.Uri);

            WebApplication = WebApp.Start(baseAddress.ToString(), Startup);
        }

        private void Startup(IAppBuilder application)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            if (DependencyResolver != null)
                httpConfiguration.DependencyResolver = DependencyResolver;

            application.UseWebApi(httpConfiguration);
        }

        public void Stop()
        {
            Log.Info("[Topshelf.Owin] Stopping OWIN self-host");
            WebApplication.Dispose();
        }

    }

}