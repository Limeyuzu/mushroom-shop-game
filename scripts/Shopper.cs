using Godot;
using Godot.Collections;

public partial class Shopper : CharacterBody2D, INavigator
{
    [Export] private NavigationAgent2D _navigationAgent2D;
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private float _movementSpeed = 50f;

    [Signal] public delegate void DialogueVariablesReadyEventHandler(Dictionary<string, string> variables);
    [Signal] public delegate void ShopCounterInteractEventHandler(Player player);

    private float _movementDelta;

    public override void _Ready()
    {
        EmitSignal(SignalName.DialogueVariablesReady,
            new Dictionary<string, string> { { "$desiredItem", GD.Randi() % 2 == 0 ? "weapon" : "potion" } });
    }

    public void SetDestination(Vector2 dest)
    {
        _navigationAgent2D.TargetPosition = dest;
    }

    public void OnShopCounterInteract(Player player)
        => EmitSignal(SignalName.ShopCounterInteract, player);

    public override void _PhysicsProcess(double delta)
    {
        NavigatePath(delta);
        UpdateAnimation();
    }

    private void NavigatePath(double delta)
    {
        // Do not query when the map has never synchronized and is empty.
        if (NavigationServer2D.MapGetIterationId(_navigationAgent2D.GetNavigationMap()) == 0)
        {
            return;
        }

        if (_navigationAgent2D.IsNavigationFinished())
        {
            _movementDelta = 0;
            return;
        }

        _movementDelta = _movementSpeed * (float)delta;
        var nextPathPosition = _navigationAgent2D.GetNextPathPosition();
        var newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;

        var collision = MoveAndCollide(newVelocity, testOnly: true);
        if (collision != null)
        {
            _movementDelta = 0;
            newVelocity = Vector2.Zero;
        }
        MoveAndCollide(newVelocity);
    }

    private void UpdateAnimation()
    {
        if (_navigationAgent2D.IsNavigationFinished() || _movementDelta < 0.01f)
        {
            _animationPlayer.Stop();
            return;
        }

        var nextPathPosition = _navigationAgent2D.GetNextPathPosition();
        var direction = GlobalPosition.DirectionTo(nextPathPosition);

        string animationName = (direction.Angle() / Mathf.Pi) switch
        {
            > -0.25f and < 0.25f => "walk_right",
            > 0.25f and < 0.75f => "walk_down",
            > 0.75f => "walk_left",
            < -0.75f => "walk_left",
            > -0.75f and < -0.25f => "walk_up",
            _ => "RESET"
        };
        _animationPlayer.Play(animationName);
    }
}
