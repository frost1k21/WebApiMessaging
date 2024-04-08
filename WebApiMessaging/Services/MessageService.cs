using WebApiMessaging.Dtos;
using WebApiMessaging.Mappers;
using WebApiMessaging.MessagesBus;

namespace WebApiMessaging.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessagesBus _messagesBus;

        public MessageService(IMessagesBus messagesBus)
        {
            this._messagesBus = messagesBus;
        }

        public async Task AddMessages(IEnumerable<MessagePostDto> messagesPostDto, CancellationToken ct)
        {
            foreach (var messageToAdd in messagesPostDto)
            {
                var message = messageToAdd.ToModel();
                await _messagesBus.AddMessageToQueue(message, ct);
            }
        }
        public async Task<(List<MessageGetDto> MessageGetDtos, string Error)> GetMessages(int userId, int messagesNumber, CancellationToken ct)
        {
            var messages = await _messagesBus.GetMessagesForUser(userId, messagesNumber, ct);
            if (messages.First().IsEmpty())
            {
                return (new (), $"User with id {userId} not found in message queue");
            }
            if (messages.First().IsEmptyForUser())
            {
                return (new (), $"There are no new messages");
            }

            var result = messages.Select(m => m.ToDto()).ToList();
            return (result, "");
        }
    }
}
