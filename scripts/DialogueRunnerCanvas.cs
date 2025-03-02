using Godot;
using YarnSpinnerGodot;

public partial class DialogueRunnerCanvas : CanvasLayer
{
    [Export] public DialogueRunner DialogueRunner;
    [Export] private GlobalState _globalState;

    [Signal] public delegate void DialogueStartedEventHandler();
    [Signal] public delegate void DialogueCompletedEventHandler();

    public void OnItemSelected(GodotObject item)
    {
        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.Call("get_title").As<string>());
        DialogueRunner.StartDialogue("ShowItem");
    }

    public void OnInteractionStartAttempt(string dialogueNode)
    {
        if (_globalState.PlayerHasControl())
        {
            DialogueRunner.StartDialogue(dialogueNode);
        }
    }

    // From DialogueRunner
    public void OnDialogueStart() => EmitSignal(SignalName.DialogueStarted);
    public void OnDialogueComplete() => EmitSignal(SignalName.DialogueCompleted);
}
