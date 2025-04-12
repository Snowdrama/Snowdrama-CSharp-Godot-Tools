using Godot;
using static Godot.Control;

namespace Snowdrama.Core;

[Tool, GlobalClass]
public partial class UIControlFitter : Node
{
    [Export] Control targetControl;
    [Export] Control parentControl;

    [ExportCategory("Settings")]
    [Export] LayoutPreset anchorType = LayoutPreset.Center;
    [Export] Vector2 originalSize = new Vector2(128.0f, 128.0f);

    [ExportCategory("Debug")]
    [Export] Vector2 scaleFactor;
    [Export] Vector2 newControlSize;

    [Export] Vector2 offsetSize;
    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (targetControl == null) { return; }
        if(parentControl == null) { return; }

        var newSize = ContentFitterTools.ControlFitterSize(originalSize, parentControl.Size);
        var newOffset = ContentFitterTools.ControlFitterOffset(newSize, parentControl.Size, anchorType);
        targetControl.Size = newSize;
        targetControl.GlobalPosition = parentControl.GlobalPosition + newOffset;
    }

}
