using System;
using Godot;
using Godot.Collections;

public partial class Dude : CharacterBody2D
{
    [Export] private NavigationAgent2D _navigationAgent2D;
    [Export] private AnimationPlayer _animationPlayer;

    [Signal] public delegate void DialogueVariablesReadyEventHandler(Dictionary<string, string> variables);

    private float _movementSpeed = 25f;
    private float _movementDelta;

    public override void _Ready()
    {
        EmitSignal(SignalName.DialogueVariablesReady,
            new Dictionary<string, string> { { "$desiredItem", GD.Randi() % 2 == 0 ? "weapon" : "potion" } });

        _navigationAgent2D.VelocityComputed += OnVelocityComputed;
    }

    public void SetMovementTarget(Vector2 movementTarget)
    {
        _navigationAgent2D.TargetPosition = movementTarget;
    }

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
            return;
        }

        _movementDelta = _movementSpeed * (float)delta;
        var nextPathPosition = _navigationAgent2D.GetNextPathPosition();
        var newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
        if (_navigationAgent2D.AvoidanceEnabled)
        {
            _navigationAgent2D.Velocity = newVelocity;
        }
        else
        {
            OnVelocityComputed(newVelocity);
        }
    }

    private void OnVelocityComputed(Vector2 safeVelocity)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safeVelocity, _movementDelta);
    }

    private void UpdateAnimation()
    {
        if (_navigationAgent2D.IsNavigationFinished())
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
