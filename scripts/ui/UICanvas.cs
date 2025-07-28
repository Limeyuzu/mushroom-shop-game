using Godot;
using Godot.Collections;

public partial class UICanvas : CanvasLayer
{
    public static UICanvas Instance { get; private set; }

    public bool IsUIOpen { get => _isDialogueRunning || _isElementalCraftingOpen || _isInventoryOpen || _isCraftingOpen; }
    private bool _isDialogueRunning = false;
    private bool _isElementalCraftingOpen = false;
    private bool _isInventoryOpen = false;
    private bool _isCraftingOpen = false;

    [Signal] public delegate void OpenInventoryCommandEventHandler(Inventory inventory, Node openInventoryActionRequester);
    [Signal] public delegate void CloseInventoryCommandEventHandler(Node openInventoryActionRequester);
    [Signal] public delegate void OpenCraftingCommandEventHandler(Inventory inventory, CombiningTable openInventoryActionRequester);
    [Signal] public delegate void OpenElementalCraftingCommandEventHandler(Inventory inventory, Cauldron openInventoryActionRequester);
    [Signal] public delegate void StartDialogueAttemptEventHandler(string dialogueNode, Player interactedBy, Dictionary<string, string> variables);

    public override void _Ready() => Instance = this;

    public void StartDialogue(string dialogueNode, Player interactedBy, Dictionary<string, string> variables = null)
    {
        if (!IsUIOpen)
        {
            EmitSignal(SignalName.StartDialogueAttempt, dialogueNode, interactedBy, variables);
        }
    }

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
        => EmitSignal(SignalName.OpenInventoryCommand, inventory, openInventoryActionRequester);
    public void ToggleInventoryWindow(Inventory inventory, Node openInventoryActionRequester)
    {
        if (_isInventoryOpen)
        {
            EmitSignal(SignalName.CloseInventoryCommand, openInventoryActionRequester);
        }
        else
        {
            EmitSignal(SignalName.OpenInventoryCommand, inventory, openInventoryActionRequester);
        }
    }

    public void OpenCrafting(Inventory inventory, CombiningTable requester)
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
