using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RV_Assignment_1.DTOs;
using RV_Assignment_1.Models;
using RV_Assignment_1.Repositories;
using RV_Assignment_1.service;

namespace RV_Assignment_1.Controllers
{
    //[ApiController]
    //[Route("api/bulletinposts")]
    public class BulletinPostsController : ODataController
    {
        //private readonly IBulletinPostRepository _repo;
        private readonly IBulletinPostService _service;
        public BulletinPostsController(IBulletinPostService service) => _service = service;
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var post = _service.GetById(key);
            if (post == null) return NotFound();
            return Ok(post);
        }
        public IActionResult Post([FromBody] BulletinPost post)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _service.Add(post);
            return Created(post);
        }

        public IActionResult Put([FromODataUri] int key, [FromBody] BulletinPost post)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != post.PostId) return BadRequest("Khóa chính không khớp.");

            _service.Update(post);
            return Updated(post);
        }

        public IActionResult Delete([FromODataUri] string key)
        {
            bool isDeleted = _service.Delete(key);
            if (!isDeleted) return NotFound();
            return NoContent();
        }
        //[HttpGet("odata/NewsArticles/Report")]
        //public IActionResult GetReport(DateTime startDate, DateTime endDate)
        //{
        //    if (startDate > endDate)
        //    {
        //        return BadRequest("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
        //    }

        //    // Lấy bài viết trong khoảng thời gian và sắp xếp giảm dần (descending)
        //    var reportData = _service.GetAll()
        //        .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
        //        .OrderByDescending(x => x.CreatedDate)
        //        .ToList();

        //    return Ok(reportData);
        //}

        //[HttpPost]
        //public IActionResult Post([FromBody] PostCreateDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var post = new BulletinPost
        //    {
        //        Title = dto.Title,
        //        Summary = dto.Summary,
        //        Content = dto.Content,
        //        CreatedDate = DateTime.Now, // Gán ngày hiện tại tự động theo chuẩn nghiệp vụ
        //        Source = dto.Source,
        //        Status = dto.Status,
        //        CategoryId = dto.CategoryId,
        //        CreatedById = dto.CreatedById
        //    };

        //    _repo.Add(post);
        //    return StatusCode(201, post);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] PostCreateDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var existingPost = _repo.GetById(id);
        //    if (existingPost == null) return NotFound("Không tìm thấy bài đăng!");

        //    existingPost.Title = dto.Title;
        //    existingPost.Summary = dto.Summary;
        //    existingPost.Content = dto.Content;
        //    existingPost.Source = dto.Source;
        //    existingPost.Status = dto.Status;
        //    existingPost.CategoryId = dto.CategoryId;

        //    _repo.Update(existingPost);
        //    return Ok(existingPost);
        //}
    }
}
