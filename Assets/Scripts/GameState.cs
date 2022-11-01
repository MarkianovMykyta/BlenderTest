using System;

public class GameState
{
    public enum SateType {Paused, InGame}
    
    public event Action<SateType> StateChanged;

    private SateType _state;

    public SateType State
    {
        get => _state;
        set
        {
            if(_state == value) return;

            _state = value;
            
            StateChanged?.Invoke(_state);
        }
    }
}
