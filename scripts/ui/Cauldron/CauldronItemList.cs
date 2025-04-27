using System.Collections.Generic;
using Godot;

public partial class CauldronItemList : ItemList
{
    [Signal] public delegate void ItemListUpdatedEventHandler();
    [Signal] public delegate void ItemAddedEventHandler(InventoryItem item);
    [Signal] public delegate void ItemRemovedEventHandler(InventoryItem item);
    [Signal] public delegate void IsUnstableUpdatedEventHandler(bool isUnstable);

    public void AddItem(InventoryItem item)
    {
        var stabilityBefore = GetStability();

        var listIndex = AddIconItem(item.GetImage());
        SetItemMetadata(listIndex, item);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemAdded, item);

        var newStability = GetStability();
        if (newStability != stabilityBefore)
        {
            EmitSignal(SignalName.IsUnstableUpdated, newStability == Stability.Uncraftable);
        }
        GD.Print($"{nameof(CauldronItemList)}: added {item} to cauldron");
    }

    public void Remove(int index)
    {
        var stabilityBefore = GetStability();

        var item = GetItemMetadata(index);
        RemoveItem(index);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemRemoved, item);

        var newStability = GetStability();
        if (newStability != stabilityBefore)
        {
            EmitSignal(SignalName.IsUnstableUpdated, newStability == Stability.Uncraftable);
        }
        GD.Print($"{nameof(CauldronItemList)}: removed {item} from cauldron");
    }

    public List<InventoryItem> GetItems()
    {
        var items = new List<InventoryItem>();
        for (int i = 0; i < ItemCount; i++)
        {
            items.Add(GetItemMetadata(i).As<InventoryItem>());
        }
        return items;
    }

    public void RemoveAll()
    {
        Clear();
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.IsUnstableUpdated, true);
    }

    public string GetResultPrototype() => PotionBrewing.GetClosestPotion(GetItems());

    public ElementAttribute GetTotalAttributes() => PotionBrewing.GetAttributes(GetItems());

    public Stability GetStability() => PotionBrewing.GetClosestPotionAndStability(GetItems()).Item2;
}
