
using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IApiService
    {
        Uri BaseAddress { get; }

        HttpClient Client { get; }

        public QuestionsList QuestionRequest(QuestionRequestModel questionRequest);

        public CategoriesList CategoryRequest();
    }
}
