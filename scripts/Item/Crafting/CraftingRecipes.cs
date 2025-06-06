using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CraftingRecipes : Node
{
    public static CraftingRecipes Instance { get; private set; }

    private const string FILE_PATH = "res://gamedata/CraftingRecipes.json";
    private Dictionary<string, List<List<string>>> _recipes = [];

    public override void _Ready()
    {
        Variant jsonFile;
        try
        {
            var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read).GetAsText();
            jsonFile = Json.ParseString(file);
        }
        catch
        {
            GD.PrintErr($"can't find or parse {FILE_PATH}");
            throw;
        }
        Deserialise(jsonFile);

        Instance = this;
    }

    public List<Recipe> GetAvailableCrafts(Inventory playerInventory)
    {
        var craftables = new List<Recipe>();
        foreach (var item in _recipes)
        {
            var recipes = item.Value;
            foreach (var recipeItems in recipes)
            {
                if (playerInventory.Contains(recipeItems))
                {
                    var recipeModel = new Recipe
                    {
                        CompletedItem = ItemDB.GetItem(item.Key),
                        Ingredients = [.. recipeItems.Select(ItemDB.GetItem)]
                    };
                    craftables.Add(recipeModel);
                }
            }
        }
        return craftables;
    }

    private void Deserialise(Variant jsonFile)
    {
        foreach (var item in jsonFile.As<Godot.Collections.Dictionary<string, Godot.Collections.Array<Godot.Collections.Array<string>>>>())
        {
            var itemName = item.Key;
            var recipes = item.Value.Select(i => i.Select(j => j).ToList()).ToList();
            _recipes.Add(itemName, recipes);
        }
    }
}
