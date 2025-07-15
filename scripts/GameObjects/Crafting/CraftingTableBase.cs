using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract partial class CraftingTableBase : StaticBody2D, ICharacterInteractable
{
    private bool _isCrafting;
    private List<InventoryItem> _ingredientsToConsume;
    private InventoryItem _itemToCraft;
    private Inventory _playerInventory;

    public abstract void OpenCraftingMenu(Inventory playerInventory);
    public abstract void ToggleCraftingTableSprite();
    public abstract float GetCraftingTime();

    public void Interact(Node2D interactedBy)
    {
        if (_isCrafting)
            return;

        if (interactedBy is Player player)
        {
            if (_itemToCraft != null)
            {
                PickUpFinishedItem();
            }
            else
            {
                _playerInventory = player.Inventory;
                OpenCraftingMenu(player.Inventory);
            }
        }
    }

    public virtual void SetItemToCraft(List<InventoryItem> ingredients, InventoryItem result)
    {
        _ingredientsToConsume = ingredients;
        _itemToCraft = result;
    }

    public async Task StartCrafting()
    {
        ToggleCraftingTableSprite();
        _isCrafting = true;
        _playerInventory.Remove(_ingredientsToConsume);
        GD.Print($"{this.GetType()}: started crafting: " + _itemToCraft);

        await Task.Delay(TimeSpan.FromSeconds(GetCraftingTime()));
        StopCrafting();
    }

    public void StopCrafting()
    {
        ToggleCraftingTableSprite();
        _isCrafting = false;

        GD.Print($"{this.GetType()}: crafted: " + _itemToCraft);
    }

    public void PickUpFinishedItem()
    {
        _playerInventory.Add(_itemToCraft);
        GD.Print($"{this.GetType()}: picked up: " + _itemToCraft);

        _itemToCraft = null;
    }
}
