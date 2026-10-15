using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticketing.Infrastructure.Efcore.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261014160000_FixClosedStatusTypo")]
public partial class FixClosedStatusTypo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            "UPDATE [Tickets] SET [TicketLifeCycle] = 'CLOSED' WHERE [TicketLifeCycle] = 'CLOESD';");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            "UPDATE [Tickets] SET [TicketLifeCycle] = 'CLOESD' WHERE [TicketLifeCycle] = 'CLOSED';");
    }
}
