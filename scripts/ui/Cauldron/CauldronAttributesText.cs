using Godot;

public partial class CauldronAttributesText : RichTextLabel
{
    [Export] private CauldronItemList _cauldronItemList;

    public override void _Ready() => Clear();

    public void OnCauldronUpdated()
    {
        var attr = _cauldronItemList.GetTotalAttributes();
        Text = $"E: {attr.Earth}\nW: {attr.Water}\nA: {attr.Air}\nF: {attr.Fire}\nD: {attr.Dark}";
    }
}
