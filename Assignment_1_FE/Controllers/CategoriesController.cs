using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public CategoriesController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // HIỂN THỊ DANH SÁCH (READ)
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var response = await client.GetAsync("Categories");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ODataResponse<Category>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(data.Value);
            }
            return View(new List<Category>());
        }

        // THÊM MỚI (CREATE) - Trả về JSON cho AJAX
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Categories", jsonContent);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        // CẬP NHẬT (UPDATE) - Trả về JSON cho AJAX
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            // OData cập nhật dùng PUT và truyền key trên URL
            var response = await client.PutAsync($"Categories/{category.CategoryId}", jsonContent);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        // XÓA (DELETE)
        [HttpDelete]
        public async Task<IActionResult> Delete(short id)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var response = await client.DeleteAsync($"Categories/{id}");
            if (response.IsSuccessStatusCode) return Ok();

            // Xử lý lỗi nếu Backend từ chối xóa (vì đã có NewsArticle)
            var errorMsg = await response.Content.ReadAsStringAsync();
            return BadRequest(errorMsg);
        }
    }
}
