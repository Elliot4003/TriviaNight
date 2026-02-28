
using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IApiService
    {
        Uri BaseAddress { get; }

        HttpClient Client { get; }

        public Task<QuestionsList> QuestionRequestAsync(QuestionRequestModel questionRequest);

        public Task<CategoriesList> CategoryRequestAsync();
    }
}
