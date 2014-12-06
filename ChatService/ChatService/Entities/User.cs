using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Entities
{
    public class User : TableEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastCountryCode { get; set; }
        public string LastCity { get; set; }
    }
}
