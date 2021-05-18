using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class BuildUtility
{
    public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories())
        {
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
        }

        foreach (FileInfo file in source.GetFiles())
        {
            //never copy meta files
            if (!file.Name.Contains(".meta"))
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }
        }
    }

    public static void RunGradleProcess(string buildPath, string gradleBuildType, string packageType = "assemble")
    {
        string directory = Path.Combine(buildPath, Application.productName);

        //FixPermissionsForDirectory(buildPath);
        string executable = Path.Combine(directory, "gradlew.bat");
        string arguments = packageType + gradleBuildType;
        if (System.Environment.OSVersion.Platform == PlatformID.MacOSX
            || System.Environment.OSVersion.Platform == PlatformID.Unix)
        {
            executable = Path.Combine(directory, "gradlew");
        }
        // Run Python to start build.
        ProcessStartInfo procStartInfo = new ProcessStartInfo();
        procStartInfo.FileName = executable;
        procStartInfo.Arguments = arguments;
        procStartInfo.UseShellExecute = false;
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.WorkingDirectory = directory;
        procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        String result = proc.StandardOutput.ReadToEnd();
        String error = proc.StandardError.ReadToEnd();  //Some ADB outputs use this

        string gradleLog = "/gradle_" + packageType + ".log";
        string gradleErrorLog = "/gradle_error_" + packageType + ".log";

        if (result.Length > 1)
        {
            if (File.Exists(buildPath + gradleLog))
            {
                File.Delete(buildPath + gradleLog);
            }
            File.WriteAllText(buildPath + gradleLog, result);
        }
        if (error.Length > 1)
        {
            if (File.Exists(buildPath + gradleErrorLog))
            {
                File.Delete(buildPath + gradleErrorLog);
            }
            File.WriteAllText(buildPath + gradleErrorLog, error);
        }
        proc.Close();
    }

    public static void RunCopyAPKFileProcess(string buildPath, string targetPath, BuildType type)
    {
        string channel = string.Empty;
        if (type == BuildType.Development)
        {
            channel += "dev_";
        }

        string path = buildPath + "/" + Application.productName + "/build/outputs/apk/release/";
        if (Directory.Exists(path))
        {
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            foreach (var item in files)
            {
                if (Path.GetExtension(item.FullName) == ".apk")
                {
                    FileInfo gradleAPK = new FileInfo(item.FullName);
                    string location = targetPath + "/{0}.apk";
                    string versionIndex = "0";
                    string oldName = Path.GetFileNameWithoutExtension(item.FullName);
                    string[] str1 = oldName.Split('_');
                    if (str1.Length >= 3)
                    {
                        string apkName = string.Format("{0}_{1}.{2}_{3}_{4}", str1[0], str1[1], versionIndex, str1[2], channel);
                        string APKLocation = string.Format(location, apkName);
                        if (File.Exists(APKLocation))
                        {
                            File.Delete(APKLocation);
                        }
                        //HungryShark_Production-3.7.0-v183.symbols.zip
                        DirectoryInfo symbols = new DirectoryInfo(targetPath);
                        FileInfo[] symbolsFiles = symbols.GetFiles();
                        foreach (var symbolsFile in symbolsFiles)
                        {
                            if (symbolsFile.Name.StartsWith(Application.productName) && symbolsFile.Name.EndsWith(".symbols.zip"))
                            {
                                string symbolsNewFile = string.Format("{0}/{1}.symbols.zip", targetPath, apkName);
                                if (File.Exists(symbolsNewFile))
                                {
                                    File.Delete(symbolsNewFile);
                                }
                                symbolsFile.MoveTo(symbolsNewFile);
                            }
                        }
                        gradleAPK.CopyTo(APKLocation, true);
                    }
                    break;
                }
            }
        }
    }

    public static void OpenDirectory(string path)
    {
        var p = Path.GetFullPath(path);
        if (!Directory.Exists(p))
        {
            throw new DirectoryNotFoundException(p);
        }
        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
        psi.FileName = "explorer";
        psi.Arguments = p;
        using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
        {
            process.WaitForExit();
        }
    }
}
