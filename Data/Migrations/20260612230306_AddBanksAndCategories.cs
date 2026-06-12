using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi_money_management.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBanksAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "BCR", "BCR" },
                    { "BRD", "BRD" },
                    { "BT", "Banca Transilvania" },
                    { "CEC", "CEC Bank" },
                    { "ING", "ING Bank" },
                    { "Other", "Other" },
                    { "Raiffeisen", "Raiffeisen Bank" },
                    { "UniCredit", "UniCredit Bank" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Salary", "income" },
                    { 2, "Freelance", "income" },
                    { 3, "Business", "income" },
                    { 4, "Investment", "income" },
                    { 5, "Gift", "income" },
                    { 6, "Refund", "income" },
                    { 7, "Other", "income" },
                    { 8, "Food & Drink", "expense" },
                    { 9, "Transport", "expense" },
                    { 10, "Utilities", "expense" },
                    { 11, "Health", "expense" },
                    { 12, "Shopping", "expense" },
                    { 13, "Entertainment", "expense" },
                    { 14, "Housing", "expense" },
                    { 15, "Education", "expense" },
                    { 16, "Other", "expense" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
