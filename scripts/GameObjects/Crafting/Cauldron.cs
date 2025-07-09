using Godot;

public partial class Cauldron : StaticBody2D, ICharacterInteractable
{
    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is Player player)
        {
            UICanvas.Instance.OpenElementalCrafting(player.Inventory, this);
        }
    }
}
