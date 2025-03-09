using Godot;

public partial class Npc : CharacterBody2D, ICharacterInteractable
{
	[Export] public string DialogueNode;
	[Export] private Node _desiredItems;

	[Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc);

	public Inventory DesiredItems;

    public override void _Ready()
	{
		DesiredItems = new Inventory(_desiredItems);
	}

	public void Interact() => EmitSignal(SignalName.StartDialogueAttempt, this);
}
