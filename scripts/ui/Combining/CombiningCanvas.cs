using Godot;
using System.Linq;

public partial class CombiningCanvas : Control
{
    [Export] public bool InitiallyVisible = true;
    [Export] private CtrlInventory _ingredientsListCtrl;
    [Export] private CombiningTableItemList _tableInventory;
    [Signal] public delegate void CraftingCanvasOpenedEventHandler();
    [Signal] public delegate void CraftingCanvasClosedEventHandler();

    private CombiningTable _craftingTableInstance;

    public override void _Ready() => Visible = InitiallyVisible;

    public void OpenCrafting(Inventory inventory, CombiningTable openInventoryActionRequester)
    {
        _ingredientsListCtrl.SetInventory(inventory);
        _craftingTableInstance = openInventoryActionRequester;
        EmitSignal(SignalName.CraftingCanvasOpened);
        Visible = true;
        GD.Print($"{GetType()}: opened by {openInventoryActionRequester.GetName()}");
    }

    public void OnNoneSelected()
    {
        _ingredientsListCtrl.Clear();
        _tableInventory.RemoveAll();
        EmitSignal(SignalName.CraftingCanvasClosed);
        Visible = false;
    }

    public void OnCombinationChosen()
    {
        var recipe = CraftingRecipes.Instance.GetAvailableCrafts(_tableInventory.GetItems()).First();

        _craftingTableInstance.OnRecipeSelected(recipe);
        GD.Print($"{GetType()}: selected {recipe.CompletedItem.GetName()}");

        EmitSignal(SignalName.CraftingCanvasClosed);
        _craftingTableInstance = null;
        Visible = false;
    }
}
