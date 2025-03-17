using Godot;
using Godot.Collections;

public partial class Npc : CharacterBody2D, ICharacterInteractable
{
	[Export] public string DialogueNode;
	[Export] public Array<string> DesiredItemTypesOrNames;

	[Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc, Node2D interactedBy);

	public void Interact(Node2D interactedBy) => EmitSignal(SignalName.StartDialogueAttempt, this, interactedBy);
}
