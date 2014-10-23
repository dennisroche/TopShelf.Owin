Topshelf.Owin
=============

Extend TopShelf to be a self-hosted API using OWIN. 

How to use
=============

Install the [Nuget](https://www.nuget.org/packages/Topshelf.Owin) package.

	Install-Package Topshelf.Owin

Then modify your TopShelf service.

```c#
using Topshelf;
using TopShelf.Owin;

namespace YourService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.RunAsNetworkService();
         
                c.Service<YourService>(s =>
                {
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    s.OwinEndpoint(configurator =>
                    {
                        configurator.Domain = "localhost";
                        configurator.Port = 8080;
                    });
                });

            });
        }

    }
}
```

If you want to use with AutoFac, then also add [AutoFac.WebApi](https://www.nuget.org/packages/Autofac.WebApi/) and [TopShelf.AutoFac](https://www.nuget.org/packages/Topshelf.Autofac/) packages.

```c#
using Autofac;
using Autofac.Integration.WebApi;
using Topshelf;
using Topshelf.Autofac;
using TopShelf.Owin;

namespace YourService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // Service
            builder.RegisterType<YourService>().SingleInstance();

            var container = builder.Build();

            HostFactory.Run(c =>
            {
                c.RunAsNetworkService();
                
                c.UseAutofacContainer(container);
                c.Service<YourService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    s.OwinEndpoint(configurator =>
                    {
                        configurator.Domain = "localhost";
                        configurator.Port = 8080;

                        configurator.UseDependencyResolver(new AutofacWebApiDependencyResolver(container));
                    });
                });

            });
        }

    }
    
}
```
