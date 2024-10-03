using HealthMed.Domain.Entities;

namespace HealthMed.Application.Common.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetAuthByLoginAndPassword(
        string login,
        string password,
        CancellationToken cancellationToken = default);
}
