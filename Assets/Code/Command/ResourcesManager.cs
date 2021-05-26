using UnityEngine;

public static class ResourcesManager
{
    private static string m_Prefabs = "Prefabs/";
    private static string m_ParticleSystems = "ParticleSystems/";

    private static string m_PanelSystems = "PanelSystems/";

    private static string m_Enemys = "Enemys/";

    public static readonly string m_Materials = "Materials/";


    public static T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public static T LoadPanel<T>(string name) where T : Object
    {
        return Resources.Load<T>($"{m_Prefabs}{m_PanelSystems}{name}");
    }


    public static T LoadParticle<T>(string name) where T : Object
    {
        return Resources.Load<T>($"{m_Prefabs}{m_ParticleSystems}{name}");
    }

    public static T LoadEnemys<T>(string name) where T : Object
    {
        return Resources.Load<T>($"{m_Prefabs}{m_Enemys}{name}");
    }
}
