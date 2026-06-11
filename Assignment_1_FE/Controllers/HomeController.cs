using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("ODataApi");
            // OData Query: Lọc các bài viết Active (NewsStatus eq true) và lấy luôn thông tin Category ($expand)
            var response = await client.GetAsync("NewsArticles?$filter=NewsStatus eq true&$expand=Category");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ODataResponse<NewsArticle>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(data.Value);
            }
            return View(new List<NewsArticle>());
        }
    }
}
