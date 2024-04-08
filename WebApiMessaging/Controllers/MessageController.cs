using Microsoft.AspNetCore.Mvc;
using WebApiMessaging.Dtos;
using WebApiMessaging.Services;

namespace WebApiMessaging.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetMessage([FromQuery]int rcpt, CancellationToken ct)
        {
            var result = await _messageService.GetMessage(rcpt, ct);
            if (!string.IsNullOrEmpty(result.Error))
            {
                return NotFound(result.Error);
            }
            return Ok(result.MessageGetDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] MessagePostDto messagePostDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _messageService.AddMessage(messagePostDto, ct);
            return CreatedAtAction(nameof(GetMessage), new { rcpt = messagePostDto.Recipients.First() }, null);
        }
    }
}
