using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillActionType
{
    None = 0,
    PickType = 1,
    ExplosionType = 2,
}

public class ActionEvent : MonoBehaviour
{
    protected SkillActionType m_actionType = SkillActionType.None;
    public SkillActionType actionType { get { return m_actionType; } }

    protected float m_value = 0.0f;
    public float Value { get { return m_value; } }

    public void SetAction(SkillActionType type, float value)
    {
        m_actionType = type;
        m_value = value;
    }
}
