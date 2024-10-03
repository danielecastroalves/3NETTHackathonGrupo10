using System.Linq.Expressions;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using HealthMed.Infrastructure.Mongo.Contexts.Interfaces;
using MongoDB.Driver;

namespace HealthMed.Infrastructure.Mongo.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IMongoContext context) : base(context)
    { }

    public async Task<User> GetAuthByLoginAndPassword(
        string login,
        string password,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Expression<Func<ClienteEntity, bool>> filter =
            x => x.Senha == password && x.Login == login && x.Ativo;

        var queryResut = await _context.GetCollection<ClienteEntity>()
            .FindAsync(filter, cancellationToken: cancellationToken);

        return await queryResut.FirstOrDefaultAsync(cancellationToken);
    }
}
