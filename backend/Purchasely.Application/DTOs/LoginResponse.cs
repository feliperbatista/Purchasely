using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record LoginResponse(
    string AccessToken,
    UserResponse User
);