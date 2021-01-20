using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    public bool playerEntered;

    void Start()
    {
        playerEntered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerEntered = true;
        }
    }
}
