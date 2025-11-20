using System;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Projects;
using API.Infrastructure.RequestDTOs.Shared;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromBody] ProjectsGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                    ? 10
                                    : model.Pager.PageSize;
            model.OrderBy ??= "Id";
            model.OrderBy = typeof(Project).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : "Id";
                                
            model.Filter ??= new ProjectsGetFilterRequest();

            ProjectServices service = new ProjectServices();

            int loggedUserId =
                Convert.ToInt32(
                    this.HttpContext.User.FindFirstValue("loggedUserId")
                );

            Expression<Func<Project, bool>> filter =
            u =>
                (u.OwnerId == loggedUserId) &&
                (string.IsNullOrEmpty(model.Filter.Title) || u.Title.Contains(model.Filter.Title)) &&
                (string.IsNullOrEmpty(model.Filter.Description) || u.Description.Contains(model.Filter.Description)) &&
                (model.Filter.OwnerId == null || u.OwnerId == model.Filter.OwnerId);

            return Ok(service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            ProjectServices service = new ProjectServices();

            return Ok(service.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProjectRequest model)
        {
            ProjectServices service = new ProjectServices();

            string loggedUserId =
            this.HttpContext.User.FindFirst("loggedUserId").Value;
            
            var item = new Project()
            {
                OwnerId = Convert.ToInt32(loggedUserId),
                Title = model.Title,
                Description = model.Description
            };

            service.Save(item);

            return Ok(model);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]ProjectRequest model)
        {
            ProjectServices service = new ProjectServices();
            Project forUpdate = service.GetById(id);
            if (forUpdate == null)
                throw new Exception("Project not found");

            forUpdate.Title = model.Title;
            forUpdate.Description = model.Description;

            service.Save(forUpdate);

            return Ok(forUpdate);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            ProjectServices service = new ProjectServices();
            Project forDelete = service.GetById(id);
            if (forDelete == null)
                throw new Exception("Project not found");

            service.Delete(forDelete);

            return Ok(forDelete);
        }

        // /api/projects/2/addmember?userId=5
        [Route("{projectId}/addmember")]
        [HttpGet]
        public IActionResult AddMember([FromRoute]int projectId, [FromQuery]int userId)
        {
            return Ok("ProjectsController is working");
        }
    }
}
