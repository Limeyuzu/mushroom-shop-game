using Godot;

public partial class UICanvas : CanvasLayer
{
    public bool IsUIOpen { get => _isDialogueRunning || _isElementalCraftingOpen || _isInventoryOpen || _isCraftingOpen; }
    private bool _isDialogueRunning = false;
    private bool _isElementalCraftingOpen = false;
    private bool _isInventoryOpen = false;
    private bool _isCraftingOpen = false;

    [Signal] public delegate void StartDialogueAttemptEventHandler(Npc npc, Player interactedBy);
    [Signal] public delegate void OpenInventoryCommandEventHandler(Inventory inventory, Node openInventoryActionRequester);
    [Signal] public delegate void ItemSelectedEventHandler(InventoryItem item, Node requestingNode);
    [Signal] public delegate void CraftingRecipeSelectedEventHandler(Recipe recipe, Node requestingNode);

    public void StartDialogue(Npc npc, Player interactedBy)
    {
        if (!IsUIOpen)
        {
            EmitSignal(SignalName.StartDialogueAttempt, npc, interactedBy);
        }
    }

    public void OnItemSelected(InventoryItem item, Node requestingNode)
        => EmitSignal(SignalName.ItemSelected, item, requestingNode);

    public void OnRecipeSelected(Recipe recipe, Node requestingNode)
        => EmitSignal(SignalName.CraftingRecipeSelected, recipe, requestingNode);

    public void OpenInventory(Inventory inventory, Node openInventoryActionRequester)
        => EmitSignal(SignalName.OpenInventoryCommand, inventory, openInventoryActionRequester);

    public void OnDialogueStarted() => _isDialogueRunning = true;
    public void OnDialogueCompleted() => _isDialogueRunning = false;
    public void OnElementalCraftingOpened() => _isElementalCraftingOpen = true;
    public void OnElementalCraftingClosed() => _isElementalCraftingOpen = false;
    public void OnInventoryOpened() => _isInventoryOpen = true;
    public void OnInventoryClosed() => _isInventoryOpen = false;
    public void OnCraftingOpened() => _isCraftingOpen = true;
    public void OnCraftingClosed() => _isCraftingOpen = false;
}
