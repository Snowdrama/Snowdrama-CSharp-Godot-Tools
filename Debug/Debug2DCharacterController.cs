using Godot;
using System;

public partial class Debug2DCharacterController : CharacterBody2D
{
    [Export]
    float speed;
    Vector2 moveDirection;
    public override void _PhysicsProcess(double delta)
    {
        this.Velocity = moveDirection * speed;
        this.MoveAndSlide();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsAction("MoveLeft") || @event.IsAction("MoveRight") || @event.IsAction("MoveDown") || @event.IsAction("MoveUp"))
        {
            moveDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
        }
    }
}
