using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Repository
{
    public partial class TableStorageRepo
    {
        private CloudTableClient tableClient;

        protected CloudTableClient TableClient
        {
            get
            {
                if (tableClient == null)
                {
                    var cred = new StorageCredentials(Settings.AccountName, Settings.AccessKey);
                    var account = new CloudStorageAccount(cred, false);
                    tableClient = account.CreateCloudTableClient();
                }

                return tableClient;
            }
        }

        private async Task UpdateProperties(TableEntity entity, Dictionary<string, EntityProperty> propertiesToUpdate,
            CloudTable table)
        {
            DynamicTableEntity updatedEntity = new DynamicTableEntity(
                entity.PartitionKey, entity.RowKey, entity.ETag, propertiesToUpdate);
            var op = TableOperation.Merge(updatedEntity);
            var result = await table.ExecuteAsync(op);

        }

        private async Task UpdateSingleProperty<T, TField>(T entity, Expression<Func<T, TField>> property, TField value,
            CloudTable table)
            where T : TableEntity
        {
            string propertyName = RepositoryHelper.GetName(property);
            EntityProperty entityProp = CreateEntityProperty(value);

            await UpdateProperties(
                entity,
                new Dictionary<string, EntityProperty> { { propertyName, entityProp } },
                table);
        }

        private EntityProperty CreateEntityProperty(object value)
        {
            if (value is int)
                return new EntityProperty((int)value);

            if (value is string)
                return new EntityProperty((string)value);

            if (value is Guid)
                return new EntityProperty((Guid)value);

            if (value is double)
                return new EntityProperty((double)value);

            throw new NotImplementedException();
        }
    }
}
