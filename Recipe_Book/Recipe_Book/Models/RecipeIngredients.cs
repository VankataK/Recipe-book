using System.ComponentModel.DataAnnotations.Schema;


namespace Recipe_Book.Models
{
    public class RecipeIngredients
    {
        public int Id { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int Quantity { get; set; }

        public string Unit {  get; set; }
    }
}
