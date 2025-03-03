using Godot;

public partial class InventoryItem : RefCounted
{
    private GodotObject _internalInventoryItem;
    private Prototype _prototype;

    public InventoryItem(GodotObject internalObject)
    {
        _internalInventoryItem = internalObject;
        _prototype = new Prototype(internalObject.Call("get_prototype").As<GodotObject>());
    }

    public string GetTitle() => _internalInventoryItem.Call("get_title").As<string>();
    public Prototype GetPrototype() => _prototype;
}
