using Microsoft.AspNetCore.Identity;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Infrastructure.Efcore.Persistence;


internal static class SampleSeedData
{
    public static readonly Guid UserRoleId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid AgentRoleId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid AdminRoleId = Guid.Parse("10000000-0000-0000-0000-000000000003");

    public static readonly Guid Agent1Id = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid Agent2Id = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid Agent3Id = Guid.Parse("20000000-0000-0000-0000-000000000003");
    public static readonly Guid User1Id = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid User2Id = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid User3Id = Guid.Parse("30000000-0000-0000-0000-000000000003");

    public static readonly Guid Agent1ProfileId = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid Agent2ProfileId = Guid.Parse("40000000-0000-0000-0000-000000000002");
    public static readonly Guid Agent3ProfileId = Guid.Parse("40000000-0000-0000-0000-000000000003");
    public static readonly Guid User1ProfileId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid User2ProfileId = Guid.Parse("50000000-0000-0000-0000-000000000002");
    public static readonly Guid User3ProfileId = Guid.Parse("50000000-0000-0000-0000-000000000003");
    public static readonly Guid SupportDepartmentId = Guid.Parse("55000000-0000-0000-0000-000000000001");

    private static readonly DateTime SeedDate = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);

    public static readonly object[] AppUsers =
    [
        IdentityUser(Agent1Id, "agent1@ticketing.local", "AQAAAAIAAYagAAAAEDUGzibteyDlAUvxQdpcKKwwglvdLdu6d/YgwS6hsELupc32Ke30OkdY+HiFNxou0w=="),
        IdentityUser(Agent2Id, "agent2@ticketing.local", "AQAAAAIAAYagAAAAELU9XAraK+2taoTLAVwo6fVhRuxVNcCTw8EbH9yUH0JBV0KR5o8n0OyXTrdG7/9HJA=="),
        IdentityUser(Agent3Id, "agent3@ticketing.local", "AQAAAAIAAYagAAAAEBp0xyWEQj0QGiafpL0+hDA809FoINQNX9rNQh0Fm+mRanA4pJDlV8fSqYLVsrxr0w=="),
        IdentityUser(User1Id, "user1@ticketing.local", "AQAAAAIAAYagAAAAECfqQ1xVdA31CUg6mHPFj5Qr7iuaO0Y5/jWSnFpNxQcwbYIv+VJ5ez1H1Rf+pQKGhQ=="),
        IdentityUser(User2Id, "user2@ticketing.local", "AQAAAAIAAYagAAAAEBaB+KtSNjgYSxynxk2gPpXPBrZvkXUmrSL2SHS7OPqziQgYf09YK/rVtGL3fdLnTQ=="),
        IdentityUser(User3Id, "user3@ticketing.local", "AQAAAAIAAYagAAAAEPur9IFnPLmCjf0jEDX67qEE0nFdJpyjX39fBEd3avc/gd5tIUZPgwjRdcdczyRNUg==")
    ];

    public static readonly IdentityRole<Guid>[] Roles =
    [
        new() { Id = UserRoleId, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "10000000-0000-0000-0000-000000000011" },
        new() { Id = AgentRoleId, Name = "Agent", NormalizedName = "AGENT", ConcurrencyStamp = "10000000-0000-0000-0000-000000000012" },
        new() { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "10000000-0000-0000-0000-000000000013" }
    ];

    public static readonly IdentityUserRole<Guid>[] UserRoles =
    [
        new() { UserId = Agent1Id, RoleId = AgentRoleId },
        new() { UserId = Agent2Id, RoleId = AgentRoleId },
        new() { UserId = Agent3Id, RoleId = AgentRoleId },
        new() { UserId = User1Id, RoleId = UserRoleId },
        new() { UserId = User2Id, RoleId = UserRoleId },
        new() { UserId = User3Id, RoleId = UserRoleId }
    ];

    public static readonly object[] NormalUsers =
    [
        Profile(User1ProfileId, User1Id, SupportDepartmentId), Profile(User2ProfileId, User2Id, SupportDepartmentId), Profile(User3ProfileId, User3Id, SupportDepartmentId)
    ];

    public static readonly Department[] Departments =
    [
        new() { Id = SupportDepartmentId, Name = "پشتیبانی", CreatedAt = SeedDate, UpdatedAt = SeedDate, CreatedById = User1Id, UpdatedById = User1Id }
    ];

    public static readonly object[] Agents =
    [
        Profile(Agent1ProfileId, Agent1Id), Profile(Agent2ProfileId, Agent2Id), Profile(Agent3ProfileId, Agent3Id)
    ];

    public static readonly Ticket[] Tickets =
    [
        TicketRow("60000000-0000-0000-0000-000000000001", User1ProfileId, User1Id, "Unable to reset my password", TicketLifeCycleEnum.OPEN, TicketPriorityEnum.HIGH, 0),
        TicketRow("60000000-0000-0000-0000-000000000002", User1ProfileId, User1Id, "Invoice for my latest order", TicketLifeCycleEnum.INPROGRESS, TicketPriorityEnum.MODERATE, 1, assignedAgentId: Agent1Id),
        TicketRow("60000000-0000-0000-0000-000000000003", User2ProfileId, User2Id, "Application crashes on startup", TicketLifeCycleEnum.ANSWERED, TicketPriorityEnum.HIGH, 2, assignedAgentId: Agent2Id),
        TicketRow("60000000-0000-0000-0000-000000000004", User2ProfileId, User2Id, "Change the email on my account", TicketLifeCycleEnum.CLOSED, TicketPriorityEnum.LOW, 3, assignedAgentId: Agent1Id, closedById: Agent1Id),
        TicketRow("60000000-0000-0000-0000-000000000005", User3ProfileId, User3Id, "Missing items in my delivery", TicketLifeCycleEnum.OPEN, TicketPriorityEnum.MODERATE, 4),
        TicketRow("60000000-0000-0000-0000-000000000006", User3ProfileId, User3Id, "Cannot download my receipt", TicketLifeCycleEnum.CLOSED, TicketPriorityEnum.LOW, 5, assignedAgentId: Agent3Id, closedById: Agent3Id)
    ];

    public static readonly TicketMessage[] TicketMessages =
    [
        Message("70000000-0000-0000-0000-000000000001", 1, 10, User1Id, User1Id, "I requested a password reset, but the email has not arrived."),
        Message("70000000-0000-0000-0000-000000000002", 1, 20, User1Id, User1Id, "I checked my spam folder as well, but it is not there."),
        Message("70000000-0000-0000-0000-000000000003", 2, 20, User1Id, User1Id, "The invoice total does not match my order confirmation."),
        Message("70000000-0000-0000-0000-000000000004", 2, 40, Agent1Id, Agent1Id, "I am checking the order details and will update you shortly."),
        Message("70000000-0000-0000-0000-000000000005", 3, 20, User2Id, User2Id, "The app closes immediately after I sign in."),
        Message("70000000-0000-0000-0000-000000000006", 3, 40, Agent2Id, Agent2Id, "Please clear the app cache and try again. We are also preparing a fix for this crash."),
        Message("70000000-0000-0000-0000-000000000007", 4, 20, User2Id, User2Id, "Please update the email address associated with my account."),
        Message("70000000-0000-0000-0000-000000000008", 4, 40, Agent1Id, Agent1Id, "The email address has been updated. Please sign in with the new address."),
        Message("70000000-0000-0000-0000-000000000009", 5, 20, User3Id, User3Id, "Two items from my order were missing from the package."),
        Message("70000000-0000-0000-0000-000000000010", 6, 20, User3Id, User3Id, "I need a copy of the receipt for my last purchase."),
        Message("70000000-0000-0000-0000-000000000011", 6, 40, Agent3Id, Agent3Id, "I have attached the receipt to your account. This ticket is now closed.")
    ];

    private static object IdentityUser(Guid id, string email, string passwordHash) => new
    {
        Id = id,
        UserName = email,
        NormalizedUserName = email.ToUpperInvariant(),
        Email = email,
        NormalizedEmail = email.ToUpperInvariant(),
        EmailConfirmed = true,
        PasswordHash = passwordHash,
        SecurityStamp = $"seed-{id:N}",
        ConcurrencyStamp = $"seed-{id:N}",
        PhoneNumberConfirmed = false,
        TwoFactorEnabled = false,
        LockoutEnabled = true,
        AccessFailedCount = 0
    };

    private static object Profile(Guid id, Guid appUserId, Guid? departmentId = null) => new
    {
        Id = id, AppUserId = appUserId,
        DepartmentId = departmentId,
        CreatedAt = SeedDate, UpdatedAt = SeedDate,
        CreatedById = appUserId, UpdatedById = appUserId
    };

    private static Ticket TicketRow(string id, Guid profileId, Guid userId, string subject,
        TicketLifeCycleEnum status, TicketPriorityEnum priority, int dayOffset,
        Guid? assignedAgentId = null, Guid? closedById = null)
    {
        var createdAt = SeedDate.AddDays(dayOffset);
        return new Ticket
        {
            Id = Guid.Parse(id), NormalUserId = profileId, Subject = subject,
            TicketLifeCycle = status, TicketPriority = priority,
            AssignedAgentId = assignedAgentId,
            CreatedAt = createdAt, UpdatedAt = createdAt,
            CreatedById = userId, UpdatedById = userId,
            ClosedAt = status == TicketLifeCycleEnum.CLOSED ? createdAt.AddHours(4) : null,
            ClosedById = status == TicketLifeCycleEnum.CLOSED ? closedById : null
        };
    }

    private static TicketMessage Message(string id, int ticketNumber, int minuteOffset, Guid senderId, Guid auditUserId, string message)
    {
        var createdAt = SeedDate.AddDays(ticketNumber - 1).AddMinutes(minuteOffset);
        return new TicketMessage
        {
            Id = Guid.Parse(id), TicketId = Guid.Parse($"60000000-0000-0000-0000-{ticketNumber:000000000000}"),
            SenderId = senderId, Message = message, CreatedAt = createdAt, UpdatedAt = createdAt,
            IsConfirmed = senderId != Agent1Id && senderId != Agent2Id && senderId != Agent3Id,
            CreatedById = auditUserId, UpdatedById = auditUserId
        };
    }
}
