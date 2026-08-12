using BuildFlow.Application.Features.Projects.CreateProject;
using BuildFlow.Application.Features.Projects.DeleteProject;
using BuildFlow.Application.Features.Projects.GetProjectById;
using BuildFlow.Application.Features.Projects.GetProjects;
using BuildFlow.Application.Features.Projects.UpdateProject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/projects
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProjectRequest request)
        {
            var command = new CreateProjectCommand(request);

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET: api/projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetProjectsQuery();

            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET: api/projects/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProjectByIdQuery(id);

            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // PUT: api/projects/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProjectRequest request)
        {
            var command = new UpdateProjectCommand(id, request);

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // DELETE: api/projects/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProjectCommand(id);

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
