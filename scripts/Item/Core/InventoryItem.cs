using Godot;

public partial class InventoryItem : RefCounted
{
    private Prototype _prototype;

    public InventoryItem() { }

    public InventoryItem(Prototype prototype)
    {
        _prototype = prototype;
    }

    public string GetID() => _prototype.ID;
    public string GetName() => _prototype.Name;
    public CompressedTexture2D GetImage() => _prototype.Image;
    public Prototype GetPrototype() => _prototype;
    public override string ToString() => GetName();
}
