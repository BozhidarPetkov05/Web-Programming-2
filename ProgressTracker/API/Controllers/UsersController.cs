using System;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            UsersServices service = new UsersServices();
            return Ok(service.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            UsersServices service = new UsersServices();
            service.Save(user);
            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            UsersServices service = new UsersServices();
            User forDelete = service.GetById(id);

            if (forDelete == null)
            {
                throw new Exception("User not found!");
            }

            service.Delete(forDelete);
            return Ok(forDelete);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User data)
        {
            UsersServices service = new UsersServices();
            User forUpdate = service.GetById(id);

            if (forUpdate == null)
            {
                throw new Exception("User not found!");
            }

            forUpdate.Username = data.Username;
            forUpdate.Password = data.Password;
            forUpdate.FirstName = data.FirstName;
            forUpdate.LastName = data.LastName;

            service.Save(forUpdate);
            return Ok(forUpdate);
        }
    }
}
