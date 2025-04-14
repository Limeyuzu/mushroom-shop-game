using System.Collections.Generic;
using System.Linq;

public class PotionBrewing
{
    public ElementAttribute GetElementAttribute(List<InventoryItem> ingredients)
    {
        return ingredients
            .Select(i => i.GetElementAttribute())
            .Aggregate((a, b) => a + b);
    }

    public Stability GetStability(ElementAttribute targetRatios, ElementAttribute totalAttributes)
    {
        // todo
        return Stability.Perfect;
    }

    public string GetPotion(List<InventoryItem> ingredients)
    {
        return "enhance_potion";
    }
}
