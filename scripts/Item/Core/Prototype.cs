using System.Collections.Generic;
using Godot;

public partial class Prototype : RefCounted
{
    public class Property
    {
        public const string ELEMENTS = "elements";
    }

    public Prototype() { }
    public Prototype(
        string id,
        string name,
        CompressedTexture2D image,
        string imagePath,
        List<string> types,
        Dictionary<string, Variant> properties)
    {
        ID = id;
        Name = name;
        Image = image;
        ImagePath = imagePath;
        Types = types;
        _properties = properties;
    }

    public string ID { get; private set; }
    public string Name { get; private set; }
    public CompressedTexture2D Image { get; private set; }
    public string ImagePath { get; private set; }
    public List<string> Types { get; private set; }
    private Dictionary<string, Variant> _properties;
    private Prototype _parent;

    public bool IsTypeOf(string type) => Types.Contains(type);
    public void SetProperty(string key, Variant value) => _properties[key] = value;
    public bool HasProperty(string key) => _properties.ContainsKey(key);
    public Variant GetProperty(string key) => _properties.ContainsKey(key) ? _properties[key] : default;
}