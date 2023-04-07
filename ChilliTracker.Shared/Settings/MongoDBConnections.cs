namespace ChilliTracker.Shared.Settings
{
    public class MongoDBConnections
    {
        public string ProductionServer { get; set; } = "";
        public string ProductionDatabase { get; set; } = "";
        public string TestingServer { get; set; } = "";
        public string TestingDatabase { get; set; } = "";
    }
}
