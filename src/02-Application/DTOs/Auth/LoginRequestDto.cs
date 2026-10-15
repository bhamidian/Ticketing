namespace Ticketing.Application.DTOs.Auth;

public sealed record LoginRequestDto(string EmailOrUserName, string Password);
