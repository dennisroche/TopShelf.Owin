using Topshelf;
using TopShelf.Owin;

namespace ExampleSeflHostOwin
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.RunAsNetworkService();

                c.Service<YourService>(s =>
                {
                    s.ConstructUsing(() => new YourService());
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
