using Godot;
using Godot.Collections;

public partial class DialogueStarter : Node2D, ICharacterInteractable
{
    [Export] public string DefaultDialogueNode;

    private Dictionary<string, string> _initialDialogueVariables;
    private bool hadFirstInteraction;

    public void SetInitialDialogueVariables(Dictionary<string, string> variables) => _initialDialogueVariables = variables;

    public void Interact(Node2D interactedBy)
    {
        if (hadFirstInteraction)
        {
            UICanvas.Instance.StartDialogue(DefaultDialogueNode, (Player)interactedBy);
        }
        else
        {
            UICanvas.Instance.StartDialogue(DefaultDialogueNode, (Player)interactedBy, _initialDialogueVariables);
        }

        hadFirstInteraction = true;
    }

    public void ShopCounterInteract(Node2D interactedBy)
    {
        UICanvas.Instance.StartDialogue("ShopOrder", (Player)interactedBy);
    }
}
