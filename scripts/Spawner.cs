using Godot;

public partial class Spawner : Node2D
{
    [Export] PackedScene ObjectToSpawn;

    private int _numberToSpawn = 1;
    private Vector2 _destination;
    private bool _startSpawning;
    private int _spawnedAmount;

    public void SetNumberToSpawn(int num) => _numberToSpawn = num;

    public void SetDestination(Vector2 destination)
    {
        _destination = destination;
        _startSpawning = true;
    }

    public void OnTimeout()
    {
        if (!_startSpawning) return;

        var spawn = ObjectToSpawn.Instantiate<Node2D>();

        if (_destination != Vector2.Zero && spawn is INavigator navigator)
        {
            navigator.SetDestination(_destination);
        }

        AddChild(spawn);

        _spawnedAmount++;
        if (_spawnedAmount >= _numberToSpawn)
        {
            Reset();
        }
    }

    public void Reset()
    {
        _startSpawning = false;
        _spawnedAmount = 0;
    }
}
