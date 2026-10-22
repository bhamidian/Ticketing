using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.Features.Tickets.Queries.GetTicketImage;
using Ticketing.Application.Mapping;
using Ticketing.Endpoint.Webapi.Mapping;
using Ticketing.Domain.Enums;
using Ticketing.Endpoint.Webapi.DTOs;

namespace Ticketing.Endpoint.Webapi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class TicketController(
    ISender sender,
    IFileService fileService) : ControllerBase
{
    private const long MaxImageSize = 5 * 1024 * 1024;

    [HttpGet]
    [ProducesResponseType<PagedTicketsDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTickets(
        [FromQuery] TicketLifeCycleEnum? status,
        [FromQuery] TicketPriorityEnum? priority,
        CancellationToken cancellationToken,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string sortOrder = "newest",
        [FromQuery] bool assignedOnly = false,
        [FromQuery] bool unassignedOnly = false)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Clamp(page, 1, int.MaxValue / pageSize);
        var isAdmin = User.IsInRole("Admin");
        var isAgent = IsStaff() && !isAdmin;

        var result = await sender.Send(
            ApplicationMapper.ToGetTicketsQuery(
                userId.Value,
                isAgent,
                status,
                priority,
                page,
                pageSize,
                string.Equals(sortOrder, "oldest", StringComparison.OrdinalIgnoreCase),
                isAgent || (isAdmin && assignedOnly),
                isAdmin && unassignedOnly,
                isAdmin),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpGet("{ticketId:guid}")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(
            ApplicationMapper.ToGetTicketDetailsQuery(userId.Value, IsStaff(), ticketId, User.IsInRole("Admin")),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpGet("{ticketId:guid}/messages/{messageId:guid}/image")]
    [Produces("image/jpeg", "image/png", "image/gif", "image/webp")]
    public async Task<IActionResult> GetTicketImage(Guid ticketId, Guid messageId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(new GetTicketImageQuery(userId.Value, IsStaff(), ticketId, messageId, User.IsInRole("Admin")), cancellationToken);
        if (!result.IsSuccess)
            return ApiResultMapper.ToProblem(result.Error!);

        var stream = fileService.OpenRead(result.Value!.RelativePath);
        if (stream is null)
            return NotFound(new ProblemDetails { Title = "فایل تصویر در دسترس نیست." });

        Response.Headers.CacheControl = "private, no-store";
        return File(stream, GetImageContentType(result.Value.RelativePath), enableRangeProcessing: true);
    }

    [HttpPost("{ticketId:guid}/claim")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ClaimTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(
            ApplicationMapper.ToClaimTicketCommand(userId.Value, IsStaff(), ticketId),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status201Created)]
    [RequestSizeLimit(MaxImageSize + 64 * 1024)]
    public async Task<IActionResult> CreateTicket([FromForm] CreateTicketFormRequestDto request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        if (request.Image is not null)
        {
            var imageError = await ValidateImageAsync(request.Image, cancellationToken);
            if (imageError is not null)
                return BadRequest(new ProblemDetails { Title = "تصویر پیوست‌شده معتبر نیست.", Detail = imageError });
        }

        string? pictureUrl = null;
        if (request.Image is not null)
        {
            await using var imageStream = request.Image.OpenReadStream();
            pictureUrl = await fileService.UploadAsync(
                imageStream, request.Image.FileName, "Tickets", cancellationToken)
                ?? throw new InvalidOperationException("ذخیرهٔ تصویر پیوست‌شده انجام نشد.");
        }

        var dto = new CreateTicketRequestDto(request.Subject, request.Message, request.Priority, pictureUrl);
        var result = await sender.Send(ApplicationMapper.ToCommand(dto, userId.Value), cancellationToken);
        if (!result.IsSuccess)
        {
            if (!string.IsNullOrWhiteSpace(pictureUrl))
                fileService.Delete(pictureUrl);
            return ApiResultMapper.ToProblem(result.Error!);
        }

        return CreatedAtAction(nameof(GetTicket), new { ticketId = result.Value!.Id }, result.Value);
    }

    [HttpPost("{ticketId:guid}/messages")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMessage(
        Guid ticketId,
        AddTicketMessageRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(
            ApplicationMapper.ToCommand(request, userId.Value, IsStaff(), ticketId),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpPost("{ticketId:guid}/close")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CloseTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(
            ApplicationMapper.ToCloseTicketCommand(userId.Value, IsStaff(), ticketId),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpPatch("{ticketId:guid}/status")]
    [ProducesResponseType<TicketDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeStatus(
        Guid ticketId,
        ChangeTicketStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await sender.Send(
            ApplicationMapper.ToChangeTicketStatusCommand(userId.Value, IsStaff(), ticketId, request.Status),
            cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : ApiResultMapper.ToProblem(result.Error!);
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var userId) ? userId : null;
    }

    private bool IsStaff() => User.IsInRole("Agent") && !User.IsInRole("Admin");

    private static async Task<string?> ValidateImageAsync(IFormFile image, CancellationToken cancellationToken)
    {
        if (image.Length == 0)
            return "فایل انتخاب‌شده خالی است.";
        if (image.Length > MaxImageSize)
            return "حجم تصویر نباید بیشتر از ۵ مگابایت باشد.";

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        var expectedContentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => null
        };
        if (expectedContentType is null || !string.Equals(image.ContentType, expectedContentType, StringComparison.OrdinalIgnoreCase))
            return "فقط تصویرهای JPG، PNG، GIF یا WebP پذیرفته می‌شوند.";

        var header = new byte[12];
        await using var stream = image.OpenReadStream();
        var bytesRead = await stream.ReadAsync(header.AsMemory(), cancellationToken);
        var validSignature = expectedContentType switch
        {
            "image/jpeg" => bytesRead >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            "image/png" => bytesRead >= 8 && header.AsSpan(0, 8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            "image/gif" => bytesRead >= 6 && header.AsSpan(0, 3).SequenceEqual("GIF"u8) && header[3] == '8' && (header[4] == '7' || header[4] == '9') && header[5] == 'a',
            "image/webp" => bytesRead >= 12 && header.AsSpan(0, 4).SequenceEqual("RIFF"u8) && header.AsSpan(8, 4).SequenceEqual("WEBP"u8),
            _ => false
        };

        return validSignature ? null : "محتوای فایل با قالب تصویر اعلام‌شده هم‌خوانی ندارد.";
    }

    private static string GetImageContentType(string fileName) => Path.GetExtension(fileName).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".webp" => "image/webp",
        _ => "application/octet-stream"
    };
}
