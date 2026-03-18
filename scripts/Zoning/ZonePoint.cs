using Godot;

public partial class ZonePoint : Area2D
{
    [Export] public string NextScenePath;
    [Export] public string DestinationSpawnId;
    [Signal] public delegate void MapChangeEventHandler(string scenePath, string spawnId);

    public void OnBodyEntered(Node2D body)
    {
        var ownerNode = body.GetOwner();
        if (ownerNode is Player)
        {
            EmitSignal(SignalName.MapChange, NextScenePath, DestinationSpawnId);
        }
        else
        {
            ownerNode.QueueFree();
        }
    }
}
