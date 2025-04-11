using Godot;
using Godot.Collections;
using System;

[Tool, GlobalClass]
public partial class UICurveNode : Control
{
    UICurve _curve;
    [Export]
    public UICurve Curve
    {
        get { 
            return _curve;
        }
        set { 
            _curve = value;
            QueueRedraw();
        }
    }
    [Export] Control targetControl;

    [Export(PropertyHint.Range, "0.0, 1.0")] float t;

    [Export] LayoutPreset anchorType = LayoutPreset.Center;

    [Export] Array<UICurveHandle> handles = new Array<UICurveHandle>();

    [ExportToolButton("Add Control Point")]
    public Callable ClickMeButton => Callable.From(AddControlPointHandle);

    public void AddControlPointHandle()
    {
        _curve.controlPoints.Add(new Vector2(0.5f, 0.5f)); 
        ManageHandleCount();
    }

    private void ManageHandleCount()
    {
        if (_curve == null) { return; }
        if (_curve.controlPoints == null) { return; }
        if (_curve.controlPoints.Count <= 0) { return; }
        if (handles == null)
        {
            handles = new Array<UICurveHandle>();
        }

        foreach (var item in handles)
        {
            item.QueueFree();
        }
        handles.Clear();

        for (int i = 0; i < _curve.controlPoints.Count; i++)
        {
            var newHandle = new UICurveHandle();
            this.AddChild(newHandle);
            newHandle.Owner = this.GetTree().EditedSceneRoot;
            newHandle.Position = _curve.controlPoints[i] * this.Size;
            newHandle.Name = $"CurveNode[{i}]";
            handles.Add(newHandle);

            newHandle.parentCurve = this;
        }
    }

    public void UpdateCurve()
    {
        if (_curve == null) { return; }
        if (_curve.controlPoints == null) { _curve.controlPoints = new Array<Vector2>(); }
        if (_curve.controlPoints.Count <= 0) { return; }
        if (handles == null) { handles = new Array<UICurveHandle>(); }
        if (handles.Count <= 0) { return; }
        for (int i = 0; i < handles.Count; i++)
        {
            if (handles[i] != null)
            {
                _curve.controlPoints[i] = handles[i].Position / this.Size;
            }
        }
        QueueRedraw();
    }

    private void UpdateHandles()
    {
        for (int i = 0; i < handles.Count; i++)
        {
            handles[i].Position = _curve.controlPoints[i] * this.Size;
        }
    }

    Vector2 oldSize;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Engine.IsEditorHint())
        {
            if(oldSize != this.Size)
            {
                oldSize = this.Size;
                UpdateHandles();
            }

            if(handles.Count != _curve.controlPoints.Count)
            {
                ManageHandleCount();
            }
        }

        if (_curve == null) { return; }
        if (targetControl == null) { return; }

        var pos = _curve.EvaluateScreen(t, this);
        //Debug.Log($"Setting Pos: {pos}");
        targetControl.GlobalPosition = pos;
        switch (anchorType)
        {
            case LayoutPreset.TopLeft:
                targetControl.GlobalPosition = pos;
                break;
            case LayoutPreset.TopRight:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(1.0f, 0.0f));
                break;
            case LayoutPreset.BottomLeft:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(0.0f, 1.0f));
                break;
            case LayoutPreset.BottomRight:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(1.0f, 1.0f));
                break;
            case LayoutPreset.CenterLeft:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(0.0f, 0.5f));
                break;
            case LayoutPreset.CenterTop:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(0.0f, 1.0f));
                break;
            case LayoutPreset.CenterRight:
                targetControl.GlobalPosition = pos - (targetControl.Size * new Vector2(1.0f, 0.5f));
                break;
            case LayoutPreset.CenterBottom:
                break;
            case LayoutPreset.Center:
                targetControl.GlobalPosition = pos - (targetControl.Size * 0.5f);
                break;
            case LayoutPreset.LeftWide:
                break;
            case LayoutPreset.TopWide:
                break;
            case LayoutPreset.RightWide:
                break;
            case LayoutPreset.BottomWide:
                break;
            case LayoutPreset.VcenterWide:
                break;
            case LayoutPreset.HcenterWide:
                break;
            case LayoutPreset.FullRect:
                break;
            default:
                break;
        }

        if (Engine.IsEditorHint())
        {
            var selection = EditorInterface.Singleton.GetSelection();
            var selectedNodes = selection.GetSelectedNodes();
            if (selectedNodes != null)
            {
                //Debug.Log($"{selectedNodes.Count}");
                if (selectedNodes.Count > 0)
                {
                    var selectedNode = selectedNodes[0];
                    if (selectedNode == this)
                    {
                        QueueRedraw();
                    }
                }
            }
        }
    }

    public override void _Draw()
    {
        base._Draw();
        if (_curve == null) { return; }
        if (targetControl == null) { return; }
        for (int i = 0; i < _curve.controlPoints.Count; i++)
        {
            var controlPoint = _curve.controlPoints[i];
            var worldPoint = _curve.controlPoints[i] * this.Size;
            DrawCircle(worldPoint, 10.0f, Colors.Cyan, false);
        }

        int rez = _curve.controlPoints.Count * 10;
        for (float c = 0; c < rez; c++)
        {
            float t1 = c / rez;
            float t2 = (c + 1) / rez;
            var worldPoint1 = _curve.Evaluate(t1) * this.Size;
            var worldPoint2 = _curve.Evaluate(t2) * this.Size;
            DrawLine(worldPoint1, worldPoint2, Colors.Red);
        }
    }
    
}
