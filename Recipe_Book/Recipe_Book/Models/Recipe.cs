﻿

namespace Recipe_Book.Models
{
    public class Recipe
    {

        public Recipe(string name, string description, string ingredients, string author, string category)
        {

            Name = name;
            Description = description;
            Ingredients = ingredients;
            Author = author;
            Category = category;
            RecipeIngredients = new List<RecipeIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Author { get; set; }
        public DateTime AddDate { get; set; }
        public string Category { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
