using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutBirdviewCollider : MonoBehaviour
{
    public QuestFLayout layout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            layout.playerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            layout.playerEntered = false;
        }
    }
}
