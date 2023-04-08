using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using ChilliTracker.Data.Filters;
using ChilliTracker.Shared.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Business.Repository
{
    public class ChilliPlantRepository : IChilliPlantRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<ChilliPlant> _plants;
        public ChilliPlantRepository(IMongoDatabaseConnection connection)
        {
            _database = connection.GetProductionDatabase();
            _plants = _database.GetCollection<ChilliPlant>("ChilliPlants");
        }

        public void AddHarvestEventToChilli(ObjectId chilliPlantId, HarvestEvent harvestEvent)
        {
            var filter = Builders<ChilliPlant>.Filter.Eq(c => c._id, chilliPlantId);
            var update = Builders<ChilliPlant>.Update.Push(c => c.HarvestEvents, harvestEvent);

            _plants.UpdateOne(filter, update);
        }

        public void AddIssueToChilli(ObjectId chilliPlantId, PlantIssue plantIssue)
        {
            throw new NotImplementedException();
        }

        public void AddNewChilli(ChilliPlantCreateNewDTO newChilli, string userID)
        {
            ChilliPlant newChilliPlant = new ChilliPlant()
            {
                Variety = newChilli.Variety,
                Species = newChilli.Species,
                Identifier = newChilli.Identifier,
                Planted = newChilli.Planted ?? DateTime.UtcNow,
                IsGerminated = newChilli.IsGerminated,
                IsHealthy = newChilli.IsHealthy,
                UserID = userID,
                HarvestEvents = new List<HarvestEvent>(),
                PlantPottingEvents = new List<PlantPottingEvent>(),
                PlantIssues = new List<PlantIssue>()

            };

            _plants.InsertOne(newChilliPlant);
        }

        public void AddPottingEventToChilli(ObjectId chilliPlantId, PlantPottingEvent pottingEvent)
        {
            throw new NotImplementedException();
        }

        public ICollection<ChilliPlant> GetAll()
        {
            return _plants.Find(Builders<ChilliPlant>.Filter.Empty).ToList();
        }

        public ICollection<ChilliPlant> GetAllDueForPotting()
        {
            var pottingDateExpiry = DateTime.UtcNow.AddMonths(-6);

            // Ensure that the PlantPottingEvent array exists, has at least one element,
            // and that the last element has a date value that is less than the pottingDateExpiry
            var filter = Builders<ChilliPlant>.Filter.And(
                Builders<ChilliPlant>.Filter.Exists(x => x.PlantPottingEvents),
                Builders<ChilliPlant>.Filter.SizeGt(x => x.PlantPottingEvents, 0),
                Builders<ChilliPlant>.Filter.Lt(
                x => x.PlantPottingEvents[-1].Date, pottingDateExpiry));



            var sort = Builders<ChilliPlant>.Sort.Descending(
                x => x.PlantPottingEvents[-1].Date);
            // where the last potted date was > 6 months ago

            var options = new FindOptions<ChilliPlant, ChilliPlant>()
            {
                Sort = sort
            };

            var result = _plants.Find(filter).Sort(sort).ToList();
            return result;

        }

        public ICollection<ChilliPlant> GetAllNotGerminated()
        {

            var filter = Builders<ChilliPlant>.Filter.Or(
                Builders<ChilliPlant>.Filter.Not(
                Builders<ChilliPlant>.Filter.Exists(x => x.Germinated)),
                Builders<ChilliPlant>.Filter.Eq(c => c.Germinated, null));


            var result = _plants.Find(filter).ToList();
            return result;
        }

        public ICollection<ChilliPlant> GetAllWithFilter(ChilliPlantFilter filterParameters)
        {
            throw new NotImplementedException();
        }

        public ICollection<ChilliPlant> GetAllWithIssues()
        {
            throw new NotImplementedException();
        }

        public void RemoveChilli(ObjectId chilliPlantId)
        {
            throw new NotImplementedException();
        }

        public void RemoveChilliHarvestEvent(ObjectId chilliPlantId, string harvestEventID)
        {
            throw new NotImplementedException();
        }

        public void RemoveChilliIssue(ObjectId chilliPlantId, string plantIssueID)
        {
            throw new NotImplementedException();
        }

        public void RemoveChilliPottingEvent(ObjectId chilliPlantId, string pottingEventID)
        {
            throw new NotImplementedException();
        }
    }
}
