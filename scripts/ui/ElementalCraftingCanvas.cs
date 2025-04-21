using Godot;

public partial class ElementalCraftingCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
    [Export] private CauldronItemList _cauldronItemList;
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();
    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);

    private Node _openInventoryActionRequester;
    private Inventory _inventory;

    public override void _Ready() => Visible = InitiallyVisible;

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

    public void OnBrew()
    {
        var brewedItemProto = _cauldronItemList.GetResultPrototype();
        var brewedItem = _inventory.CreateItem(brewedItemProto);

        EmitSignal(SignalName.InventoryClosed);
        EmitSignal(SignalName.ItemSelected, brewedItem, _openInventoryActionRequester);
        _openInventoryActionRequester = null;
        Visible = false;
        GD.Print($"{nameof(ElementalCraftingCanvas)}: brewing {brewedItem.GetName()}");
    }

    private void SetInventory(Inventory inventory)
    {
        if (_ctrlInventory != null && inventory != null)
        {
            _ctrlInventory.SetInventory(inventory);
        }
        _inventory = inventory;
    }
}
