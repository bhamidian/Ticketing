using System.ComponentModel.DataAnnotations;

namespace Ticketing.Endpoint.Webapi.DTOs;

public sealed record RegisterRequestDto(
    [param: Required, EmailAddress, MaxLength(256)] string Email,
    [param: Required, MinLength(8), MaxLength(100)] string Password);
