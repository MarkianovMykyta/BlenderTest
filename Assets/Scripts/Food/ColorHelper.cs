using System.Collections.Generic;
using UnityEngine;

namespace Food
{
    public static class ColorHelper
    {
        public static Color GetColorFromIngredients(List<Ingredient> ingredients)
        {
            var resultColor = Color.black;

            for (int i = 0; i < ingredients.Count; i++)
            {
                resultColor += ingredients[i].Data.Color;
            }

            resultColor /= ingredients.Count;

            return resultColor;
        }
    }
}