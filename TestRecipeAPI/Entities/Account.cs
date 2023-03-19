using System.ComponentModel.DataAnnotations;

namespace TestRecipeAPI.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }

        //NAVIGATION 
        public List <TestRecipe>? TestRecipes { get; set; }

    }
}
