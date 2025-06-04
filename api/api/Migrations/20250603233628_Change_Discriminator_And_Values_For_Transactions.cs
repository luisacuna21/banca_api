using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Change_Discriminator_And_Values_For_Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionKind",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransferDiscriminator",
                table: "Transactions",
                type: "TEXT",
                maxLength: 55,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferDiscriminator",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionKind",
                table: "Transactions",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
