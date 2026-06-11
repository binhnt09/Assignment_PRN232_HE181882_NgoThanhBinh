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

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] short key)
        {
            var account = _service.GetById(key);
            if (account == null) return NotFound();
            return Ok(account);
        }

        public IActionResult Post([FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _service.Add(account);
            return Created(account);
        }

        public IActionResult Put([FromODataUri] short key, [FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != account.AccountId) return BadRequest("Khóa chính không khớp.");

            _service.Update(account);
            return Updated(account);
        }

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
