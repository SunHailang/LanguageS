using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PostBuildStep : IBuildStep
{
    public void Execute(BuildTarget target, BuildType type, string path)
    {
        if (target == BuildTarget.Android)
        {
            PostBuildAndroid(type, path);
        }
    }

    public BuildStepType GetBuildType()
    {
        return BuildStepType.Post;
    }

    void PostBuildAndroid(BuildType type, string path)
    {
        if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
        {
            BuildUtility.CopyFilesRecursively(new DirectoryInfo(Application.dataPath + "/../Tools/JenkinsScripts/AndroidGradleStuff"), new DirectoryInfo(path + "/" + Application.productName));
            BuildUtility.CopyFilesRecursively(new DirectoryInfo(Application.dataPath + "/../Tools/Android"), new DirectoryInfo(path + "/" + Application.productName));

            string gradleBuildType = "Debug";
            if (type == BuildType.Production || type == BuildType.Preproduction)
            {
                gradleBuildType = "Release";
            }
            //return;
            try
            {
                //BuildUtility.Instance.RunGradleProcess_CN(path, gradleBuildType);

                //  Run APK support
                BuildUtility.RunGradleProcess(path, gradleBuildType);

                //  Bundle support
                BuildUtility.RunGradleProcess(path, gradleBuildType, "bundle");

                string location = Application.dataPath + "/../Builds/";
                location += "Android";
                BuildUtility.RunCopyAPKFileProcess(path, location, type);

                FileInfo endObbFile = new FileInfo(path + "/" + Application.productName + ".main.obb");
                string obbFinalName = "main." + PlayerSettings.Android.bundleVersionCode.ToString() + "." + Application.identifier + ".obb";
                if (endObbFile.Exists)
                {
                    endObbFile.MoveTo(location + "/" + obbFinalName);
                }
                BuildUtility.OpenDirectory(location);
            }
            catch (Exception e)
            {
                Debug.LogError("Android build python process failed. msg : " + e.Message);
            }
        }
    }
}
