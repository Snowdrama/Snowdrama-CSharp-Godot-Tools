using Godot;
using System;

public interface IInteractableUI
{
    void OnHoverEnter(Vector2 cursorWorldPos);
    void OnHoverExit(Vector2 cursorWorldPos);
    void OnMouseDown(Vector2 cursorWorldPos);
    void OnMouseUp(bool releasedOnSame);
    void OnDrag(Vector2 cursorWorldPos);
    int GetZIndex();
    StringName GetName();
}
