using Godot;
using Godot.Collections;
using System.Linq.Expressions;

[GlobalClass]
public partial class GodotKeyValuePair: Resource
{
    [Export] public string Key;
    [Export] public Variant Value;
}
