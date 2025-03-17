using Godot;

public partial class CharacterInteractionArea : Area2D
{
    [Export] private Node2D _subject;

    private Area2D _currentAreaInside;

    public override void _Process(double delta)
    {
        if (_currentAreaInside != null && Input.IsActionJustPressed("ui_accept") && _subject is ICharacterInteractable interactable)
        {
            interactable.Interact(_currentAreaInside);
        }
    }

    public void OnAreaEntered(Area2D area) => _currentAreaInside = area;

    public void OnAreaExited(Area2D area) => _currentAreaInside = null;
}
