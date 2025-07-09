using Godot;

public partial class GlobalState : Node
{
    public static GlobalState Instance { get; private set; }

    public override void _Ready() => Instance = this;
}
