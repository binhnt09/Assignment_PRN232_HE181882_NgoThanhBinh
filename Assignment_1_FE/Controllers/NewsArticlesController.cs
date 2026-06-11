using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public NewsArticlesController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            // Kiểm tra phân quyền (Chỉ Staff/Admin mới được vào)
            if (HttpContext.Session.GetString("Role") == null) return RedirectToAction("Login", "Auth");

            var client = _clientFactory.CreateClient("ODataApi");

            // Lấy danh sách Bài viết
            var newsRes = await client.GetAsync("NewsArticles?$expand=Category");
            // Lấy danh sách Category để đổ vào Dropdown list lúc Add/Edit
            var catRes = await client.GetAsync("Categories");

            if (newsRes.IsSuccessStatusCode && catRes.IsSuccessStatusCode)
            {
                var newsData = JsonSerializer.Deserialize<ODataResponse<NewsArticle>>(await newsRes.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var catData = JsonSerializer.Deserialize<ODataResponse<Category>>(await catRes.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.Categories = catData.Value;
                return View(newsData.Value);
            }
            return View(new List<NewsArticle>());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsArticle article)
        {
            // Tự động gán người tạo là Staff đang Login
            article.CreatedById = (short?)HttpContext.Session.GetInt32("AccountId");
            article.CreatedDate = DateTime.Now;

            var client = _clientFactory.CreateClient("ODataApi");
            var content = new StringContent(JsonSerializer.Serialize(article), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("NewsArticles", content);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NewsArticle article)
        {
            article.ModifiedDate = DateTime.Now;
            var client = _clientFactory.CreateClient("ODataApi");
            var content = new StringContent(JsonSerializer.Serialize(article), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"NewsArticles('{article.NewsArticleId}')", content);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var response = await client.DeleteAsync($"NewsArticles('{id}')");
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }
    }
}
