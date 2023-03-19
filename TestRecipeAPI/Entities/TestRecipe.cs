namespace TestRecipeAPI.Entities
{
    public class TestRecipe
    {
        public int Id { get; set; }

        public string? Name { get; set; } = string.Empty;

        public string? Ingredient { get; set; } = string.Empty;

        public string? Instruction { get; set; } = string.Empty;

        public int? Favourite { get; set; } = 0;

        //NAVIGATION 
        public List<Account>? Accounts { get; set; }

    }
}
