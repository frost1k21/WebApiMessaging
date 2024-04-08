using WebApiMessaging.Dtos;

namespace WebApiMessaging.Services
{
    public interface IMessageService
    {
        Task AddMessages(IEnumerable<MessagePostDto> messagesPostDto, CancellationToken ct);
        Task<(MessageGetDto MessageGetDto, string Error)> GetMessage(int userId, CancellationToken ct);
    }
}