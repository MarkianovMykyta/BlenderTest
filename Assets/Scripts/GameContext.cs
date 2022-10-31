public class GameContext
{
    public GameState GameState { get; private set; }

    public GameContext(GameState gameState)
    {
        GameState = gameState;
    }
}