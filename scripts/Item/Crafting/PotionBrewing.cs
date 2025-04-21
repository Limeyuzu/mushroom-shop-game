using System.Collections.Generic;
using System.Linq;

public static class PotionBrewing
{
    private static readonly Dictionary<ElementAttribute, string> _potionRatios = new()
    {
        { new ElementAttribute(1, 1, 0, 0, 0), "health_potion" },
        { new ElementAttribute(1, 0, 0, 1, 0), "enhance_potion" }
    };

    public static Stability GetStability(ElementAttribute targetRatio, ElementAttribute totalAttributes)
        => GetStability(GetDeviation(targetRatio, totalAttributes));

    public static ElementAttribute GetAttributes(List<InventoryItem> ingredients)
        => ingredients
            .Select(i => i.GetElementAttribute())
            .Aggregate(new ElementAttribute(), (a, b) => a + b);

    public static string GetClosestPotion(List<InventoryItem> ingredients)
    {
        var (name, _) = GetClosestPotionAndStability(ingredients);
        return name;
    }

    public static (string, Stability) GetClosestPotionAndStability(List<InventoryItem> ingredients)
    {
        if (!ingredients.Any()) return (null, Stability.Uncraftable);

        // find closest potion
        var potionStabilities = new List<(float stab, string name)>();
        var attributes = GetAttributes(ingredients);
        foreach (var potion in _potionRatios)
        {
            var ratio = potion.Key;
            var potionName = potion.Value;
            var stability = GetDeviation(ratio, attributes);
            potionStabilities.Add((stability, potionName));
        }

        var (stab, name) = potionStabilities.OrderBy(ps => ps.stab).FirstOrDefault();
        return (name, GetStability(stab));
    }

    private static float MeanSquaredError(float a, float b) => (a - b) * (a - b);

    private static float GetDeviation(ElementAttribute targetRatio, ElementAttribute totalAttributes)
    {
        var targetNormal = targetRatio.Normalise();
        var givenNormal = totalAttributes.Normalise();
        var diff = targetNormal.Combine(givenNormal, MeanSquaredError).TotalValue();

        return diff;
    }

    private static Stability GetStability(float deviation)
        => deviation switch
        {
            < 0.001f => Stability.Perfect,
            < 0.05f => Stability.Stable,
            < 0.10f => Stability.Unstable,
            _ => Stability.Uncraftable
        };
}
