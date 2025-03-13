using Godot;
using YarnSpinnerGodot;

public partial class DialogueRunnerCanvas : CanvasLayer
{
    [Export] public DialogueRunner DialogueRunner;
    [Export] private GlobalState _globalState;

    [Signal] public delegate void DialogueStartedEventHandler(Npc dialogueNpc);
    [Signal] public delegate void DialogueCompletedEventHandler();
    [Signal] public delegate void OpenInventoryRequestedEventHandler(Node requester);

    private Npc _currentDialogueNpc { get; set; }

    public void OnInteractionStartAttempt(Npc dialogueNpc)
    {
        if (_globalState.PlayerHasControl())
        {
            DialogueRunner.StartDialogue(dialogueNpc.DialogueNode);
            EmitSignal(SignalName.DialogueStarted, dialogueNpc);
            _currentDialogueNpc = dialogueNpc;
        }
    }

    public void OnDialogueComplete() => EmitSignal(SignalName.DialogueCompleted);

    [YarnCommand("OpenInventory")]
    public void OpenInventory() => EmitSignal(SignalName.OpenInventoryRequested, this);

    public void OnItemSelected(InventoryItem item, Node requester)
    {
        if (requester != this)
            return;

        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.GetName());
        DialogueRunner.VariableStorage.SetValue("$desiredItem", GetNpcDesiredItem(_currentDialogueNpc));
        DialogueRunner.VariableStorage.SetValue("$isDesired", IsDesiredItem(_currentDialogueNpc, item));
        DialogueRunner.StartDialogue("ShowItem");
    }

    private string GetNpcDesiredItem(Npc npc)
    {
        var firstItem = npc.DesiredItemTypesOrNames[0];
        return firstItem;
    }

    private bool IsDesiredItem(Npc npc, InventoryItem item)
    {
        var itemNameOrType = GetNpcDesiredItem(npc);
        GD.Print("desired: " + itemNameOrType);
        var isDesired = item.GetPrototype().IsTypeOf(itemNameOrType);
        return isDesired;
    }
}
