using Assignment_1.Models;
using Assignment_1.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment_1.Controllers
{
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _service;

        public SystemAccountsController(ISystemAccountService service)
        {
            _service = service;
        }

        // 1. Khai báo rõ đây là GET tất cả
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        // 2. Khai báo rõ đây là GET theo ID, có tham số {key} để Swagger phân biệt với hàm Get() ở trên
        [HttpGet("({key})")]
        [EnableQuery]
        public IActionResult Get([FromODataUri] short key)
        {
            var account = _service.GetById(key);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // 3. Khai báo rõ đây là POST (Thêm mới)
        [HttpPost]
        public IActionResult Post([FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _service.Add(account);
            return Created(account);
        }

        // 4. Khai báo rõ đây là PUT (Cập nhật), có chứa {key} trên đường dẫn
        [HttpPut("({key})")]
        public IActionResult Put([FromODataUri] short key, [FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != account.AccountId) return BadRequest("Khóa chính không khớp.");

            _service.Update(account);
            return Updated(account);
        }

        // 5. Khai báo rõ đây là DELETE (Xóa), có chứa {key}
        [HttpDelete("({key})")]
        public IActionResult Delete([FromODataUri] short key)
        {
            bool isDeleted = _service.Delete(key);
            if (!isDeleted)
            {
                return BadRequest("Không thể xóa tài khoản này vì đã có bài viết (News Articles) liên kết.");
            }
            return NoContent();
        }
    }
}
