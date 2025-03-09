using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TriviaNight.Models;

namespace TriviaNight.Controllers;

public class HomeController : Controller
{
    private readonly Uri baseAdress = new Uri("https://opentdb.com/");
    private readonly HttpClient _client;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _client = new HttpClient();
        _client.BaseAddress = baseAdress;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<CategoryModel> categories = new List<CategoryModel>();
        HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "api_category.php").Result;
        CategoriesQuestionCountResponse categoryQuestionCount = CategoryCount();

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<CategoriesResponse>(data);
            categories = result?.Categories ?? new List<CategoryModel>();
            foreach(CategoryModel category in categories)
            {
                CategoryQuestionCountModel categoryResult = (CategoryQuestionCountModel)categoryQuestionCount.CategoriesQuestionCount.Where(x => x.Keys.ToString() == category.Id.ToString());
                category.QuestionCount = categoryResult.TotalNumOfQuestions;
            }
        }

        return View(categories);
    }

    [HttpGet]
    private CategoriesQuestionCountResponse CategoryCount()
    {
        CategoriesQuestionCountResponse categoryQuestionCount = new CategoriesQuestionCountResponse();
        HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "api_count_global.php").Result;

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<CategoriesQuestionCountResponse>(data);
            categoryQuestionCount = result ?? new CategoriesQuestionCountResponse();
        }

        return categoryQuestionCount;
    }

    [HttpGet]
    public IActionResult Questions(int category, int amount, string difficulty)
    {
        List<QuestionModel> questions = new List<QuestionModel>();
        string request = "api.php?category=" + category + "&amount=" + amount.ToString() + "&difficulty=" + difficulty + "&";
        HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + request).Result;

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<QuestionsResponse>(data);
            questions = result?.Questions ?? new List<QuestionModel>();
        }

        return View(questions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
