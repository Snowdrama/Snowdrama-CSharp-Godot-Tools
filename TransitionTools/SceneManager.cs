using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;

/// <summary>
/// NOTE FUTURE ME: This uses the first node's name in the file, NOT THE SCENES FILE NAME!!!!
/// </summary
[GlobalClass]
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

    private static bool _isTransitioningScenes = false;
    public static bool IsTransitioning
    {
        get
        {
            return _isTransitioningScenes;
        }
    }

    [ExportCategory("Debug")]
    [Export] bool pulPackedScenesFromAutoPath;
    public override void _EnterTree()
    {
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#0080FF]Reminder the SceneManager uses the first node's name in the file, NOT THE SCENES FILE NAME!!!![/color][/wave]");
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#00FF80]Also that if the name contains spaces it will escape them with underscores!!![/color][/wave]");

        for (int i = 0; i < packedScenes.Length; i++)
        {
            if (packedScenes[i] != null)
            {
                string possible_name = packedScenes[i].ResourcePath.Split("/").GetLastElement().Replace(".tscn", "");
                string name = packedScenes[i].GetState().GetNodeName(0);
                name = name.Replace(" ", "_");
                if (!_scenes.ContainsKey(name))
                {
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
                LoadScenesFromDirectory(resourcePath, sceneResourcePath);
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

    

    private void LoadScenesFromDirectory(string resourcePath, DirAccess sceneResourcePath)
    {
        var filePaths = sceneResourcePath.GetFiles();
        for (int i = 0; i < filePaths.Length; i++)
        {
            GD.Print($"Loading from: {resourcePath}/{filePaths[i]}");
            var possiblePath = $"{resourcePath}/{filePaths[i]}";
            if (possiblePath.Contains(".remap"))
            {
                possiblePath = possiblePath.Replace(".remap", "");
            }

            if (!ResourceLoader.Exists(possiblePath)){
                GD.PrintErr($"Path {possiblePath} doesn't point to a path the resource loader can load");
                continue;
            }

            if (!possiblePath.Contains(".tscn"))
            {
                Debug.LogWarn($"Path {possiblePath} is not a scene so skipping");
                continue;
            }

            var possiblyAScene = ResourceLoader.Load<PackedScene>(possiblePath);
            if(possiblyAScene == null)
            {
                Debug.LogWarn($"Path {possiblePath} loaded but is null for some reason, skipping");
                continue;
            }

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

        var directoryPaths = sceneResourcePath.GetDirectories();

        for (int i = 0; i < directoryPaths.Length; i++)
        {
            var directoryResourcePath = DirAccess.Open($"{resourcePath}/{directoryPaths[i]}");
            if(directoryResourcePath != null)
            {
                LoadScenesFromDirectory($"{resourcePath}/{directoryPaths[i]}", directoryResourcePath);
            }
        }
    }

    //Thread asyncPackedSceneLoader;
    private void SwapScenes(Node sceneToRemove, string newSceneName)
    {
        if (_scenes.ContainsKey(newSceneName))
        {
            if (sceneToRemove != null)
            {
                GD.Print($"Removing Scene: {sceneToRemove.Name}");
                previousSceneName = sceneToRemove.Name;
                sceneToRemove.QueueFree();
            }

            GD.Print($"Instatiating Scene: {newSceneName}");
            var instantiatedScene = _scenes[newSceneName].Instantiate();
            instantiatedScene.Ready += () =>
            {
                GD.Print("Instantiated Scene Ready!");
                sceneLoaded = true;
                currentScene = instantiatedScene;
            };
            this.AddSibling(instantiatedScene);
        }
        else
        {
            GD.PrintErr($"Scene {sceneTarget} was not found in list of scenes");
        }
    }

    public static void LoadSceneGDS(string sceneName)
    {
        LoadScene(sceneName);
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
        _isTransitioningScenes = true;
        TransitionManager.StartTransition(
            () => { }, 
            OnTransitionBlackout, 
            OnFakeLoadComplete, 
            () => { }, 
            () => {
                _isTransitioningScenes = false;
            }, 
            transitionName, 
            fakeLoadTime);
    }

    private void OnFakeLoadComplete()
    {
        fakeLoadComplete = true;
    }

    public override void _Process(double delta)
    {
        if (transitioning && sceneLoaded && fakeLoadComplete)
        {
            transitioning = false;
            sceneLoaded = false;
            fakeLoadComplete = false;
            TransitionManager.StartShowingScreen();
        }
    }

    private void OnTransitionBlackout()
    {
        if(currentScene != null)
        {
            previousSceneName = currentScene.Name;
        }
        SwapScenes(currentScene, sceneTarget);
    }

    public static string GetPreviousSceneName()
    {
        return previousSceneName;
    }

    public static void SetCurrentScene(Node currentNode)
    {
        if(currentNode != null)
        {
            instance.currentScene = currentNode;
        }
    }
}