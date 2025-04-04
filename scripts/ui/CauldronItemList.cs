using Godot;

public partial class CauldronItemList : ItemList
{
    public void AddItem(InventoryItem item)
    {
        var listIndex = AddIconItem(item.GetImage());
        SetItemMetadata(listIndex, item);
        GD.Print($"{nameof(CauldronItemList)}: added {item} to cauldron");
    }

    public void Remove(int index)
    {
        var item = GetItemMetadata(index);
        RemoveItem(index);
        GD.Print($"{nameof(CauldronItemList)}: removed {item} from cauldron");
    }
}
