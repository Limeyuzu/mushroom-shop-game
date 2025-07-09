using Godot;

public partial class UICanvas : CanvasLayer
{
    public static UICanvas Instance { get; private set; }

    public bool IsUIOpen { get => _isDialogueRunning || _isElementalCraftingOpen || _isInventoryOpen || _isCraftingOpen; }
    private bool _isDialogueRunning = false;
    private bool _isElementalCraftingOpen = false;
    private bool _isInventoryOpen = false;
    private bool _isCraftingOpen = false;

    [Signal] public delegate void OpenInventoryCommandEventHandler(Inventory inventory, Node openInventoryActionRequester);
    [Signal] public delegate void OpenCraftingCommandEventHandler(Inventory inventory, CraftingTable openInventoryActionRequester);
    [Signal] public delegate void OpenElementalCraftingCommandEventHandler(Inventory inventory, Cauldron openInventoryActionRequester);
    [Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc, Player interactedBy);

    public override void _Ready() => Instance = this;

    public void StartDialogue(Npc npc, Player interactedBy)
    {
        if (!IsUIOpen)
        {
            EmitSignal(SignalName.StartDialogueAttempt, npc, interactedBy);
        }
    }

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
        => EmitSignal(SignalName.OpenInventoryCommand, inventory, openInventoryActionRequester);

    public void OpenCrafting(Inventory inventory, CraftingTable requester)
        => EmitSignal(SignalName.OpenCraftingCommand, inventory, requester);

    public void OpenElementalCrafting(Inventory inventory, Cauldron requester)
        => EmitSignal(SignalName.OpenElementalCraftingCommand, inventory, requester);

    public void OnDialogueStarted() => _isDialogueRunning = true;
    public void OnDialogueCompleted() => _isDialogueRunning = false;
    public void OnElementalCraftingOpened() => _isElementalCraftingOpen = true;
    public void OnElementalCraftingClosed() => _isElementalCraftingOpen = false;
    public void OnInventoryOpened() => _isInventoryOpen = true;
    public void OnInventoryClosed() => _isInventoryOpen = false;
    public void OnCraftingOpened() => _isCraftingOpen = true;
    public void OnCraftingClosed() => _isCraftingOpen = false;
}
