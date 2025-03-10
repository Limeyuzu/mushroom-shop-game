using Godot;

public partial class CtrlInventory : Node
{
    private Node _internalObject;

    public CtrlInventory(Node internalObject)
    {
        _internalObject = internalObject;
    }

    public InventoryItem GetSelectedInventoryItem() => new InventoryItem(_internalObject.Call("get_selected_inventory_item").As<GodotObject>());
    public void SetInventory(Inventory inventory) =>_internalObject.Set("inventory", inventory.GetInternalNode());
}