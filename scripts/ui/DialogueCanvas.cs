using Godot;
using Godot.Collections;
using YarnSpinnerGodot;

public partial class DialogueCanvas : Control
{
    public static DialogueCanvas Instance { get; private set; }

    [Export] public DialogueRunner DialogueRunner;

    [Signal] public delegate void DialogueStartedEventHandler();
    [Signal] public delegate void DialogueCompletedEventHandler();
    [Signal] public delegate void OpenInventoryRequestedEventHandler(Inventory inventory, Node requester);

    public override void _Ready() => Instance = this;

    public void SetVariable(string variableName, string stringValue)
        => DialogueRunner.VariableStorage.SetValue(variableName, stringValue);
    public void SetVariable(string variableName, float floatValue)
        => DialogueRunner.VariableStorage.SetValue(variableName, floatValue);
    public void SetVariable(string variableName, bool boolValue)
        => DialogueRunner.VariableStorage.SetValue(variableName, boolValue);
    public bool TryGetVariable<T>(string variableName, out T result)
    {
        var success = DialogueRunner.VariableStorage.TryGetValue(variableName, out T value);
        result = value;
        return success;
    }

    public async void OnInteractionStartAttempt(string dialogueNode, Player player)
    {
        await DialogueRunner.StartDialogue(dialogueNode);
        EmitSignal(SignalName.DialogueStarted);

        GD.Print($"{nameof(DialogueCanvas)}: interaction started by {player.GetName()}");
    }

    public void OnDialogueComplete() => EmitSignal(SignalName.DialogueCompleted);
}
