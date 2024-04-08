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

            var resultOne = await sut.GetMessagesForUser(1, 1);
            var resultTwo = await sut.GetMessagesForUser(2, 1);
            var resultThree = await sut.GetMessagesForUser(3, 1);

            Assert.NotNull(resultOne);
            Assert.Equal(body, resultOne.First().Body);
            Assert.Equal(subject, resultOne.First().Subject);
            Assert.NotEqual(Guid.Empty, resultOne.First().MessageId);

            Assert.Equal(resultOne.First().Subject, resultTwo.First().Subject);
            Assert.Equal(resultOne.First().MessageId, resultThree.First().MessageId);
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

            var _ = await sut.GetMessagesForUser(1, 1);
            var result = await sut.GetMessagesForUser(1, 1);
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.First().MessageId);
            Assert.True(result.First().IsEmptyForUser());
            Assert.Equal(1, result.First().Recipients.First());
        }

        [Fact]
        public async Task ShouldReturnEmptyMessage_WhenUserNotInQueue()
        {
            var sut = new InMemoryMessagesBus();

            var result = await sut.GetMessagesForUser(1, 1);

            Assert.True(result.First().IsEmpty());
        }
    }
}
