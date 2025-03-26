using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaNight.Interfaces;
using TriviaNight.Models;
using System.Text.Json;
using System.Security.AccessControl;

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

        [HttpGet]
        public QuestionsList QuestionRequest(QuestionRequestModel questionRequest)
        {
            QuestionsList questions = new QuestionsList();
            string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty.ToLower();
            HttpResponseMessage response = this.Client.GetAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionsList>(data);
                questions = result ?? new QuestionsList();

                return questions;
            }
            return questions;
        }

        [HttpGet]
        public CategoriesList CategoryRequest()
        {
            CategoriesList categories = new CategoriesList();
            HttpResponseMessage response = this.Client.GetAsync("api_category.php").Result;
            CategoriesQuestionCountResponse categoryQuestionCount = CategoryCount();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesList>(data);
                categories = result ?? new CategoriesList();
                foreach (CategoryModel category in categories.Categories)
                {
                    category.QuestionCount = categoryQuestionCount.CategoriesQuestionCount.Where(x => x.Key.ToString() == category.Id.ToString()).Select(x => x.Value).FirstOrDefault().TotalNumOfVerifiedQuestions;
                }
            }

            return(categories);
        }

        [HttpGet]
        private CategoriesQuestionCountResponse CategoryCount()
        {
            CategoriesQuestionCountResponse categoryQuestionCount = new CategoriesQuestionCountResponse();
            HttpResponseMessage response = this.Client.GetAsync("api_count_global.php").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesQuestionCountResponse>(data);
                categoryQuestionCount = result ?? new CategoriesQuestionCountResponse();
            }

            return categoryQuestionCount;
        }

    }
}
