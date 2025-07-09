using Godot;

public partial class ShopStartChair : StaticBody2D, ICharacterInteractable
{
    public void Interact(Node2D interactedBy)
    {
        GD.Print("start shop");
    }
}
