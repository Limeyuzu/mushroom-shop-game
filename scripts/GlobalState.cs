using Godot;

public partial class GlobalState : Node
{
    public bool IsInDialogue { get; private set;}
    public bool IsInventoryOpen { get; private set; }

    public bool PlayerHasControl() => !IsInDialogue && !IsInventoryOpen;

    private void OnDialogueStarted(Npc dialogueNpc) => IsInDialogue = true;
    private void OnDialogueCompleted() => IsInDialogue = false;
    private void OnInventoryOpened() => IsInventoryOpen = true;
    private void OnInventoryClosed() => IsInventoryOpen = false;
}
