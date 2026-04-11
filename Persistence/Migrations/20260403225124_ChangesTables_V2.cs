using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangesTables_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_PayerId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PayerId",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Expenses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpensePayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AmountPaid = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePayments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensePayments_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ApplicationUserId",
                table: "Expenses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_ExpenseId",
                table: "ExpensePayments",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_UserId",
                table: "ExpensePayments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_ApplicationUserId",
                table: "Expenses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_ApplicationUserId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpensePayments");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ApplicationUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Expenses");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PayerId",
                table: "Expenses",
                column: "PayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_PayerId",
                table: "Expenses",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
