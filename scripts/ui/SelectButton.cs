using Godot;
using System;

public partial class SelectButton : Button
{
    [Export] private Node _ctrlInventory;
    [Export] private InventoryWrapper _inventoryWrapper;

    public void OnPressed() 
    {
        var selectedItem = _ctrlInventory.Call("get_selected_inventory_item").As<GodotObject>();

        _inventoryWrapper.SelectItem(selectedItem);
    }
}
