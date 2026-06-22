using System.Collections.Generic;
using Godot;

public partial class Shopper : Node2D, INavigator, IItemHandler
{
    [Export] public string ShopperDialogueStartNode = "Shopper";
    [Signal] public delegate void SetNavigationDestinationEventHandler(Vector2 destination);

    private Player _interactingWith;
    private string _shopperShowItemDialogueNode;
    private List<string> _desiredItems = ["weapon", "potion"];
    private string _desiredItem;

    public override void _Ready()
    {
        _desiredItem = _desiredItems[(int)(GD.Randi() % _desiredItems.Count)];
        _shopperShowItemDialogueNode = $"{ShopperDialogueStartNode}ShowItem";
    }

    public void ShopCounterInteract(Player interactedBy)
    {
        _interactingWith = interactedBy;
        // TODO this sets $desiredItem for all instances of this npc type, 
        // what happens if dialogue is interrupted and a new dialogue starts with another instance
        DialogueCanvas.Instance.SetVariable("$desiredItem", _desiredItem);
        UICanvas.Instance.StartDialogue(ShopperDialogueStartNode, interactedBy);
    }

    public void HandleItem(InventoryItem item)
    {
        DialogueCanvas.Instance.SetVariable("$selectedItem", item.GetName());
        DialogueCanvas.Instance.SetVariable("$isDesired", IsDesiredItem(item));
        UICanvas.Instance.StartDialogue(_shopperShowItemDialogueNode, _interactingWith);
    }

    public void SetDestination(Vector2 destination) => EmitSignal(SignalName.SetNavigationDestination, destination);

    private bool IsDesiredItem(InventoryItem item)
    {
        GD.Print($"{nameof(DialogueCanvas)}: desired: {_desiredItem}");
        var isDesired = item.IsTypeOf(_desiredItem);
        return isDesired;
    }
}
