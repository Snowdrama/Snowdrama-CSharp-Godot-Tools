using Godot;
using Godot.Collections;
using System;
public partial class CustomCursorShapeSetter : Node
{
    [Export] Dictionary<Input.CursorShape, CursorShapeData> CursorShapes;



    public override void _Ready()
    {
        foreach (var cursorData in CursorShapes)
        {
            if (cursorData.Value.image != null)
            {
                Input.SetCustomMouseCursor(cursorData.Value.image, cursorData.Key, cursorData.Value.hotspot);
            }
        }
    }
}
