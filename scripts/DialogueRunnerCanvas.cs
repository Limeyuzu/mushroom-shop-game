using Godot;
using Godot.Collections;
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

    public void OnItemSelected(GodotObject item)
    {
        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.Call("get_title").As<string>());
        DialogueRunner.VariableStorage.SetValue("$desiredItem", GetNpcDesiredItem(_currentDialogueNpc).Call("get_title").As<string>());
        DialogueRunner.VariableStorage.SetValue("$isDesired", IsDesiredItem(_currentDialogueNpc, item));
        DialogueRunner.StartDialogue("ShowItem");
    }

    private GodotObject GetNpcDesiredItem(Npc npc)
    {
        var firstItem = npc.DesiredItems.Call("get_items").As<Array>()[0].As<GodotObject>();
        return firstItem;
    }

    private bool IsDesiredItem(Npc npc, GodotObject item)
    {
        var firstItem = npc.DesiredItems.Call("get_items").As<Array>()[0].As<GodotObject>();
        var itemId = firstItem.Call("get_prototype").As<GodotObject>().Call("get_id");
        GD.Print("desired: " + itemId);
        GD.Print("offered: " + item.Call("get_title"));
        var isDesired = item.Call("get_prototype").As<GodotObject>().Call("inherits", itemId).As<bool>();
        return isDesired;
    } 
}
