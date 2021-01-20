using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestXGraffitiYear : Scene
{
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Interacted", "Another number... It seems to be sprayed on the wall... 1919.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        PushMessageToMaster(texts["Interacted"]);
        WriteTextToDreamJournalMaster(texts["Interacted"]);
        RemoveFromInteractables();
        this.GetComponent<BoxCollider>().enabled = false;
        Destroy(particles);
    }
}
