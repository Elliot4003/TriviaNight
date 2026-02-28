using Microsoft.AspNetCore.Mvc;
using TriviaNight.Interfaces;
using TriviaNight.Models;
using System.Text.Json;

namespace TriviaNight.Services
{
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
        public QuestionsList QuestionRequest(QuestionRequestModel questionRequest)
        {
            var questions = new QuestionsList();
            string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty.ToLower();
            HttpResponseMessage response = Client.GetAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionsList>(data);
                questions = result ?? new QuestionsList();

                return questions;
            }
            return questions;
        }

        /// <summary>
        /// Récupère toutes les catégories via l'API OTDB
        /// </summary>
        /// <returns>Les catégories</returns>
        [HttpGet]
        public CategoriesList CategoryRequest()
        {
            var categories = new CategoriesList();
            HttpResponseMessage response = Client.GetAsync("api_category.php").Result;
            var categoryQuestionCount = new CategoryQuestionCountResponse();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesList>(data);
                categories = result ?? new CategoriesList();

                if (categories.Categories != null) 
                {
                    foreach (var category in categories.Categories)
                    {
                        categoryQuestionCount = CategoryQuestionCount(category.Id);
                        if (categoryQuestionCount.CategoryQuestionCount != null) 
                        {
                            category.EasyQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalEasyQuestionCount;
                            category.MediumQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalMediumQuestionCount;
                            category.HardQuestionCount = categoryQuestionCount.CategoryQuestionCount.TotalHardQuestionCount;
                        }
                    }
                }
            }

            return(categories);
        }

        /// <summary>
        /// Récupère le nombre total de questions disponibles sur OTDB
        /// </summary>
        /// <returns>Le nombre de questions</returns>
        [HttpGet]
        private QuestionCountResponse QuestionCount()
        {
            var questionCount = new QuestionCountResponse();
            HttpResponseMessage response = this.Client.GetAsync("api_count_global.php").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionCountResponse>(data);
                questionCount = result ?? new QuestionCountResponse();
            }

            return questionCount;
        }

        /// <summary>
        /// Récupère le nombre de questions d'une catégorie via l'API OTDB
        /// </summary>
        /// <param name="id">Id de la catégorie</param>
        /// <returns>Le nombre de questions d'une catégorie</returns>
        [HttpPost]
        private CategoryQuestionCountResponse CategoryQuestionCount(int id)
        {
            var categoryQuestionCount = new CategoryQuestionCountResponse();
            HttpResponseMessage response = this.Client.GetAsync("api_count.php?category=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoryQuestionCountResponse>(data);
                categoryQuestionCount = result ?? new CategoryQuestionCountResponse();
            }

            return categoryQuestionCount;
        }
    }
}
