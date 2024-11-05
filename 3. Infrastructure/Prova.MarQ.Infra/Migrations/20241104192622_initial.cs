using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prova.MarQ.Infra.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbcompanies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Document = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbcompanies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "TbtimeEntries",
                columns: table => new
                {
                    TimeEntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsEntry = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbtimeEntries", x => x.TimeEntryId);
                });

            migrationBuilder.CreateTable(
                name: "Tbemployees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Document = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PIN = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbemployees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Tbemployees_Tbcompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbcompanies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Tbemployees_CompanyId",
                table: "Tbemployees",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTimeEntry");

            migrationBuilder.DropTable(
                name: "Tbemployees");

            migrationBuilder.DropTable(
                name: "TbtimeEntries");

            migrationBuilder.DropTable(
                name: "Tbcompanies");
        }
    }
}
