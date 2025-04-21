using System.Collections.Generic;
using System.Linq;

public static class PotionBrewing
{
    public static Stability GetStability(ElementAttribute targetRatios, ElementAttribute totalAttributes)
    {
        var targetNormal = targetRatios.Normalise();
        var givenNormal = totalAttributes.Normalise();
        var diff = targetNormal.Combine(givenNormal, MeanSquaredError).TotalValue();

        return diff switch
        {
            < 0.001f => Stability.Perfect,
            < 0.05f => Stability.Stable,
            < 0.10f => Stability.Unstable,
            _ => Stability.Uncraftable
        };
    }

    private static float MeanSquaredError(float a, float b) => (a - b) * (a - b);

    public static string GetPotion(List<InventoryItem> ingredients)
    {
        // find closest potion

        return "enhance_potion";
    }

    public static ElementAttribute GetAttributes(List<InventoryItem> ingredients)
        => ingredients
            .Select(i => i.GetElementAttribute())
            .Aggregate(new ElementAttribute(), (a, b) => a + b);
}
