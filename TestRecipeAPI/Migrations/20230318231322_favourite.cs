using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRecipeAPI.Migrations
{
    /// <inheritdoc />
    public partial class favourite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favourites");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTestRecipe");

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    TestRecipeId = table.Column<int>(type: "int", nullable: true),
                    FavouriteBool = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favourites_TestRecipes_TestRecipeId",
                        column: x => x.TestRecipeId,
                        principalTable: "TestRecipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_AccountId",
                table: "Favourites",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_TestRecipeId",
                table: "Favourites",
                column: "TestRecipeId");
        }
    }
}
