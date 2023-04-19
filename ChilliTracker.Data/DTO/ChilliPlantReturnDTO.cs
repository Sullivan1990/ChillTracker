using ChilliTracker.Data.DataModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DTO
{
    public class ChilliPlantReturnDTO
    {
        public string? _id { get; set; }
        public string? Identifier { get; set; }
        public string? Species { get; set; }
        public string? Variety { get; set; }
        public DateTime? Planted { get; set; }
        public DateTime? Germinated { get; set; }
        public DateTime? FirstHarvest { get; set; }
        public bool IsHealthy { get; set; } = true;
        public bool IsGerminated { get; set; } = false;
        public List<HarvestEvent> HarvestEvents { get; set; } = new List<HarvestEvent>();
        public List<PlantIssue> PlantIssues { get; set; } = new List<PlantIssue>();
        public List<PlantPottingEvent> PlantPottingEvents { get; set; } = new List<PlantPottingEvent>();
    }
}
