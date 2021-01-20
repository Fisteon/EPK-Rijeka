using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHOneOfSix : Scene
{
    public string pickupText;
    [Range(1, 100)]
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        interactionText += " " + this.transform.name.Split('_')[1].ToLower();
        //texts.Add("Finish", "You picked up <i>one of the six items</i> you need for the quest.");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        PushMessageToMaster(pickupText);
        QuestITimeMachine.itemsCollected += 1;
        RemoveFromInteractables();
        finished = true;
        this.gameObject.SetActive(false);
    }
}
