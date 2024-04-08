using WebApiMessaging.Dtos;

namespace WebApiMessaging.Services
{
    public interface IMessageService
    {
        Task AddMessages(IEnumerable<MessagePostDto> messagesPostDto, CancellationToken ct);
        Task<(List<MessageGetDto> MessageGetDtos, string Error)> GetMessages(int userId, int messagesNumber, CancellationToken ct);
    }
}