using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_CreditorId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_DebtorId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_AspNetUsers_PayeeId",
                table: "Settlements");

            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_AspNetUsers_PayerId",
                table: "Settlements");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RefreshTokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "GroupMembers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ExpenseSplits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Categories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_ApplicationUserId",
                table: "GroupMembers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseSplits_ApplicationUserId",
                table: "ExpenseSplits",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ApplicationUserId1",
                table: "Categories",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_CreditorId",
                table: "Balances",
                column: "CreditorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_DebtorId",
                table: "Balances",
                column: "DebtorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId1",
                table: "Categories",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseSplits_AspNetUsers_ApplicationUserId",
                table: "ExpenseSplits",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_ApplicationUserId",
                table: "GroupMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_AspNetUsers_PayeeId",
                table: "Settlements",
                column: "PayeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_AspNetUsers_PayerId",
                table: "Settlements",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_CreditorId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_DebtorId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId1",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseSplits_AspNetUsers_ApplicationUserId",
                table: "ExpenseSplits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_ApplicationUserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_AspNetUsers_PayeeId",
                table: "Settlements");

            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_AspNetUsers_PayerId",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_ApplicationUserId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseSplits_ApplicationUserId",
                table: "ExpenseSplits");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ApplicationUserId1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ExpenseSplits");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Categories");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_CreditorId",
                table: "Balances",
                column: "CreditorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_DebtorId",
                table: "Balances",
                column: "DebtorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                table: "Expenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_AspNetUsers_PayeeId",
                table: "Settlements",
                column: "PayeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_AspNetUsers_PayerId",
                table: "Settlements",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
