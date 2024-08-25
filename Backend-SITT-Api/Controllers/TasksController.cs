using Backend_SITT_Api.Aplication.Common.Errors;
using Backend_SITT_Api.Aplication.Features.Task.Common.Add;
using Backend_SITT_Api.Aplication.Features.Task.Common.Delete;
using Backend_SITT_Api.Aplication.Features.Task.Common.Update;
using Backend_SITT_Api.Aplication.Features.Task.Query;
using Backend_SITT_Api.Aplication.Models.ViewModel;
using Backend_SITT_Api.Domain.Entityes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend_SITT_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(CodeErrorException), (int)HttpStatusCode.BadRequest)]
    [Authorize]
    public class TasksController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("TaskList")]
        [ProducesResponseType(typeof(ActionResult<IEnumerable<TasksVM>>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<TasksVM>>> ListTask()
         => Ok(await _mediator.Send(new ListTasksQuery()));

        [HttpPost("Add")]
        [ProducesResponseType(typeof(ActionResult<IEnumerable<bool>>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<bool>>> AddTask([FromBody] AddTasksCommon request)
         => Ok(await _mediator.Send(request));

        [HttpPut("Task")]
        [ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<bool>>> UpdateTask([FromQuery] int IdTask , [FromBody] UpdateTasksCommon request)
        {
            if( IdTask == request.Id)
            {
                return Ok(await _mediator.Send(request));
            }
            else
            {
                return BadRequest("Error to data");
            }

        }

        [HttpDelete()]
        [ProducesResponseType(typeof(ActionResult<IEnumerable<bool>>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<bool>>> deleteTask([FromQuery] DeleteTaskCommon request)
         => Ok(await _mediator.Send(request));

    }
}
