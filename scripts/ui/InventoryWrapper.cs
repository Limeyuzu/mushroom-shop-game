using Godot;
using System;
using YarnSpinnerGodot;

public partial class InventoryWrapper : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;

    [Signal] public delegate void ItemSelectedEventHandler(GodotObject item);
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

    public void SelectItem(GodotObject item)
    {
        GD.Print("selected item: " + item.Call("get_title"));
        EmitSignal(SignalName.ItemSelected, item);
        Visible = false;
        EmitSignal(SignalName.InventoryClosed);
    }
}
