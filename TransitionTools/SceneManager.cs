using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;

/// <summary>
/// NOTE FUTURE ME: This uses the first node's name in the file, NOT THE SCENES FILE NAME!!!!
/// </summary>
public partial class SceneManager : Node
{
    [ExportCategory("Manual Assignment")]
    [Export] PackedScene[] packedScenes = new PackedScene[0];

    [ExportCategory("Automatic Assignment (no final slash example: res://Scenes)")]
    [Export] string[] automaticScenePaths = new string[] { "res://Scenes" };

    [ExportCategory("Runtime Exposed Values")]
    [Export] public Node currentScene;
    [Export] string sceneTarget;

    [ExportCategory("Note: This uses the first node's name in the file, NOT the file name!")]
    static SceneManager instance;
    Dictionary<string, PackedScene> _scenes = new Dictionary<string, PackedScene>();
    static string previousSceneName;

    bool transitioning;
    bool sceneLoaded;
    bool fakeLoadComplete;
    public override void _Ready()
    {
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#0080FF]Reminder the SceneManager uses the first node's name in the file, NOT THE SCENES FILE NAME!!!![/color][/wave]");
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#00FF80]Also that if the name contains spaces it will escape them with underscores!!![/color][/wave]");


        for (int i = 0; i < packedScenes.Length; i++)
        {
            if (packedScenes[i] != null)
            {
                string possible_name = packedScenes[i].ResourcePath.Split("/").GetLastElement().Replace(".tscn", "");
                GD.Print($"packedScenes[i].ResourcePath: {packedScenes[i].ResourcePath}");
                GD.Print($"possible_name: {possible_name}");
                string name = packedScenes[i].GetState().GetNodeName(0);
                name = name.Replace(" ", "_");
                if (!_scenes.ContainsKey(name))
                {
                    GD.Print($"Adding to Scene Manager: {name}");
                    _scenes.Add(name, packedScenes[i]);
                }
                else
                {
                    GD.PrintErr($"Scene Already Exiists in List: {name}");
                }
            }
        }

        foreach (var resourcePath in automaticScenePaths)
        {
            var sceneResourcePath = DirAccess.Open(resourcePath);
            if (sceneResourcePath != null)
            {
                var filePaths = sceneResourcePath.GetFiles();
                for (int i = 0; i < filePaths.Length; i++)
                {
                    GD.Print($"Loading Scene from: {resourcePath}/{filePaths[i]}");
                    var possiblyAScene = GD.Load<PackedScene>($"{resourcePath}/{filePaths[i]}");
                    string name = possiblyAScene.GetState().GetNodeName(0);
                    name = name.Replace(" ", "_");

                    if (!_scenes.ContainsKey(name))
                    {
                        _scenes.Add(name, possiblyAScene);
                    }
                    else
                    {
                        GD.PrintErr($"Scene Already Exiists in List: {name}");
                    }
                }
            }
        }

        if (instance != null)
        {
            instance.QueueFree();
        }

        instance = this;
        this.ProcessMode = ProcessModeEnum.Always;

        if(currentScene != null)
        {
            previousSceneName = currentScene.Name;
        }
    }

    //Thread asyncPackedSceneLoader;
    private void SwapScenes(Node sceneToRemove, string newSceneName)
    {
        if (_scenes.ContainsKey(newSceneName))
        {
            if (sceneToRemove != null)
            {
                previousSceneName = sceneToRemove.Name;
                sceneToRemove.QueueFree();
            }

            //TODO: Figure out how to load the scene async.
            //asyncPackedSceneLoader = new Thread(() => { LoadSceenAsync(_scenes[newSceneName]); });
            //asyncPackedSceneLoader.IsBackground = true;
            //asyncPackedSceneLoader.Start();

            var instantiatedScene = _scenes[newSceneName].Instantiate();
            this.AddSibling(instantiatedScene);
            sceneLoaded = true;
            currentScene = instantiatedScene;
        }
        else
        {
            GD.PrintErr($"Scene {sceneTarget} was not found in list of scenes");
        }
    }

    public static void LoadScene(
        string sceneName, 
        string transitionName = null, 
        float fakeLoadTime = 1.0f,
        Action onStartHide = null,
        Action onBlackout = null,
        Action onFakeLoadComplete = null,
        Action onStartShow = null,
        Action onEnded = null
        )
    {
        instance.InstanceLoadScene(
            sceneName, 
            transitionName, 
            fakeLoadTime,
            onStartHide,
            onBlackout,
            onFakeLoadComplete,
            onStartShow,
            onEnded
            );
    }

    private void InstanceLoadScene(
        string sceneName, 
        string transitionName, 
        float fakeLoadTime,
        Action onStartHide,
        Action onBlackout,
        Action onFakeLoadComplete,
        Action onStartShow,
        Action onEnded
        )
    {
        transitioning = true;
        sceneLoaded = false;
        fakeLoadComplete = false;

        sceneTarget = sceneName;
        GD.Print($"Trying To Load: {sceneTarget}");
        if(sceneTarget == null)
        {
            GD.PrintErr($"Scene name in null, need to pass a scene name! Use one of these:");
            foreach (var item in _scenes)
            {
                GD.PrintErr($"Scene Name Keys: {item.Key}");
            }
            return;
        }
        if (!_scenes.ContainsKey(sceneTarget))
        {
            GD.PrintErr($"Scene name {sceneTarget} not found in list. It needs to be in the list! Scenes in List:");
            foreach (var item in _scenes)
            {
                GD.PrintErr($"Scene Name Keys: {item.Key}");
            }
            return;
        }
        GD.Print($"Loading: {sceneTarget}");
        TransitionManager.StartTransition(
            () => { }, 
            OnTransitionBlackout, 
            OnFakeLoadComplete, 
            () => { }, 
            () => { }, 
            transitionName, 
            fakeLoadTime);
    }

    private void OnFakeLoadComplete()
    {
        fakeLoadComplete = true;
    }

    public override void _Process(double delta)
    {
        if (transitioning  && sceneLoaded && fakeLoadComplete)
        {
            transitioning = false;
            sceneLoaded = false;
            fakeLoadComplete = false;
            TransitionManager.StartShowingScreen();
        }
    }

    private void OnTransitionBlackout()
    {
        previousSceneName = currentScene.Name;
        SwapScenes(currentScene, sceneTarget);
    }

    public static string GetPreviousSceneName()
    {
        return previousSceneName;
    }

    public static void SetCurrentScene(Node currentNode)
    {
        instance.currentScene = currentNode;
    }
}