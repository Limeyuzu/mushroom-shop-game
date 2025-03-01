using Godot;
using System;
using YarnSpinnerGodot;

public partial class InventoryWrapper : CanvasLayer
{
    [Export] public bool InitiallyVisible = true;

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

    public void CloseInventory() 
    {
        GD.Print("close inventory");
        Visible = false;
    }
}
