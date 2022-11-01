using System.Threading.Tasks;

namespace Orders
{
    public class OrdersManager
    {
        private GameContext _gameContext;

        public OrdersManager(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task<OrderResult> StartOrder()
        {
            await Task.Delay(100);//TODO: Delete

            return new OrderResult()
            {
                SuccessRate = 0.5f,
                Success = true
            };
        }

        public void PrepareNextOrder()
        {
            
        }

        public void RestartCurrentOrder()
        {
            
        }
    }
}