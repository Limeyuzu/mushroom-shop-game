using Godot;

public partial class GlobalState : Node
{
    public static GlobalState Instance { get; private set; }

    public override void _Ready() => Instance = this;

    public bool IsInDialogue { get; private set; }
    public bool IsMenuOpen { get; private set; }

    public bool PlayerHasControl() => !IsInDialogue && !IsMenuOpen;

    private void OnDialogueStarted(Npc dialogueNpc) => IsInDialogue = true;
    private void OnDialogueCompleted() => IsInDialogue = false;
    private void OnMenuOpened() => IsMenuOpen = true;
    private void OnMenuClosed() => IsMenuOpen = false;
}
