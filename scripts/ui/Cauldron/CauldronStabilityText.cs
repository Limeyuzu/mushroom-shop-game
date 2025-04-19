using System.Collections.Generic;
using Godot;

public partial class CauldronStabilityText : RichTextLabel
{
    [Export] private CauldronItemList _cauldronItemList;

    public override void _Ready() => Clear();

    public void OnCauldronUpdated()
    {
        var attr = _cauldronItemList.GetTotalAttributes();
        var stability = PotionBrewing.GetStability(new ElementAttribute(1, 1, 0, 0, 0), attr);
        var colours = new Dictionary<Stability, string>
        {
            { Stability.Perfect, "#99ff9c" },
            { Stability.Stable, "#60a060" },
            { Stability.Unstable, "#f9e255" },
            { Stability.Uncraftable, "#c1272c" }
        };

        Text = $"Stability: [color={colours[stability]}]{stability}[/color]";
    }
}
