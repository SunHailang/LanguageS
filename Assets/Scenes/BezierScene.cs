using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierScene : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_tras;


    Vector3[] m_points;

    // LineRenderer 
    private LineRenderer lineRenderer;
    private int layerOrder = 0;

    


    // Start is called before the first frame update
    void Start()
    {
        m_points = new Vector3[m_tras.Length];

        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_tras.Length; i++)
        {
            m_points[i] = m_tras[i].position;
        }

        //Vector3[] points = BezierUtils.GetBezierThreePoints(m_points[0], m_points[1], m_points[2], m_points[3]);
        Vector3[] points = BezierUtils.GetBezierPoints(m_points);
        // 设置 LineRenderer 的点个数，并赋值点值
        lineRenderer.positionCount = (points.Length);
        lineRenderer.SetPositions(points);
    }
}
