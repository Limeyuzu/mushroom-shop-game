using Godot;
using YarnSpinnerGodot;

public partial class InventoryCanvas : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;

    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item);
    [Signal] public delegate void InventoryOpenedEventHandler();
    [Signal] public delegate void InventoryClosedEventHandler();

    public override void _Ready()
    {
        Visible = InitiallyVisible;
    }

    [YarnCommand("OpenInventory")]
    public void OpenInventory()
    {
        Visible = true;
        EmitSignal(SignalName.InventoryOpened);
    }

    public void SelectItem(InventoryItem item)
    {
        GD.Print("selected item: " + item.GetTitle());
        EmitSignal(SignalName.ItemSelected, item);
        Visible = false;
        EmitSignal(SignalName.InventoryClosed);
    }
}
