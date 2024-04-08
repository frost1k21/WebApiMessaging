using WebApiMessaging.Dtos;

namespace WebApiMessaging.Services
{
    public interface IMessageService
    {
        Task AddMessage(MessagePostDto messagePostDto, CancellationToken ct);
        Task<(MessageGetDto MessageGetDto, string Error)> GetMessage(int userId, CancellationToken ct);
    }
}