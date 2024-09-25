using Godot;
using System;

public partial class VirtualCamera3D : Node3D
{
    [Export] int activePriority = 10;
    [Export] int inactivePriority = 0;
    [Export] public int virtualCameraPriority;


    [Export] public bool lerpToTarget { get; private set; }

    [Export] public bool enableLookAtTarget { get; private set; }
    [Export] public Node3D targetToLookAt {  get; private set; }
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        VirtualCameraBrain3D.RegisterCamera(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        VirtualCameraBrain3D.UnregisterCamera(this);
    }

    // Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
    public override void _Process(double delta)
    {
        if (!Engine.IsEditorHint())
        {
            //UpdateScreenSize();
        }
    }

    public void MarkAsActive()
    {
        virtualCameraPriority = activePriority;

    }
    public void MarkAsInactive()
    {
        virtualCameraPriority = inactivePriority;
    }
}
