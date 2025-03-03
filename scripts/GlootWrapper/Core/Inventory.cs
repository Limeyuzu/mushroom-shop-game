using System.Linq;
using Godot;
using Godot.Collections;

public partial class Inventory : Node
{
    private Node _internalObject;

    public Inventory(Node internalObject)
    {
        _internalObject = internalObject;
    }

    public Array<InventoryItem> GetItems()
    {
        var internalInventoryItems = _internalObject.Call("get_items").As<Array>();
        return new Array<InventoryItem>(internalInventoryItems.Select(ii => new InventoryItem(ii.As<GodotObject>())));
    } 
}