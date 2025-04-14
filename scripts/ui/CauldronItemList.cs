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
        var total = new ElementAttribute();
        for (int i = 0; i < ItemCount; i++)
        {
            var item = GetItemMetadata(i).As<InventoryItem>();
            total += item.GetElementAttribute();
        }

        return total;
    }
}
