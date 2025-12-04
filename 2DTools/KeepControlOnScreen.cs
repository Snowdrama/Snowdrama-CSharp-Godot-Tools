using Godot;
using System;

public partial class KeepControlOnScreen : Node
{
    private Control target;

    private Vector2 targetrPosition;
    private Vector2 currentPosition;
    public override void _Ready()
    {
        base._Ready();
        //this should be a child of a control
        target = this.GetParent<Control>();
        if (target == null)
        {
            Debug.LogError($"We need this to be a child of a control!");
        }
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (target.Visible)
        {
            var vrect = GetViewport().GetVisibleRect();
            var srect = target.GetRect();

            if (!vrect.Encloses(srect))
            {
                var intersection = vrect.Intersection(srect);
                currentPosition =
                    targetrPosition -
                    new Vector2(0, target.Size.Y) -
                    new Vector2(target.Size.X - intersection.Size.X, -(target.Size.Y - intersection.Size.Y));
                target.GlobalPosition = currentPosition;
            }
        }
    }

    public void SetTargetPosition(Vector2 setPosition)
    {
        targetrPosition = setPosition;
    }
}
