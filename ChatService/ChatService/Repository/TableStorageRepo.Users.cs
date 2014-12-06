using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Repository
{
    public partial class TableStorageRepo
    {
        private const string UserPartitionKey = "user";

        private CloudTable usersTable;

        protected CloudTable UsersTable
        {
            get
            {
                if (usersTable == null)
                    usersTable = TableClient.GetTableReference("users");

                return usersTable;
            }
        }

        public async Task<User> CreateUser(string name)
        {
            var entity = new User
            {
                Name = name,
                CreatedDate = DateTime.UtcNow,
                PartitionKey = UserPartitionKey,
                RowKey = Guid.NewGuid().ToString()
            };
            
            var op = TableOperation.InsertOrReplace(entity);
            await UsersTable.ExecuteAsync(op);
            return entity;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var query = UsersTable.CreateQuery<User>();
            var result = await Task.Run(() => query.ToList());
            return result;
        }

        public async Task<User> GetUser(string rowKey)
        {
            var op = TableOperation.Retrieve<User>(UserPartitionKey, rowKey);
            var result = await UsersTable.ExecuteAsync(op);
            return result.Result as User;
        }
    }
}
