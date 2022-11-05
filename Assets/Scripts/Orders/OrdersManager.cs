using System.Threading.Tasks;
using Food;
using Food.Data;
using Orders.Bubble;

namespace Orders
{
    public class OrdersManager
    {
        private GameContext _gameContext;

        private Order _currentOrder;
        private RecipeData[] _recipes;
        private OrderBubble _bubble;
        private int _currentOrderIndex;

        private TaskCompletionSource<OrderResult> _orderTask;

        public OrdersManager(GameContext gameContext)
        {
            _gameContext = gameContext;
            _recipes = gameContext.OrdersProvider.GetOrders();
            _bubble = gameContext.OrdersProvider.GetOrderBubble();

            gameContext.Blender.IngredientMixed += OnIngredientMixed;
            gameContext.Blender.ShakeCompleted += OnShakeCompleted;

            _currentOrderIndex = 0;
        }

        public async Task<OrderResult> StartOrder()
        {
            _currentOrder = new Order(_recipes[_currentOrderIndex]);
            
            _gameContext.IngredientsManager.Restart();
            _gameContext.Blender.Clean();

            _bubble.Show(ColorHelper.GetColorFromIngredients(_currentOrder.Recipe.Ingredients));
            
            if (_orderTask != null)
            {
                _orderTask.SetResult(null);
            }

            _orderTask = new TaskCompletionSource<OrderResult>();

            return await _orderTask.Task;
        }

        public void PrepareNextOrder()
        {
            _currentOrderIndex++;
            _currentOrderIndex %= _recipes.Length;
        }

        public void RestartCurrentOrder()
        {
            
        }
        
        private void OnIngredientMixed(Ingredient ingredient)
        {
            _currentOrder.AddIngredient(ingredient);
        }
        
        private void OnShakeCompleted()
        {
            var successRate = _currentOrder.SuccessRate;

            var orderResult = new OrderResult()
            {
                Success = successRate > 0.75f,
                SuccessRate = successRate
            };
            
            _orderTask.TrySetResult(orderResult);

            _orderTask = null;
            _currentOrder = null;
        }
    }
}