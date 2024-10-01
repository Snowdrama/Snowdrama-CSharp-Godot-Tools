using Godot;
using System;


[Tool]
public partial class TouchAnalogStick : Control
{

	[Export] Rect2 inputArea;
    [Export] bool useHalfScreenSize = true;
    [Export] bool useLeftHalf = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var screenSize = DisplayServer.WindowGetSize();
        if (useHalfScreenSize)
        {
            if (useLeftHalf)
            {
                inputArea.Position = Vector2.Zero;
                inputArea.Size = new Vector2(screenSize.X / 2.0f, screenSize.Y);
            }
            else
            {
                inputArea.Position = Vector2.Zero + new Vector2(screenSize.X / 2.0f, 0.0f);
                inputArea.Size = new Vector2(screenSize.X / 2.0f, screenSize.Y);
            }
        }

        this.Position = inputArea.Position;
        this.Size = inputArea.Size;

        if (!Engine.IsEditorHint())
        {
        }
        else
        {
            QueueRedraw();
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if(@event is InputEventScreenTouch touch)
        {
            if (inputArea.HasPoint(touch.Position))
            {
                GD.Print($"We're touching in the area for {this.Name}");
            }
        }
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint())
		{
            DrawLine(inputArea.Position, inputArea.Position + new Vector2(0, inputArea.Size.Y), Colors.Cyan);
            DrawLine(inputArea.Position, inputArea.Position + new Vector2(inputArea.Size.X, 0), Colors.Cyan);
            DrawLine(inputArea.Position + new Vector2(0, inputArea.Size.Y), inputArea.Position + new Vector2(inputArea.Size.X, inputArea.Size.Y), Colors.Cyan);
            DrawLine(inputArea.Position + new Vector2(inputArea.Size.X, 0), inputArea.Position + new Vector2(inputArea.Size.X, inputArea.Size.Y), Colors.Cyan);
        }
    }
}
