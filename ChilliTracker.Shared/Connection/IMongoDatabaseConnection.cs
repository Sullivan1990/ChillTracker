using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillTracker.API.Connection
{
    public interface IMongoDatabaseConnection
    {
        IMongoDatabase GetProductionDatabase(string databaseName = "");
        IMongoDatabase GetTestingDatabase(string databaseName = "");
    }
}
