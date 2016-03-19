using TopShelf.Owin;

namespace ExampleSeflHostOwin
{
    internal class YourService : IOwinService
    {
        public bool Stop()
        {
            return true;
        }

        public bool Start()
        {
            return true;
        }
    }
}