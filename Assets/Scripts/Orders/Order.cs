using System.Collections.Generic;
using Food;
using Food.Data;
using UnityEngine;

namespace Orders
{
    public class Order
    {
        private List<Ingredient> _addedIngredients;

        public RecipeData Recipe { get; private set; }
        
        public float SuccessRate
        {
            get
            {
                var matchedElements = 0f;

                var addedIngredientsTemp = new List<Ingredient>(_addedIngredients);
                for (var i = 0; i < Recipe.Ingredients.Count; i++)
                {
                    var targetIngredient = Recipe.Ingredients[i];

                    var addedIngredient = addedIngredientsTemp.Find(x => x.Data == targetIngredient.Data);
                    if (addedIngredient != null)
                    {
                        matchedElements++;
                        addedIngredientsTemp.Remove(addedIngredient);
                    }
                }

                var max = Mathf.Max(Recipe.Ingredients.Count, _addedIngredients.Count);
                var rate = matchedElements / max;
                
                return rate;
            }
        }

        public Order(RecipeData recipe)
        {
            Recipe = recipe;

            _addedIngredients = new List<Ingredient>();
        }


        public void AddIngredient(Ingredient ingredient)
        {
            _addedIngredients.Add(ingredient);
        }
    }
}