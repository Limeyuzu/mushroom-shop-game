using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://editor/icon_inventory.svg")]
public partial class Inventory : Node
{
    [Export(PropertyHint.File, "*.json")] public Json PrototypeTreeJson;
    [Export] private Array<string> _itemNames;
    public List<InventoryItem> Items = [];
    private PrototypeTree _prototypeTree;
    private Dictionary _serialisedFormat = [];

    public override void _Ready()
    {
        _prototypeTree = new PrototypeTree(PrototypeTreeJson);
        foreach (var itemName in _itemNames)
        {
            CreateAndAddItem(itemName);
        }
    }

    public InventoryItem CreateAndAddItem(string prototypeId)
    {
        if (!_prototypeTree.Tree.ContainsKey(prototypeId))
            return null;

        var item = new InventoryItem(_prototypeTree.Tree[prototypeId]);
        Items.Add(item);

        return item;
    }

    public InventoryItem GetItemWithId(string prototypeId)
    {
        if (!_prototypeTree.Tree.ContainsKey(prototypeId))
            return null;

        foreach (var item in Items)
        {
            if (item.GetID() == prototypeId)
                return item;
        }

        return null;
    }
}