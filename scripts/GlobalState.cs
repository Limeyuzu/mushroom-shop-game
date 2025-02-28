using Godot;
using YarnSpinnerGodot;

public partial class GlobalState : Node
{
    public DialogueRunner DialogueRunner => _dialogueRunnerCanvas.DialogueRunner;
    [Export] private DialogueRunnerCanvas _dialogueRunnerCanvas;
}
