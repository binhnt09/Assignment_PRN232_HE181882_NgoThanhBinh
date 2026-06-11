using Assignment_1.Models;
using Assignment_1.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment_1.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
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
            var category = _service.GetById(key);
            if (category == null) return NotFound();
            return Ok(category);
        }
        public IActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _service.Add(category);
            return Created(category);
        }
        public IActionResult Put([FromODataUri] short key, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != category.CategoryId) return BadRequest("Khóa chính không khớp.");

            _service.Update(category);
            return Updated(category);
        }
        public IActionResult Delete([FromODataUri] short key)
        {
            bool isDeleted = _service.Delete(key);
            if (!isDeleted)
            {
                return BadRequest("Không thể xóa danh mục này vì đang chứa bài viết (News Articles).");
            }
            return NoContent();
        }
    }
}
