namespace WebApiMessaging.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HashSet<int> Recipients { get; set; } = new ();
        public static Message Empty
        {
            get
            {
                return new Message() { MessageId = Guid.Empty };
            }
        }

        public bool IsEmpty()
            => Guid.Empty == MessageId && Recipients.Count == 0;

        public bool IsEmptyForUser()
            => Guid.Empty == MessageId && Recipients.Count == 1;

    }
}
