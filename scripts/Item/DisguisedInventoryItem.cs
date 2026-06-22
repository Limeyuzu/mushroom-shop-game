public partial class DisguisedInventoryItem : InventoryItem
{
    public InventoryItem UnderlyingItem;

    public DisguisedInventoryItem(InventoryItem visibleItem, InventoryItem underlyingItem) : base(visibleItem)
    {
        UnderlyingItem = underlyingItem;
    }

    public override string GetName() => $"{base.GetName()}({UnderlyingItem.GetName()})";
    public string GetDisguisedName() => UnderlyingItem.GetName();

    public override string ToString() => GetName();
    public override bool Equals(object obj) => obj is DisguisedInventoryItem otherItem && GetID() == otherItem.GetID();
    public override int GetHashCode() => Prototype.GetHashCode() * UnderlyingItem.GetHashCode();
}
