using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

/// <summary>
/// NOTE FUTURE ME: This uses the first node's name in the file, NOT THE SCENES FILE NAME!!!!
/// </summary>
public partial class SceneManager : Node
{
    [ExportCategory("Manual Assignment")]
    [Export] PackedScene[] packedScenes;

    [ExportCategory("Automatic Assignment - res://Path (no final slash)")]
    [Export] string resourcePath = "res://Scenes";

    [ExportCategory("Runtime Exposed Values")]
    [Export] public Node currentScene;
    [Export] string sceneTarget;


    [ExportCategory("Note: This uses the first node's name in the file, NOT the file name!")]
    static SceneManager instance;
    Dictionary<string, PackedScene> _scenes = new Dictionary<string, PackedScene>();
    static string previousSceneName;
    public override void _Ready()
    {
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#0080FF]Reminder the SceneManager uses the first node's name in the file, NOT THE SCENES FILE NAME!!!![/color][/wave]");
        GD.PrintRich("[wave amp=25.0 freq=10.0][color=#00FF80]Also that if the name contains spaces it will escape them with underscores!!![/color][/wave]");
        for (int i = 0; i < packedScenes.Length; i++)
        {
            if (packedScenes[i] != null)
            {
                //var name = packedScenes[i].ResourcePath.Split("/").GetLastElement().Replace(".tscn", "");
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

        var sceneResourcePath = DirAccess.Open(resourcePath);
        if (sceneResourcePath != null)
        {
            var filePaths = sceneResourcePath.GetFiles();
            for (int i = 0; i < filePaths.Length; i++)
            {
                var possiblyAScene = GD.Load<PackedScene>($"{resourcePath}/{filePaths[i]}");
                string name = possiblyAScene.GetState().GetNodeName(0);
                name = name.Replace(" ", "_");

                if (!_scenes.ContainsKey(name))
                {
                    GD.Print($"Adding to Scene Manager: {name}");
                    _scenes.Add(name, possiblyAScene);
                }
                else
                {
                    GD.PrintErr($"Scene Already Exiists in List: {name}");
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


    private Node SwapScenes(Node sceneToRemove, string newSceneName)
    {
        if (_scenes.ContainsKey(newSceneName))
        {
            if (sceneToRemove != null)
            {
                previousSceneName = sceneToRemove.Name;
                sceneToRemove.QueueFree();
            }
            var instantiatedScene = _scenes[newSceneName].Instantiate();
            this.AddSibling(instantiatedScene);

            return instantiatedScene;
        }
        else
        {
            GD.PrintErr($"Scene {sceneTarget} was not found in list of scenes");
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
        previousSceneName = currentScene.Name;
        currentScene = SwapScenes(currentScene, sceneTarget);
        TransitionManager.FakeLoadTransition();
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