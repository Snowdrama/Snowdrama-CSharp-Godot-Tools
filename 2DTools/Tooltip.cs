using Godot;
using System;

public class ShowTooltipMessage : AMessage<Vector2, string> { }
public class UpdateTooltipPositionMessage : AMessage<Vector2> { }
public class HideTooltipMessage : AMessage { }
public partial class Tooltip : Control
{
    [Export] public Label TooltipLabel { get; set; }
    [Export] public float MaxWidth { get; set; } = 512f;
    [Export] public float HoverTime { get; set; } = 1.0f;
    private float _currentHoverTime = 1.0f;
    private bool _showing = false;

    private ShowTooltipMessage showTooltip;
    private HideTooltipMessage hideTooltip;
    private UpdateTooltipPositionMessage updateTooltipPosition;
    public override void _EnterTree()
    {
        Visible = false;
        showTooltip = Messages.Get<ShowTooltipMessage>();
        hideTooltip = Messages.Get<HideTooltipMessage>();
        updateTooltipPosition = Messages.Get<UpdateTooltipPositionMessage>();
        showTooltip.AddListener(OnShowTooltip);
        hideTooltip.AddListener(OnHideTooltip);
        updateTooltipPosition.AddListener(UpdatePosition);
    }

    public override void _ExitTree()
    {
        showTooltip.RemoveListener(OnShowTooltip);
        hideTooltip.RemoveListener(OnHideTooltip);
        updateTooltipPosition.RemoveListener(UpdatePosition);
        Messages.Return<UpdateTooltipPositionMessage>();
        Messages.Return<ShowTooltipMessage>();
        Messages.Return<HideTooltipMessage>();
        updateTooltipPosition = null;
        showTooltip = null;
        hideTooltip = null;
    }

    public override void _Process(double delta)
    {
        if (_showing)
        {
            if (_currentHoverTime > 0)
            {
                _currentHoverTime -= (float)delta;
            }
            else
            {
                Visible = true;
            }
        }
    }

    private void OnShowTooltip(Vector2 pos, string tooltipString)
    {
        _showing = true;

        var font = GetThemeFont("font");
        var currentFontSize = GetThemeFontSize("font");

        // Measure text size
        var size = font.GetMultilineStringSize(
            tooltipString,
            HorizontalAlignment.Center,
            MaxWidth,
            currentFontSize + 4
        );

        TooltipLabel.Text = tooltipString;
        TooltipLabel.Size = size;
        Size = size;

        UpdatePosition(pos);
    }

    private void UpdatePosition(Vector2 pos)
    {

        GlobalPosition = pos - new Vector2(0, Size.Y);

        var vrect = GetViewportRect();
        var srect = GetRect();

        if (!vrect.Encloses(srect))
        {
            var intersection = vrect.Intersection(srect);
            GlobalPosition = pos - new Vector2(0, Size.Y) -
                             new Vector2(Size.X - intersection.Size.X,
                                         -(Size.Y - intersection.Size.Y));
        }
    }

    private void OnHideTooltip()
    {
        _showing = false;
        Visible = false;
        _currentHoverTime = HoverTime;
    }
}
