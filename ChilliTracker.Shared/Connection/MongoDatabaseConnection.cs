using ChilliTracker.Shared.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Shared.Connection
{
    public class MongoDatabaseConnection : IMongoDatabaseConnection
    {
        private readonly MongoDBConnections _connections;
        public MongoDatabaseConnection(MongoDBConnections connections)
        {
            _connections = connections;
        }
        public IMongoDatabase GetDatabase(string databaseName = "")
        {
            var client = new MongoClient(_connections.ProductionServer);
            return client.GetDatabase(String.IsNullOrEmpty(databaseName) ? _connections.ProductionDatabase : databaseName);
        }

    }
}
