using Godot;

public partial class CombiningTable : CraftingTableBase
{
    [Export] private Sprite2D _offSprite;
    [Export] private Sprite2D _onSprite;
    [Export] public float CraftingTimeSeconds = 5f;

    public override void OpenCraftingMenu(Inventory playerInventory)
        => UICanvas.Instance.OpenCrafting(playerInventory, this);

    public override void ToggleCraftingTableSprite()
    {
        _onSprite.Visible = !_onSprite.Visible;
        _offSprite.Visible = !_offSprite.Visible;
    }

    public override float GetCraftingTime() => CraftingTimeSeconds;

    public async void OnRecipeSelected(Recipe recipe)
    {
        SetItemToCraft(recipe.Ingredients, recipe.CompletedItem);
        await StartCrafting();
    }
}
