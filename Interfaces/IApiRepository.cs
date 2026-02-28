using TriviaNight.Models;

namespace TriviaNight.Interfaces;

public interface IApiRepository
{
    Uri BaseAddress { get; }

    HttpClient Client { get; }

    public Task<QuestionsList> QuestionRequestAsync(QuestionRequestModel questionRequest);

    public Task<CategoriesList> CategoryRequestAsync();
}
