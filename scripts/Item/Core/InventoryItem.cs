using Godot;

public partial class InventoryItem : RefCounted
{
    protected Prototype Prototype;

    public InventoryItem() { }

    public InventoryItem(Prototype prototype)
    {
        Prototype = prototype;
    }

    protected InventoryItem(InventoryItem other)
    {
        Prototype = other.Prototype;
    }

    public string GetID() => Prototype.ID;
    public virtual string GetName() => Prototype.Name;
    public CompressedTexture2D GetImage() => Prototype.Image;
    public string GetImagePath() => Prototype.ImagePath;
    public bool IsTypeOf(string type) => Prototype.IsTypeOf(type);
    public ElementAttribute GetElementAttribute()
        => Prototype.HasProperty(Prototype.Property.ELEMENTS)
            ? new ElementAttribute(Prototype.GetProperty(Prototype.Property.ELEMENTS))
            : new ElementAttribute();

    public override string ToString() => GetName();
    public override bool Equals(object obj) => obj is InventoryItem otherItem && GetID() == otherItem.GetID();
    public override int GetHashCode() => Prototype.GetHashCode();
}
