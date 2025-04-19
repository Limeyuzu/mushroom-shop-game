using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public static class PotionBrewing
{
    public static ElementAttribute GetElementAttribute(List<InventoryItem> ingredients)
    {
        return ingredients
            .Select(i => i.GetElementAttribute())
            .Aggregate((a, b) => a + b);
    }

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
        return "enhance_potion";
    }

    public static ElementAttribute GetAttributes(List<InventoryItem> ingredients)
        => ingredients
            .Select(i => i.GetElementAttribute())
            .Aggregate(new ElementAttribute(), (a, b) => a + b);
}
