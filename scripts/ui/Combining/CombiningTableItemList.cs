using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CombiningTableItemList : ItemList
{
    [Signal] public delegate void ItemListUpdatedEventHandler();
    [Signal] public delegate void ItemAddedEventHandler(InventoryItem item);
    [Signal] public delegate void ItemRemovedEventHandler(InventoryItem item);
    [Signal] public delegate void HasValidRecipeUpdatedEventHandler(bool invalid);

    public void AddItem(InventoryItem item)
    {
        var listIndex = AddItem(item.GetName(), item.GetImage());
        SetItemMetadata(listIndex, item);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemAdded, item);
        EmitSignal(SignalName.HasValidRecipeUpdated, !CanCraftRecipe());

        GD.Print($"{GetType()}: added {item} to combining table");
    }

    public void Remove(int index)
    {
        var item = GetItemMetadata(index);
        RemoveItem(index);
        EmitSignal(SignalName.ItemListUpdated);
        EmitSignal(SignalName.ItemRemoved, item);
        EmitSignal(SignalName.HasValidRecipeUpdated, !CanCraftRecipe());

        GD.Print($"{GetType()}: removed {item} combining table");
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
    }

    private bool CanCraftRecipe()
        => CraftingRecipes.Instance.GetAvailableCrafts(GetItems()).Any();
}
