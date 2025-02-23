using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
    private float _runSpeedInitial = 100;
    private float _runSpeed = 100;
    private float _runSpeedCap = 400;
    private float _friction = 0.85f;

    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateVelocity();
        UpdateAnimation();
        MoveAndSlide();
    }

    private void UpdateVelocity()
    {
        var velocity = Velocity;

        var inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        velocity += inputDir * (velocity.Length() < _runSpeedInitial ? _runSpeedInitial : _runSpeed);

        velocity *= _friction;

        velocity = velocity.LimitLength(_runSpeedCap);

        Velocity = velocity;
    }

    private void UpdateAnimation() 
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

        if (Velocity.Length() < 10) 
        {
            _animationPlayer.Stop();
        }
    }
}
