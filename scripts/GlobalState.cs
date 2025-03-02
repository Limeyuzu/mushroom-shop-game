using Godot;

public partial class GlobalState : Node
{
    private bool IsDialogueRunning = false;
    private bool IsInventoryOpen = false;

    public bool PlayerHasControl() => !IsDialogueRunning && !IsInventoryOpen;

    public void OnDialogueStarted() => IsDialogueRunning = true;
    public void OnDialogueCompleted() => IsDialogueRunning = false;
    public void OnInventoryOpened() => IsInventoryOpen = true;
    public void OnInventoryClosed() => IsInventoryOpen = false;
}
