using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftingTable : StaticBody2D, ICharacterInteractable
{
    [Export] private CraftingTableCanvas _craftingTableCanvas;
    [Export] private Sprite2D _offSprite;
    [Export] private Sprite2D _onSprite;
    [Export] public float CraftingTimeSeconds = 1f;

    private bool _craftComplete;
    private InventoryItem _itemToCraft;

    public void Interact()
    {
        _craftingTableCanvas.Visible = true;
    }

    public async void StartCrafting(InventoryItem itemToCraft)
    {
        _offSprite.Visible = false;
        _onSprite.Visible = true;
        _craftComplete = false;
        _itemToCraft = itemToCraft;

        await Task.Delay(TimeSpan.FromSeconds(CraftingTimeSeconds));
        StopCrafting();
    }

    public void StopCrafting()
    {
        _onSprite.Visible = false;
        _offSprite.Visible = true;
        _craftComplete = true;
        GD.Print("crafted: " + _itemToCraft);
    }
}
