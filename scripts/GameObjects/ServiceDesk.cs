using Godot;

public partial class ServiceDesk : StaticBody2D, ICharacterInteractable
{
    [Export] Node2D NavigationDestination;

    [Signal] public delegate void ServiceDeskInteractEventHandler();

    public void Interact(Node2D interactedBy)
    {
        GD.Print("service desk interact: ");
        EmitSignal(SignalName.ServiceDeskInteract);
    }
}
