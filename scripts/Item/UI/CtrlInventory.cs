using System.Collections.Generic;
using Godot;

[GlobalClass, Icon("res://editor/icon_ctrl_inventory.svg")]
public partial class CtrlInventory : ItemList
{
    [Export] private Inventory _inventory;

    [Signal] public delegate void OnItemSelectedEventHandler(InventoryItem item);

    private Dictionary<int, InventoryItem> _listIndexToItemMapping = [];

    public Inventory GetInventory() => _inventory;

    public void SetInventory(Inventory inventory)
    {
        if (inventory == null)
            return;

        _inventory = inventory;
        ClearCurrentInventory();
        foreach (var item in inventory.Items)
        {
            var listIndex = AddItem(item.GetName(), item.GetImage());
            _listIndexToItemMapping.Add(listIndex, item);
        }
    }

    public void GetSelectedInventoryItem()
    {
        var selectedIndex = GetSelectedItems()[0];
        EmitSignal(SignalName.OnItemSelected, _listIndexToItemMapping[selectedIndex]);
    }

    private void ClearCurrentInventory()
    {
        Clear();
        _listIndexToItemMapping = [];
    }
}