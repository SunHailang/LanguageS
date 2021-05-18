using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Builds
{

    [MenuItem("LangS/Builds/Android/Dev")]
    public static void BuildDev()
    {
        BuildsProcess.PerformBuild(BuildTarget.Android, BuildType.Development);
    }
}
