using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWriter : MonoBehaviour
{
    public List<GameObject> zones;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name.Split('_')[1]);
    }
}
