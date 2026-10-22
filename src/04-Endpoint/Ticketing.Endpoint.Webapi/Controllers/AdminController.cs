using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Features.Tickets.Commands.ConfirmMessage;
using Ticketing.Application.Features.Tickets.Commands.UpdateMessage;
using Ticketing.Endpoint.Webapi.Mapping;

namespace Ticketing.Endpoint.Webapi.Controllers;

[ApiController]
[Authorize(Policy = "Admin")]
[Route("api/[controller]")]
public sealed class AdminController(ISender sender, IAdminRepository admins, IAgentRepository agents) : ControllerBase
{
    [HttpGet("agents")]
    public async Task<IActionResult> GetAgents(CancellationToken cancellationToken)
    {
        var result = await agents.GetAgentResponseCountsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("tickets/{ticketId:guid}/assign")]
    public async Task<IActionResult> AssignTicket(
        Guid ticketId,
        AssignTicketRequest request,
        CancellationToken cancellationToken)
    {
        var adminUserId = GetUserId();
        if (adminUserId is null)
            return Unauthorized();

        var assigned = await admins.AssignTicketToAgentByIdAsync(
            ticketId, request.AgentUserId, adminUserId.Value, cancellationToken);
        return assigned
            ? NoContent()
            : NotFound(new ProblemDetails { Title = "تیکت یا ایجنت پیدا نشد، یا تیکت بسته است." });
    }

    [HttpPatch("messages/{messageId:guid}/confirm")]
    public async Task<IActionResult> ConfirmAgentMessage(Guid messageId, CancellationToken cancellationToken)
    {
        var adminUserId = GetUserId();
        if (adminUserId is null)
            return Unauthorized();

        var result = await sender.Send(new ConfirmMessageCommand { MessageId = messageId, AdminUserId = adminUserId.Value }, cancellationToken);
        return result.IsSuccess ? NoContent() : ApiResultMapper.ToProblem(result.Error!);
    }

    [HttpPatch("tickets/{ticketId:guid}/messages/{messageId:guid}")]
    public async Task<IActionResult> UpdateAgentMessage(
        Guid ticketId,
        Guid messageId,
        UpdateAgentMessageRequest request,
        CancellationToken cancellationToken)
    {
        var adminUserId = GetUserId();
        if (adminUserId is null)
            return Unauthorized();

        var result = await sender.Send(
            new UpdateMessageCommand(adminUserId.Value, ticketId, messageId, request.Message),
            cancellationToken);
        return result.IsSuccess ? NoContent() : ApiResultMapper.ToProblem(result.Error!);
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var userId) ? userId : null;
    }
}

public sealed record AssignTicketRequest(Guid AgentUserId);
public sealed record UpdateAgentMessageRequest(string Message);
