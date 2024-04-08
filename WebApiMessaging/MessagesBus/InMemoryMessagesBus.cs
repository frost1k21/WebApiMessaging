using WebApiMessaging.Models;

namespace WebApiMessaging.MessagesBus
{
    public class InMemoryMessagesBus : IMessagesBus
    {
        private readonly Dictionary<Guid, Message> _messages = new();
        private readonly Dictionary<int, Queue<Guid>> _usersMessageQueues = new();

        public Task AddMessageToQueue(Message message, CancellationToken ct = default)
        {
            var messageId = Guid.NewGuid();
            message.MessageId = messageId;
            _messages[messageId] = message;
            foreach (var userId in message.Recipients)
            {
                if (!(_usersMessageQueues.ContainsKey(userId)))
                {
                    _usersMessageQueues[userId] = new Queue<Guid>();
                }
                _usersMessageQueues[userId].Enqueue(messageId);
            }
            return Task.CompletedTask;
        }

        public Task<Message> GetMessageForUser(int userId, CancellationToken ct = default)
        {
            if (!_usersMessageQueues.ContainsKey(userId))
            {
                return Task.FromResult(Message.Empty);
            }

            if (!(_usersMessageQueues[userId].Count > 0))
            {
                var emptyMessageForUser = Message.Empty;
                emptyMessageForUser.Recipients.Add(userId);
                return Task.FromResult(emptyMessageForUser);
            }

            var messageId = _usersMessageQueues[userId].Dequeue();
            var message = _messages[messageId];
            message.Recipients.Remove(userId);
            if(message.Recipients.Count == 0)
            {
                _messages.Remove(messageId);
            }
            return Task.FromResult(message);
        }
    }
}
