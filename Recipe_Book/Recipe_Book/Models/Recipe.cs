using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Book.Models
{
    public class Recipe
    {

        public Recipe()
        {
            RecipeIngredients = new List<RecipeIngredient>();
        }

        public Recipe(string name, string description, string author, int categoryId)
        {
            Name = name;
            Description = description;
            Author = author;
            CategoryId = categoryId;
            RecipeIngredients = new List<RecipeIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime AddDate { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
