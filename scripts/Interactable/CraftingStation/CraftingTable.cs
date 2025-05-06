using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftingTable : StaticBody2D, ICharacterInteractable
{
    [Export] private CanvasItem _offSprite;
    [Export] private CanvasItem _onSprite;
    [Export] public float CraftingTimeSeconds = 1f;

    [Signal] public delegate void OpenCraftingRequestedEventHandler(Inventory inventory, Node requester);

    private bool _craftComplete;
    private InventoryItem _itemToCraft;
    private Inventory _playerInventory;

    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is PlayerPointer playerPointer)
        {
            _playerInventory = playerPointer.Player.Inventory;
            EmitSignal(SignalName.OpenCraftingRequested, playerPointer.Player.Inventory, this);
        }
    }

    public async void OnRecipeSelected(Recipe recipe, Node requester)
    {
        if (requester != this)
            return;

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
