using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftingTable : StaticBody2D, ICharacterInteractable
{
    [Export] private Sprite2D _offSprite;
    [Export] private Sprite2D _onSprite;
    [Export] public float CraftingTimeSeconds = 5f;

    private bool _craftComplete;
    private InventoryItem _itemToCraft;
    private Inventory _playerInventory;

    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is Player player)
        {
            _playerInventory = player.Inventory;
            UICanvas.Instance.OpenCrafting(player.Inventory, this);
        }
    }

    public async void OnRecipeSelected(Recipe recipe)
    {
        await StartCrafting(recipe);
    }

    public async Task StartCrafting(Recipe recipe)
    {
        _offSprite.Visible = false;
        _onSprite.Visible = true;
        _craftComplete = false;
        _itemToCraft = recipe.CompletedItem;

        _playerInventory.RemoveAll(recipe.Ingredients.Contains);

        await Task.Delay(TimeSpan.FromSeconds(CraftingTimeSeconds));
        StopCrafting();
    }

    public void StopCrafting()
    {
        _onSprite.Visible = false;
        _offSprite.Visible = true;
        _craftComplete = true;

        _playerInventory.Add(_itemToCraft);

        GD.Print($"{nameof(CraftingTable)}: crafted: " + _itemToCraft);
    }
}
