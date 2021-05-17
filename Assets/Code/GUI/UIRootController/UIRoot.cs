using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
