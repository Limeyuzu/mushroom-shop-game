using Godot;
using YarnSpinnerGodot;

public partial class DialogueRunnerCanvas : CanvasLayer
{
    [Export] public DialogueRunner DialogueRunner;
    [Export] private GlobalState _globalState;

    [Signal] public delegate void DialogueStartedEventHandler(Npc dialogueNpc);
    [Signal] public delegate void DialogueCompletedEventHandler();

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

    public void OnDialogueComplete()
    {
        EmitSignal(SignalName.DialogueCompleted);
    }

    public void OnItemSelected(InventoryItem item)
    {
        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.GetTitle());
        DialogueRunner.VariableStorage.SetValue("$desiredItem", GetNpcDesiredItem(_currentDialogueNpc).GetTitle());
        DialogueRunner.VariableStorage.SetValue("$isDesired", IsDesiredItem(_currentDialogueNpc, item));
        DialogueRunner.StartDialogue("ShowItem");
    }

    private InventoryItem GetNpcDesiredItem(Npc npc)
    {
        var firstItem = npc.DesiredItems.GetItems()[0];
        return firstItem;
    }

    private bool IsDesiredItem(Npc npc, InventoryItem item)
    {
        var itemId = GetNpcDesiredItem(npc).GetPrototype().GetId();
        GD.Print("desired: " + itemId);
        GD.Print("offered: " + item.GetTitle());
        var isDesired = item.GetPrototype().Inherits(itemId);
        return isDesired;
    }
}
