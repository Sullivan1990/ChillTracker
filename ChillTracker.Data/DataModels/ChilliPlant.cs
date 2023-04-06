using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillTracker.Data.DataModels
{
    public class ChilliPlant
    {
        public ObjectId _id { get; set; }
        public string? Identifier { get; set; }
        public string? Species { get; set; }
        public string? Variety { get; set; }
        public DateTime? Planted { get; set; }
        public DateTime? Germinated { get; set; }
        public DateTime? FirstHarvest { get; set; }
        public bool IsHealthy { get; set; } = true;
        public bool IsGerminated { get; set; } = false;
        public ObjectId UserID { get; set; }
        public List<HarvestEvent> HarvestEvents { get; set; } = new List<HarvestEvent>();
        public List<PlantIssue> PlantIssues { get; set; } = new List<PlantIssue>();
        public List<PlantPottingEvent> PlantPottingEvents { get; set; } = new List<PlantPottingEvent>();

    }
}
