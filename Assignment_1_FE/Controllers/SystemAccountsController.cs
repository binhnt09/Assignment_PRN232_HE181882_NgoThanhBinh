using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class SystemAccountsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public SystemAccountsController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // 1. GIAO DIỆN QUẢN LÝ TÀI KHOẢN
        public async Task<IActionResult> Index()
        {
            // Bảo mật: Kiểm tra xem có phải là Admin không (giả sử Role Admin bạn lưu trong session là "Admin")
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            var client = _clientFactory.CreateClient("ODataApi");
            var response = await client.GetAsync("SystemAccounts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ODataResponse<SystemAccount>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(data.Value);
            }
            return View(new List<SystemAccount>());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemAccount account)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("SystemAccounts", content);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SystemAccount account)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            // Gọi Put kèm Key trên URL theo chuẩn OData
            var response = await client.PutAsync($"SystemAccounts({account.AccountId})", content);
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(short id)
        {
            var client = _clientFactory.CreateClient("ODataApi");
            var response = await client.DeleteAsync($"SystemAccounts({id})");
            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        // ==============================================
        // 2. GIAO DIỆN BÁO CÁO THỐNG KÊ (REPORT)
        // ==============================================
        public IActionResult Report()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View(new List<NewsArticle>()); // Trả về view rỗng ban đầu
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Login", "Auth");

            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");

            var endOfDay = endDate.AddDays(1).AddTicks(-1);
            var client = _clientFactory.CreateClient("ODataApi");

            // ODATA: Lọc từ ngày đến ngày ($filter) và Sắp xếp giảm dần ($orderby)
            string filterQuery = $"NewsArticles?$filter=CreatedDate ge {startDate:yyyy-MM-ddTHH:mm:ssZ} and CreatedDate le {endOfDay:yyyy-MM-ddTHH:mm:ssZ}&$orderby=CreatedDate desc";

            // Gọi API Report ở Back-End mà chúng ta đã làm ở bước trước
            var response = await client.GetAsync(filterQuery);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // SỬA LỖI JSON Ở ĐÂY: Dùng ODataResponse để "mở hộp" lấy thuộc tính Value
                var result = JsonSerializer.Deserialize<ODataResponse<NewsArticle>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Trả danh sách bài viết ra giao diện
                return View(result?.Value ?? new List<NewsArticle>());
            }

            ViewBag.Error = "Lỗi khi tải dữ liệu báo cáo!";
            return View(new List<NewsArticle>());
        }
    }
}
