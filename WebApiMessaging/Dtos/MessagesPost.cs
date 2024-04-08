using System.ComponentModel.DataAnnotations;
using WebApiMessaging.ValidationAttributes;

namespace WebApiMessaging.Dtos
{
    public class MessagesPost
    {
        [Required]
        [NotNullOrEmpty(ErrorMessage = "Messages must be not empty")]
        public MessagePostDto[] Messages { get; set; }
    }
}
