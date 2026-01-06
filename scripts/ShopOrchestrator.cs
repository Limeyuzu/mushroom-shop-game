using System.Linq;
using Godot;

public partial class ShopOrchestrator : Node
{
    [Export] public int NumberOfCustomers = 3;
    [Export] Area2D CustomerWaitingArea;

    [Signal] public delegate void InitNumberOfCustomersEventHandler(int num);
    [Signal] public delegate void SpawnShopNpcEventHandler(Vector2 npcDestination);

    public Player Player { get; private set; }

    public override void _Ready()
    {
        EmitSignal(SignalName.InitNumberOfCustomers, NumberOfCustomers);
    }

    public void SetPlayer(Player player) => Player = player;

    public void ShopStart(Vector2 npcDestination) => EmitSignal(SignalName.SpawnShopNpc, npcDestination);

    public void ServeNextCustomer()
    {
        var body = CustomerWaitingArea.GetOverlappingBodies().FirstOrDefault();
        if (body != null && body is Shopper shopper)
        {
            shopper.OnShopCounterInteract(Player);
        }
    }
}
