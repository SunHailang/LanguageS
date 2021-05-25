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
    }

    float m_time = 0;
    bool m_dir = true;


    private void Update()
    {
        m_bezierHomeVecs = BezierUtils.GetBezierPoints(m_bezierHome);

        // 设置 LineRenderer 的点个数，并赋值点值
        lineRenderer.positionCount = (m_bezierHomeVecs.Length);
        lineRenderer.SetPositions(m_bezierHomeVecs);

        if (m_time <= 3.0f)
        {
            int index = Mathf.RoundToInt((m_bezierHomeVecs.Length - 1) * (m_time / 3.0f));
            m_bezierHomeTrans.position = m_bezierHomeVecs[index];
            m_time += Time.deltaTime * (m_dir ? 1 : -1);
            if (m_time > 3.0f)
            {
                m_dir = false;
                m_time = 3.0f;
            }
            if (m_time < 0.0f)
            {
                m_dir = true;
                m_time = 0.0f;
            }
        }
    }
}
