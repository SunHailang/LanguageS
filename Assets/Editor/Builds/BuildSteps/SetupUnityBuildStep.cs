using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetupUnityBuildStep : IBuildStep
{
    public void Execute(BuildTarget target, BuildType type, string path)
    {
        if (target == BuildTarget.Android)
        {
            UpdateAndroid(target, type);
        }

        //  Shared amongst multiple targets
        UpdateAllPlatforms(target, type);
    }

    void UpdateAndroid(BuildTarget target, BuildType type)
    {
        //  Check if SDK is setup
        CheckAndroidSKDPath();

        CheckOrientation(type);
        //  Check for OBB variant
        CheckForOBB(type);

        CheckForAndroidProject();

        PlayerSettings.strippingLevel = StrippingLevel.StripByteCode;
        //	Override ETC
        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ETC;
    }

    void CheckAndroidSKDPath()
    {
        string sdkPath = EditorPrefs.GetString("AndroidSdkRoot");
        if (sdkPath == null || sdkPath.Length == 0)
        {
            throw new Exception("Android SDK path is not set!!!");
        }
    }

    void CheckForOBB(BuildType type)
    {
        //  AAB FILES ARE NOW IN FASION - NO OBBs ANYMORE
        PlayerSettings.Android.useAPKExpansionFiles = false;
    }

    void CheckOrientation(BuildType type)
    {
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
    }

    void CheckForAndroidProject()
    {
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
    }

    //  Shared platform logic
    void UpdateAllPlatforms(BuildTarget target, BuildType type)
    {
        //  Define symbols
        UpdatePreprocessorSymbols(target, type);

        //  AOT updates
        UpdateAOTSettings(type);

    }

    void UpdatePreprocessorSymbols(BuildTarget target, BuildType type)
    {
        BuildTargetGroup group = BuildTargetGroup.iOS;
        switch (target)
        {
            case BuildTarget.iOS:
                group = BuildTargetGroup.iOS;
                break;
            case BuildTarget.tvOS:
                group = BuildTargetGroup.tvOS;
                break;
            case BuildTarget.Android:
                group = BuildTargetGroup.Android;
                break;
            default:
                Debug.LogError("Build (PreBuild) :: UNKNOWN BUILD TARGET - " + target);
                break;
        };

        string scriptingDefines = SetupUnityBuildStepSettings.PreprocessorDefines[target][type];
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, scriptingDefines);
    }

    void UpdateAOTSettings(BuildType type)
    {
        PlayerSettings.aotOptions = SetupUnityBuildStepSettings.AOT_CompilationOptions;
    }


    public BuildStepType GetBuildType()
    {
        return BuildStepType.Direct;
    }
}
