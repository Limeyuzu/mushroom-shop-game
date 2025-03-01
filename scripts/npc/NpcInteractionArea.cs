using Godot;
using YarnSpinnerGodot;

public partial class NpcInteractionArea : Area2D
{
    [Export] private Npc _npc;
    private bool _isPointerInArea = false;
    private GlobalState _globalState;

    public override void _Ready()
    {
        _globalState = _npc.GlobalState;
    }

    public override void _Process(double delta)
    {
        if (_isPointerInArea && Input.IsActionJustPressed("ui_accept") && _globalState.PlayerHasControl())
        {
            _globalState.DialogueRunner.StartDialogue(_globalState.DialogueRunner.startNode);
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
