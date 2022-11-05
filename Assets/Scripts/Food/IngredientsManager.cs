using System;
using System.Collections.Generic;
using Food.Flyer;
using Food.Spawner;
using Food.Swiper;
using UnityEngine;

namespace Food
{
    public class IngredientsManager : MonoBehaviour, IIngredientsManager
    {
        public event Action<Ingredient> IngredientClicked; 

        [SerializeField] private IngredientsSpawner _spawner;
        [SerializeField] private IngredientsSwiper _swiper;
        [SerializeField] private IngredientsFlyer _flyer;

        private List<Ingredient> _availableIngredients;

        public void Restart()
        {
            for (int i = _availableIngredients.Count - 1; i >= 0; i--)
            {
                _availableIngredients[i].DestroyIngredient();
            }
            
            _spawner.Activate();
        }
        
        private void Awake()
        {
            _availableIngredients = new List<Ingredient>();

            _flyer.Initialize();
            _spawner.Initialize();
            _swiper.Initialize(_availableIngredients);

            _spawner.IngredientSpawned += OnIngredientBecameAvailable;
        }

        private void OnIngredientBecameAvailable(Ingredient ingredient)
        {
            ingredient.WasDestroyed += OnIngredientDestroyed;
            ingredient.WasClicked += OnIngredientWasClicked;
            
            _availableIngredients.Add(ingredient);
        }

        private void OnIngredientDestroyed(Ingredient ingredient)
        {
            ingredient.WasDestroyed -= OnIngredientDestroyed;
            ingredient.WasClicked -= OnIngredientWasClicked;

            _availableIngredients.Remove(ingredient);
        }
        
        private void OnIngredientWasClicked(Ingredient ingredient)
        {
            _flyer.LaunchIngredientToTarget(ingredient);
            IngredientClicked?.Invoke(ingredient);
        }
    }
}