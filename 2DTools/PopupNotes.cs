using Godot;
using System;

/// <summary>
/// this is  tooltip that allows you to take notes.
/// </summary>
public class PopupNotesTextChangedMessage : AMessage<string> { }
public class ShowPopupNotesMessage : AMessage<Vector2, string> { }
public class UpdatePopupNotesPositionMessage : AMessage<Vector2> { }
public class HidePopupNotesMessage : AMessage { }
public class PopupNotesClosedMessage : AMessage { }
public partial class PopupNotes : Control, IInteractableUI
{
    [Export] private float paddingSize = 64.0f;
    [Export] private TextEdit TextEdit { get; set; }
    [Export] public float MaxWidth { get; set; } = 512f;
    [Export] public float HoverTime { get; set; } = 1.0f;

    [Export] public Button SaveAndClose { get; set; }
    private float _currentHoverTime = 0f;
    private bool _showing = false;
    private ShowPopupNotesMessage showPopupNotes;
    private HidePopupNotesMessage hidePopupNotes;
    private PopupNotesTextChangedMessage notePopupNotesChanged;
    private UpdatePopupNotesPositionMessage updatePopupNotesPosition;
    private PopupNotesClosedMessage popupNotesClosed;
    public override void _Ready()
    {
        base._Ready();
        TextEdit.TextChanged += TextEdit_TextChanged;
        _currentHoverTime = 0;

        SaveAndClose.ButtonDown += SaveAndClose_ButtonDown;
    }

    private void SaveAndClose_ButtonDown()
    {
        notePopupNotesChanged.Dispatch(TextEdit.Text);
        popupNotesClosed.Dispatch();
        OnHidePopupNotes();
    }

    private void TextEdit_TextChanged()
    {
        notePopupNotesChanged.Dispatch(TextEdit.Text);
    }

    public override void _EnterTree()
    {
        Visible = false;

        showPopupNotes = Messages.Get<ShowPopupNotesMessage>();
        hidePopupNotes = Messages.Get<HidePopupNotesMessage>();
        updatePopupNotesPosition = Messages.Get<UpdatePopupNotesPositionMessage>();
        notePopupNotesChanged = Messages.Get<PopupNotesTextChangedMessage>();
        popupNotesClosed = Messages.Get<PopupNotesClosedMessage>();

        showPopupNotes.AddListener(OnShowPopupNotes);
        hidePopupNotes.AddListener(OnHidePopupNotes);
        updatePopupNotesPosition.AddListener(UpdatePosition);
    }

    public override void _ExitTree()
    {
        showPopupNotes.RemoveListener(OnShowPopupNotes);
        hidePopupNotes.RemoveListener(OnHidePopupNotes);
        updatePopupNotesPosition.RemoveListener(UpdatePosition);


        Messages.Return<UpdatePopupNotesPositionMessage>();
        Messages.Return<ShowPopupNotesMessage>();
        Messages.Return<HidePopupNotesMessage>();
        Messages.Return<PopupNotesTextChangedMessage>();
        Messages.Return<PopupNotesClosedMessage>();

        notePopupNotesChanged = null;
        updatePopupNotesPosition = null;
        showPopupNotes = null;
        hidePopupNotes = null;
        popupNotesClosed = null;
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

    private void OnShowPopupNotes(Vector2 pos, string tooltipString)
    {
        _showing = true;
        TextEdit.Text = tooltipString;
        UpdatePosition(pos);
    }

    private void UpdatePosition(Vector2 pos)
    {
        GlobalPosition = pos - new Vector2(0, Size.Y);

        var vrect = GetViewportRect();
        var srect = GetRect();
        srect.Position += new Vector2(paddingSize, paddingSize);
        srect.Size -= new Vector2(paddingSize, paddingSize);

        if (!vrect.Encloses(srect))
        {
            var intersection = vrect.Intersection(srect);
            GlobalPosition = pos - new Vector2(0, Size.Y) -
                             new Vector2(Size.X - intersection.Size.X,
                                         -(Size.Y - intersection.Size.Y));
        }
    }

    private void OnHidePopupNotes()
    {
        _showing = false;
        Visible = false;
        _currentHoverTime = HoverTime;
        popupNotesClosed.Dispatch();
    }

    public void OnHoverEnter(Vector2 cursorWorldPos)
    {

    }

    public void OnHoverExit(Vector2 cursorWorldPos)
    {

    }

    public void OnMouseDown(Vector2 cursorWorldPos)
    {

    }

    public void OnMouseUp(bool releasedOnSame)
    {

    }

    public void OnDrag(Vector2 cursorWorldPos)
    {
    }
}