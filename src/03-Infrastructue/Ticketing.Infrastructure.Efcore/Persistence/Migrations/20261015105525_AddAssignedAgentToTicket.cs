using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Infrastructure.Efcore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedAgentToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedAgentId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000003"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000004"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000005"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000006"),
                column: "AssignedAgentId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedAgentId_TicketLifeCycle",
                table: "Tickets",
                columns: new[] { "AssignedAgentId", "TicketLifeCycle" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedAgentId",
                table: "Tickets",
                column: "AssignedAgentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedAgentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedAgentId_TicketLifeCycle",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AssignedAgentId",
                table: "Tickets");
        }
    }
}
