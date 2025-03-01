using Godot;

public partial class Player : Node2D
{
    [Export] private GlobalState _globalState;

    public bool CanMove() => _globalState.PlayerHasControl();
}
