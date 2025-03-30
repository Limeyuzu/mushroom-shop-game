using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CraftingRecipes : Node
{
    [Export(PropertyHint.File, "*.json")] public Json jsonFile;

    private Dictionary<string, List<List<string>>> _recipes = [];

    public override void _Ready() => Deserialise(jsonFile);

    public Inventory GetAvailableCrafts(Inventory playerInventory)
    {
        var craftables = new Inventory(this.ShopGlobal().PrototypeTree);
        foreach (var item in _recipes)
        {
            var recipes = item.Value;
            foreach (var recipeItems in recipes)
            {
                if (playerInventory.Contains(recipeItems))
                {
                    craftables.CreateAndAddItem(item.Key);
                    break;
                }
            }
        }
        return craftables;
    }

    private void Deserialise(Json jsonFile)
    {
        foreach (var item in jsonFile.Data.As<Godot.Collections.Dictionary<string, Godot.Collections.Array<Godot.Collections.Array<string>>>>())
        {
            var itemName = item.Key;
            var recipes = item.Value.Select(i => i.Select(j => j).ToList()).ToList();
            _recipes.Add(itemName, recipes);
        }
    }
}
