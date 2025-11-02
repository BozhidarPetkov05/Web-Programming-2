using System;
using System.Linq;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Users;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            UserServices service = new UserServices();

            return Ok(service.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            UserServices service = new UserServices();

            return Ok(service.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserRequest model)
        {
            UserServices service = new UserServices();

            var item = new User
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            service.Save(item);

            return Ok(model);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]UserRequest model)
        {
            UserServices service = new UserServices();
            User forUpdate = service.GetById(id);
            if (forUpdate == null)
                throw new Exception("User not found");

            forUpdate.Username = model.Username;
            forUpdate.Password = model.Password;
            forUpdate.FirstName = model.FirstName;
            forUpdate.LastName = model.LastName;

            service.Save(forUpdate);

            return Ok(forUpdate);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            UserServices service = new UserServices();
            User forDelete = service.GetById(id);
            if (forDelete == null)
                throw new Exception("User not found");

            service.Delete(forDelete);

            return Ok(forDelete);
        }
    }
}
