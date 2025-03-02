using Godot;
using Godot.Collections;

public partial class Npc : CharacterBody2D
{
	[Export] public string DialogueNode;
	[Export] public Node DesiredItems;

	[Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc);

	public void EmitStartDialogueAttempt()
	{
		EmitSignal(SignalName.StartDialogueAttempt, this);
	}

	public override void _Ready()
	{
		var firstItem = DesiredItems.Call("get_items").As<Array>()[0].As<GodotObject>();
		GD.Print(firstItem.Call("get_prototype").As<GodotObject>().Call("get_id"));
	}
}
