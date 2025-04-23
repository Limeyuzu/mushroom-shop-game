using Godot;

public partial class ItemDB : Node
{
    public static InventoryItem GetItem(string prototypeId)
        => !ProtoTree().TryGetValue(prototypeId, out Prototype value) ? null : new InventoryItem(value);

    public static bool Exists(string prototypeId) => ProtoTree().ContainsKey(prototypeId);

    private static Godot.Collections.Dictionary<string, Prototype> ProtoTree() => PrototypeTree.Instance.Tree;
}