using Godot;
using YarnSpinnerGodot;

public partial class NpcInteractionArea : Area2D
{
    [Export] private Npc _npc;
    private bool _isPointerInArea = false;
    private DialogueRunner _dialogueRunner;

    public override void _Ready()
    {
        _dialogueRunner = _npc.DialogueNode.DialogueRunner;
    }

    public override void _Process(double delta)
    {
        if (_isPointerInArea && Input.IsActionJustPressed("ui_accept"))
        {
            GD.Print("pressed enter");
            _dialogueRunner.StartDialogue(_dialogueRunner.startNode);
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
