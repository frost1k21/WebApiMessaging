using WebApiMessaging.Dtos;
using WebApiMessaging.Models;

namespace WebApiMessaging.Mappers
{
    public static class MessageMapper
    {
        public static MessageGetDto ToDto(this Message message) 
        {
            return new MessageGetDto
            {
                Subject = message.Subject,
                Body = message.Body,
            };
        }

        public static Message ToModel(this MessagePostDto messagePostDto)
        {
            return new Message
            {
                Subject = messagePostDto.Subject,
                Body = messagePostDto.Body,
                Recipients = new HashSet<int>(messagePostDto.Recipients),
            };
        }
    }
}
