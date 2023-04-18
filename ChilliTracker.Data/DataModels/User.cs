using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Data.DataModels
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastAccess { get; set; }
    }
}
