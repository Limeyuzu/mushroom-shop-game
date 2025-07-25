using Godot;

public partial class PlayerInteractionPointer : Area2D
{
    [Export] public Player Player;
    [Export] public float DistanceFromPlayer = 10;

    public override void _Process(double delta)
    {
        if (Player.CanMove())
        {
            UpdateFacing();
        }
    }

    private void UpdateFacing()
    {
        if (Input.IsActionPressed("ui_right"))
        {
            Position = new Vector2(DistanceFromPlayer, 0);
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            Position = new Vector2(-DistanceFromPlayer, 0);
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            Position = new Vector2(0, -DistanceFromPlayer);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Position = new Vector2(0, DistanceFromPlayer);
        }
    }
}
