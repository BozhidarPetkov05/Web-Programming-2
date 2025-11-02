using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Infrastructure.RequestDTOs.Users;
using Common.Entities;
using Common.Services;
using FluentValidation;
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
        private IValidator<User> _validator;
        public UsersController(IValidator<User> validator) 
        {
            // Inject our validator and also a DB context for storing our person object.
            _validator = validator;
        }
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
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]UserRequestFluent model)
        {
            UserServices service = new UserServices();
            User forUpdate = service.GetById(id);
            if (forUpdate == null)
                throw new Exception("User not found");

            ValidationResult result = _validator.Validate(model);

            // forUpdate.Username = model.Username;
            // forUpdate.Password = model.Password;
            // forUpdate.FirstName = model.FirstName;
            // forUpdate.LastName = model.LastName;

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
