using System;
using Topshelf.ServiceConfigurators;

namespace TopShelf.Owin
{
    public static class OwinServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<T> OwinEndpoint<T>(this ServiceConfigurator<T> configurator, Action<WebAppConfigurator> appConfigurator = null) where T : class
        {
            var config = new WebAppConfigurator();
            appConfigurator?.Invoke(config);

            configurator.BeforeStartingService(t => config.Start());
            configurator.AfterStoppingService(t => config.Stop());

            return configurator;
        }
    }
}
