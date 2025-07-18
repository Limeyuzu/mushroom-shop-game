using Godot;

public partial class CauldronAttributesText : RichTextLabel
{
    [Export] private CauldronItemList _cauldronItemList;
    [Export] private CompressedTexture2D _earthIcon;
    [Export] private CompressedTexture2D _waterIcon;
    [Export] private CompressedTexture2D _airIcon;
    [Export] private CompressedTexture2D _fireIcon;
    [Export] private CompressedTexture2D _darkIcon;

    public override void _Ready() => Clear();

    public void OnCauldronUpdated()
    {
        var attr = _cauldronItemList.GetTotalAttributes();
        Text = $"[img=48x48]{_earthIcon.ResourcePath}[/img]: {attr.Earth}\n"
             + $"[img=48x48]{_waterIcon.ResourcePath}[/img]: {attr.Water}\n"
             + $"[img=48x48]{_airIcon.ResourcePath}[/img]: {attr.Air}\n"
             + $"[img=48x48]{_fireIcon.ResourcePath}[/img]: {attr.Fire}\n"
             + $"[img=48x48]{_darkIcon.ResourcePath}[/img]: {attr.Dark}";
    }
}
