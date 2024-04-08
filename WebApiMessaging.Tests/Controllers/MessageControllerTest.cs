using Microsoft.AspNetCore.Mvc;
using WebApiMessaging.Controllers;
using WebApiMessaging.Dtos;
using WebApiMessaging.MessagesBus;
using WebApiMessaging.Services;

namespace WebApiMessaging.Tests.Controllers
{
    public class MessageControllerTest
    {
        [Fact]
        public async Task ShouldReturnMessage()
        {
            var messageDto = new MessagePostDto
            {
                Subject = "testSubject",
                Body = "testBody",
                Recipients = new int[] { 1, 2 }
            };
            var messageBus = new InMemoryMessagesBus();
            var messageService = new MessageService(messageBus);
            var sut = new MessageController(messageService);
            

            var _ = await sut.SendMessage(messageDto, default);
            var result = await sut.GetMessage(1, default);

            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<OkObjectResult>(actionResult);

            var messageFromResponse = ((OkObjectResult)actionResult).Value as MessageGetDto;
            Assert.Equal(messageDto.Subject, messageFromResponse!.Subject);
            Assert.Equal(messageDto.Body, messageFromResponse!.Body);
        }
    }
}
