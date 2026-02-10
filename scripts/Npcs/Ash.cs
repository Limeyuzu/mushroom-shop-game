using Godot;

public partial class Ash : Node2D, ICharacterInteractable
{
    [Export] public string DefaultDialogueNode;

    public void Interact(Node2D interactedBy)
    {
        UICanvas.Instance.StartDialogue(DefaultDialogueNode, (Player)interactedBy);
    }
}
