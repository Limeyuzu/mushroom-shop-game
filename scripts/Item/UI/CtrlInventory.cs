using Godot;

[GlobalClass, Icon("res://editor/icon_ctrl_inventory.svg")]
public partial class CtrlInventory : ItemList
{
    [Export] private Inventory _originalInventory;
    private Inventory _virtualInventory;

    [Signal] public delegate void OnItemSelectedEventHandler(InventoryItem item);

    public void SetInventory(Inventory inventory)
    {
        if (inventory == null)
            return;

        _originalInventory = inventory;
        _virtualInventory = inventory.Duplicate();

        ResetInventory(inventory);
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

    public void GetSelectedInventoryItem()
    {
        var selected = GetSelectedItems();
        if (selected.Length == 0)
            return;

        var selectedIndex = selected[0];
        GetSelectedInventoryItem(selectedIndex);
    }

    public void GetSelectedInventoryItem(int index) => EmitSignal(SignalName.OnItemSelected, GetItemMetadata(index));

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
}