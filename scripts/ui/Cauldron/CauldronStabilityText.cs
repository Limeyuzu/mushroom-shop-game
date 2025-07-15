using System.Collections.Generic;
using Godot;

public partial class CauldronStabilityText : RichTextLabel
{
    [Export] private CauldronItemList _cauldronItemList;

    public override void _Ready() => Clear();

    public void OnCauldronUpdated()
    {
        var (potionName, stability) = PotionBrewing.GetClosestPotionAndStability(_cauldronItemList.GetItems());

        if (potionName == null)
        {
            Text = string.Empty;
            return;
        }

        var colours = new Dictionary<Stability, string>
        {
            { Stability.Perfect, "#99ff9c" },
            { Stability.Stable, "#60a060" },
            { Stability.Unstable, "#f9e255" },
            { Stability.Uncraftable, "#c1272c" }
        };

        var potion = ItemDB.GetItem(potionName);
        Text = $"[img=64x64]{potion.GetImagePath()}[/img] {potion.GetName()} \nStability: [color={colours[stability]}]{stability}[/color]";
    }
}
