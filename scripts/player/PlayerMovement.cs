using Godot;

public partial class PlayerMovement : CharacterBody2D
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private Player _player;

    private float _runSpeedInitial = 100;
    private float _runSpeed = 100;
    private float _runSpeedCap = 350;
    private float _friction = 0.85f;

    public void OnResetVelocity() => Velocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        UpdateVelocity();
        UpdateAnimation();
        MoveAndSlide();
    }

    private void UpdateVelocity()
    {
        var velocity = Velocity;

        var inputDir = _player.CanMove() ? Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down") : Vector2.Zero;
        velocity += inputDir * (velocity.Length() < _runSpeedInitial ? _runSpeedInitial : _runSpeed);

        velocity *= _friction;

        velocity = velocity.LimitLength(_runSpeedCap);

        Velocity = velocity;
    }

    private void UpdateAnimation()
    {
        if (_player.CanMove())
        {
            if (Input.IsActionPressed("ui_right"))
            {
                _animationPlayer.Play("walk_right");
            }
            else if (Input.IsActionPressed("ui_left"))
            {
                _animationPlayer.Play("walk_left");
            }
            else if (Input.IsActionPressed("ui_up"))
            {
                _animationPlayer.Play("walk_up");
            }
            else if (Input.IsActionPressed("ui_down"))
            {
                _animationPlayer.Play("walk_down");
            }
        }

        if (Velocity.Length() < 10)
        {
            _animationPlayer.Stop();
        }
    }
}
