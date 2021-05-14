using UnityEngine;

public static class ResourcesManager
{
    private static string m_Prefabs = "Prefabs/";
    private static string m_ParticleSystems = "ParticleSystems/";



    public static T LoadParticle<T>(string name) where T : Object
    {
        return Resources.Load<T>($"{m_Prefabs}{m_ParticleSystems}{name}");
    }
}
