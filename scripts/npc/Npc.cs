using Godot;

public partial class Npc : CharacterBody2D, ICharacterInteractable
{
	[Export] public string DialogueNode;
	[Export] public Inventory DesiredItems;

	[Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc);

	public void Interact() => EmitSignal(SignalName.StartDialogueAttempt, this);
}
