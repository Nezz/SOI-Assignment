using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Entities;
using ChatService.ServiceReference1;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Repository
{
    public partial class TableStorageRepo
    {
        private CloudTable messagesTable;

        protected CloudTable MessagesTable
        {
            get
            {
                if (messagesTable == null)
                    messagesTable = TableClient.GetTableReference("messages");

                return messagesTable;
            }
        }

        public async Task<Message> CreateMessage(string author, string text, GeoLocation geoLocation)
        {
            var entity = new Message
            {
                Author = author,
                City = geoLocation.City,
                Country = geoLocation.Country,
                CreatedDate = DateTime.UtcNow,
                Latitude = geoLocation.Latitude,
                Longitude = geoLocation.Longitude,
                PartitionKey = geoLocation.CountryCode,
                RowKey = Guid.NewGuid().ToString(),
                Text = text
            };
            
            var op = TableOperation.InsertOrReplace(entity);
            await MessagesTable.ExecuteAsync(op);
            return entity;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            var query = MessagesTable.CreateQuery<Message>();
            var result = await Task.Run(() => query.ToList());
            return result;
        }

        public async Task<List<Message>> GetCountryMessages(string countryCode)
        {
            var query = MessagesTable.CreateQuery<Message>().Where(message => message.PartitionKey == countryCode);
            var result = await Task.Run(() => query.ToList());
            return result;
        }

        public async Task<List<Message>> GetCityMessages(string countryCode, string city)
        {
            var query = MessagesTable.CreateQuery<Message>().Where(message => message.PartitionKey == countryCode && message.City == city);
            var result = await Task.Run(() => query.ToList());
            return result;
        }

        public async Task<Message> GetMessage(string countryCode, string rowKey)
        {
            var op = TableOperation.Retrieve<Message>(countryCode, rowKey);
            var result = await MessagesTable.ExecuteAsync(op);
            return result.Result as Message;
        }
    }
}
