using Godot;

public partial class ShopStartChair : StaticBody2D, ICharacterInteractable
{
    [Export] Node2D NavigationDestination;

    [Signal] public delegate void ShopStartChairInteractionEventHandler(Vector2 npcDestination);

    public void Interact(Node2D interactedBy)
        => EmitSignal(SignalName.ShopStartChairInteraction, NavigationDestination.GlobalPosition);
}
