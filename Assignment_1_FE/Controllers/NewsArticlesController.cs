using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public NewsArticlesController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        //public async Task<IActionResult> Index()
        public async Task<IActionResult> Index(string searchTitle = "", int page = 1)
        {
            // Kiểm tra phân quyền (Chỉ Staff/Admin mới được vào)
            if (HttpContext.Session.GetString("Role") == null) return RedirectToAction("Login", "Auth");

            int pageSize = 2; // Số bài viết trên 1 trang
            int skip = (page - 1) * pageSize; // Số bài cần bỏ qua

            var client = _clientFactory.CreateClient("ODataApi");
            // $count=true : Yêu cầu đếm tổng số bài
            // $top và $skip : Để phân trang
            // $expand=Category : Lấy tên danh mục
            string query = $"NewsArticles?$count=true&$expand=Category&$top={pageSize}&$skip={skip}";
            if (!string.IsNullOrEmpty(searchTitle))
            {
                string encodedSearch = Uri.EscapeDataString(searchTitle);
                query += $"&$filter=contains(NewsTitle, '{encodedSearch}')";
            }
            // Lấy danh sách Bài viết
            //var newsRes = await client.GetAsync("NewsArticles?$expand=Category");
            // Lấy danh sách Category để đổ vào Dropdown list lúc Add/Edit
            var newsRes = await client.GetAsync(query);
            var catRes = await client.GetAsync("Categories");

            if (newsRes.IsSuccessStatusCode && catRes.IsSuccessStatusCode)
            {
                var newsData = JsonSerializer.Deserialize<ODataResponse<NewsArticle>>(await newsRes.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var catData = JsonSerializer.Deserialize<ODataResponse<Category>>(await catRes.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.Categories = catData.Value;

                ViewBag.CurrentPage = page;
                ViewBag.SearchTitle = searchTitle;
                if (newsData.OdataCount.HasValue)
                {
                    // Tính tổng số trang (Ví dụ 12 bài / 5 = 3 trang)
                    ViewBag.TotalPages = (int)Math.Ceiling((double)newsData.OdataCount.Value / pageSize);
                    ViewBag.TotalRecords = newsData.OdataCount.Value;
                }
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
