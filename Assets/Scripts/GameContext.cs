using Popups;

public class GameContext
{
    public GameState GameState { get; private set; }
    public IPopupService PopupService { get; private set; }
    
    public GameContext(GameState gameState, IPopupService popupService)
    {
        GameState = gameState;
        PopupService = popupService;
    }
}