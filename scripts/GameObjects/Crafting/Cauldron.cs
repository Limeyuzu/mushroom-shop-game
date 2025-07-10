using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public partial class Cauldron : StaticBody2D, ICharacterInteractable
{
    [Export] private Sprite2D _offSprite;
    [Export] private AnimatedSprite2D _onSprite;
    [Export] public float CraftingTimeSeconds = 5f;

    private bool _craftComplete;
    private InventoryItem _itemToCraft;
    private Inventory _playerInventory;

    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is Player player)
        {
            _playerInventory = player.Inventory;
            UICanvas.Instance.OpenElementalCrafting(player.Inventory, this);
        }
    }

    public async void OnBrewSelected(List<InventoryItem> ingredients)
    {
        await StartCrafting(ingredients);
    }

    public async Task StartCrafting(List<InventoryItem> ingredients)
    {
        _offSprite.Visible = false;
        _onSprite.Visible = true;
        _craftComplete = false;
        _itemToCraft = ItemDB.GetItem(PotionBrewing.GetClosestPotion(ingredients));

        _playerInventory.RemoveAll(ingredients);

        GD.Print($"{nameof(Cauldron)}: brewing {_itemToCraft.GetName()}");

        await Task.Delay(TimeSpan.FromSeconds(CraftingTimeSeconds));
        StopCrafting();
    }

    public void StopCrafting()
    {
        _onSprite.Visible = false;
        _offSprite.Visible = true;
        _craftComplete = true;

        _playerInventory.Add(_itemToCraft);

        GD.Print($"{nameof(Cauldron)}: crafted: " + _itemToCraft);
    }
}
