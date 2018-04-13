namespace Topshelf.Owin
{
    public interface IOwinService
    {
        bool Stop();
        bool Start();
    }
}