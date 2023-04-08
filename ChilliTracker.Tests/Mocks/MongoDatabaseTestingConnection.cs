using ChilliTracker.Shared.Connection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Tests.Mocks
{
    internal class MongoDatabaseTestingConnection : IMongoDatabaseConnection
    {
        private string _connectionString = "";
        private string _databaseName = "";

        public MongoDatabaseTestingConnection()
        {
            _connectionString = "mongodb://localhost:27017";
            _databaseName = "ChilliTracker_Tests";
        }

        public MongoDatabaseTestingConnection(string databaseName, string connectionString)
        {

            _databaseName = databaseName;
            _connectionString = connectionString;
        }

        public IMongoDatabase GetDatabase(string databaseName = "")
        {
            var client = new MongoClient(_connectionString);
            return client.GetDatabase(String.IsNullOrEmpty(databaseName) ? _databaseName : databaseName);
        }

    }
}
