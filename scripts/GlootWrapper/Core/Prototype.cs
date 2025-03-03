using Godot;

public partial class Prototype : RefCounted
{
    private GodotObject _internalObject;

    public Prototype(GodotObject internalObject)
    {
        _internalObject = internalObject;
    }

    public string GetId() => _internalObject.Call("get_id").As<string>();

    public bool Inherits(string otherId) => _internalObject.Call("inherits", otherId).As<bool>();
}