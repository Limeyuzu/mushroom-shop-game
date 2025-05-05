using System;
using Godot;

[GlobalClass, Icon("res://editor/icon_ctrl_inventory.svg")]
public partial class CtrlInventory : ItemList
{
    [Export] private Inventory _originalInventory;
    private Inventory _virtualInventory;

    [Signal] public delegate void OnItemSelectedEventHandler(InventoryItem item);
    [Signal] public delegate void OnItemHighlightedEventHandler(InventoryItem item);

    public override void _Ready() => SetInventory(_originalInventory);

    public void SetInventory(Inventory inventory, Predicate<InventoryItem> viewFilter = null)
    {
        if (inventory == null)
            return;

        _originalInventory = inventory;
        _virtualInventory = inventory.DuplicateInventory();
        if (viewFilter != null)
        {
            _virtualInventory.RemoveAll(i => !viewFilter(i));
        }

        ResetInventory(_virtualInventory);
    }

    private void ResetInventory(Inventory inventory)
    {
        Clear();
        foreach (var item in inventory.Items)
        {
            var listIndex = AddItem(item.GetName(), item.GetImage());
            SetItemMetadata(listIndex, item);
        }
    }

    public void EmitSelectedInventoryItem()
    {
        var selected = GetSelectedItems();
        if (selected.Length == 0)
            return;

        var selectedIndex = selected[0];
        EmitSelectedInventoryItem(selectedIndex);
    }

    public void EmitSelectedInventoryItem(int index) => EmitSignal(SignalName.OnItemSelected, GetItemMetadata(index));

    // Adds an item to ItemList without affecting original Inventory
    public void AddItemToDisplay(InventoryItem item)
    {
        _virtualInventory.Add(item);
        ResetInventory(_virtualInventory);
    }

    // Removes an item from ItemList without affecting original Inventory
    public void RemoveItemFromDisplay(InventoryItem item)
    {
        _virtualInventory.Remove(item);
        ResetInventory(_virtualInventory);
    }

    public void OnInventoryItemHighlighted(int index) => EmitSignal(SignalName.OnItemHighlighted, GetItemMetadata(index));

    public void Revert() => SetInventory(_originalInventory);
}