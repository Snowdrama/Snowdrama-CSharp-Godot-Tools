using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
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
    [Signal] public delegate void FakeLoadCompleteEventHandler();
    [Signal] public delegate void EndedEventHandler();

    private Action startHide;
    private Action blackout; //the scene can no longer be seen. 
    private Action fakeLoadComplete;
    private Action startShow;
    private Action ended;

    [Export] Transition[] transitions = new Transition[0];

    string targetTransitionName;
    Transition currentTransition;

    //fake Time
    bool automaticallyStartFakeTime;
    float transitionValue;
    float fakeLoadTime;
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

        //if we don't have a transition here we should get one or we'll break!
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
                    if (automaticallyStartFakeTime)
                    {
                        state = TransitionState.FakeLoad;
                    }
                }
                break;
            case TransitionState.FakeLoad:
                currentFakeLoadTime -= (float)delta;
                if (currentFakeLoadTime < 0)
                {
                    currentFakeLoadTime = fakeLoadTime;
                    fakeLoadComplete?.Invoke();
                    EmitSignal(SignalName.FakeLoadComplete);
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




    public static void StartShowingScreen()
    {
        instance.StartInstanceShow();
    }

    public static void StartFakeLoad()
    {
        instance.StartInstanceFakeLoad();
    }
    public static string[] GetAvailableTransitions()
    {
        return instance.transitions.Select(x => x.TransitionName).ToArray();
    }

    public void StartInstanceFakeLoad()
    {
        state = TransitionState.FakeLoad;
    }

    public void StartInstanceHide()
    {
        //try to get the 
        if (!string.IsNullOrEmpty(targetTransitionName))
        {
            var possibleTransition = transitions.Where(x => x.TransitionName == targetTransitionName.Trim().ToLower()).FirstOrDefault();
            if (possibleTransition != null)
            {
                currentTransition = possibleTransition;
            }
            else
            {
                GD.PrintErr($"No Transition Found named: {targetTransitionName} Using Random Transition");
                //we didn't find a transition so random
                currentTransition = transitions.GetRandom();
            }
        }
        else
        {
            //the transitionName is null so we should get random
            currentTransition = transitions.GetRandom();
        }
        currentFakeLoadTime = fakeLoadTime;
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


    /// <summary>
    /// WARNING: 
    /// The phases don't automatically move between states, this is to allow you to do work duing the blackout
    /// For example, once the blackout callback is triggered, work should begin, once done, StartShow() should be called
    /// 
    /// WARNING:
    /// Fake load by default happens automatically, you should only Call StartShow() from the onFakeLoad callback
    /// this will ensure you correctly wait for the fake load to end, even if work is done before the fake load is done.
    /// 
    /// Starts a transition, and takes action callbacks for each of the phases of the transition so 
    /// the caller can respond to the phases of the transition
    /// 
    /// The transition has 5 states the most important one is the Blackout and Ended callbacks, 
    /// blackout indicates the screen is obscured so anything that is done after this is hidden from the player's view
    /// and ended is called when the transition is complete and the transition manager is ready for a new transition.
    /// 
    /// </summary>
    /// <param name="onStartHide">The callback used the first frame after the transition started</param>
    /// <param name="onBlackout">The callback used the first fram after the screen was fully obscured</param>
    /// <param name="onFakeLoadComplete">The callback used after the set fake load delay</param>
    /// <param name="onStartShow">The callback used the frame that we start revealing the new scene.</param>
    /// <param name="onEnded">The callback used the frame after the transition has fully ended</param>
    /// <param name="transitionName">the name of the transition to use, if left null it will choose one at random</param>
    /// <param name="fakeLoadTime">the UpdateTimeMax of how long the fake load should wait before calling complete</param>
    /// <param name="automaticallyStartFakeTime">Should it automatically go to the fakeLoad on blackout?</param>
    public static void StartTransition(
        Action onStartHide, 
        Action onBlackout, 
        Action onFakeLoadComplete, 
        Action onStartShow, 
        Action onEnded,
        string transitionName = null,
        float fakeLoadTime = 1.0f,
        bool automaticallyStartFakeTime = true
        )
    {
        instance.startHide = onStartHide;
        instance.blackout = onBlackout;
        instance.fakeLoadComplete = onFakeLoadComplete;
        instance.startShow = onStartShow;
        instance.ended = onEnded;
        instance.targetTransitionName = transitionName;
        instance.fakeLoadTime = fakeLoadTime;
        instance.automaticallyStartFakeTime = automaticallyStartFakeTime;
        instance.StartInstanceHide();
    }
}