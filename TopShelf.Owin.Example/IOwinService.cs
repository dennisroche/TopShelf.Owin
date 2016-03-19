namespace TopShelf.Owin.Example
{
    internal interface IOwinService
    {
        bool Stop();
        bool Start();
    }
}