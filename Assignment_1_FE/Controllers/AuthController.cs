using Assignment_1_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Assignment_1_FE.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public AuthController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Kiểm tra Admin từ appsettings.json
            var adminEmail = _config["AdminAccount:Email"];
            var adminPass = _config["AdminAccount:Password"];

            if (email == adminEmail && password == adminPass)
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Email", email);
                return RedirectToAction("Index", "SystemAccounts"); // Admin quản lý Account
            }

            // 2. Nếu không phải Admin, gọi API OData để kiểm tra trong Database
            var client = _clientFactory.CreateClient("ODataApi");
            // Câu lệnh truy vấn OData chuẩn:
            string safeEmail = email.Replace("'", "''");
            string safePassword = password.Replace("'", "''");

            // Mã hóa URL để các ký tự đặc biệt (như @, #, &, khoảng trắng) truyền đi an toàn
            string filterQuery = $"AccountEmail eq '{safeEmail}' and AccountPassword eq '{safePassword}'";
            string encodedFilter = Uri.EscapeDataString(filterQuery);

            var response = await client.GetAsync($"SystemAccounts?$filter={encodedFilter}");
            if (response.IsSuccessStatusCode)
            {
                var jsonStr = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ODataResponse<SystemAccount>>(jsonStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null && result.Value.Count > 0)
                {
                    var user = result.Value[0];
                    HttpContext.Session.SetString("Role", user.AccountRole == 1 ? "Staff" : "Lecturer");
                    HttpContext.Session.SetString("Email", user.AccountEmail);
                    HttpContext.Session.SetInt32("AccountId", user.AccountId);

                    return RedirectToAction("Index", "Categories"); // Staff vào quản lý Category
                }
            }

            ViewBag.Error = "Email hoặc Mật khẩu không đúng!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
