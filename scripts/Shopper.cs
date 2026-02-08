using Godot;
using Godot.Collections;
using YarnSpinnerGodot;

public partial class Shopper : Node2D, INavigator
{
    [Signal] public delegate void DialogueVariablesReadyEventHandler(Dictionary<string, string> variables);
    [Signal] public delegate void SetNavigationDestinationEventHandler(Vector2 destination);

    private Player _interactingWith;
    private Dictionary<string, string> _dialogueVariables;

    public override void _Ready()
    {
        _dialogueVariables = new Dictionary<string, string> { { "$desiredItem", GD.Randi() % 2 == 0 ? "weapon" : "potion" } };
        EmitSignal(SignalName.DialogueVariablesReady, _dialogueVariables);
    }

    public void ShopCounterInteract(Player interactedBy)
    {
        _interactingWith = interactedBy;
        UICanvas.Instance.StartDialogue("Shopper", interactedBy, _dialogueVariables);
    }

    [YarnCommand("ShopperOpenInventory")]
    public void OpenInventory() => UICanvas.Instance.OpenInventory(_interactingWith.Inventory, this);

    public void SetDestination(Vector2 destination) => EmitSignal(SignalName.SetNavigationDestination, destination);
}
