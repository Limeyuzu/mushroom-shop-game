using Godot;
using System;

public partial class SelectButton : Button
{
    [Export] private Node _ctrlInventory;

    public void OnPressed() 
    {
        GD.Print("pressed Select");
        var selectedItem = _ctrlInventory.Call("get_selected_inventory_item");
        var gObject = selectedItem.As<GodotObject>();
        GD.Print("selected item: " + gObject.Call("get_title"));
    }
}
