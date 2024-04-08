using WebApiMessaging.Dtos;
using WebApiMessaging.MessagesBus;
using WebApiMessaging.Services;

namespace WebApiMessaging.Tests.Services
{
    public class MessageServiceTest
    {
        [Fact]
        public async Task ShouldReturnMessageGetDto()
        {
            var subject = "testSubject";
            var body = "testBody";
            var recipients = new int[] { 1, 2, 3 };

            var messageBus = new InMemoryMessagesBus();
            var sut = new MessageService(messageBus);

            var messagePostDto = new MessagePostDto
            {
                Subject = subject,
                Body = body,
                Recipients = recipients
            };

            await sut.AddMessage(messagePostDto, default);
            var result = await sut.GetMessage(1, default);
            Assert.Equal(subject, result.MessageGetDto.Subject);
            Assert.Equal(body, result.MessageGetDto.Body);
        }

        [Fact]
        public async Task ShouldReturnError_WhenEmptyMessageQueueForUser()
        {
            var subject = "testSubject";
            var body = "testBody";
            var recipients = new int[] { 1, 2, 3 };

            var messageBus = new InMemoryMessagesBus();
            var sut = new MessageService(messageBus);

            var messagePostDto = new MessagePostDto
            {
                Subject = subject,
                Body = body,
                Recipients = recipients
            };

            await sut.AddMessage(messagePostDto, default);
            var _ = await sut.GetMessage(1, default);
            var result = await sut.GetMessage(1, default);

            Assert.Equal("There are no new messages", result.Error);
        }

        [Fact]
        public async Task ShouldReturnError_WhenUserNotExistsInQueue()
        {
            var userId = 1;
            var messageBus = new InMemoryMessagesBus();
            var sut = new MessageService(messageBus);

            var result = await sut.GetMessage(userId, default);

            Assert.Equal($"User with id {userId} not found in message queue", result.Error);
        }
    }
}
