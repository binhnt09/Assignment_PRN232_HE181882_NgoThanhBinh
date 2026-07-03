using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;
using Assignment_2_BE.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment_2_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Staff can manage categories
        [Authorize(Roles = "1")] 
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [Authorize(Roles = "1")]
        [HttpGet("{id}")]
        public IActionResult GetCategory(short id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Post([FromBody] CategoryDTO categoryDto)
        {
            var created = _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = created.CategoryId }, created);
        }

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public IActionResult Put(short id, [FromBody] CategoryDTO categoryDto)
        {
            var updated = _categoryService.UpdateCategory(id, categoryDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public IActionResult Delete(short id)
        {
            var deleted = _categoryService.DeleteCategory(id);
            if (!deleted) return BadRequest("Cannot delete category. It may not exist or it is already used in a news article.");
            return NoContent();
        }
    }
}
