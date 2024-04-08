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
        public async Task<(MessageGetDto MessageGetDto, string Error)> GetMessage(int userId, CancellationToken ct)
        {
            var message = await _messagesBus.GetMessageForUser(userId, ct);
            if (message.IsEmpty())
            {
                return (new MessageGetDto(), $"User with id {userId} not found in message queue");
            }
            if (message.IsEmptyForUser())
            {
                return (new MessageGetDto(), $"There are no new messages");
            }

            var result = message.ToDto();
            return (result, "");
        }
    }
}
