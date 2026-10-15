using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ticketing.Domain.Enums;

namespace Ticketing.Endpoint.Webapi.DTOs;

public sealed class CreateTicketFormRequestDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Subject { get; init; } = string.Empty;

    [Required, StringLength(4000, MinimumLength = 1)]
    public string Message { get; init; } = string.Empty;

    public TicketPriorityEnum Priority { get; init; }

    public IFormFile? Image { get; init; }
}
