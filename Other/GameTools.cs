using Godot;
using System;

public partial class GameTools : Node
{
    private static bool _isClosing = false;
    public static bool ApplicationIsClosing
    {
        get
        {
            return _isClosing;
        }
    }
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            _isClosing = true;
        }
    }
}
