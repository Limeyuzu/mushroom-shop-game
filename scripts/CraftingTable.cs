using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftingTable : StaticBody2D, ICharacterInteractable
{
    [Export] private Sprite2D _offSprite;
    [Export] private Sprite2D _onSprite;
    [Export] public float CraftingTimeSeconds = 1f;

    [Signal] public delegate void OpenInventoryRequestedEventHandler(Node2D interactedBy, Node requester);

    private bool _craftComplete;
    private InventoryItem _itemToCraft;

    public void Interact(Node2D interactedBy)
    {
        if (interactedBy is PlayerPointer playerPointer)
        {
            var craftingList = Crafting.GetAvailableCrafts(playerPointer.Player.Inventory);
            EmitSignal(SignalName.OpenInventoryRequested, craftingList, this);
        } 
    } 

    public void OnItemSelected(InventoryItem item, Node requester)
    {
        if (requester != this)
            return;

        StartCrafting(item).ConfigureAwait(false);
    }

    public async Task StartCrafting(InventoryItem itemToCraft)
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
        GD.Print($"{nameof(CraftingTable)}: crafted: " + _itemToCraft);
    }
}
