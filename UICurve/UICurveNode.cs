using Godot;
using Godot.Collections;
using System;

namespace Snowdrama.Core;

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
        if (_curve == null) { return; }
        if (targetControl == null) { return; }

        if (Engine.IsEditorHint() && GodotTools.IsNodeSelectedEditor(this))
        {
            if(handles.Count != _curve.controlPoints.Count)
            {
                ManageHandleCount();
            }
            UpdateHandles();
        }


        var curvePos = _curve.EvaluateScreen(t, this);
        var relativeCurvePos = curvePos + this.GlobalPosition;
        //targetControl.GlobalPosition = curvePos + this.GlobalPosition;
        var offset = ContentFitterTools.CornerOffset(targetControl.Size, anchorType);
        targetControl.GlobalPosition = relativeCurvePos - offset;
        //switch (anchorType)
        //{
        //    case LayoutPreset.TopLeft:
        //        targetControl.GlobalPosition = globalPos;
        //        break;
        //    case LayoutPreset.TopRight:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(1.0f, 0.0f));
        //        break;
        //    case LayoutPreset.BottomLeft:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(0.0f, 1.0f));
        //        break;
        //    case LayoutPreset.BottomRight:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(1.0f, 1.0f));
        //        break;
        //    case LayoutPreset.CenterLeft:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(0.0f, 0.5f));
        //        break;
        //    case LayoutPreset.CenterTop:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(0.0f, 1.0f));
        //        break;
        //    case LayoutPreset.CenterRight:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * new Vector2(1.0f, 0.5f));
        //        break;
        //    case LayoutPreset.CenterBottom:
        //        break;
        //    case LayoutPreset.Center:
        //        targetControl.GlobalPosition = globalPos - (targetControl.Size * 0.5f);
        //        break;
        //    case LayoutPreset.LeftWide:
        //        break;
        //    case LayoutPreset.TopWide:
        //        break;
        //    case LayoutPreset.RightWide:
        //        break;
        //    case LayoutPreset.BottomWide:
        //        break;
        //    case LayoutPreset.VcenterWide:
        //        break;
        //    case LayoutPreset.HcenterWide:
        //        break;
        //    case LayoutPreset.FullRect:
        //        break;
        //    default:
        //        break;
        //}

        QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();
        if (GodotTools.IsNodeSelectedEditor(this))
        {
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
    
}
