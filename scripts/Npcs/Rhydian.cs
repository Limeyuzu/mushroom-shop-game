using Godot;
using Godot.Collections;

public partial class Rhydian : CharacterBody2D
{
    [Signal] public delegate void DialogueVariablesReadyEventHandler(Dictionary<string, string> variables);

    public override void _Ready()
        => EmitSignal(SignalName.DialogueVariablesReady,
            new Dictionary<string, string> {
                { "$rhydian_desired_item", "luck potion"},
                { "$rhydian_quest_active", "false" },
                { "$rhydianOpinion", "NEUTRAL" }
            });

}
