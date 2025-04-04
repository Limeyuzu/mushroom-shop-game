using Godot;

public partial class ElementalCraftingCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
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
        GD.Print($"{nameof(ElementalCraftingCanvas)}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void OnNoneSelected()
    {
        EmitSignal(SignalName.InventoryClosed);
        Visible = false;
    }

    private void SetInventory(Inventory inventory)
    {
        if (_ctrlInventory != null && inventory != null)
        {
            _ctrlInventory.SetInventory(inventory);
        }
    }
}
