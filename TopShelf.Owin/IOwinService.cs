namespace TopShelf.Owin
{
    public interface IOwinService
    {
        bool Stop();
        bool Start();
    }
}