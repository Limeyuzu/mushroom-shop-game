using Godot;

public partial class CraftingTableCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;
    [Export] private Node _ctrlInventoryNode;

    [Signal] public delegate void CraftingSelectionChosenEventHandler(InventoryItem chosen);

    private CtrlInventory _ctrlInventory;

    public override void _Ready()
    {
        Visible = InitiallyVisible;
        _ctrlInventory = new CtrlInventory(_ctrlInventoryNode);
    }

    public void OnItemSelected()
    {
        Visible = false;
        EmitSignal(SignalName.CraftingSelectionChosen, _ctrlInventory.GetSelectedInventoryItem());
    }
}
