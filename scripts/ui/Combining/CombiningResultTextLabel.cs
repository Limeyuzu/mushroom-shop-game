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
            var colour = item is DisguisedInventoryItem ? Colors.MediumPurple : Colors.White;
            Text = $"[img=64x64]{item.GetImagePath()}[/img] [color=#{colour.ToHtml()}]{item.GetName()}[/color]";
        }
        else
        {
            Text = "";
        }
    }
}
