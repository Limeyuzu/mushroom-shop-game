using Godot;

public partial class MapLoader : Node2D
{
    [Export] private string _initialScenePath;
    [Export] private string _initialSpawnLocationId;
    [Export] private Player _player;

    public override void _Ready() => LoadMap(_initialScenePath, _initialSpawnLocationId);

    public void LoadMap(string scenePath, string spawnId)
    {
        Callable.From(() =>
        {
            if (GetChildCount() > 0)
            {
                GetChild(0).QueueFree();
            }
            var newScene = (MapScene)ResourceLoader.Load<PackedScene>(scenePath).Instantiate();
            AddChild(newScene);

            newScene.SpawnPlayer(_player, spawnId);
            _player.OnMapChange();
            newScene.MapChange += (scenePath, spawnId) => LoadMap(scenePath, spawnId);
        }).CallDeferred();
    }
}
