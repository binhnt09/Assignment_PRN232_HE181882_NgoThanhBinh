using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RV_Assignment_1.Repositories;
using RV_Assignment_1.Models;

namespace RV_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class BulletinCategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        public BulletinCategoriesController(ICategoryRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult Get() => Ok(_repo.GetAll());

        [HttpPost]
        public IActionResult Post([FromBody] BulletinCategory category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _repo.Add(category);
            return CreatedAtAction(nameof(Get), new { id = category.CategoryId }, category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _repo.GetById(id);
            if (category == null) return NotFound("Không tìm thấy danh mục!");

            // THỰC THI NGHIỆP VỤ CHẶN XÓA: Kiểm tra xem danh mục đã dính với bài viết nào chưa
            if (_repo.HasPosts(id))
            {
                return BadRequest("Không thể xóa danh mục này vì nó đã chứa bài viết thông báo!");
            }

            _repo.Delete(id);
            return Ok(new { Message = "Xóa danh mục thành công!" });
        }

    }
}
