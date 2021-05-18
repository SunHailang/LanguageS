using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildsProcess
{
    //  List of all build steps to execute
    private static readonly List<IBuildStep> m_steps;

    //  Flag to check the build type used
    public static BuildType BuildType = BuildType.Development;

    static BuildsProcess()
    {
        m_steps = new List<IBuildStep>();

        m_steps.Add(new SetupUnityBuildStep());

        m_steps.Add(new PlayerBuildStep());

        //m_steps.Add(new PostBuildStep());
    }

    public static List<IBuildStep> GetBuildSteps(BuildStepType type)
    {
        return m_steps.Where(s => s.GetBuildType() == type).ToList();
    }

    public static List<IBuildStep> GetStepsSorted()
    {
        List<IBuildStep> steps = new List<IBuildStep>();
        steps.AddRange(GetBuildSteps(BuildStepType.Pre));
        steps.AddRange(GetBuildSteps(BuildStepType.Direct));
        steps.AddRange(GetBuildSteps(BuildStepType.Post));
        return steps;
    }

    private static void Execute(BuildTarget target, BuildType type, string path)
    {
        Debug.Log("Making build " + target + " to " + path);

        BuildType = type;

        try
        {
            //  Execute all steps in sequence
            BuildStepExecutor.Execute(GetStepsSorted(), target, type, path);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static void PerformBuild(BuildTarget target, BuildType type)
    {
        string openedScene = GetLoadedScene();

        string buildPath = BuildsProcess.GetBuildPath(target, type);
        Execute(target, type, buildPath);

        if (!string.IsNullOrEmpty(openedScene) && openedScene != GetLoadedScene())
        {
            EditorSceneManager.OpenScene(openedScene);
        }
    }

    private static string GetLoadedScene()
    {
        UnityEngine.SceneManagement.Scene currentScene = EditorSceneManager.GetActiveScene();
        return currentScene.path;
    }

    public static string GetBuildPath(BuildTarget target, BuildType type)
    {
        string path = Application.dataPath + "/../Builds/";
        switch (target)
        {
            case BuildTarget.iOS:
                path += "iOS/";
                break;
            case BuildTarget.Android:
                path += "Android/";
                break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                path += "PC/";
                break;
            case BuildTarget.tvOS:
                path += "TVOS/";
                break;
            default:
                path += "Unknown/";
                break;
        };

        path += GetBuildOutputName(type, target);

        //  To prevent unity throwing exception on 5.6+
        string absolutePath = System.IO.Path.GetFullPath(path);
        return absolutePath;
    }

    public static string GetBuildOutputName(BuildType type, BuildTarget target)
    {
        string toRet = PlayerSettings.productName.Replace(" ", "") + "_" + type;
        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            toRet += ".exe";
        return toRet;
    }


}
