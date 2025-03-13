using Godot;

public partial class InventoryCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
    [Export] public Inventory Inventory;

    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();

    private InventoryItem _selectedItem;
    private Node _openInventoryActionRequester;

    public override async void _Ready()
    {
        Visible = InitiallyVisible;
        if (_ctrlInventory != null && Inventory != null)
        {
            await ToSignal(Inventory, SignalName.Ready);
            _ctrlInventory.SetInventory(Inventory);
        }
    }

    public void OpenInventory(Node openInventoryActionRequester)
    {
        _openInventoryActionRequester = openInventoryActionRequester;
        Visible = true;
        EmitSignal(SignalName.InventoryOpened);
    }

    public void OnItemSelected(InventoryItem item)
    {
        EmitSignal(SignalName.ItemSelected, item, _openInventoryActionRequester);
        _openInventoryActionRequester = null;
        Visible = false;
        EmitSignal(SignalName.InventoryClosed);
        GD.Print($"InventoryCanvas: selected {item.GetName()}");
    }
}
