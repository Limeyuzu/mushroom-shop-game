using System.Linq;
using Godot;
using Godot.Collections;

public partial class MapScene : Node2D
{
    [Export] public Array<SpawnPoint> SpawnPoints;
    [Signal] public delegate void MapChangeEventHandler(string scenePath, string destinationSpawnId);

    public void OnZonePointEntered(string scenePath, string destinationSpawnId)
    {
        EmitSignal(SignalName.MapChange, scenePath, destinationSpawnId);
    }

    public void SpawnPlayer(Player player, string spawnId)
    {
        var spawnPoint = SpawnPoints.First(s => s.SpawnId == spawnId);
        player.GlobalPosition = spawnPoint.GlobalPosition;
    }
}
