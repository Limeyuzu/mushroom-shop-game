using Godot;

public static class NodeExtensions
{
    public static GlobalState ShopGlobal(this Node node) => node.GetNode<GlobalState>("/root/Shop");
}
