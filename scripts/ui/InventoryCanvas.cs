using Godot;
using YarnSpinnerGodot;

public partial class InventoryCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;
    [Export] private Node _ctrlInventoryNode;
    [Export] private Inventory _playerInventory;

    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item);
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();

    private InventoryItem _selectedItem;
    private CtrlInventory _ctrlInventory;

    public override void _Ready()
    {
        Visible = InitiallyVisible;
        _ctrlInventory = new CtrlInventory(_ctrlInventoryNode);
        _ctrlInventory.SetInventory(_playerInventory);
    }

    [YarnCommand("OpenInventory")]
    public void OpenInventory()
    {
        Visible = true;
        EmitSignal(SignalName.InventoryOpened);
    }

    public void OnSelectPressed()
    {
        EmitSignal(SignalName.ItemSelected, _ctrlInventory.GetSelectedInventoryItem());
        Visible = false;
        EmitSignal(SignalName.InventoryClosed);
    }
}
