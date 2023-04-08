using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DataModels
{
    public class HarvestEvent
    {
        public string HarvestEventID { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int? Quantity { get; set; }
        public double? WeightGrams { get; set; }
        public string? Notes { get; set; }

    }
}
