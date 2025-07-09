using Godot;

public partial class ZonePoint : Area2D
{
    [Export] public string NextScenePath;
    [Export] public string DestinationSpawnId;
    [Signal] public delegate void MapChangeEventHandler(string scenePath, string spawnId);

    public void OnAreaEntered(Area2D area) => EmitSignal(SignalName.MapChange, NextScenePath, DestinationSpawnId);
}
