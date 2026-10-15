using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticketing.Infrastructure.Efcore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompileTimeSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "10000000-0000-0000-0000-000000000011", "User", "USER" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "10000000-0000-0000-0000-000000000012", "Agent", "AGENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), 0, "seed-20000000000000000000000000000001", "agent1@ticketing.local", true, true, null, "AGENT1@TICKETING.LOCAL", "AGENT1@TICKETING.LOCAL", "AQAAAAIAAYagAAAAEDUGzibteyDlAUvxQdpcKKwwglvdLdu6d/YgwS6hsELupc32Ke30OkdY+HiFNxou0w==", null, false, "seed-20000000000000000000000000000001", false, "agent1@ticketing.local" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), 0, "seed-20000000000000000000000000000002", "agent2@ticketing.local", true, true, null, "AGENT2@TICKETING.LOCAL", "AGENT2@TICKETING.LOCAL", "AQAAAAIAAYagAAAAELU9XAraK+2taoTLAVwo6fVhRuxVNcCTw8EbH9yUH0JBV0KR5o8n0OyXTrdG7/9HJA==", null, false, "seed-20000000000000000000000000000002", false, "agent2@ticketing.local" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), 0, "seed-20000000000000000000000000000003", "agent3@ticketing.local", true, true, null, "AGENT3@TICKETING.LOCAL", "AGENT3@TICKETING.LOCAL", "AQAAAAIAAYagAAAAEBp0xyWEQj0QGiafpL0+hDA809FoINQNX9rNQh0Fm+mRanA4pJDlV8fSqYLVsrxr0w==", null, false, "seed-20000000000000000000000000000003", false, "agent3@ticketing.local" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), 0, "seed-30000000000000000000000000000001", "user1@ticketing.local", true, true, null, "USER1@TICKETING.LOCAL", "USER1@TICKETING.LOCAL", "AQAAAAIAAYagAAAAECfqQ1xVdA31CUg6mHPFj5Qr7iuaO0Y5/jWSnFpNxQcwbYIv+VJ5ez1H1Rf+pQKGhQ==", null, false, "seed-30000000000000000000000000000001", false, "user1@ticketing.local" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), 0, "seed-30000000000000000000000000000002", "user2@ticketing.local", true, true, null, "USER2@TICKETING.LOCAL", "USER2@TICKETING.LOCAL", "AQAAAAIAAYagAAAAEBaB+KtSNjgYSxynxk2gPpXPBrZvkXUmrSL2SHS7OPqziQgYf09YK/rVtGL3fdLnTQ==", null, false, "seed-30000000000000000000000000000002", false, "user2@ticketing.local" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), 0, "seed-30000000000000000000000000000003", "user3@ticketing.local", true, true, null, "USER3@TICKETING.LOCAL", "USER3@TICKETING.LOCAL", "AQAAAAIAAYagAAAAEPur9IFnPLmCjf0jEDX67qEE0nFdJpyjX39fBEd3avc/gd5tIUZPgwjRdcdczyRNUg==", null, false, "seed-30000000000000000000000000000003", false, "user3@ticketing.local" }
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "AppUserId", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "NormalUsers",
                columns: new[] { "Id", "AppUserId", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "ClosedAt", "ClosedById", "CreatedAt", "CreatedById", "NormalUserId", "Subject", "TicketLifeCycle", "TicketPriority", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), null, null, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), new Guid("50000000-0000-0000-0000-000000000001"), "Unable to reset my password", "OPEN", "HIGH", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), null, null, new DateTime(2026, 1, 16, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), new Guid("50000000-0000-0000-0000-000000000001"), "Invoice for my latest order", "INPROGRESS", "MODERATE", new DateTime(2026, 1, 16, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), null, null, new DateTime(2026, 1, 17, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002"), new Guid("50000000-0000-0000-0000-000000000002"), "Application crashes on startup", "ANSWERED", "HIGH", new DateTime(2026, 1, 17, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 18, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 18, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002"), new Guid("50000000-0000-0000-0000-000000000002"), "Change the email on my account", "CLOSED", "LOW", new DateTime(2026, 1, 18, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), null, null, new DateTime(2026, 1, 19, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003"), new Guid("50000000-0000-0000-0000-000000000003"), "Missing items in my delivery", "OPEN", "MODERATE", new DateTime(2026, 1, 19, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 20, 13, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 20, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003"), new Guid("50000000-0000-0000-0000-000000000003"), "Cannot download my receipt", "CLOSED", "LOW", new DateTime(2026, 1, 20, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "TicketMessages",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Message", "PictureUrl", "SenderId", "TicketId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 10, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "I requested a password reset, but the email has not arrived.", null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 10, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "I checked my spam folder as well, but it is not there.", null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 16, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "The invoice total does not match my order confirmation.", null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 16, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 16, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), "I am checking the order details and will update you shortly.", null, new Guid("20000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 16, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 17, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002"), "The app closes immediately after I sign in.", null, new Guid("30000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 17, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 17, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), "Please clear the app cache and try again. We are also preparing a fix for this crash.", null, new Guid("20000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 17, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000007"), new DateTime(2026, 1, 18, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002"), "Please update the email address associated with my account.", null, new Guid("30000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 18, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000008"), new DateTime(2026, 1, 18, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), "The email address has been updated. Please sign in with the new address.", null, new Guid("20000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 18, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000009"), new DateTime(2026, 1, 19, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003"), "Two items from my order were missing from the package.", null, new Guid("30000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 19, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000010"), new DateTime(2026, 1, 20, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003"), "I need a copy of the receipt for my last purchase.", null, new Guid("30000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 20, 9, 20, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000011"), new DateTime(2026, 1, 20, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003"), "I have attached the receipt to your account. This ticket is now closed.", null, new Guid("20000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 20, 9, 40, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000003") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "TicketMessages",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "NormalUsers",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "NormalUsers",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "NormalUsers",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));
        }
    }
}
