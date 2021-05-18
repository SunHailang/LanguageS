using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PlayerBuildStep : IBuildStep
{
    public void Execute(BuildTarget target, BuildType type, string path)
    {
        string[] scenes = GetScenes();
        BuildOptions options = GetOptions(target, type == BuildType.Development);

        //  Create output dir
        Directory.CreateDirectory(path);

        string outputName = path;

        //  Start player build
        var buildReport = BuildPipeline.BuildPlayer(scenes, outputName, target, options);
        if (buildReport.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {

            Debug.LogError("BUILD PLAYER ERROR: " + buildReport.summary);
            throw new Exception("BuildPlayer returned errors: " + buildReport.summary.totalErrors);
        }
    }

    private BuildOptions GetOptions(BuildTarget target, bool debug)
    {
        var options = BuildOptions.None;

        if (target == BuildTarget.Android)
        {
            options |= BuildOptions.AcceptExternalModificationsToPlayer;
        }

#if !PRODUCTION && !PRE_PRODUCTION
        options |= BuildOptions.Development;
#endif

        return options;
    }

    private string[] GetScenes()
    {
        List<string> toRet = new List<string>();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled && File.Exists(scene.path))
            {
                toRet.Add(scene.path);
            }
        }
        return toRet.ToArray();
    }

    public BuildStepType GetBuildType()
    {
        return BuildStepType.Direct;
    }
}
