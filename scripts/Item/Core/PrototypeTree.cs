using System.Collections.Generic;
using Godot;

public partial class PrototypeTree : Node
{
    public static PrototypeTree Instance { get; private set; }
    public Godot.Collections.Dictionary<string, Prototype> Tree = [];

    private const string FILE_PATH = "res://gamedata/Items.json";

    public override void _Ready()
    {
        Variant jsonFile;
        try
        {
            var file = FileAccess.Open(FILE_PATH, FileAccess.ModeFlags.Read).GetAsText();
            jsonFile = Json.ParseString(file);
        }
        catch
        {
            GD.PrintErr($"can't find or parse {FILE_PATH}");
            throw;
        }
        Deserialise(jsonFile);

        Instance = this;
    }

    private void Deserialise(Variant jsonFile)
    {
        foreach (var item in jsonFile.As<Godot.Collections.Dictionary<string, Variant>>())
        {
            var id = item.Key;
            List<string> types = [];
            string name = string.Empty;
            CompressedTexture2D image = null;
            string imagePath = string.Empty;
            Dictionary<string, Variant> otherProperties = [];

            var properties = item.Value.As<Godot.Collections.Dictionary<string, Variant>>();
            foreach (var prop in properties)
            {
                if (prop.Key == "types")
                {
                    types = [.. prop.Value.As<Godot.Collections.Array<string>>()];
                }
                else if (prop.Key == "name")
                {
                    name = prop.Value.As<string>();
                }
                else if (prop.Key == "image")
                {
                    imagePath = prop.Value.As<string>();
                    image = GD.Load<CompressedTexture2D>(prop.Value.As<string>());
                }
                else
                {
                    otherProperties.Add(prop.Key, prop.Value);
                }
            }

            Tree.Add(id, new Prototype(id, name, image, imagePath, types, otherProperties));
        }
    }
}
