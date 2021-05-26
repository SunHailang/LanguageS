using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TerrainBezier : MonoBehaviour
{

    [SerializeField]
    private Transform m_bezierHomeTrans;
    [SerializeField]
    private Transform[] m_bezierHome;

    private Vector3[] m_bezierHomeVecs;

    // LineRenderer 
    private LineRenderer lineRenderer;
    private int layerOrder = 0;

    private void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.yellow;

        m_bezierHomeVecs = BezierUtils.GetBezierPoints(m_bezierHome);
    }

    float m_time = 0;
    bool m_dir = true;
    [Range(5.0f, 15.0f)]
    public float m_totalTime = 15.0f;
    private void Update()
    {
        m_bezierHomeVecs = BezierUtils.GetBezierPoints(m_bezierHome);

        // 设置 LineRenderer 的点个数，并赋值点值
        lineRenderer.positionCount = (m_bezierHomeVecs.Length);
        lineRenderer.SetPositions(m_bezierHomeVecs);

        if (m_time <= m_totalTime)
        {
            int index = Mathf.RoundToInt((m_bezierHomeVecs.Length - 1) * (m_time / m_totalTime));
            m_bezierHomeTrans.position = m_bezierHomeVecs[index];

            //if (index < m_bezierHomeVecs.Length - 1)
            //{
            //    Vector3 dir = m_bezierHomeVecs[index + 1] - m_bezierHomeVecs[index];
            //    m_bezierHomeTrans.forward = dir.normalized;
            //}

            m_time += Time.deltaTime * (m_dir ? 1 : -1);
            if (m_time > m_totalTime)
            {
                m_dir = true;
                m_time = 0.0f;
            }
            //if (m_time < 0.0f)
            //{
            //    m_dir = true;
            //    m_time = 0.0f;
            //}
        }
    }
}
