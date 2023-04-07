using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillTracker.Data.DataModels
{
    public class PlantIssue
    {
        public string PlantIssueID { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? SolutionApplied { get; set; }
        public string? Notes { get; set; }
    }
}
