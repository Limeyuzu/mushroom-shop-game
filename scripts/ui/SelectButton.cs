using Godot;

public partial class SelectButton : Button
{
    [Export] private Node _ctrlInventory;
    [Export] private InventoryCanvas _inventoryCanvas;

    public CtrlInventory CtrlInventory;

    public override void _Ready()
    {
        CtrlInventory = new CtrlInventory(_ctrlInventory);
    }

    public void OnPressed()
    {
        _inventoryCanvas.SelectItem(CtrlInventory.GetSelectedInventoryItem());
    }
}
