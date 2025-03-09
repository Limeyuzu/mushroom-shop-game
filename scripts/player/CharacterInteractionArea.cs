using System.Collections.Generic;
using Godot;

[Tool]
public partial class CharacterInteractionArea : Area2D
{
    [Export] private Node2D _subject;

    private bool _isPointerInArea = false;

    public override void _Process(double delta)
    {
        if (_isPointerInArea && Input.IsActionJustPressed("ui_accept") && _subject is ICharacterInteractable interactable)
        {
            interactable.Interact();
        }
    }

    public void OnPlayerPointerEntered(Area2D area) => _isPointerInArea = true;

    public void OnPlayerPointerExited(Area2D area) => _isPointerInArea = false;

    public override string[] _GetConfigurationWarnings()
    {
        var warnings = new List<string>();
        warnings.AddRange(base._GetConfigurationWarnings() ?? []);

        if (_subject is null)
        {
            warnings.Add("subject Node is empty.");
        }
        return [.. warnings];
    }
}
