using Godot;

public partial class CauldronItemText : RichTextLabel
{
    [Export] private CauldronItemList _cauldronItemList;

    public void OnCauldronUpdated()
    {
        var attr = _cauldronItemList.GetTotalAttributes();
        Text = $"E: {attr.Earth}\nW: {attr.Water}\nA: {attr.Air}\nF: {attr.Fire}\nD: {attr.Dark}";
    }
}
