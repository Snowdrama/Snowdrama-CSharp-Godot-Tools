using Godot;
using System;
using static Godot.Control;

namespace Snowdrama.Core;
public static class ContentFitterTools
{

    /// <summary>
    /// Take some original size and scales it up to fit inside some parent size keeping it's aspect ratio
    /// 
    /// This should be applied to a control.Size = that is a child of the parent!
    /// </summary>
    /// <param name="originalSize"></param>
    /// <param name="parentControlSize"></param>
    /// <returns></returns>
    public static Vector2 ControlFitterSize(Vector2 originalSize, Vector2 parentControlSize)
    {
        var scaleFactor = Vector2Extensions.FindScaleFactor(originalSize, parentControlSize);
        return originalSize * scaleFactor;
    }

    /// <summary>
    /// Takes a current size an moves it to fit in the corner based on the layout preset
    /// 
    /// Note that if the currentSize and parentSize have one axis that matches, some layout options won't
    /// produce different results
    /// 
    /// For example if both have the same Y axis size, then center, centerTop, centerBottom, and fullSize
    /// all would have the same effect!
    /// </summary>
    /// <param name="currentSize"></param>
    /// <param name="parentControlSize"></param>
    /// <param name="layoutPreset"></param>
    /// <returns></returns>
    public static Vector2 ControlFitterOffset(Vector2 currentSize, Vector2 parentControlSize, LayoutPreset layoutPreset)
    {
        var offsetSize = parentControlSize - currentSize;
        switch (layoutPreset)
        {
            case LayoutPreset.TopLeft:
                Debug.Log("Top Left");
                return offsetSize * new Vector2(0.0f, 0.0f);
            case LayoutPreset.TopRight:
                return offsetSize * new Vector2(1.0f, 0.0f);
            case LayoutPreset.BottomLeft:
                return offsetSize * new Vector2(0.0f, 1.0f);
            case LayoutPreset.BottomRight:
                return offsetSize * new Vector2(1.0f, 1.0f);
            case LayoutPreset.CenterLeft:
                return offsetSize * new Vector2(0.0f, 0.5f);
            case LayoutPreset.CenterTop:
                return offsetSize * new Vector2(0.5f, 0.0f);
            case LayoutPreset.CenterRight:
                return offsetSize * new Vector2(1.0f, 0.5f);
            case LayoutPreset.CenterBottom:
                return offsetSize * new Vector2(0.5f, 1.0f);
            case LayoutPreset.Center:
                return offsetSize * 0.5f;
            case LayoutPreset.LeftWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.TopWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.RightWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.BottomWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.VcenterWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.HcenterWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.FullRect:
                return offsetSize * 0.5f;
            default:
                Debug.LogWarning("Content fitter is using default center offset! Preset Used: {layoutPreset}");
                return offsetSize * 0.5f;
        }
    }
    public static Vector2 CornerOffset(Vector2 currentSize, LayoutPreset layoutPreset)
    {
        switch (layoutPreset)
        {
            case LayoutPreset.TopLeft:
                return currentSize * new Vector2(0.0f, 0.0f);
            case LayoutPreset.TopRight:
                return currentSize * new Vector2(1.0f, 0.0f);
            case LayoutPreset.BottomLeft:
                return currentSize * new Vector2(0.0f, 1.0f);
            case LayoutPreset.BottomRight:
                return currentSize * new Vector2(1.0f, 1.0f);
            case LayoutPreset.CenterLeft:
                return currentSize * new Vector2(0.0f, 0.5f);
            case LayoutPreset.CenterTop:
                return currentSize * new Vector2(0.5f, 0.0f);
            case LayoutPreset.CenterRight:
                return currentSize * new Vector2(1.0f, 0.5f);
            case LayoutPreset.CenterBottom:
                return currentSize * new Vector2(0.5f, 1.0f);
            case LayoutPreset.Center:
                return currentSize * 0.5f;
            case LayoutPreset.LeftWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.TopWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.RightWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.BottomWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.VcenterWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.HcenterWide:
                throw new NotImplementedException("Control Fitter Doesn't implement 'wide' preset options");
            case LayoutPreset.FullRect:
                return currentSize * 0.5f;
            default:
                Debug.LogWarning("Using Default Center offset!");
                return currentSize * 0.5f;
        }
    }
}
