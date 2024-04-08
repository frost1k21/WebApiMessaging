using System.ComponentModel.DataAnnotations;
using WebApiMessaging.ValidationAttributes;

namespace WebApiMessaging.Dtos
{
    public class MessagePostDto
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        [Required]
        [NotNullOrEmpty(ErrorMessage = "Recipients must be not empty")]
        public int[] Recipients { get; set; }
    }
}
