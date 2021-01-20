using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestYClock : Scene
{
    public GameObject blockade;
    public GameObject particles;
    public GameObject wildClocksHolder;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Interacted", "Oh god, I started the clock – and my time here started running out!");
        particles.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        this.GetComponent<ClockCrazySpinning>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = false;

        GameController.Master.StartTheClock();
        PushMessageToMaster(texts["Interacted"]);
        WriteTextToDreamJournalMaster(texts["Interacted"]);
        RemoveFromInteractables();
        finished = true;
        Destroy(particles);
        Destroy(blockade);

        for (int i = 0; i < wildClocksHolder.transform.childCount; i++)
        {
            wildClocksHolder.transform.GetChild(i).GetComponent<ClockCrazySpinning>().enabled = true;
        }
    }
}
