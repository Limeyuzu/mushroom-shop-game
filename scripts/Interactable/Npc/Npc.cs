using Godot;
using Godot.Collections;

public partial class Npc : CharacterBody2D, ICharacterInteractable
{
	[Export] public string DialogueNode;
	[Export] public Array<string> DesiredItemTypesOrNames;

	[Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc, Player interactedBy);

	public override void _Ready()
	{
		if (DesiredItemTypesOrNames.Count == 0)
		{
			DesiredItemTypesOrNames = GD.Randi() % 2 == 0 ? ["weapon"] : ["potion"];
		}
	}

	public void Interact(Node2D interactedBy) => EmitSignal(SignalName.StartDialogueAttempt, this, (Player)interactedBy);
}
