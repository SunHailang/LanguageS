using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    private string m_preName = string.Empty;
    public string preName { get => m_preName; }

    private string m_nextName = string.Empty;
    public string nextName { get => m_nextName; }

    public bool startNext { get; private set; }

    public SceneData(string _perName, string _nextName)
    {
        m_preName = _perName;
        m_nextName = _nextName;
    }

    public void SetStartNext()
    {
        startNext = true;
    }
}
