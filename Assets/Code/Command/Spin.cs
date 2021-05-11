using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_direction = Vector3.up;



    private void Update()
    {
        transform.Rotate(m_direction * Time.deltaTime);
    }
}
