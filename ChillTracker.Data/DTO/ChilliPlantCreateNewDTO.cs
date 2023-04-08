using ChilliTracker.Data.DataModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DTO
{
    public class ChilliPlantCreateNewDTO
    {
        [StringLength(100, MinimumLength = 5)]
        public string? Identifier { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string? Species { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string? Variety { get; set; }

        public DateTime? Planted { get; set; }
        public bool IsHealthy { get; set; } = true;
        public bool IsGerminated { get; set; } = false;
    }
}
