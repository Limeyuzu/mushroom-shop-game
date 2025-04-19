
using System;
using Godot;
using Godot.Collections;

public class ElementAttribute
{
    public float Earth { get; set; }
    public float Water { get; set; }
    public float Air { get; set; }
    public float Fire { get; set; }
    public float Dark { get; set; }

    public ElementAttribute() { }
    public ElementAttribute(Variant rawList)
    {
        if (rawList.As<Array<float>>() is null)
            return;

        var elements = rawList.As<Array<float>>();
        Earth = elements[0];
        Water = elements[1];
        Air = elements[2];
        Fire = elements[3];
        Dark = elements[4];
    }
    public ElementAttribute(float e, float w, float a, float f, float d)
    {
        Earth = e;
        Water = w;
        Air = a;
        Fire = f;
        Dark = d;
    }

    public ElementAttribute Combine(ElementAttribute other, Func<float, float, float> combineFunc)
        => new(
            combineFunc(Earth, other.Earth),
            combineFunc(Water, other.Water),
            combineFunc(Air, other.Air),
            combineFunc(Fire, other.Fire),
            combineFunc(Dark, other.Dark));

    public ElementAttribute Normalise()
    {
        var sum = TotalValue();
        return new ElementAttribute(
            Earth / sum,
            Water / sum,
            Air / sum,
            Fire / sum,
            Dark / sum);
    }

    public float TotalValue() => Earth + Water + Air + Fire + Dark;


    public static ElementAttribute operator +(ElementAttribute left, ElementAttribute right)
        => left.Combine(right, (a, b) => a + b);
    public static ElementAttribute operator -(ElementAttribute left, ElementAttribute right)
        => left.Combine(right, (a, b) => a - b);
}
