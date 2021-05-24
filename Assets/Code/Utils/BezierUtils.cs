using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierUtils
{
    // 设置贝塞尔插值个数
    private static int m_segmentNum = 50;

    public static Vector3[] GetBezierPoints(Vector3[] points)
    {
        if (points == null || points.Length <= 0) return null;
        Vector3[] pointsArray = new Vector3[m_segmentNum + 2];
        int powN = points.Length - 1;
        pointsArray[0] = points[0];
        for (int t = 1; t <= m_segmentNum; t++)
        {
            Vector3 point = Vector3.zero;
            float time = t / (float)m_segmentNum;
            for (int i = 0; i < points.Length; i++)
            {
                point += GetPoint(points[i], powN, i, time);
            }
            pointsArray[t] = point;
        }
        pointsArray[m_segmentNum + 1] = points[powN];
        return pointsArray;
    }

    private static Vector3 GetPoint(Vector3 point, int powN, int powI, float t)
    {
        int pow = (powI == 0 || powI == powN) ? 1 : powN;
        return pow * Mathf.Pow((1 - t), powN - powI) * Mathf.Pow(t, powI) * point;
    }
}
