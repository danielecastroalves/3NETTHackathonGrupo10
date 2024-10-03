using MongoDB.Driver;

namespace HealthMed.Infrastructure.Mongo.Utils.Interfaces;

public interface IMongoConnection
{
    IMongoDatabase GetDatabase();
}
