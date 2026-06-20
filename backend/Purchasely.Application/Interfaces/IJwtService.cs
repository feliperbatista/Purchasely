using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}