using System.ComponentModel.DataAnnotations.Schema;


namespace Recipe_Book.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public decimal Quantity { get; set; }

        [ForeignKey("Unit")]
        public int UnitId { get; set; }
        public Unit Unit {  get; set; }
    }
}
