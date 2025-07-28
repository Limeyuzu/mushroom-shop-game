using Godot;
using Godot.Collections;

public partial class Dude : CharacterBody2D
{
    [Signal] public delegate void DialogueVariablesReadyEventHandler(Dictionary<string, string> variables);

    public override void _Ready()
        => EmitSignal(SignalName.DialogueVariablesReady,
            new Dictionary<string, string> { { "$desiredItem", GD.Randi() % 2 == 0 ? "weapon" : "potion" } });
}
