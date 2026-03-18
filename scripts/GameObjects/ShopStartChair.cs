using Godot;

public partial class ShopStartChair : StaticBody2D, ICharacterInteractable
{
    [Signal] public delegate void ShopStartChairInteractionEventHandler();

    public void Interact(Node2D interactedBy) => EmitSignal(SignalName.ShopStartChairInteraction);
}
