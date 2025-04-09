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
            CategoriesList categories = new();
            HttpResponseMessage response = this.Client.GetAsync("api_category.php").Result;
            CategoryQuestionCountResponse categoryQuestionCount = new();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesList>(data);
                categories = result ?? new();

                if (categories.Categories != null) 
                {
                    foreach (CategoryModel category in categories.Categories)
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

        [HttpGet]
        private QuestionCountResponse QuestionCount()
        {
            QuestionCountResponse questionCount = new();
            HttpResponseMessage response = this.Client.GetAsync("api_count_global.php").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionCountResponse>(data);
                questionCount = result ?? new();
            }

            return questionCount;
        }

        [HttpGet]
        private CategoryQuestionCountResponse CategoryQuestionCount(int id)
        {
            CategoryQuestionCountResponse categoryQuestionCount = new();
            HttpResponseMessage response = this.Client.GetAsync("api_count.php?category=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoryQuestionCountResponse>(data);
                categoryQuestionCount = result ?? new();
            }

            return categoryQuestionCount;
        }
    }
}
