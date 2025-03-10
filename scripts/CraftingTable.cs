using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftingTable : StaticBody2D, ICharacterInteractable
{
    [Export] private Sprite2D _offSprite;
    [Export] private Sprite2D _onSprite;
    [Export] private Inventory _playerInventory;
    [Export] public float CraftingTimeSeconds = 1f;

    [Signal] public delegate void CraftingTableInteractedEventHandler(Inventory craftingList);

    private bool _craftComplete;
    private InventoryItem _itemToCraft;

    public void Interact() => EmitSignal(SignalName.CraftingTableInteracted, GetAvailableCrafts(_playerInventory));

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

    public Inventory GetAvailableCrafts(Inventory playerInventory)
    {
        // var craftingList = new Inventory();
        // var result = craftingList.CreateAndAddItem("iron_sword_enhanced");
        // GD.Print(result);
        return playerInventory;
    }
}
