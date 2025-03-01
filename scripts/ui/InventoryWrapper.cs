using Godot;
using System;
using YarnSpinnerGodot;

public partial class InventoryWrapper : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;

    [Signal] public delegate void ItemSelectedEventHandler(GodotObject item);

    public override void _Ready() 
    {
        Visible = InitiallyVisible;
    }

    [YarnCommand("OpenInventory")]
    public void OpenInventory() 
    {
        GD.Print("open inventory");
        Visible = true;
    }

    public void SelectItem(GodotObject item) 
    {
        GD.Print("selected item: " + item.Call("get_title"));
        EmitSignal(SignalName.ItemSelected, item);
        GD.Print("close inventory");
        Visible = false;
    }
}
