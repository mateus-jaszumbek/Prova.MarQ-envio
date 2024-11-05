using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prova.MarQ.Infra.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTimeEntry");

            migrationBuilder.CreateIndex(
                name: "IX_TbtimeEntries_EmployeeId",
                table: "TbtimeEntries",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbtimeEntries_Tbemployees_EmployeeId",
                table: "TbtimeEntries",
                column: "EmployeeId",
                principalTable: "Tbemployees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbtimeEntries_Tbemployees_EmployeeId",
                table: "TbtimeEntries");

            migrationBuilder.DropIndex(
                name: "IX_TbtimeEntries_EmployeeId",
                table: "TbtimeEntries");

            migrationBuilder.CreateTable(
                name: "EmployeeTimeEntry",
                columns: table => new
                {
                    EmployeesEmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeEntriesTimeEntryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTimeEntry", x => new { x.EmployeesEmployeeId, x.TimeEntriesTimeEntryId });
                    table.ForeignKey(
                        name: "FK_EmployeeTimeEntry_Tbemployees_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Tbemployees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTimeEntry_TbtimeEntries_TimeEntriesTimeEntryId",
                        column: x => x.TimeEntriesTimeEntryId,
                        principalTable: "TbtimeEntries",
                        principalColumn: "TimeEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTimeEntry_TimeEntriesTimeEntryId",
                table: "EmployeeTimeEntry",
                column: "TimeEntriesTimeEntryId");
        }
    }
}
