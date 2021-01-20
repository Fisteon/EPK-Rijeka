using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{
    public static int material;
    public static GameObject data;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        data = this.gameObject;
    }
}
