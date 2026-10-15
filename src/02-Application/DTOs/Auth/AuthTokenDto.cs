using System.Text.Json.Serialization;

namespace Ticketing.Application.DTOs.Auth;

public sealed record AuthTokenDto(
    string AccessToken,
    [property: JsonIgnore] string RefreshToken,
    DateTime AccessTokenExpiresAtUtc,
    DateTime RefreshTokenExpiresAtUtc);
