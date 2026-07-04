using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<List<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}