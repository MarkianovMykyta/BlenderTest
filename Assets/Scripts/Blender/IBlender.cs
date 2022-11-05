using System;
using Food;

namespace Blender
{
    public interface IBlender
    {
        event Action<Ingredient> IngredientMixed;
        event Action ShakeCompleted;

        void Clean();
    }
}