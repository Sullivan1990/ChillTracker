using ChilliTracker.Shared.Connection;
using ChilliTracker.Data.DataModels;
using MongoDB.Driver;
using ChilliTracker.Business.Repository;
using ChilliTracker.Tests.Mocks;
using MongoDB.Bson;

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
                TestingDatabase = "ChilliTracker_Tests"
            };
            _connection = new MongoDatabaseTestingConnection();
            _database = _connection.GetDatabase();
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");
        }

        [Test]
        public void GetAllWhereTwoEntriesInserted_ReturnsTwoEntries()
        {
            // Arrange
            ChilliPlantRepository repo = new ChilliPlantRepository(_connection);
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");

            // Ensure Deleted
            _plants.DeleteMany(Builders<ChilliPlant>.Filter.Empty);

            // Create Testing Data
            _plants.InsertMany(new List<ChilliPlant>

            {
                new ChilliPlant
                {
                    Identifier = "Chilli1",
                    Species = "Capsicum",
                    Variety = "Giant Red",
                    Planted = DateTime.UtcNow.AddDays(-30),
                    Germinated = DateTime.UtcNow.AddDays(-5),
                    IsGerminated = true,
                    UserID = "abc1234"
                },
                new ChilliPlant
                {
                    Identifier = "Chilli2",
                    Species = "Capsicum",
                    Variety = "Giant Yellow",
                    Planted = DateTime.UtcNow.AddDays(-27),
                    Germinated = DateTime.UtcNow.AddDays(-3),
                    IsGerminated = true,
                    UserID = "abc1234"
                }
            });

            // Act
            var result = repo.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetAllForUserWhereOneEntryForUserExists()
        {
            // Arrange
            ChilliPlantRepository repo = new ChilliPlantRepository(_connection);
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");

            // Ensure Deleted
            _plants.DeleteMany(Builders<ChilliPlant>.Filter.Empty);

            // Create Testing Data
            _plants.InsertMany(new List<ChilliPlant>

            {
                new ChilliPlant
                {
                    Identifier = "Chilli1",
                    Species = "Capsicum",
                    Variety = "Giant Red",
                    Planted = DateTime.UtcNow.AddDays(-30),
                    Germinated = DateTime.UtcNow.AddDays(-5),
                    IsGerminated = true,
                    UserID = "abc12345"
                },
                new ChilliPlant
                {
                    Identifier = "Chilli2",
                    Species = "Capsicum",
                    Variety = "Giant Yellow",
                    Planted = DateTime.UtcNow.AddDays(-27),
                    Germinated = DateTime.UtcNow.AddDays(-3),
                    IsGerminated = true,
                    UserID = "abc1234"
                }
            });

            // Act
            var result = repo.GetAllForUser("abc1234");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }
    }
}