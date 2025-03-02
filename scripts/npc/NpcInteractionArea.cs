using Godot;

public partial class NpcInteractionArea : Area2D
{
    [Export] private Npc _npc;

    private bool _isPointerInArea = false;

    public override void _Process(double delta)
    {
        if (_isPointerInArea && Input.IsActionJustPressed("ui_accept"))
        {
            _npc.EmitStartDialogueAttempt();
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
