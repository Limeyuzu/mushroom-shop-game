using Godot;

public partial class Npc : CharacterBody2D
{
    [Export] public GlobalState GlobalState;
    [Export] public string DialogueNode;

    [Signal] public delegate void StartDialogueAttemptEventHandler(string dialogueNode);

    public void EmitStartDialogueAttempt()
    {
        EmitSignal(SignalName.StartDialogueAttempt, DialogueNode);
    }
}
