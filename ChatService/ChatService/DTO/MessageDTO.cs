using System;

namespace ChatService.DTO
{
    public class MessageDTO
    {
        public string Author { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
