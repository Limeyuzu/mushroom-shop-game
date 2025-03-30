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
    public ElementAttribute GetElementAttribute() => new ElementAttribute(_prototype.GetProperty("elements"));


    public override string ToString() => GetName();
    public override bool Equals(object obj) => obj is InventoryItem otherItem && GetID() == otherItem.GetID();
    public override int GetHashCode() => _prototype.GetHashCode();
}
