using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Entities
{
    public class Message : TableEntity
    {
        public string Author { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
