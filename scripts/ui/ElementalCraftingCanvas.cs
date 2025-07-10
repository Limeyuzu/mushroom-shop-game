using System;
using Godot;

public partial class ElementalCraftingCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ctrlInventory;
    [Export] private CauldronItemList _cauldronItemList;
    [Signal] public delegate void ElementalCraftingOpenedEventHandler();
    [Signal] public delegate void ElementalCraftingClosedEventHandler();

    private Cauldron _requestingNode;
    private readonly Predicate<InventoryItem> _viewFilter = i => i.IsTypeOf("ingredient");

    public override void _Ready() => Visible = InitiallyVisible;

    public void OpenInventory(Inventory inventory, Cauldron requestingNode)
    {
        SetCtrlInventory(inventory);
        _requestingNode = requestingNode;
        Visible = true;
        EmitSignal(SignalName.ElementalCraftingOpened);
    }

    public void OnNoneSelected()
    {
        EmitSignal(SignalName.ElementalCraftingClosed);
        _cauldronItemList.RemoveAll();
        Visible = false;
    }

    public void OnBrewSelected()
    {
        var brewedItemProto = _cauldronItemList.GetResultPrototype();
        if (brewedItemProto == null) return;

        EmitSignal(SignalName.ElementalCraftingClosed);

        _requestingNode.OnBrewSelected(_cauldronItemList.GetItems());

        _cauldronItemList.RemoveAll();
        _requestingNode = null;
        Visible = false;
    }

    private void SetCtrlInventory(Inventory inventory)
    {
        if (_ctrlInventory != null && inventory != null)
        {
            _ctrlInventory.SetInventory(inventory, _viewFilter);
        }
    }
}
