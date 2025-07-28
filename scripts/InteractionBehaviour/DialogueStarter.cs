using Godot;
using Godot.Collections;

public partial class DialogueStarter : Node2D, ICharacterInteractable
{
    [Export] public string DialogueNode;

    private Dictionary<string, string> _dialogueVariables;

    public void SetDialogueVariables(Dictionary<string, string> variables) => _dialogueVariables = variables;

    public void Interact(Node2D interactedBy)
        => UICanvas.Instance.StartDialogue(DialogueNode, _dialogueVariables, (Player)interactedBy);
}
