using ChillTracker.Data.DataModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillTracker.Data.DTO
{
    public class ChilliPlantCreateNewDTO
    {
        public string? Identifier { get; set; }
        public string? Species { get; set; }
        public string? Variety { get; set; }
        public DateTime? Planted { get; set; }
        public bool IsHealthy { get; set; } = true;
        public bool IsGerminated { get; set; } = false;
        public string UserID { get; set; }
    }
}
