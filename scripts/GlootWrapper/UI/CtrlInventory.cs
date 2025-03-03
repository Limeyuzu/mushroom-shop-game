using Godot;

public partial class CtrlInventory : ItemList
{
    private Node _internalObject;

    public CtrlInventory(Node internalObject)
    {
        _internalObject = internalObject;
    }

    public InventoryItem GetSelectedInventoryItem() => new InventoryItem(_internalObject.Call("get_selected_inventory_item").As<GodotObject>());
}