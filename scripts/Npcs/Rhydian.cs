using Godot;
using Godot.Collections;
using YarnSpinnerGodot;

public partial class Rhydian : Node2D, ICharacterInteractable, IItemHandler
{
    [Export] public string DefaultDialogueNode;

    private Player _interactingWith;

    public void Interact(Node2D interactedBy)
    {
        _interactingWith = (Player)interactedBy;
        UICanvas.Instance.StartDialogue(DefaultDialogueNode, _interactingWith);
    }

    [YarnCommand("OpenInventory")]
    public void OpenInventory() => UICanvas.Instance.OpenInventory(_interactingWith.Inventory, this);

    public void HandleItem(InventoryItem item)
    {
        DialogueCanvas.Instance.SetVariable("$rhydian_shown_item", item.GetName());
        UICanvas.Instance.StartDialogue("ShowRhydianItem", _interactingWith);
    }
}
