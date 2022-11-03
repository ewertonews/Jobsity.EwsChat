using System.Net.Mime;
using Jobsity.EwsChat.Server.Services;
using Jobsity.EwsChat.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jobsity.EwsChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<string>> Get([FromQuery] string roomId)
        {
            var stringMessages = await _messagesService.GetMessages(roomId);
            if (string.IsNullOrEmpty(stringMessages))
            {
                return NotFound();
            }

            return Ok(stringMessages);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> Post(Message message)
        {
            var addMessageSucceeded = await _messagesService.AddMessage(message);
            if (!addMessageSucceeded)
            {
                return BadRequest();
            }

            //normally it would be: CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            //but it's not needed in this context.
            return new ObjectResult(true) { StatusCode = StatusCodes.Status201Created };
        }

    }
}
