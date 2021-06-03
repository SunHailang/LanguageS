using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform m_player;

    private Camera m_camera;

    private LineRenderer m_renderer;

    private Vector3 m_direction = Vector3.zero;
    private Vector3 m_directionReturn = Vector3.zero;
    private float m_distanceMax = 0.0f;
    private float m_distanceRay = 0.0f;

    private void Awake()
    {
        //m_initPosition = transform.position;

        m_camera = GetComponent<Camera>();
        m_renderer = GetComponent<LineRenderer>();

        m_renderer.sortingLayerID = 0;
        m_renderer.startColor = Color.cyan;
        m_renderer.endColor = Color.yellow;

        m_distanceMax = Vector3.Distance(m_player.position, transform.position);
        m_distanceRay = m_distanceMax + 2.0f;
    }

    private void Update()
    {
        // 设置 LineRenderer 的点个数，并赋值点值
        //m_renderer.positionCount = (2);
        //m_renderer.SetPositions(new Vector3[] { transform.position, m_player.position });
    }


    private void FixedUpdate()
    {
        m_directionReturn = (transform.position - m_player.position).normalized;
        m_direction = (m_player.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_direction, out hit))
        {
            if (hit.collider.tag != "Player")
            {
                transform.position += m_direction * Time.deltaTime * 6.0f;
            }
            else
            {
                float dis = Vector3.Distance(m_player.position, transform.position);
                if (dis < m_distanceMax)
                {
                    Vector3 nexDis = m_directionReturn * Time.deltaTime * 10f;
                    if (Physics.Raycast(transform.position + nexDis, m_direction, out hit))
                    {
                        if (hit.collider.tag == "Player")
                        {
                            transform.position += nexDis;
                            if (Vector3.Distance(m_player.position, transform.position) > m_distanceMax)
                            {
                                Ray ray = new Ray(m_player.position, m_directionReturn);
                                transform.position = ray.GetPoint(m_distanceMax);
                            }
                        }
                    }
                }
            }
        }
    }

}
