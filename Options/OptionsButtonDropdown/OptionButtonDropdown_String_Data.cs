using Godot;

[GlobalClass]
public partial class OptionButtonDropdown_String_Data : Resource
{
	[Export] public string text = "";
	[Export] public Texture2D icon = null;
	[Export] public int id = -1;
	[Export] public bool disabled = false;
	[Export] public bool separator = false;

	[Export] public string data = null; //the data behind the option
}
