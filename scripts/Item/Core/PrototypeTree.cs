using System.Collections.Generic;
using Godot;

public partial class PrototypeTree : RefCounted
{
    public PrototypeTree() {}

    public PrototypeTree(Json jsonFile)
    {
        if (jsonFile != null)
        {
            Deserialise(jsonFile);
        }
    }

    public Godot.Collections.Dictionary<string, Prototype> Tree = [];

    public void Deserialise(Json jsonFile)
    {
        foreach (var item in jsonFile.Data.As<Godot.Collections.Dictionary<string, Variant>>())
        {
            var id = item.Key;
            List<string> types = [];
            string name = string.Empty;
            CompressedTexture2D image = null;
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
                    image = GD.Load<CompressedTexture2D>(prop.Value.As<string>());
                }
                else
                {
                    otherProperties.Add(prop.Key, prop.Value);
                }
            }

            Tree.Add(id, new Prototype(id, name, image, types, otherProperties));
        }
    }
}