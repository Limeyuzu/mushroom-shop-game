using Godot;

[GlobalClass, Icon("res://editor/icon_ctrl_inventory.svg")]
public partial class CtrlInventory : ItemList
{
    [Export] private Inventory _inventory;

    [Signal] public delegate void OnItemSelectedEventHandler(InventoryItem item);

    public Inventory GetInventory() => _inventory;

    public void SetInventory(Inventory inventory)
    {
        if (inventory == null)
            return;

        _inventory = inventory;
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
        EmitSignal(SignalName.OnItemSelected, GetItemMetadata(selectedIndex));
    }
}