using System.Collections.Generic;
using Godot;

public partial class CauldronItemList : ItemList
{
    [Signal] public delegate void ItemListUpdatedEventHandler();

    public void AddItem(InventoryItem item)
    {
        var listIndex = AddIconItem(item.GetImage());
        SetItemMetadata(listIndex, item);
        EmitSignal(SignalName.ItemListUpdated);
        GD.Print($"{nameof(CauldronItemList)}: added {item} to cauldron");
    }

    public void Remove(int index)
    {
        var item = GetItemMetadata(index);
        RemoveItem(index);
        EmitSignal(SignalName.ItemListUpdated);
        GD.Print($"{nameof(CauldronItemList)}: removed {item} from cauldron");
    }

    public ElementAttribute GetTotalAttributes()
    {
        var items = new List<InventoryItem>();
        for (int i = 0; i < ItemCount; i++)
        {
            items.Add(GetItemMetadata(i).As<InventoryItem>());
        }

        return PotionBrewing.GetAttributes(items);
    }
}
