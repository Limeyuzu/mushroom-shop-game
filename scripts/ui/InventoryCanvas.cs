using Godot;

public partial class InventoryCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;

    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();

    private Node _openInventoryActionRequester;

    public override void _Ready()
    {
        Visible = InitiallyVisible;
    }

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
    {
        SetInventory(inventory);
        _openInventoryActionRequester = openInventoryActionRequester;
        Visible = true;
        EmitSignal(SignalName.InventoryOpened);
        GD.Print($"{nameof(InventoryCanvas)}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void OnItemSelected(InventoryItem item)
    {
        EmitSignal(SignalName.ItemSelected, item, _openInventoryActionRequester);
        _openInventoryActionRequester = null;
        Visible = false;
        EmitSignal(SignalName.InventoryClosed);
        GD.Print($"{nameof(InventoryCanvas)}: selected {item.GetName()}");
    }

    private void SetInventory(Inventory inventory)
    {
        if (_ctrlInventory != null && inventory != null)
        {
            _ctrlInventory.SetInventory(inventory);
        }
    }
}
