using Godot;

public partial class GlobalState : Node
{
    [Export(PropertyHint.File, "*.json")] public Json PrototypeTreeJson;
    public PrototypeTree PrototypeTree { get; private set; }
    public bool IsInDialogue { get; private set; }
    public bool IsMenuOpen { get; private set; }

    public bool PlayerHasControl() => !IsInDialogue && !IsMenuOpen;

    private void OnDialogueStarted(Npc dialogueNpc) => IsInDialogue = true;
    private void OnDialogueCompleted() => IsInDialogue = false;
    private void OnMenuOpened() => IsMenuOpen = true;
    private void OnMenuClosed() => IsMenuOpen = false;

    public override void _EnterTree() => PrototypeTree = new PrototypeTree(PrototypeTreeJson);
}
