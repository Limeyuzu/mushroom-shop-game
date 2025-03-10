using Godot;

public partial class CraftingTableCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;
    [Export] private Node _ctrlInventoryNode;

    [Signal] public delegate void CraftingSelectionChosenEventHandler(InventoryItem chosen);
    [Signal] public delegate void CraftingOpenedEventHandler();
    [Signal] public delegate void CraftingClosedEventHandler();

    private CtrlInventory _ctrlInventory;

    public override void _Ready()
    {
        Visible = InitiallyVisible;
        _ctrlInventory = new CtrlInventory(_ctrlInventoryNode);
    }

    public void OpenCrafting(Inventory craftingList)
    {
        _ctrlInventory.SetInventory(craftingList);
        Visible = true;
        EmitSignal(SignalName.CraftingOpened);
    }

    public void OnItemSelected()
    {
        EmitSignal(SignalName.CraftingSelectionChosen, _ctrlInventory.GetSelectedInventoryItem());
        Visible = false;
        EmitSignal(SignalName.CraftingClosed);
    }
}
