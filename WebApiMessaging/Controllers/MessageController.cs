﻿using Microsoft.AspNetCore.Mvc;
using WebApiMessaging.Dtos;
using WebApiMessaging.Services;
using WebApiMessaging.ValidationAttributes;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageGetDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetMessages([FromQuery]int rcpt, int messageNumbers = 1, CancellationToken ct = default)
        {
            var result = await _messageService.GetMessages(rcpt, messageNumbers, ct);
            if (!string.IsNullOrEmpty(result.Error))
            {
                return NotFound(result.Error);
            }
            return Ok(result.MessageGetDtos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] MessagesPost messagesPostDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _messageService.AddMessages(messagesPostDto.Messages, ct);
            return CreatedAtAction(nameof(GetMessages), new { rcpt = messagesPostDto.Messages.First().Recipients.First() }, null);
        }
    }
}
