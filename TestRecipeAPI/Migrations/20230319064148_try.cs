using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRecipeAPI.Migrations
{
    /// <inheritdoc />
    public partial class @try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTestRecipe");

            migrationBuilder.CreateTable(
                name: "FavouriteTables",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    TestRecipesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteTables", x => new { x.TestRecipesId, x.AccountsId });
                    table.ForeignKey(
                        name: "FK_FavouriteTables_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteTables_TestRecipes_TestRecipesId",
                        column: x => x.TestRecipesId,
                        principalTable: "TestRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteTables_AccountsId",
                table: "FavouriteTables",
                column: "AccountsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteTables");

            migrationBuilder.CreateTable(
                name: "AccountTestRecipe",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    TestRecipesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTestRecipe", x => new { x.AccountsId, x.TestRecipesId });
                    table.ForeignKey(
                        name: "FK_AccountTestRecipe_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTestRecipe_TestRecipes_TestRecipesId",
                        column: x => x.TestRecipesId,
                        principalTable: "TestRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTestRecipe_TestRecipesId",
                table: "AccountTestRecipe",
                column: "TestRecipesId");
        }
    }
}
