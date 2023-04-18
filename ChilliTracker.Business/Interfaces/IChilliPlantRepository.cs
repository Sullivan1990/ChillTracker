using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using ChilliTracker.Data.Filters;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChilliTracker.Business.Interfaces
{
    public interface IChilliPlantRepository
    {
        // Get All Chillies
        ICollection<ChilliPlant> GetAll();

        ICollection<ChilliPlant> GetAllForUser(string userID);

        // Get All Chillies with Filter
        ICollection<ChilliPlant> GetAllWithFilter(ChilliPlantFilter filterParameters);

        // All Chillies with Issues
        ICollection<ChilliPlant> GetAllWithIssues(string UserID = "");
        // All Chillies Potted up more than 6 months ago (due for upgrade)
        ICollection<ChilliPlant> GetAllDueForPotting(string UserID = "");
        // All Chillies Not Germinated
        ICollection<ChilliPlant> GetAllNotGerminated(string UserID = "");

        // Add new Chilli
        void AddNewChilli(ChilliPlantCreateNewDTO newChilli, string UserID);

        // Add new Harvest Event to Chilli
        void AddHarvestEventToChilli(ObjectId chilliPlantId, HarvestEvent harvestEvent, string userID);
        // Add new Potting Event to Chilli
        void AddPottingEventToChilli(string chilliPlantId, PlantPottingEvent pottingEvent, string userID);
        // Add new Issue to Chilli
        void AddIssueToChilli(ObjectId chilliPlantId, PlantIssue plantIssue, string userID);

        // Delete Chilli
        void RemoveChilli(ObjectId chilliPlantId, string userID);

        // Remove Chilli Harvest Event
        void RemoveChilliHarvestEvent(ObjectId chilliPlantId, string harvestEventID, string userID);

        // Remove Chilli Potting Event
        void RemoveChilliPottingEvent(ObjectId chilliPlantId, string pottingEventID, string userID);
        // Remove Chilli Issue
        void RemoveChilliIssue(ObjectId chilliPlantId, string plantIssueID, string userID);

        // TODO - Update Methods 
            // Update Chilli Details 
            // Update specific Event Details

    }
}
