using Godot;
using Godot.Collections;
using System;


/// <summary>
/// This is an example resource used for setting a player's input device.
/// 
/// This is used due to Godot's InputMap.
/// 
/// This basically has a list of strings that should be found as actions
/// in the input map.
/// 
/// If your game doesn't need these interactions you should make a new 
/// resource and use the IInputDevice interface. 
/// </summary>



public interface IInputDevice
{
    public bool GetInputString(string keyName, out string outputValue);
}


public enum InputDeviceEventType
{
    Key = 0,
    JoyButton = 1,
    JoyAxis = 2,
}

struct InputDeviceEventDescription
{
    public string eventName;
    public InputDeviceEventType type;
    public int inputButtonIndex; //JoyButton.Start, Key.Escape
    public float inputButtonValue; //typicaly used for JoyAxis to denote the axis direction.
}

public partial class InputDevice_JoyButtonInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public JoyButton inputButtonIndex;
}
public partial class InputDevice_JoyAxisInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public JoyAxis inputButtonIndex; 
    [Export] public float inputButtonValue; 
}
public partial class InputDevice_KeyInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public Key inputButtonIndex;
}

public partial class InputDevice_Configuration : Resource
{
    [Export] Godot.Collections.Array<InputDevice_KeyInput> _defaultKeys;
    [Export] Godot.Collections.Array<InputDevice_JoyButtonInput> _defaultJoyButtons;
    [Export] Godot.Collections.Array<InputDevice_JoyAxisInput> _defaultJoyAxis;

    public Godot.Collections.Array<InputDevice_KeyInput> DefaultKeys{
        get { return _defaultKeys; }    
    }
    public Godot.Collections.Array<InputDevice_JoyButtonInput> DefaultJoyButtons{
        get { return _defaultJoyButtons; }    
    }
    public Godot.Collections.Array<InputDevice_JoyAxisInput> DefaultJoyAxis{
        get { return _defaultJoyAxis; }    
    }
}

[GlobalClass]
public partial class InputDeviceResource : Resource
{
    [Export] InputDevice_Configuration DefaultInputConfig;


    Godot.Collections.Dictionary<string, InputEvent> inputEvents;


    ConfigFile inputConfig;

    string inputConfigPath
    {
        get
        {
            return $"user://input/input_{deviceId}.cfg";
        }
    }


    int deviceId;
    public void LoadDeviceSettings()
    {
        if (inputConfig != null)
        {
            inputConfig.Load(inputConfigPath);
        }
        else
        {
            inputConfig = new ConfigFile();
            inputConfig.Load(inputConfigPath);
        }
    }

    public void SaveDeviceSettings()
    {
        if(inputConfig != null)
        {
            inputConfig.Save(inputConfigPath);
        }
        else
        {
            inputConfig = new ConfigFile();
            inputConfig.Save(inputConfigPath);
        }
    }


    //We have the events already, so let's write them to the config
    private void WriteDeviceEventsToConfig()
    {
        foreach (var de in deviceEvents)
        {
            inputConfig.SetValue(de.Value.eventName, "Type", (int)de.Value.type);
            inputConfig.SetValue(de.Value.eventName, "InputButtonIndex", de.Value.inputButtonIndex);
            inputConfig.SetValue(de.Value.eventName, "InputButtonValue", de.Value.inputButtonValue);
        }
    }

    //we are initializing so load the events from the config
    private void LoadDeviceEventsFromConfig()
    {
        if(inputConfig == null)
        {

        }
        else
        {
            var eventSections = inputConfig.GetSections();
            for (int i = 0; i < eventSections.Length; i++)
            {
                var eventKeys = inputConfig.GetSectionKeys(eventSections[i]);
                for (int j = 0; j < eventKeys.Length; j++)
                {

                }
            }
        }
    }


    System.Collections.Generic.Dictionary<string, InputDeviceEventDescription> deviceEvents;


