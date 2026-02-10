using System.Linq;
using Godot;
using YarnSpinnerGodot;

public partial class ShopOrchestrator : Node
{
    [Export] public int NumberOfCustomers = 3;
    [Export] Area2D CustomerWaitingArea;

    [Signal] public delegate void InitNumberOfCustomersEventHandler(int num);
    [Signal] public delegate void SpawnShopNpcEventHandler(Vector2 npcDestination);

    public Player Player { get; private set; }

    private Shopper _currentShopper;

    public override void _Ready()
    {
        EmitSignal(SignalName.InitNumberOfCustomers, NumberOfCustomers);
    }

    public void SetPlayer(Player player) => Player = player;

    public void ShopStart(Vector2 npcDestination) => EmitSignal(SignalName.SpawnShopNpc, npcDestination);

    public void ServeNextCustomer()
    {
        var body = CustomerWaitingArea.GetOverlappingBodies().FirstOrDefault();
        if (body != null && body.GetOwner() is Shopper shopper)
        {
            _currentShopper = shopper;
            _currentShopper.ShopCounterInteract(Player);
        }
    }

    // This is here because YarnCommand only works with a named node. Each instance of shopper has a different node name
    [YarnCommand("ShopperOpenInventory")]
    public void OpenInventory() => UICanvas.Instance.OpenInventory(Player.Inventory, _currentShopper);
}
