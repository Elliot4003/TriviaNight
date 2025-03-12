
namespace TriviaNight.Interfaces
{
    public interface IApi
    {
        Uri BaseAddress { get; }

        HttpClient Client { get;  }
    }
}
