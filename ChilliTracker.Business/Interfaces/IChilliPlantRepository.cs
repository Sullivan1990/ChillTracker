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
    interface IChilliPlantRepository
    {
        // Get All Chillies
        ICollection<ChilliPlant> GetAll();

        // Get All Chillies with Filter
        ICollection<ChilliPlant> GetAllWithFilter(ChilliPlantFilter filterParameters);

        // All Chillies with Issues
        ICollection<ChilliPlant> GetAllWithIssues();
        // All Chillies Potted up more than 6 months ago (due for upgrade)
        ICollection<ChilliPlant> GetAllDueForPotting();
        // All Chillies Not Germinated
        ICollection<ChilliPlant> GetAllNotGerminated();

        // Add new Chilli
        void AddNewChilli(ChilliPlantCreateNewDTO newChilli, string UserID);

        // Add new Harvest Event to Chilli
        void AddHarvestEventToChilli(ObjectId chilliPlantId, HarvestEvent harvestEvent);
        // Add new Potting Event to Chilli
        void AddPottingEventToChilli(ObjectId chilliPlantId, PlantPottingEvent pottingEvent);
        // Add new Issue to Chilli
        void AddIssueToChilli(ObjectId chilliPlantId, PlantIssue plantIssue);

        // Delete Chilli
        void RemoveChilli(ObjectId chilliPlantId);

        // Remove Chilli Harvest Event
        void RemoveChilliHarvestEvent(ObjectId chilliPlantId, string harvestEventID);

        // Remove Chilli Potting Event
        void RemoveChilliPottingEvent(ObjectId chilliPlantId, string pottingEventID);
        // Remove Chilli Issue
        void RemoveChilliIssue(ObjectId chilliPlantId, string plantIssueID);

        // TODO - Update Methods 
            // Update Chilli Details 
            // Update specific Event Details

    }
}
