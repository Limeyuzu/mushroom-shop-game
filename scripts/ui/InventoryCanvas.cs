using Godot;

public partial class InventoryCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
    [Export] private Panel _buttonPanel;

    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();

    public bool ShowButtonPanel;

    private Node _openInventoryActionRequester;

    public override void _Ready() => Visible = InitiallyVisible;

    public override void _Process(double delta)
    {
        if (Visible && Input.IsActionJustPressed("cancel"))
        {
            OnNoneSelected();
        }
    }

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
    {
        SetInventory(inventory);
        _openInventoryActionRequester = openInventoryActionRequester;
        Visible = true;
        _buttonPanel.Visible = openInventoryActionRequester is not Player;
        EmitSignal(SignalName.InventoryOpened);
        GD.Print($"{nameof(InventoryCanvas)}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void CloseInventory(Node openInventoryActionRequester) => OnNoneSelected();

    public void OnItemSelected(InventoryItem item)
    {
        EmitSignal(SignalName.InventoryClosed);
        EmitSignal(SignalName.ItemSelected, item, _openInventoryActionRequester);
        _openInventoryActionRequester = null;
        Visible = false;
        GD.Print($"{nameof(InventoryCanvas)}: selected {item.GetName()}");
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
