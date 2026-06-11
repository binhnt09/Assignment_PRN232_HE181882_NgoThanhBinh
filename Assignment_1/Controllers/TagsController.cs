using Assignment_1.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment_1.Controllers
{
    public class TagsController : ODataController
    {
        private readonly ITagService _service;
        public TagsController(ITagService service) => _service = service;

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }
    }
}
