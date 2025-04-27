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
    public string GetImagePath() => _prototype.ImagePath;
    public bool IsTypeOf(string type) => _prototype.IsTypeOf(type);
    public ElementAttribute GetElementAttribute()
        => _prototype.HasProperty(Prototype.Property.ELEMENTS)
            ? new ElementAttribute(_prototype.GetProperty(Prototype.Property.ELEMENTS))
            : new ElementAttribute();

    public override string ToString() => GetName();
    public override bool Equals(object obj) => obj is InventoryItem otherItem && GetID() == otherItem.GetID();
    public override int GetHashCode() => _prototype.GetHashCode();
}
