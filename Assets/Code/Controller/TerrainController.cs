using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    private Transform m_mushroomsParent;
    [SerializeField]
    private Transform m_mushroomsPrefab;


    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            Transform trans = Instantiate(m_mushroomsPrefab, m_mushroomsParent);
            float x = 10 + i * 0.6f;
            float z = i * 1.6f - 40;
            trans.localPosition = new Vector3(x, 0, z);
            trans.gameObject.AddComponent<ActionEvent>().SetAction(SkillActionType.PickType);
            trans.gameObject.name = $"{i}";
        }

    }

}
