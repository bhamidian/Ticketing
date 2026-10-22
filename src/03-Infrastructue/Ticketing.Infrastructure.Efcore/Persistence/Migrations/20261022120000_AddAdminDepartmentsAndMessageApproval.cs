using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ticketing.Infrastructure.Efcore.Persistence;

#nullable disable

namespace Ticketing.Infrastructure.Efcore.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261022120000_AddAdminDepartmentsAndMessageApproval")]
public partial class AddAdminDepartmentsAndMessageApproval : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = 'ADMIN') INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES ('10000000-0000-0000-0000-000000000003', '10000000-0000-0000-0000-000000000013', 'Admin', 'ADMIN')");
        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Departments", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Admins",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Admins", x => x.Id);
                table.ForeignKey("FK_Admins_AspNetUsers_AppUserId", x => x.AppUserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.Sql("INSERT INTO [Departments] ([Id], [Name], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById]) VALUES ('55000000-0000-0000-0000-000000000001', N'پشتیبانی', '2026-01-15T09:00:00', '2026-01-15T09:00:00', '30000000-0000-0000-0000-000000000001', '30000000-0000-0000-0000-000000000001')");

        migrationBuilder.AddColumn<Guid>(name: "DepartmentId", table: "NormalUsers", type: "uniqueidentifier", nullable: true);
        migrationBuilder.Sql("UPDATE [NormalUsers] SET [DepartmentId] = '55000000-0000-0000-0000-000000000001'");
        migrationBuilder.AlterColumn<Guid>(name: "DepartmentId", table: "NormalUsers", type: "uniqueidentifier", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
        migrationBuilder.AddColumn<bool>(name: "IsConfirmed", table: "TicketMessages", type: "bit", nullable: true);
        migrationBuilder.Sql("UPDATE [TicketMessages] SET [IsConfirmed] = CASE WHEN EXISTS (SELECT 1 FROM [AspNetUserRoles] ur JOIN [AspNetRoles] r ON r.[Id] = ur.[RoleId] WHERE ur.[UserId] = [TicketMessages].[SenderId] AND r.[NormalizedName] = 'AGENT') THEN 0 ELSE 1 END");

        migrationBuilder.CreateIndex(name: "IX_Admins_AppUserId", table: "Admins", column: "AppUserId", unique: true, filter: "[AppUserId] IS NOT NULL");
        migrationBuilder.CreateIndex(name: "IX_NormalUsers_DepartmentId", table: "NormalUsers", column: "DepartmentId");
        migrationBuilder.AddForeignKey(name: "FK_NormalUsers_Departments_DepartmentId", table: "NormalUsers", column: "DepartmentId", principalTable: "Departments", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_NormalUsers_Departments_DepartmentId", table: "NormalUsers");
        migrationBuilder.DropTable(name: "Admins");
        migrationBuilder.DropTable(name: "Departments");
        migrationBuilder.DropIndex(name: "IX_NormalUsers_DepartmentId", table: "NormalUsers");
        migrationBuilder.DropColumn(name: "DepartmentId", table: "NormalUsers");
        migrationBuilder.DropColumn(name: "IsConfirmed", table: "TicketMessages");
    }
}
