using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HH");

            migrationBuilder.CreateTable(
                name: "Operation",
                schema: "HH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationPlanned",
                schema: "HH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationPlanned", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationPlanned_Operation",
                        column: x => x.OperationId,
                        principalSchema: "HH",
                        principalTable: "Operation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperationStarted",
                schema: "HH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationPlannedId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationStarted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationStarted_Operation",
                        column: x => x.OperationPlannedId,
                        principalSchema: "HH",
                        principalTable: "OperationPlanned",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationPlanned_OperationId",
                schema: "HH",
                table: "OperationPlanned",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationStarted_OperationPlannedId",
                schema: "HH",
                table: "OperationStarted",
                column: "OperationPlannedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationStarted",
                schema: "HH");

            migrationBuilder.DropTable(
                name: "OperationPlanned",
                schema: "HH");

            migrationBuilder.DropTable(
                name: "Operation",
                schema: "HH");
        }
    }
}
