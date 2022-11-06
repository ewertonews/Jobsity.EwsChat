using Jobsity.EwsChat.Server.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jobsity.EwsChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public MessagesController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> Get(string message)
        {
            
            await _chatBotService.ProcessSpecialMessage(message);
            return Ok(true);
        }

    }
}
