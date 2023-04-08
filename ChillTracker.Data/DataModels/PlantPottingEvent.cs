using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DataModels
{
    public class PlantPottingEvent
    {
        public string PottingEventID { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Container { get; set; }
        public string? Medium { get; set; }
        public string? Conditions { get; set; }
        public string? Notes { get; set; }

    }
}
