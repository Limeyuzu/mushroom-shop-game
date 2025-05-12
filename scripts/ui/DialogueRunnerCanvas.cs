using Godot;
using YarnSpinnerGodot;

public partial class DialogueRunnerCanvas : CanvasLayer
{
    [Export] public DialogueRunner DialogueRunner;

    [Signal] public delegate void DialogueStartedEventHandler(Npc dialogueNpc);
    [Signal] public delegate void DialogueCompletedEventHandler();
    [Signal] public delegate void OpenInventoryRequestedEventHandler(Inventory inventory, Node requester);

    private Player _player;
    private Npc _currentDialogueNpc;

    public void OnInteractionStartAttempt(Npc dialogueNpc, Node2D interactedBy)
    {
        if (GlobalState.Instance.PlayerHasControl() && interactedBy is PlayerPointer playerPointer)
        {
            DialogueRunner.VariableStorage.SetValue("$desiredItem", GetNpcDesiredItem(dialogueNpc));
            DialogueRunner.StartDialogue(dialogueNpc.DialogueNode);

            EmitSignal(SignalName.DialogueStarted, dialogueNpc);
            _player = playerPointer.Player;
            _currentDialogueNpc = dialogueNpc;
            GD.Print($"{nameof(DialogueRunnerCanvas)}: interaction started by {_player.GetName()}");
        }
    }

    public void OnDialogueComplete() => EmitSignal(SignalName.DialogueCompleted);

    [YarnCommand("OpenInventory")]
    public void OpenInventory() => EmitSignal(SignalName.OpenInventoryRequested, _player.Inventory, this);

    public void OnItemSelected(InventoryItem item, Node requester)
    {
        if (requester != this)
            return;

        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.GetName());
        DialogueRunner.VariableStorage.SetValue("$isDesired", IsDesiredItem(_currentDialogueNpc, item));
        DialogueRunner.StartDialogue("ShowItem");
        EmitSignal(SignalName.DialogueStarted, _currentDialogueNpc);
    }

    private string GetNpcDesiredItem(Npc npc)
    {
        var firstItem = npc.DesiredItemTypesOrNames[0];
        return firstItem;
    }

    private bool IsDesiredItem(Npc npc, InventoryItem item)
    {
        var itemNameOrType = GetNpcDesiredItem(npc);
        GD.Print($"{nameof(DialogueRunnerCanvas)}: desired: {itemNameOrType}");
        var isDesired = item.IsTypeOf(itemNameOrType);
        return isDesired;
    }
}
