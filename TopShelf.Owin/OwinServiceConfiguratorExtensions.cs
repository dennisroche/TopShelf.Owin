using System;
using System.Linq;
using Topshelf.ServiceConfigurators;

namespace TopShelf.Owin
{
    public static class OwinServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<T> OwinEndpoint<T>(this ServiceConfigurator<T> configurator, Action<WebAppConfigurator> appConfigurator = null) where T : class
        {
            var config = new WebAppConfigurator();
            if (appConfigurator != null) 
                appConfigurator(config);

            configurator.BeforeStartingService(t => config.Start());
            configurator.AfterStoppingService(t => config.Stop());

            return configurator;
        }
    }
}
