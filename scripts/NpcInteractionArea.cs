using Godot;

public partial class NpcInteractionArea : Area2D
{
    private bool _isPointerInArea = false;

    public override void _Process(double delta)
    {
        if (_isPointerInArea && Input.IsActionJustPressed("ui_accept"))
        {
            GD.Print("pressed enter");
        }
    }

    public void OnPlayerPointerEntered(Area2D area) 
    {
        _isPointerInArea = true;
    }

    public void OnPlayerPointerExited(Area2D area) 
    {
        _isPointerInArea = false;
    }
}
