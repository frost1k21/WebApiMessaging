using WebApiMessaging.Models;

namespace WebApiMessaging.MessagesBus
{
    public interface IMessagesBus
    {
        Task AddMessageToQueue(Message message, CancellationToken ct);
        Task<Message> GetMessageForUser(int userId, CancellationToken ct);
    }
}