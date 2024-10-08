using System.Linq.Expressions;
using HealthMed.Domain.Entities;

namespace HealthMed.Application.Common.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<Guid> AddAsync
    (
        TEntity entity,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> GetByFilterAsync
    (
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> GetByIdAsync
    (
       Guid id,
       CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity>> GetListByFilterAsync
    (
       Expression<Func<TEntity, bool>> filter,
       CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity>> GetAsync
    (
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity>> GetAllAsync
    (
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync
    (
        Expression<Func<TEntity, bool>> filter,
        TEntity entity,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteByIdAsync
    (
       Guid id,
       CancellationToken cancellationToken = default
    );

    Task UpdateAppointmentAsync
    (
        Expression<Func<AppointmentSchedulingEntity, bool>> filter,
        AppointmentSchedulingEntity entity,
        CancellationToken cancellationToken = default
    );
}
