using Assignment_2_BE.Models;
using Assignment_2_BE.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment_2_BE.Controllers
{
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleService _service;

        public NewsArticlesController(INewsArticleService service)
        {
            _service = service;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] string key)
        {
            var news = _service.GetById(key);
            if (news == null) return NotFound();
            return Ok(news);
        }

        public IActionResult Post([FromBody] NewsArticle news)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _service.Add(news);
            return Created(news);
        }

        public IActionResult Put([FromODataUri] string key, [FromBody] NewsArticle news)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != news.NewsArticleId) return BadRequest("Khóa chính không khớp.");

            _service.Update(news);
            return Updated(news);
        }

        public IActionResult Delete([FromODataUri] string key)
        {
            bool isDeleted = _service.Delete(key);
            if (!isDeleted) return NotFound();
            return NoContent();
        }
        [HttpGet("odata/NewsArticles/Report")]
        public IActionResult GetReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }

            // Lấy bài viết trong khoảng thời gian và sắp xếp giảm dần (descending)
            var reportData = _service.GetAll()
                .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return Ok(reportData);
        }
    }
}
