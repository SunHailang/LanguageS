using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_enemyPoints;


    private float m_createTime = 0.0f;

    private void Update()
    {
        if (PlayerController.Instance.IsDeath) return;
        if (m_createTime >= 15.0f)
        {
            int index = UnityEngine.Random.Range(0, m_enemyPoints.Length);
            EnemyAI enemy = Instantiate(ResourcesManager.LoadEnemys<EnemyAI>("Enemy1"), m_enemyPoints[index]);
            enemy.transform.position = m_enemyPoints[index].position;
            enemy.Init();
            m_createTime = 0.0f;
        }
        m_createTime += Time.deltaTime;
    }
}
