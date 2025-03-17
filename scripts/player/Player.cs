using Godot;

public partial class Player : Node2D
{
    [Export] private GlobalState _globalState;
    [Export] public Inventory Inventory;

    public bool CanMove() => _globalState.PlayerHasControl();
}
