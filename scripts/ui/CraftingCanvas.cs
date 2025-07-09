using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CraftingCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _recipeListCtrl;
    [Export] private CtrlInventory _ingredientsListCtrl;
    [Signal] public delegate void CraftingCanvasOpenedEventHandler();
    [Signal] public delegate void CraftingCanvasClosedEventHandler();

    private CraftingTable _craftingTableInstance;
    private List<Recipe> _recipes;

    public override void _Ready() => Visible = InitiallyVisible;

    public void OpenCrafting(Inventory inventory, CraftingTable openInventoryActionRequester)
    {
        _craftingTableInstance = openInventoryActionRequester;

        _recipes = CraftingRecipes.Instance.GetAvailableCrafts(inventory);

        _recipeListCtrl.SetInventory(new Inventory { Items = [.. _recipes.Select(r => r.CompletedItem)] });

        Visible = true;
        EmitSignal(SignalName.CraftingCanvasOpened);
        GD.Print($"{nameof(CraftingCanvas)}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void OnNoneSelected()
    {
        EmitSignal(SignalName.CraftingCanvasClosed);
        Visible = false;
    }

    public void OnRecipeHighlighted(InventoryItem item)
    {
        _ingredientsListCtrl.Clear();

        var ingredientInventory = new Inventory();

        var recipes = _recipes.Where(r => r.CompletedItem == item);
        foreach (var recipeOption in recipes)
        {
            foreach (var ingredient in recipeOption.Ingredients)
            {
                ingredientInventory.Add(ingredient);
            }
            // todo add a spacer to show next possible recipe option
        }

        _ingredientsListCtrl.SetInventory(ingredientInventory);
    }

    public void OnCraftChosen(InventoryItem item)
    {
        // todo: find the actual recipe selected instead of the first option
        var recipe = _recipes.First(r => r.CompletedItem == item);

        _craftingTableInstance.OnRecipeSelected(recipe);
        GD.Print($"{nameof(CraftingCanvas)}: selected {recipe.CompletedItem.GetName()}");

        EmitSignal(SignalName.CraftingCanvasClosed);
        _craftingTableInstance = null;
        Visible = false;
    }
}
