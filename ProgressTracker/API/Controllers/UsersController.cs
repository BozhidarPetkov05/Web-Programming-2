using System;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = users.FirstOrDefault(i => i.Id == id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            newUser.Id = users.Count + 1;
            users.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
    }
}
