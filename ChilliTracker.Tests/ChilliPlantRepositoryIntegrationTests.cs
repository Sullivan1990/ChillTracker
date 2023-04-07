using ChillTracker.Shared.Connection;
using ChillTracker.Data.DataModels;
using MongoDB.Driver;

namespace ChilliTracker.Tests
{
    public class Tests
    {
        IMongoDatabaseConnection _connection;
        IMongoDatabase _database;
        IMongoCollection<ChilliPlant> _plants;
        
        [SetUp]
        public void Setup()
        {
            var testingDBConnection = new Shared.Settings.MongoDBConnections
            {
                TestingServer = "mongodb://localhost:27017",
                TestingDatabase = "ChillTracker_Tests"
            };
            _connection = new MongoDatabaseConnection(testingDBConnection);
            _database = _connection.GetTestingDatabase();
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}