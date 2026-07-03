using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RV_Assignment_1.Repositories;

namespace RV_Assignment_1.Controllers
{
    public class ODataPostsController : ODataController
    {
        private readonly IBulletinPostRepository _repo;
        public ODataPostsController(IBulletinPostRepository repo) => _repo = repo;

        // Bật tính năng OData đầy đủ: Select, Filter, OrderBy, Count...
        [EnableQuery]
        [HttpGet("odata/BulletinPosts")]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll().AsQueryable());
        }

    }
}
