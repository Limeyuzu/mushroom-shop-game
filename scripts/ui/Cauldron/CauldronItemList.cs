using System.Collections.Generic;
using Godot;

public partial class CauldronItemList : ItemList
{
    [Signal] public delegate void ItemListUpdatedEventHandler();
    [Signal] public delegate void ItemAddedEventHandler(InventoryItem item);
    [Signal] public delegate void ItemRemovedEventHandler(InventoryItem item);

    public void AddItem(InventoryItem item)
    {
        var listIndex = AddIconItem(item.GetImage());
        SetItemMetadata(listIndex, item);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemAdded, item);
        GD.Print($"{nameof(CauldronItemList)}: added {item} to cauldron");
    }

    public void Remove(int index)
    {
        var item = GetItemMetadata(index);
        RemoveItem(index);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemRemoved, item);
        GD.Print($"{nameof(CauldronItemList)}: removed {item} from cauldron");
    }

    public string GetResultPrototype() => PotionBrewing.GetClosestPotion(GetItems());

    public ElementAttribute GetTotalAttributes() => PotionBrewing.GetAttributes(GetItems());

    public List<InventoryItem> GetItems()
    {
        var items = new List<InventoryItem>();
        for (int i = 0; i < ItemCount; i++)
        {
            items.Add(GetItemMetadata(i).As<InventoryItem>());
        }
        return items;
    }
}
