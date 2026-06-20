using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role
);