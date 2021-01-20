using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInteraction : MonoBehaviour
{

    
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
        if (other.gameObject.layer == 15)
        {
            GameController.Master.WriteLocation(other.transform.name.Split('_')[1]);
        }
        else if (other.transform.tag == "questitem")
        {
            PlayerController._PlayerController.interactables.Add(other.gameObject);
            Debug.Log("PRESS \"E\" TO INTERACT WITH " + other.name + "!");
            GameController.Master._GUI_interaction_text.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                /*"[E] " + */other.gameObject.GetComponent<Scene>().interactionText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController._PlayerController.interactables.Remove(other.gameObject);
    }
}
