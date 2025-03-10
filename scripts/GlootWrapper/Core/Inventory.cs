using System.Linq;
using Godot;
using Godot.Collections;

public partial class Inventory : Node
{
    [Export] private Node _internalObject;

    public Inventory()
    {
        var gdInventory = GD.Load<GDScript>("res://addons/gloot/core/inventory.gd");
        _internalObject = (Node)gdInventory.New();
        _internalObject.Set("protoset", "res://Items.json");
    }

    public Array<InventoryItem> GetItems()
    {
        var internalInventoryItems = _internalObject.Call("get_items").As<Array>();
        return [.. internalInventoryItems.Select(ii => new InventoryItem(ii.As<GodotObject>()))];
    }

    public InventoryItem CreateAndAddItem(string prototypeId) => new InventoryItem(_internalObject.Call("create_and_add_item", prototypeId).As<GodotObject>());

    public Node GetInternalNode() => _internalObject;
}