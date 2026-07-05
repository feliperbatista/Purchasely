namespace Purchasely.Application.DTOs;

public record TokensResponse(
    string AccessToken,
    string RefreshToken
);