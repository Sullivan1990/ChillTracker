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

        // Add Potting Event to Chilli
        [Test]
        public void AddSinglePottingEventToChilliAddNewItemToArray()
        {
            // Arrange
            ChilliPlantRepository repo = new ChilliPlantRepository(_connection);
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");

            // Ensure Deleted
            _plants.DeleteMany(Builders<ChilliPlant>.Filter.Empty);

            // Create Testing Data
            _plants.InsertOne(
                new ChilliPlant
                {
                    Identifier = "Chilli1",
                    Species = "Capsicum",
                    Variety = "Giant Red",
                    Planted = DateTime.UtcNow.AddDays(-30),
                    Germinated = DateTime.UtcNow.AddDays(-5),
                    IsGerminated = true,
                    UserID = "abc12345"
                });

            var pottingEvent = new PlantPottingEvent
            {
                PottingEventID = ObjectId.GenerateNewId().ToString(),
                Conditions = "cold week",
                Container = "100 mm pot",
                Date = DateTime.UtcNow,
                Medium = "Searles Potting Mix",
                Notes = ""
            };

            // retrieve created chilli (for the _id)
            var insertedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();
            
            // Act
            repo.AddPottingEventToChilli(insertedChilliPlant._id.ToString(), pottingEvent, insertedChilliPlant.UserID);

            // Assert
            var updatedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();

            Assert.That(updatedChilliPlant.PlantPottingEvents.Count == 1);
            Assert.That(updatedChilliPlant.PlantPottingEvents.First().Container == "100 mm pot");
        }

        // Add Potting Event to Chilli
        [Test]
        public void AddMultiplePottingEventToChilliAddsNewItemsToArrayInOrder()
        {
            // Arrange
            ChilliPlantRepository repo = new ChilliPlantRepository(_connection);
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");

            // Ensure Deleted
            _plants.DeleteMany(Builders<ChilliPlant>.Filter.Empty);

            // Create Testing Data
            _plants.InsertOne(
                new ChilliPlant
                {
                    Identifier = "Chilli1",
                    Species = "Capsicum",
                    Variety = "Giant Red",
                    Planted = DateTime.UtcNow.AddDays(-30),
                    Germinated = DateTime.UtcNow.AddDays(-5),
                    IsGerminated = true,
                    UserID = "abc12345"
                });

            var pottingEvent1 = new PlantPottingEvent
            {
                PottingEventID = ObjectId.GenerateNewId().ToString(),
                Conditions = "cold week",
                Container = "100 mm pot",
                Date = DateTime.UtcNow,
                Medium = "Searles Potting Mix",
                Notes = ""
            };

            var pottingEvent2 = new PlantPottingEvent
            {
                PottingEventID = ObjectId.GenerateNewId().ToString(),
                Conditions = "warm week",
                Container = "120 mm pot",
                Date = DateTime.UtcNow,
                Medium = "50% Vermiculite, 50% Peat Moss",
                Notes = ""
            };

            // retrieve created chilli (for the _id)
            var insertedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();

            // Act
            repo.AddPottingEventToChilli(insertedChilliPlant._id.ToString(), pottingEvent1, insertedChilliPlant.UserID);
            repo.AddPottingEventToChilli(insertedChilliPlant._id.ToString(), pottingEvent2, insertedChilliPlant.UserID);

            // Assert
            var updatedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();

            Assert.That(updatedChilliPlant.PlantPottingEvents.Count == 2);
            Assert.That(updatedChilliPlant.PlantPottingEvents.First().Container == "100 mm pot");
            Assert.That(updatedChilliPlant.PlantPottingEvents[1].Container == "120 mm pot");
        }

        [Test]
        public void AddSingleHarvestEventToChilliAddsNewItemToArray()
        {
            // Arrange
            ChilliPlantRepository repo = new ChilliPlantRepository(_connection);
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");

            // Ensure Deleted
            _plants.DeleteMany(Builders<ChilliPlant>.Filter.Empty);

            // Create Testing Data
            _plants.InsertOne(
                new ChilliPlant
                {
                    Identifier = "Chilli1",
                    Species = "Capsicum",
                    Variety = "Giant Red",
                    Planted = DateTime.UtcNow.AddDays(-30),
                    Germinated = DateTime.UtcNow.AddDays(-5),
                    IsGerminated = true,
                    UserID = "abc12345"
                });

            var harvestEvent = new HarvestEvent
            {
                HarvestEventID = ObjectId.GenerateNewId().ToString(),
                Quantity = 50,
                WeightGrams = 235,
                Date = DateTime.UtcNow,
                Notes = ""
            };

            // retrieve created chilli (for the _id)
            var insertedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();

            // Act
            repo.AddHarvestEventToChilli(insertedChilliPlant._id.ToString(), harvestEvent, insertedChilliPlant.UserID);
            
            // Assert
            var updatedChilliPlant = _plants.Find(Builders<ChilliPlant>.Filter.Empty).FirstOrDefault();

            Assert.That(updatedChilliPlant.HarvestEvents.Count == 1);
            Assert.That(updatedChilliPlant.HarvestEvents.First().Quantity == 50);
        }
    }
}