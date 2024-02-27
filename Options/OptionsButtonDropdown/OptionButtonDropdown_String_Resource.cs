using Godot;

[GlobalClass]
public partial class OptionButtonDropdown_String_Resource : Resource
{
	[Export] public Godot.Collections.Array<OptionButtonDropdown_String_Data> optionsData = new Godot.Collections.Array<OptionButtonDropdown_String_Data>();
}
