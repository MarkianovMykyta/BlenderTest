using Food.Data;
using Orders.Bubble;
using UnityEngine;

namespace Orders
{
    public class OrdersProvider : MonoBehaviour
    {
        [SerializeField] private RecipeData[] _recieps;
        [SerializeField] private OrderBubble _orderBubble;

        public RecipeData[] GetOrders()
        {
            return _recieps;
        }

        public OrderBubble GetOrderBubble()
        {
            return _orderBubble;
        }
    }
}