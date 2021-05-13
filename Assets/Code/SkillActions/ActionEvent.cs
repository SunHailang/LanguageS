using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillActionType
{
    None = 0,
    PickType = 1,
}

public class ActionEvent : MonoBehaviour
{
    protected SkillActionType m_actionType = SkillActionType.None;

    public SkillActionType actionType { get { return m_actionType; } }


    public void SetAction(SkillActionType type)
    {
        m_actionType = type;
    }


    protected virtual void OnDestroy()
    {
        GameObject obj = Resources.Load("Prefabs/ParticleSystems/AddBlood") as GameObject;
        GameObject data = Instantiate(obj, transform.parent);
        data.transform.position = transform.position;
        Destroy(data, 1.0f);
    }
}
