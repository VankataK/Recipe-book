using System.ComponentModel.DataAnnotations.Schema;


namespace Recipe_Book.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

    }
}
