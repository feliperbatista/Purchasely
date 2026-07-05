namespace Purchasely.Application.DTOs;

public record LoginResponse(
    TokensResponse Tokens,
    UserResponse User
);