using Godot;
using Godot.Collections;
using YarnSpinnerGodot;

public partial class DialogueCanvas : Control
{
    [Export] public DialogueRunner DialogueRunner;

    [Signal] public delegate void DialogueStartedEventHandler();
    [Signal] public delegate void DialogueCompletedEventHandler();
    [Signal] public delegate void OpenInventoryRequestedEventHandler(Inventory inventory, Node requester);

    private Player _player;
    private Dictionary<string, string> _currentDialogueVariables;

    public void OnInteractionStartAttempt(string dialogueNode, Dictionary<string, string> variables, Player player)
    {
        foreach (var kvp in variables)
        {
            DialogueRunner.VariableStorage.SetValue(kvp.Key, kvp.Value);
        }

        DialogueRunner.StartDialogue(dialogueNode);
        EmitSignal(SignalName.DialogueStarted);

        _player = player;
        _currentDialogueVariables = variables;
        GD.Print($"{nameof(DialogueCanvas)}: interaction started by {_player.GetName()}");
    }

    public void OnDialogueComplete() => EmitSignal(SignalName.DialogueCompleted);

    [YarnCommand("OpenInventory")]
    public void OpenInventory() => EmitSignal(SignalName.OpenInventoryRequested, _player.Inventory, this);

    public void OnItemSelected(InventoryItem item, Node requester)
    {
        if (requester != this)
            return;

        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.GetName());
        DialogueRunner.VariableStorage.SetValue("$isDesired", IsDesiredItem(item));
        DialogueRunner.StartDialogue("ShowItem");
        EmitSignal(SignalName.DialogueStarted);
    }

    private bool IsDesiredItem(InventoryItem item)
    {
        var exists = _currentDialogueVariables.TryGetValue("$desiredItem", out var itemNameOrType);
        if (!exists) return false;

        GD.Print($"{nameof(DialogueCanvas)}: desired: {itemNameOrType}");
        var isDesired = item.IsTypeOf(itemNameOrType);
        return isDesired;
    }
}
