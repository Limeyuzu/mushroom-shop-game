using Godot;
using YarnSpinnerGodot;

public partial class DialogueRunnerCanvas : CanvasLayer
{
    [Export] public DialogueRunner DialogueRunner;

    public void OnItemSelected(GodotObject item)
    {
        DialogueRunner.VariableStorage.SetValue("$selectedItem", item.Call("get_title").As<string>());
        DialogueRunner.StartDialogue("ShowItem");
    }
}
