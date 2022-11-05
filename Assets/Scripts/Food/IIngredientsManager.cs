using System;

namespace Food
{
    public interface IIngredientsManager
    {
        event Action<Ingredient> IngredientClicked;

        void Restart();
    }
}