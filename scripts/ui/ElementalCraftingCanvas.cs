using System;
using Godot;

public partial class ElementalCraftingCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
    [Export] private CauldronItemList _cauldronItemList;
    [Signal] public delegate void ElementalCraftingOpenedEventHandler();
    [Signal] public delegate void ElementalCraftingClosedEventHandler();
    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);

    private Node _openInventoryActionRequester;
    private Inventory _inventory;
    private readonly Predicate<InventoryItem> _viewFilter = i => i.IsTypeOf("ingredient");

    public override void _Ready() => Visible = InitiallyVisible;

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
    {
        SetCtrlInventory(inventory);
        _openInventoryActionRequester = openInventoryActionRequester;
        Visible = true;
        EmitSignal(SignalName.ElementalCraftingOpened);
        GD.Print($"{nameof(ElementalCraftingCanvas)}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void OnNoneSelected()
    {
        EmitSignal(SignalName.ElementalCraftingClosed);
        _cauldronItemList.RemoveAll();
        _ctrlInventory.Revert();
        Visible = false;
    }

    public void OnBrew()
    {
        var brewedItemProto = _cauldronItemList.GetResultPrototype();
        if (brewedItemProto == null) return;

        var consumedItems = _cauldronItemList.GetItems();
        var brewedItem = ItemDB.GetItem(brewedItemProto);

        EmitSignal(SignalName.ElementalCraftingClosed);
        EmitSignal(SignalName.ItemSelected, brewedItem, _openInventoryActionRequester);

        _inventory.RemoveAll(consumedItems.Contains);
        _cauldronItemList.RemoveAll();
        _inventory.Add(brewedItem);

        _openInventoryActionRequester = null;
        Visible = false;
        GD.Print($"{nameof(ElementalCraftingCanvas)}: brewing {brewedItem.GetName()}");
    }

    private void SetCtrlInventory(Inventory inventory)
    {
        if (_ctrlInventory != null && inventory != null)
        {
            _ctrlInventory.SetInventory(inventory, _viewFilter);
        }
        _inventory = inventory;
    }
}
