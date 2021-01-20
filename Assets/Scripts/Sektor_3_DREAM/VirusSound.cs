using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSound : MonoBehaviour
{
    public GameObject soundTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            soundTrigger.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            soundTrigger.SetActive(false);
        }
    }

}
