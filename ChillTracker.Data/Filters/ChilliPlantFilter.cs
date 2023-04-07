using ChillTracker.Data.DataModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillTracker.Data.Filters
{
    public class ChilliPlantFilter
    {
        public string? IdentifierPartial { get; set; }
        public string? SpeciesPartial { get; set; }
        public string? VarietyPartial { get; set; }
        public DateTime? PlantedFrom { get; set; }
        public DateTime? PlantedTo { get; set; }
        public DateTime? GerminatedFrom { get; set; }
        public DateTime? GerminatedTo { get; set; }
        public DateTime? FirstHarvestFrom { get; set; }
        public DateTime? FirstHarvestTo { get; set; }
        public bool? IsHealthyFilter { get; set; }
        public bool? IsGerminatedFilter { get; set; }
        public bool? HasHarvestEvents { get; set; }
        public bool? HasIssues { get; set; }
        public bool? HasPottingEvents { get; set; }

    }
}
