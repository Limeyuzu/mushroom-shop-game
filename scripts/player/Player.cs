using Godot;

public partial class Player : Node2D
{
    [Export] public Inventory Inventory;

    public bool CanMove() => this.ShopGlobal().PlayerHasControl();
}
