using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Entities
{
    public class Message : TableEntity
    {
        public string Author { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
