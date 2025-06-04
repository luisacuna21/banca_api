using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Add_TransferIn_TransferOut_Support : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceAccountId",
                table: "Transactions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceAccountId",
                table: "Transactions");
        }
    }
}
