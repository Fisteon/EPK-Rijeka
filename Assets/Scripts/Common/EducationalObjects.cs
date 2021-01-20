using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducationalObjects : MonoBehaviour
{
    public string text;
    public bool onlyOnce;

    public bool visited = false;

    public bool writtenInJournal = false;
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
        //if (onlyOnce && writtenInJournal) return;
        if (onlyOnce && visited) return;

        if (other.transform.tag == "Player")
        {
            visited = true;
            GameController.Master.WriteSceneMessage(text);
            if (!writtenInJournal)
            {
                GameController.Master.WriteInDreamJournal(text);
                writtenInJournal = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameController.Master.WriteSceneMessage("");
        }
    }
}
