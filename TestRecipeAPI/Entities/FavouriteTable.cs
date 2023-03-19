namespace TestRecipeAPI.Entities
{
    public class FavouriteTable
    {
        public int AccountsId { get; set; }
        public int TestRecipesId { get; set; }
        public Account Account { get; set; }
        public TestRecipe TestRecipe { get; set; }
    }
}
