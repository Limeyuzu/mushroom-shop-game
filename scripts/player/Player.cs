using System.Collections.Generic;
using Godot;

public partial class Player : Node2D
{
    [Export] public Inventory Inventory;
    [Export] public UICanvas UICanvas;
    [Export] public Vector2 InitialFacing = Vector2.Down;

    [Signal] public delegate void ToggleInventoryWindowRequestedEventHandler(Inventory inventory, Node requester);
    [Signal] public delegate void ResetVelocityEventHandler();

    [Export] private Sprite2D _sprite;

    public bool CanMove() => !UICanvas.IsUIOpen;

    public override void _Ready()
    {
        var directionToFrame = new Dictionary<Vector2, int>
        {
            { Vector2.Down, 1 },
            { Vector2.Left, 4 },
            { Vector2.Right, 7 },
            { Vector2.Up, 10 },
        };

        _sprite.Frame = directionToFrame[InitialFacing];
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory_open"))
        {
            EmitSignal(SignalName.ToggleInventoryWindowRequested, Inventory, this);
        }
    }

    public void OnMapChange() => EmitSignal(SignalName.ResetVelocity);
}
