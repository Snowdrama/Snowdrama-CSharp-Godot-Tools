using Godot;
using System;



/// <summary>
/// the idea is that during player select, this class can distribute an
/// IInputDevice Resource that has a list of all the InptuMap values 
/// for a single player. 
///
/// These can be used to pass the strings to functions like:
/// IInputDevice device;
/// Input.GetVector(device.Get("MoveLeft"), device.Get("MoveRight"), device.Get("MoveUp"), device.Get("MoveDown");
/// 
/// Which for different devices might be the equivalent of:
/// Input.GetVector("MoveLeft_Device_0", "MoveRight_Device_0", "MoveUp_Device_0", "MoveDown_Device_0";
/// Input.GetVector("MoveLeft_Device_1", "MoveRight_Device_1", "MoveUp_Device_1", "MoveDown_Device_1";
/// 
/// Each corresponding to a different player
/// 
/// 
/// 
/// 
/// </summary>

public partial class InputManager : Node
{
	[Export] Godot.Collections.Dictionary<int, Resource> inputResourcesByDevice = new Godot.Collections.Dictionary<int, Resource>();


    public override void _Ready()
    {
        for (int i = 0; i < 8; i++)
        {
            InputMap.AddAction($"MoveUp_{i}");
            InputMap.AddAction($"MoveDown_{i}");
            InputMap.AddAction($"MoveLeft_{i}");
            InputMap.AddAction($"MoveRight_{i}");

            InputMap.AddAction($"LookUp_{i}");
            InputMap.AddAction($"LookDown_{i}");
            InputMap.AddAction($"LookLeft_{i}");
            InputMap.AddAction($"LookRight_{i}");

            InputMap.AddAction($"Pause_{i}");

            //Move Key
            InputEventKey moveEvent_key_Up = new InputEventKey();
            moveEvent_key_Up.Keycode = Key.W;
            moveEvent_key_Up.Device = i;
            InputMap.ActionAddEvent($"MoveUp_{i}", moveEvent_key_Up);

            InputEventKey moveEvent_key_down = new InputEventKey();
            moveEvent_key_down.Keycode = Key.S;
            moveEvent_key_down.Device = i;
            InputMap.ActionAddEvent($"MoveDown_{i}", moveEvent_key_down);

            InputEventKey moveEvent_key_left = new InputEventKey();
            moveEvent_key_left.Keycode = Key.A;
            moveEvent_key_left.Device = i;
            InputMap.ActionAddEvent($"MoveLeft_{i}", moveEvent_key_left);

            InputEventKey moveEvent_key_right = new InputEventKey();
            moveEvent_key_right.Keycode = Key.D;
            moveEvent_key_right.Device = i;
            InputMap.ActionAddEvent($"MoveRight_{i}", moveEvent_key_right);

            //Move Joy
            InputEventJoypadMotion moveEvent_joy_Up = new InputEventJoypadMotion();
            moveEvent_joy_Up.Axis = JoyAxis.LeftY;
            moveEvent_joy_Up.AxisValue = -1;
            moveEvent_joy_Up.Device = i;
            InputMap.ActionAddEvent($"MoveUp_{i}", moveEvent_joy_Up);

            InputEventJoypadMotion moveEvent_joy_down = new InputEventJoypadMotion();
            moveEvent_joy_down.Axis = JoyAxis.LeftY;
            moveEvent_joy_down.AxisValue = 1;
            moveEvent_joy_down.Device = i;
            InputMap.ActionAddEvent($"MoveDown_{i}", moveEvent_joy_down);

            InputEventJoypadMotion moveEvent_joy_left = new InputEventJoypadMotion();
            moveEvent_joy_left.Axis = JoyAxis.LeftX;
            moveEvent_joy_left.AxisValue = -1;
            moveEvent_joy_left.Device = i;
            InputMap.ActionAddEvent($"MoveLeft_{i}", moveEvent_joy_left);

            InputEventJoypadMotion moveEvent_joy_right = new InputEventJoypadMotion();
            moveEvent_joy_right.Axis = JoyAxis.LeftX;
            moveEvent_joy_right.AxisValue = 1;
            moveEvent_joy_right.Device = i;
            InputMap.ActionAddEvent($"MoveRight_{i}", moveEvent_joy_right);


            //Look Key
            InputEventKey lookEvent_key_Up = new InputEventKey();
            lookEvent_key_Up.Keycode = Key.Up;
            lookEvent_key_Up.Device = i;
            InputMap.ActionAddEvent($"MoveUp_{i}", lookEvent_key_Up);

            InputEventKey lookEvent_key_down = new InputEventKey();
            lookEvent_key_down.Keycode = Key.Down;
            lookEvent_key_down.Device = i;
            InputMap.ActionAddEvent($"MoveDown_{i}", lookEvent_key_down);

            InputEventKey lookEvent_key_left = new InputEventKey();
            lookEvent_key_left.Keycode = Key.Left;
            lookEvent_key_left.Device = i;
            InputMap.ActionAddEvent($"MoveLeft_{i}", lookEvent_key_left);

            InputEventKey lookEvent_key_right = new InputEventKey();
            lookEvent_key_right.Keycode = Key.Right;
            lookEvent_key_right.Device = i;
            InputMap.ActionAddEvent($"MoveRight_{i}", moveEvent_key_right);

            //Look Joy
            InputEventJoypadMotion lookEvent_joy_Up = new InputEventJoypadMotion();
            lookEvent_joy_Up.Axis = JoyAxis.RightY;
            lookEvent_joy_Up.AxisValue = -1;
            lookEvent_joy_Up.Device = i;
            InputMap.ActionAddEvent($"MoveUp_{i}", lookEvent_joy_Up);

            InputEventJoypadMotion lookEvent_joy_down = new InputEventJoypadMotion();
            lookEvent_joy_down.Axis = JoyAxis.RightY;
            lookEvent_joy_down.AxisValue = 1;
            lookEvent_joy_down.Device = i;
            InputMap.ActionAddEvent($"MoveDown_{i}", lookEvent_joy_down);

            InputEventJoypadMotion lookEvent_joy_left = new InputEventJoypadMotion();
            lookEvent_joy_left.Axis = JoyAxis.RightX;
            lookEvent_joy_left.AxisValue = -1;
            lookEvent_joy_left.Device = i;
            InputMap.ActionAddEvent($"MoveLeft_{i}", lookEvent_joy_left);

            InputEventJoypadMotion lookEvent_joy_right = new InputEventJoypadMotion();
            lookEvent_joy_right.Axis = JoyAxis.RightX;
            lookEvent_joy_right.AxisValue = 1;
            lookEvent_joy_right.Device = i;
            InputMap.ActionAddEvent($"MoveRight_{i}", lookEvent_joy_right);




            //pause Key
            InputEventKey pause_key = new InputEventKey();
            pause_key.Keycode = Key.Escape;
            pause_key.Device = i;
            InputMap.ActionAddEvent($"Pause_{i}", pause_key);

            InputEventJoypadButton pause_joy = new InputEventJoypadButton();
            pause_joy.ButtonIndex = JoyButton.Start;
            pause_joy.Device = i;
            InputMap.ActionAddEvent($"Pause_{i}", pause_joy);

        }
    }
}