    public void AssignJoyButtonToAction(string actionEventName, JoyButton buttonAssignment)
    {
        if (!InputMap.HasAction($"{actionEventName}_{deviceId}"))
        {
            InputMap.AddAction($"{actionEventName}_{deviceId}");
        }
        InputEventJoypadButton buttonEvent = new InputEventJoypadButton();
        buttonEvent.ButtonIndex = buttonAssignment;
        buttonEvent.Device = deviceId;
        InputMap.ActionAddEvent($"{actionEventName}_{deviceId}", buttonEvent);

        
    }

    public void RemoveJoyButtonFromAction(string actionEventName, JoyButton buttonAssignment)
    {
        if (!InputMap.HasAction($"{actionEventName}_{deviceId}"))
        {
            InputMap.AddAction($"{actionEventName}_{deviceId}");
        }
        InputEventJoypadButton buttonEvent = new InputEventJoypadButton();
        buttonEvent.ButtonIndex = buttonAssignment;
        buttonEvent.Device = deviceId;
        InputMap.ActionEraseEvent($"{actionEventName}_{deviceId}", buttonEvent);
    }


    public void AssignJoyAxisToAction(string actionEventName, JoyAxis buttonAssignment, float axisValue)
    {
        string deviceActionName = $"{actionEventName}_{deviceId}";
        if (!InputMap.HasAction(deviceActionName))
        {
            InputMap.AddAction(deviceActionName);
        }

        InputEventJoypadMotion buttonEvent = new InputEventJoypadMotion();
        buttonEvent.Axis = buttonAssignment;
        buttonEvent.AxisValue = axisValue;
        buttonEvent.Device = deviceId;

        if(!inputEvents.ContainsKey(deviceActionName))
        {
            //doesn't have one so just add
            inputEvents.Add(deviceActionName, buttonEvent);
            InputMap.ActionAddEvent(deviceActionName, buttonEvent);
        }
        else
        {
            //we have the key so try and remove it from the map first
            if(InputMap.ActionHasEvent(deviceActionName, inputEvents[deviceActionName]))
            {
                InputMap.ActionEraseEvent(actionEventName, inputEvents[deviceActionName]);
            }
            //then we add the new one in
            inputEvents[deviceActionName] = buttonEvent;
            InputMap.ActionAddEvent(deviceActionName, buttonEvent);
        }
    }

    public void RemoveJoyAxisFromAction(string actionEventName, JoyAxis buttonAssignment, float axisValue)
    {
        if (!InputMap.HasAction($"{actionEventName}_{deviceId}"))
        {
            InputMap.AddAction($"{actionEventName}_{deviceId}");
        }
        InputEventJoypadMotion buttonEvent = new InputEventJoypadMotion();
        buttonEvent.Axis = buttonAssignment;
        buttonEvent.AxisValue = axisValue;
        buttonEvent.Device = deviceId;
        InputMap.ActionEraseEvent($"{actionEventName}_{deviceId}", buttonEvent);
    }

    public void AssignKeyToAction(string actionEventName, Key buttonAssignment)
    {
        if (!InputMap.HasAction($"{actionEventName}_{deviceId}"))
        {
            InputMap.AddAction($"{actionEventName}_{deviceId}");
        }
        InputEventKey buttonEvent = new InputEventKey();
        buttonEvent.Keycode = buttonAssignment;
        buttonEvent.Device = deviceId;
        InputMap.ActionAddEvent($"{actionEventName}_{deviceId}", buttonEvent);
    }

    public void RemoveKeyFromAction(string actionEventName, Key buttonAssignment)
    {
        if (!InputMap.HasAction($"{actionEventName}_{deviceId}"))
        {
            InputMap.AddAction($"{actionEventName}_{deviceId}");
        }
        InputEventKey buttonEvent = new InputEventKey();
        buttonEvent.Keycode = buttonAssignment;
        buttonEvent.Device = deviceId;
        InputMap.ActionEraseEvent($"{actionEventName}_{deviceId}", buttonEvent);
    }
}
