using Godot;

public partial class ServiceDesk : StaticBody2D, ICharacterInteractable
{
    [Signal] public delegate void ServiceDeskInteractEventHandler();

    public void Interact(Node2D interactedBy) => EmitSignal(SignalName.ServiceDeskInteract);
}
