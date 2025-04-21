using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://editor/icon_inventory.svg")]
public partial class Inventory : Node
{
    [Export] private Array<string> _itemNames;
    public List<InventoryItem> Items = [];
    private PrototypeTree _prototypeTree;
    private Dictionary _serialisedFormat = [];

    public Inventory() { }
    public Inventory(PrototypeTree prototypeTree) => _prototypeTree = prototypeTree;

    public override void _Ready()
    {
        _prototypeTree ??= this.ShopGlobal().PrototypeTree;

        foreach (var itemName in _itemNames)
        {
            CreateAndAddItem(itemName);
        }
    }

    public void Add(InventoryItem item) => Items.Add(item);
    public void Remove(InventoryItem item) => Items.Remove(item);
    public void RemoveAll(Predicate<InventoryItem> condition) => Items.RemoveAll(condition);

    public InventoryItem CreateItem(string prototypeId)
        => !_prototypeTree.Tree.TryGetValue(prototypeId, out Prototype value) ? null : new InventoryItem(value);

    public InventoryItem CreateAndAddItem(string prototypeId)
    {
        if (!_prototypeTree.Tree.ContainsKey(prototypeId))
            return null;

        var item = CreateItem(prototypeId);
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

    public Inventory Duplicate()
    {
        var newInventory = new Inventory(_prototypeTree)
        {
            Items = Items
        };
        return newInventory;
    }

    public bool Contains(InventoryItem item) => Items.Contains(item);
    public bool Contains(List<string> itemIds) => !itemIds.Except(Items.Select(i => i.GetID())).Any();
}