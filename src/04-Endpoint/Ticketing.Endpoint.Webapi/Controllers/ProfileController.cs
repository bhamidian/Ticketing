using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Endpoint.Webapi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ProfileController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserProfileDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!Guid.TryParse(idValue, out var userId))
            return Unauthorized();

        var profile = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new CurrentUserProfileDto(
                user.Id,
                user.Email,
                user.NormalUser == null ? null : user.NormalUser.Department.Name))
            .SingleOrDefaultAsync(cancellationToken);

        return profile is null ? NotFound() : Ok(profile);
    }
}

public sealed record CurrentUserProfileDto(Guid UserId, string? Email, string? DepartmentName);
