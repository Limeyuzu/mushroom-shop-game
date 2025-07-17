using Godot;
using System.Linq;

public partial class CombiningResultTextLabel : RichTextLabel
{
    [Export] private CombiningTableItemList _tableInventory;

    public override void _Ready() => Clear();

    public void OnTableInventoryUpdated()
    {
        var result = CraftingRecipes.Instance.GetAvailableCrafts(_tableInventory.GetItems());
        if (result.Any())
        {
            var item = result.First().CompletedItem;
            Text = $"[img=64x64]{item.GetImagePath()}[/img] {item.GetName()}";
        }
        else
        {
            Text = "";
        }
    }
}
