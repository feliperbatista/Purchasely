namespace Purchasely.Application.Interfaces;

public interface ICurrentUserService
{
    Guid Id { get; }
    string Name { get; }
    string Email { get; }
}