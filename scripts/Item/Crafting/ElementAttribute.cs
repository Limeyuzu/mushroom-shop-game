
using Godot;
using Godot.Collections;

public class ElementAttribute
{
    public int Earth { get; set; }
    public int Water { get; set; }
    public int Air { get; set; }
    public int Fire { get; set; }
    public int Dark { get; set; }

    public ElementAttribute() { }
    public ElementAttribute(Variant rawList)
    {
        if (rawList.As<Array<int>>() is null)
            return;

        var elements = rawList.As<Array<int>>();
        Earth = elements[0];
        Water = elements[1];
        Air = elements[2];
        Fire = elements[3];
        Dark = elements[4];
    }
    public ElementAttribute(int e, int w, int a, int f, int d)
    {
        Earth = e;
        Water = w;
        Air = a;
        Fire = f;
        Dark = d;
    }

    public static ElementAttribute operator +(ElementAttribute a, ElementAttribute b)
        => new ElementAttribute(a.Earth + b.Earth, a.Water + b.Water, a.Air + b.Air, a.Fire + b.Fire, a.Dark + b.Dark);
}
