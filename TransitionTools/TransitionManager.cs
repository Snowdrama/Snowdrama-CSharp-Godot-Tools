using Godot;
using Godot.Collections;
using System;

public partial class TransitionManager : Node
{
    static TransitionManager instance;
    private enum TransitionState{
        Waiting,
        HideScreen,
        FakeLoad,
        Blackout,
        ShowScreen,
    }
    private TransitionState state;
    [Signal] public delegate void StartHideEventHandler();
    [Signal] public delegate void BlackoutEventHandler();
    [Signal] public delegate void StartShowEventHandler();
    [Signal] public delegate void FakeLoadEventHandler();
    [Signal] public delegate void EndedEventHandler();
    private Action startHide;
    private Action blackout; //the scene can no longer be seen. 
    private Action fakeLoad;
    private Action startShow;
    private Action ended;

    [Export(PropertyHint.NodeType, "Transition")] Transition[] transitions = new Transition[0];

    Transition currentTransition;

    float transitionValue;

    [Export] float fakeLoadTime;
    float currentFakeLoadTime;
    public override void _EnterTree()
    {
        if(instance != null)
        {
            instance.QueueFree();
        }
        instance = this;
        this.ProcessMode = ProcessModeEnum.Always;


        
    }

    public override void _Process(double delta)
    {
        if(currentTransition == null)
        {
            currentTransition = transitions.GetRandom();
        }
        switch (state)
        {
            case TransitionState.Waiting:
                //do nothing we're waiting for something to happen
                break;
            case TransitionState.HideScreen:
                transitionValue += (float)delta;
                currentTransition.SetTransitionValue(transitionValue);
                if (transitionValue > 1.0f)
                {
                    state = TransitionState.Blackout;
                    blackout?.Invoke();
                    EmitSignal(SignalName.Blackout);
                }
                break;
            case TransitionState.FakeLoad:
                currentFakeLoadTime -= (float)delta;
                if (currentFakeLoadTime < 0)
                {
                    currentFakeLoadTime = fakeLoadTime;
                    state = TransitionState.ShowScreen;
                    fakeLoad?.Invoke();
                    EmitSignal(SignalName.FakeLoad);
                }
                break;
            case TransitionState.Blackout:
                break;
            case TransitionState.ShowScreen:
                transitionValue -= (float)delta;
                currentTransition.SetTransitionValue(transitionValue);
                if(transitionValue < 0.0f)
                {
                    state = TransitionState.Waiting;
                    ended?.Invoke();
                    EmitSignal(SignalName.Ended);
                }
                break;
        }
    }

    public static void StartHideTransition()
    {
        instance.StartInstanceHide();
    }

    public static void StartShowTransition()
    {
        instance.StartInstanceShow();
    }

    public static void FakeLoadTransition()
    {
        instance.StartInstanceFakeLoad();
    }

    public void StartInstanceFakeLoad()
    {
        state = TransitionState.FakeLoad;
    }

    public void StartInstanceHide()
    {
        currentTransition = transitions.GetRandom();
        state = TransitionState.HideScreen;
        startHide?.Invoke();
        EmitSignal(SignalName.StartHide);
    }


    public void StartInstanceShow()
    {
        state = TransitionState.ShowScreen;
        startShow?.Invoke();
        EmitSignal(SignalName.StartShow);
    }



#region Callback Registers
    public static void AddSartHideCallback(Action callback)
    {
        instance.startHide += callback;
    }
    public static void RemoveSartHideCallback(Action callback)
    {
        instance.startHide -= callback;
    }

    public static void AddBlackoutCallback(Action callback)
    {
        instance.blackout += callback;
    }
    public static void RemoveBlackoutCallback(Action callback)
    {
        instance.blackout -= callback;
    }
    public static void AddFakeLoadCallback(Action callback)
    {
        instance.fakeLoad += callback;
    }
    public static void RemoveFakeLoadCallback(Action callback)
    {
        instance.fakeLoad -= callback;
    }
    public static void AddStartShowCallback(Action callback)
    {
        instance.startShow += callback;
    }
    public static void RemoveStartShowCallback(Action callback)
    {
        instance.startShow -= callback;
    }
    public static void AddEndedCallback(Action callback)
    {
        instance.ended += callback;
    }
    public static void RemoveEndedCallback(Action callback)
    {
        instance.ended -= callback;
    }
#endregion
}