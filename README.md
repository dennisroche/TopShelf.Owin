Topshelf.Owin [![Build status](https://ci.appveyor.com/api/projects/status/fx8d5f2apa553gge?svg=true)](https://ci.appveyor.com/project/dennisroche/topshelf-owin) [![NuGet Version](http://img.shields.io/nuget/v/Topshelf.Owin.svg?style=flat)](https://www.nuget.org/packages/Topshelf.Owin/)
=============

Extend TopShelf to be a self-hosted API using [OWIN](http://owin.org/) (Open Web Interface for .NET) 

**NB:** Owin requires .NET 4.5 or later.

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

                    s.OwinEndpoint(app =>
                    {
                        app.Domain = "localhost";
                        app.Port = 8080;
                    });
                });

            });
        }

    }
}
```

If you want to use with AutoFac, then also add [AutoFac.WebApi](https://www.nuget.org/packages/Autofac.WebApi/) and [TopShelf.AutoFac](https://www.nuget.org/packages/Topshelf.Autofac/) packages and then set the `DependencyResolver`.

```c#
s.OwinEndpoint(app =>
{
    app.UseDependencyResolver(new AutofacWebApiDependencyResolver(container));
}
```

By default, it will map routes using attributes on the actions. If you want to change the `HttpCongfiguration` to change routes or change the [JSON serializer](https://www.nuget.org/packages/Newtonsoft.Json/), you can:

```c#
s.OwinEndpoint(app =>
{
    app.ConfigureHttp(httpConfiguration =>
    {
        httpConfiguration.MapHttpAttributeRoutes();
        httpConfiguration.Formatters.Clear();
        httpConfiguration.Formatters.Add(new JsonMediaTypeFormatter());

        var jsonSettings = httpConfiguration.Formatters.JsonFormatter.SerializerSettings;
        jsonSettings.Formatting = Formatting.Indented;
        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });
});
```

If you want to bind any OWIN app/middleware other than WebAPI, you can do so by accessing the `IAppBuilder` instance directly, e.g.:

```c#
s.OwinEndpoint(app =>
{
    app.ConfigureAppBuilder(appBuilder => appBuilder.UseNancy());
});
```
