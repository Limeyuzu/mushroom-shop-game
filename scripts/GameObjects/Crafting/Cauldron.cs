using System.Collections.Generic;
using Godot;

public partial class Cauldron : CraftingTableBase
{
    [Export] private Sprite2D _offSprite;
    [Export] private AnimatedSprite2D _onSprite;
    [Export] private Sprite2D _doneBubbleSprite;
    [Export] private Sprite2D _doneInnerSprite;
    [Export] public float CraftingTimeSeconds = 5f;

    public override void OpenCraftingMenu(Inventory playerInventory)
        => UICanvas.Instance.OpenElementalCrafting(playerInventory, this);

    public override void ToggleCraftingTableSprite()
    {
        _onSprite.Visible = !_onSprite.Visible;
        _offSprite.Visible = !_offSprite.Visible;
    }
    public override void ToggleDoneSprite() => _doneBubbleSprite.Visible = !_doneBubbleSprite.Visible;

    public override float GetCraftingTime() => CraftingTimeSeconds;

    public async void OnBrewSelected(List<InventoryItem> ingredients)
    {
        var itemToCraft = ItemDB.GetItem(PotionBrewing.GetClosestPotion(ingredients));
        SetItemToCraft(ingredients, itemToCraft);
        _doneInnerSprite.Texture = itemToCraft.GetImage();

        await StartCrafting();
    }
}
