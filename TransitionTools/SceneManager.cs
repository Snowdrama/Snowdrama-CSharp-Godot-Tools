using Godot;
using System;
using System.Collections.Generic;

public partial class SceneManager : Node
{
    static SceneManager instance;
    Dictionary<string, PackedScene> _scenes = new Dictionary<string, PackedScene>();


    [Export] PackedScene[] packedScenes;

    [Export] public Node currentScene;
    [Export] string sceneTarget;
    [Export] bool run;
    public override void _Ready()
    {
        for (int i = 0; i < packedScenes.Length; i++)
        {
            if (packedScenes[i] != null)
            {
                //var name = packedScenes[i].ResourcePath.Split("/").GetLastElement().Replace(".tscn", "");
                var name = packedScenes[i].GetState().GetNodeName(0);
                GD.Print(name);
                _scenes.Add(name, packedScenes[i]);
            }
        }

        if (instance != null)
        {
            instance.QueueFree();
        }
        instance = this;
        this.ProcessMode = ProcessModeEnum.Always;

        //GDConsole.AddCommand<string>("GoToScene", InstanceLoadScene);
    }

    public override void _ExitTree()
    {
        //GDConsole.RemoveCommand<string>("GoToScene", InstanceLoadScene);
    }

    public override void _Process(double delta)
    {
        if (run)
        {
            run = false;
            SceneManager.LoadScene(sceneTarget);

        }
    }

    private Node SwapScenes(Node sceneToRemove, string newSceneName)
    {
        if (_scenes.ContainsKey(newSceneName))
        {
            if (sceneToRemove != null)
            {
                sceneToRemove.QueueFree();
            }
            var instantiatedScene = _scenes[newSceneName].Instantiate();
            this.AddSibling(instantiatedScene);

            return instantiatedScene;
        }
        else
        {
            Debug.LogError($"Scene {sceneTarget} was not found in list of scenes");
            return null;
        }
    }


    public static void LoadScene(string sceneName)
    {
        instance.InstanceLoadScene(sceneName);
    }
    public void InstanceLoadScene(string sceneName)
    {
        sceneTarget = sceneName;
        GD.Print($"Trying To Load: {sceneTarget}");
        if(sceneTarget == null)
        {
            Debug.LogError($"Scene name in null, need to pass a scene name!");
        }
        if (!_scenes.ContainsKey(sceneTarget))
        {
            Debug.LogError($"Scene name {sceneTarget} not found in list");
        }
        GD.Print($"Loading: {sceneTarget}");
        TransitionManager.AddBlackoutCallback(OnTransitionBlackout);
        TransitionManager.AddEndedCallback(OnTransitionEnded);
        TransitionManager.AddFakeLoadCallback(OnFakeLoadComplete);
        TransitionManager.StartHideTransition();
    }

    private void OnTransitionEnded()
    {
        TransitionManager.RemoveBlackoutCallback(OnTransitionBlackout);
        TransitionManager.RemoveEndedCallback(OnTransitionEnded);
    }
    private void OnFakeLoadComplete()
    {
        TransitionManager.StartShowTransition();
    }

    private void OnTransitionBlackout()
    {
        currentScene = SwapScenes(currentScene, sceneTarget);
        TransitionManager.FakeLoadTransition();
    }
}