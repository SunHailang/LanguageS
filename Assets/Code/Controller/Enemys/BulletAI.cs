using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletAI : MonoBehaviour
{
    private float m_hurt = 0.0f;
    private float m_speed = 0.0f;
    private Vector3 m_dir = Vector3.zero;

    public string Origin { get; private set; }

    private bool m_init = false;

    public void Init(string origin, float hurt, float speed, Vector3 dir)
    {
        this.Origin = origin;

        this.m_hurt = hurt;
        this.m_speed = speed;
        this.m_dir = dir.normalized;

        transform.forward = this.m_dir;

        m_init = true;
    }

    private void FixedUpdate()
    {
        if (!m_init) return;

        transform.position += m_dir * m_speed * Time.fixedDeltaTime;

        if (transform.position.x > 50.0f || transform.position.x < -50.0f
            || transform.position.z > 50.0f || transform.position.z < -50f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Player":
                    if (this.Origin == "Enemy")
                    {
                        PlayerData.Instance.SetPlayerData(ReplyType.Blood, m_hurt);
                        DestroySelf(other.ClosestPoint(transform.position));
                    }
                    break;
                case "Enemy":
                    if (this.Origin == "Player")
                    {
                        PlayerData.Instance.SetScore(1);
                        Destroy(other.gameObject);
                        DestroySelf(other.ClosestPoint(transform.position));
                    }
                    break;
            }
        }
    }

    private void DestroySelf(Vector3 point)
    {
        Destroy(gameObject);
    }
}
