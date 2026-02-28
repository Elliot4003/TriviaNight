using Microsoft.AspNetCore.Mvc;
using TriviaNight.Interfaces;
using TriviaNight.Models;
using System.Text.Json;

namespace TriviaNight.Services;

public class ApiService : IApiService
{
    public Uri BaseAddress { get; } = new Uri("https://opentdb.com/");
    public HttpClient Client { get; }
    
    public ApiService()
    {
        Client = new HttpClient { BaseAddress = BaseAddress };
    }

    /// <summary>
    /// Récupère les questions via l'API OTDB en fonction de la requête émise par l'utilisateur
    /// </summary>
    /// <param name="questionRequest">Contient la catégorie, le nombre de questions et la difficulté</param>
    /// <returns>Les questions</returns>
    [HttpPost]
    public async Task<QuestionsList> QuestionRequestAsync(QuestionRequestModel questionRequest)
    {
        string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty.ToLower();
        HttpResponseMessage response = await Client.GetAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        var data = response.Content.ReadAsStream();
        var questions = await JsonSerializer.DeserializeAsync<QuestionsList>(data);

        if (questions is null)
            throw new Exception("L'API n'a retourné aucun résultat");

        return questions;
    }

    /// <summary>
    /// Récupère toutes les catégories via l'API OTDB
    /// </summary>
    /// <returns>Les catégories</returns>
    [HttpGet]
    public async Task<CategoriesList> CategoryRequestAsync()
    {
        HttpResponseMessage response = await Client.GetAsync("api_category.php");

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        var data = response.Content.ReadAsStream();
        var categories = await JsonSerializer.DeserializeAsync<CategoriesList>(data);

        if (categories is null)
            throw new Exception("L'API n'a retourné aucun résultat");

        foreach (var category in categories.Categories)
        {
            var categoryQuestionCount = await CategoryQuestionCountRequestAsync(category.Id);
                
            category.EasyQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalEasyQuestionCount;
            category.MediumQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalMediumQuestionCount;
            category.HardQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalHardQuestionCount;
        }
        
        return categories;
    }

    /// <summary>
    /// Récupère le nombre total de questions disponibles sur OTDB
    /// </summary>
    /// <returns>Le nombre de questions</returns>
    [HttpGet]
    private async Task<QuestionCountResponse> QuestionCountRequestAsync()
    {
        HttpResponseMessage response = await Client.GetAsync("api_count_global.php");

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        var data = response.Content.ReadAsStream();
        var questionCount = await JsonSerializer.DeserializeAsync<QuestionCountResponse>(data);
        
        if (questionCount is null)
            throw new Exception("L'API n'a retourné aucun résultat");

        return questionCount;
    }

    /// <summary>
    /// Récupère le nombre de questions d'une catégorie via l'API OTDB
    /// </summary>
    /// <param name="id">Id de la catégorie</param>
    /// <returns>Le nombre de questions d'une catégorie</returns>
    [HttpPost]
    private async Task<CategoryQuestionCountResponse> CategoryQuestionCountRequestAsync(int id)
    {
        HttpResponseMessage response = await Client.GetAsync("api_count.php?category=" + id);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        var data = response.Content.ReadAsStream();
        var categoryQuestionCount = await JsonSerializer.DeserializeAsync<CategoryQuestionCountResponse>(data);
        
        if (categoryQuestionCount is null)
            throw new Exception("L'API n'a retourné aucun résultat");

        return categoryQuestionCount;
    }
}
