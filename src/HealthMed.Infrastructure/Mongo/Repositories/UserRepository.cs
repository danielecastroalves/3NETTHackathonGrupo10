using System.Linq.Expressions;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using HealthMed.Infrastructure.Mongo.Contexts.Interfaces;
using MongoDB.Driver;

namespace HealthMed.Infrastructure.Mongo.Repositories;

public class UserRepository(IMongoContext context)
    : GenericRepository<User>(context), IUserRepository
{
    public async Task<User> GetAuthByLoginAndPassword(
        string login,
        string password,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Expression<Func<PersonEntity, bool>> filter =
            x => x.Senha == password && x.Email == login && x.Ativo;

        var queryResut = await _context.GetCollection<PersonEntity>()
            .FindAsync(filter, cancellationToken: cancellationToken);

        return await queryResut.FirstOrDefaultAsync(cancellationToken);
    }
}
