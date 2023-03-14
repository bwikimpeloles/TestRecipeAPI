namespace TestRecipeAPI.Entities
{
    public class Favourite
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Username { get; set; }
        public bool FavouriteBool { get; set; } = false;
    }
}
