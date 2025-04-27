using Godot;

public partial class Cauldron : StaticBody2D, ICharacterInteractable
{
    [Signal] public delegate void OpenInventoryRequestedEventHandler(Node2D interactedBy, Node requester);

    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is PlayerPointer playerPointer)
        {
            EmitSignal(SignalName.OpenInventoryRequested, playerPointer.Player.Inventory, this);
        }
    }
}
