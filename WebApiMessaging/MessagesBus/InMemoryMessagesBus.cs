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

        public Task<List<Message>> GetMessagesForUser(int userId, int messagesNumber = 1, CancellationToken ct = default)
        {
            var messages = new List<Message>();
            if (!_usersMessageQueues.ContainsKey(userId))
            {
                messages.Add(Message.Empty);
                return Task.FromResult(messages);
            }

            if (!(_usersMessageQueues[userId].Count > 0))
            {
                var emptyMessageForUser = Message.Empty;
                emptyMessageForUser.Recipients.Add(userId);
                messages.Add(emptyMessageForUser);
                return Task.FromResult(messages);
            }
            
            for (int i = 1; i <= messagesNumber; i++)
            {
                var messageId = _usersMessageQueues[userId].Dequeue();
                var message = _messages[messageId];
                message.Recipients.Remove(userId);
                if (message.Recipients.Count == 0)
                {
                    _messages.Remove(messageId);
                }
                messages.Add(message);
                if ((_usersMessageQueues[userId].Count == 0))
                {
                    break;
                }
            }
            return Task.FromResult(messages);
        }
    }
}
