using System.Diagnostics.CodeAnalysis;
using HealthMed.Domain.Entities;
using HealthMed.Infrastructure.Mongo.Contexts.Interfaces;
using MongoDB.Driver;

namespace HealthMed.Infrastructure.Mongo.Contexts.EntityConfig;

[ExcludeFromCodeCoverage]
public abstract class EntityConfig<T> where T : Entity
{
    protected IndexKeysDefinitionBuilder<T> Builder { get; }

    private readonly IMongoCollection<T>? _collection;

    private readonly List<CreateIndexModel<T>> _newIndexes;

    private readonly IMongoContext _context;

    private readonly IEnumerable<string?> _existingIndexes = new List<string?>();

    protected EntityConfig(IMongoContext context)
    {
        _context = context;
        _collection = context.GetCollection<T>();
        _newIndexes = new List<CreateIndexModel<T>>();

        Builder = Builders<T>.IndexKeys;

        if (_collection != null)
        {
            _existingIndexes = _collection.Indexes.List().ToList().Select(c => c.GetValue("name").ToString());
        }
    }

    protected void AddTtlIndex()
    {
        const string indexName = "TTL";

        var ttlIndex = new CreateIndexModel<T>(
            Builder.Ascending(x => x.DataInsercao),
            new CreateIndexOptions
            {
                Background = true,
                Name = indexName,
                ExpireAfter = new TimeSpan(_context.DefaultTtlDays, 0, 0, 0)
            });

        if (!_existingIndexes.Contains(indexName))
        {
            _newIndexes.Add(ttlIndex);
        }
    }

    public void CreateIndexes()
    {
        ConfigureIndexes();
        _collection?.Indexes.CreateMany(_newIndexes);
    }

    protected abstract void ConfigureIndexes();

    protected void AddIndexes(List<CreateIndexModel<T>> indexes)
    {
        _newIndexes.AddRange(indexes);
    }
}
