using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SetupUnityBuildStepSettings
{
    public static readonly Dictionary<BuildTarget, Dictionary<BuildType, string>> PreprocessorDefines = new Dictionary<BuildTarget, Dictionary<BuildType, string>>
        {
            {
                BuildTarget.iOS, new Dictionary<BuildType, string>
                {
                    { BuildType.Development, "" },
                    { BuildType.Preproduction, "PREPRODUCTION;" },
                    { BuildType.Production, "PRODUCTION;" },
                    { BuildType.Marketing, "PRODUCTION;MARKETING_BUILD;" }
                }
            },
            {
                BuildTarget.Android, new Dictionary<BuildType, string>
                {
                    { BuildType.Development, "" },
                    { BuildType.Preproduction, "PREPRODUCTION;" },
                    { BuildType.Production, "PRODUCTION;" },
                    { BuildType.Marketing, "" }
                }
            }
        };

    public static readonly string AOT_CompilationOptions = "nimt-trampolines=512";
}
