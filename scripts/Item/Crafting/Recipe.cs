using Godot;
using System.Collections.Generic;

public partial class Recipe : RefCounted
{
    public InventoryItem CompletedItem { get; set; }
    public List<InventoryItem> Ingredients { get; set; }
}
