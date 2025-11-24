using Godot;

public partial class ShopStartChair : StaticBody2D, ICharacterInteractable
{
    [Export] Node2D NavigationDestination;

    [Signal] public delegate void ShopStartEventHandler(Vector2 npcDestination);

    public void Interact(Node2D interactedBy)
    {
        GD.Print("start shop, destination: " + NavigationDestination.GlobalPosition);
        EmitSignal(SignalName.ShopStart, NavigationDestination.GlobalPosition);
    }
}
