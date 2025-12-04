using Godot;

[GlobalClass]
public partial class OptionButtonDropdown_String_Data : Resource
{
    [Export] public string Text = "";
    [Export] public Texture2D Icon = null;
    [Export] public int Id = -1;
    [Export] public bool Disabled = false;
    [Export] public bool Separator = false;

    [Export] public string Data = null; //the data behind the option
}
