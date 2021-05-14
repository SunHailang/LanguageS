using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    private Transform m_mushroomsParent;
    [SerializeField]
    private Transform m_mushroomsPrefab;
    [SerializeField]
    private Transform m_flowersPrefab;
    [SerializeField]
    private Transform m_flowersLowPrefab;


    private void Start()
    {
        for (float i = -49; i < 49; i += 10.5f)
        {
            for (float j = -40; j < 40; j += 8.8f)
            {
                Transform trans = Instantiate(m_mushroomsPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.6f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.7f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                trans.gameObject.AddComponent<ExplosionActionEvent>().SetAction(SkillActionType.ExplosionType, -12.5f);
                trans.gameObject.name = $"mushroom{i + j}";
            }
        }

        for (float i = -49; i < 49; i += 8.5f)
        {
            for (float j = -40; j < 40; j += 5.8f)
            {
                Transform trans = Instantiate(m_flowersPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.6f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.7f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                PickActionEvent action = trans.gameObject.AddComponent<PickActionEvent>();
                action.SetAction(SkillActionType.PickType, 10.5f);
                action.SetReplyType(ReplyType.Blood);
                trans.gameObject.name = $"flowers{i + j}";
            }
        }

        for (float i = -49; i < 49; i += 5.5f)
        {
            for (float j = -40; j < 40; j += 3.8f)
            {
                Transform trans = Instantiate(m_flowersLowPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.5f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.5f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                PickActionEvent action = trans.gameObject.AddComponent<PickActionEvent>();
                action.SetAction(SkillActionType.PickType, 6.5f);
                action.SetReplyType(ReplyType.Magic);
                trans.gameObject.name = $"flowersLow{i + j}";
            }
        }

    }

}
