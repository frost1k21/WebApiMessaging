using WebApiMessaging.Models;

namespace WebApiMessaging.MessagesBus
{
    public interface IMessagesBus
    {
        Task AddMessageToQueue(Message message, CancellationToken ct);
        Task<List<Message>> GetMessagesForUser(int userId, int messagesNumber, CancellationToken ct);
    }
}