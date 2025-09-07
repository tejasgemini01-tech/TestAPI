using demo_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace demo_api.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<FormSubmission> _submissionsCollection;

        public MongoDbService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _submissionsCollection = database.GetCollection<FormSubmission>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<FormSubmission>> GetAsync() =>
            await _submissionsCollection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(FormSubmission submission) =>
            await _submissionsCollection.InsertOneAsync(submission);
    }

    // Helper class to map appsettings.json
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}
