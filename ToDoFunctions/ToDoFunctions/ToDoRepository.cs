using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoFunctions
{
    public class ToDoRepository
    {
        private readonly CloudTable _table;

        public ToDoRepository()
        {                                    
            var connectionString = ConfigurationManager.AppSettings["TableConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference("Items");
            _table.CreateIfNotExists();            
        }

        public List<ToDo> GetAll()
        {
            return _table.ExecuteQuery(new TableQuery<ToDo>()).ToList();
        }

        public List<ToDo> List()
        {
            return _table.ExecuteQuery(new TableQuery<ToDo>()).Where(x => !x.DeletedOn.HasValue).ToList();
        }

        public ToDo GetById(int id)
        {
            return _table
                .ExecuteQuery(new TableQuery<ToDo>())
                .FirstOrDefault(x =>
                    x.PartitionKey == "todo" &&
                    x.UniqueID == id &&
                    (x.DeletedOn == null || x.DeletedOn == DateTime.MinValue));
        }

        public void Insert(ToDo item)
        {
            item.PartitionKey = "todo";
            item.CreatedOn = DateTime.Now;
            item.UniqueID = GetUniqueID();
            item.UpdatedOn = null;
            item.CompletedOn = null;
            item.DeletedOn = null;
            item.RowKey = item.UniqueID.ToString();

            _table.Execute(TableOperation.Insert(item));
        }

        public bool Update(ToDo item)
        {
            var dbItem = GetById(item.UniqueID);

            if (dbItem == null)
            {
                return false;
            }

            dbItem.Title = item.Title;
            dbItem.Description = item.Description;
            dbItem.UpdatedOn = DateTime.Now;

            _table.Execute(TableOperation.Merge(dbItem));
            return true;
        }

        public bool MarkAsCompleted(int id)
        {
            var dbItem = GetById(id);

            if (dbItem == null)
            {
                return false;
            }

            dbItem.CompletedOn = DateTime.Now;
            _table.Execute(TableOperation.Merge(dbItem));

            return true;
        }

        public bool MarkAsDeleted(int id)
        {
            var dbItem = GetById(id);

            if (dbItem == null)
            {
                return false;
            }

            dbItem.DeletedOn = DateTime.Now;
            _table.Execute(TableOperation.Merge(dbItem));

            return true;
        }

        private int GetUniqueID()
        {
            var query = _table.ExecuteQuery(new TableQuery<ToDo>());
            return query.Any() ? query.Max(x => x.UniqueID) + 1 : 1;
        }
    }
}
