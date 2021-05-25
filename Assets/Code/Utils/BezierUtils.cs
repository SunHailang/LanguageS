using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierUtils
{
    // 设置贝塞尔插值个数
    private static int m_segmentNum = 300;

    public static Vector3[] GetBezierPoints(Transform[] points)
    {
        if (points == null || points.Length <= 0) return null;
        Vector3[] pointsArray = new Vector3[m_segmentNum + 1];
        int powN = points.Length - 1;
        pointsArray[0] = points[0].position;

        ulong[] modulus = GetPointModulus(powN);

        for (int t = 1; t < m_segmentNum; t++)
        {
            Vector3 point = Vector3.zero;
            float time = t / (float)m_segmentNum;
            for (int i = 0; i < points.Length; i++)
            {
                point += GetPoint(points[i].position, modulus[i], powN - i, i, time);
            }
            pointsArray[t] = point;
        }
        pointsArray[m_segmentNum] = points[powN].position;
        return pointsArray;
    }

    public static Vector3[] GetBezierPoints(Vector3[] points)
    {
        if (points == null || points.Length <= 0) return null;
        Vector3[] pointsArray = new Vector3[m_segmentNum + 2];
        int powN = points.Length - 1;
        pointsArray[0] = points[0];

        ulong[] modulus = GetPointModulus(powN);

        for (int t = 1; t <= m_segmentNum; t++)
        {
            Vector3 point = Vector3.zero;
            float time = t / (float)m_segmentNum;
            for (int i = 0; i < points.Length; i++)
            {
                point += GetPoint(points[i], modulus[i], powN - i, i, time);
            }
            pointsArray[t] = point;
        }
        pointsArray[m_segmentNum + 1] = points[powN];
        return pointsArray;
    }
    /// <summary>
    /// 获取组合数的数组
    /// </summary>
    /// <param name="n"> n大于等于1 </param>
    /// <returns></returns>
    private static ulong[] GetPointModulus(int n)
    {
        ulong[] result = new ulong[n + 1];
        for (int i = 1; i <= n; i++)
        {
            result[i] = 1;
            for (int j = i - 1; j >= 1; j--)
                result[j] += result[j - 1];
            result[0] = 1;
        }
        return result;
    }
    private static Vector3 GetPoint(Vector3 point, ulong pow, int powN, int powI, float t)
    {
        return pow * Mathf.Pow(t, powI) * Mathf.Pow((1 - t), powN) * point;
    }
}
