using Godot;
using YarnSpinnerGodot;

public partial class GlobalState : Node
{
    [Export] private DialogueRunnerCanvas _dialogueRunnerCanvas;
    [Export] private InventoryWrapper _inventoryWrapper;

    public DialogueRunner DialogueRunner => _dialogueRunnerCanvas.DialogueRunner;
    public InventoryWrapper InventoryWrapper => _inventoryWrapper;

    public bool PlayerHasControl() => !DialogueRunner.IsDialogueRunning && !InventoryWrapper.Visible;
}
