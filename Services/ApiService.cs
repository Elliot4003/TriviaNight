using TriviaNight.Interfaces;

namespace TriviaNight.Services
{
    public class ApiService : IApi
    {
        public Uri BaseAddress { get; } = new Uri("https://opentdb.com/");
        public HttpClient Client { get; }
        
        public ApiService()
        {
            Client = new HttpClient { BaseAddress = BaseAddress };
        }
    }
}
