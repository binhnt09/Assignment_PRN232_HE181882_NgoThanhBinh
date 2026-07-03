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
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _accountService;

        public SystemAccountsController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "0")] // Admin role
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_accountService.GetAllAccounts());
        }

        [Authorize(Roles = "0")]
        [HttpGet("{id}")]
        public IActionResult GetAccount(short id)
        {
            var account = _accountService.GetAccountById(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [Authorize(Roles = "0")]
        [HttpPost]
        public IActionResult Post([FromBody] SystemAccount account)
        {
            var created = _accountService.CreateAccount(account);
            return CreatedAtAction(nameof(GetAccount), new { id = created.AccountId }, created);
        }

        [Authorize(Roles = "0")]
        [HttpPut("{id}")]
        public IActionResult Put(short id, [FromBody] SystemAccount account)
        {
            var updated = _accountService.UpdateAccount(id, account);
            if (!updated) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "0")]
        [HttpDelete("{id}")]
        public IActionResult Delete(short id)
        {
            var deleted = _accountService.DeleteAccount(id);
            if (!deleted) return BadRequest("Cannot delete account. It may not exist or it has already created news articles.");
            return NoContent();
        }
    }
}
