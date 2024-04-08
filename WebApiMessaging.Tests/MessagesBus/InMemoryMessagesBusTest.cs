using WebApiMessaging.MessagesBus;
using WebApiMessaging.Models;

namespace WebApiMessaging.Tests.MessagesBus
{
    public  class InMemoryMessagesBusTest
    {
        [Fact]
        public async Task ShouldStoreMessage()
        {
            var subject = "testSubject";
            var body = "testBody";
            var message = new Message
            {
                Subject = subject,
                Body = body,
                Recipients = new HashSet<int> { 1, 2, 3 },
            };
            var sut = new InMemoryMessagesBus();

            await sut.AddMessageToQueue(message);

            var resultOne = await sut.GetMessageForUser(1);
            var resultTwo = await sut.GetMessageForUser(2);
            var resultThree = await sut.GetMessageForUser(3);

            Assert.NotNull(resultOne);
            Assert.Equal(body, resultOne.Body);
            Assert.Equal(subject, resultOne.Subject);
            Assert.NotEqual(Guid.Empty, resultOne.MessageId);

            Assert.Equal(resultOne.Subject, resultTwo.Subject);
            Assert.Equal(resultOne.MessageId, resultThree.MessageId);
        }

        [Fact]
        public async Task ShouldReturnEmptyMessageForUser_WhenQueueEmpty()
        {
            var subject = "testSubject";
            var body = "testBody";
            var message = new Message
            {
                Subject = subject,
                Body = body,
                Recipients = new HashSet<int> { 1, 2, 3 },
            };
            var sut = new InMemoryMessagesBus();

            await sut.AddMessageToQueue(message);

            var _ = await sut.GetMessageForUser(1);
            var result = await sut.GetMessageForUser(1);
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.MessageId);
            Assert.True(result.IsEmptyForUser());
            Assert.Equal(1, result.Recipients.First());
        }

        [Fact]
        public async Task ShouldReturnEmptyMessage_WhenUserNotInQueue()
        {
            var sut = new InMemoryMessagesBus();

            var result = await sut.GetMessageForUser(1);

            Assert.True(result.IsEmpty());
        }
    }
}
