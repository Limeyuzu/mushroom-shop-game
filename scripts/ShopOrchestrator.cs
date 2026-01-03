using Godot;

public partial class ShopOrchestrator : Node
{
    [Signal] public delegate void SpawnShopNpcEventHandler(Vector2 npcDestination);

    public void ShopStart(Vector2 npcDestination) => EmitSignal(SignalName.SpawnShopNpc, npcDestination);
    public void ServeNextCustomer()
    {

    }
}
