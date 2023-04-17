using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DataModels
{
    public class User
    {
        public ObjectId _id { get; set; }
        public string ApiKey { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastAccess { get; set; }
    }
}
