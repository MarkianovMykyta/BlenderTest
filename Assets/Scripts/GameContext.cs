using Blender;
using Food;
using Orders;
using Popups;

public class GameContext
{
    public GameState GameState { get; private set; }
    public IPopupService PopupService { get; private set; }
    public OrdersProvider OrdersProvider { get; private set; }
    public IBlender Blender { get; private set; }
    public IIngredientsManager IngredientsManager { get; private set; }
    
    public GameContext(GameState gameState, IPopupService popupService, OrdersProvider ordersProvider, IBlender blender, IIngredientsManager ingredientsManager)
    {
        Blender = blender;
        GameState = gameState;
        PopupService = popupService;
        OrdersProvider = ordersProvider;
        IngredientsManager = ingredientsManager;
    }
}