using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDCannonBall : Scene
{
    public QuestXCannonBallHole hole;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Picked_up", "It looks like a cannonball. I think I know a place where this might fit nicely.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        PushMessageToMaster(texts["Picked_up"]);
        hole.acquiredCannonBall = true;
        RemoveFromInteractables();
        finished = true;
        this.gameObject.SetActive(false);
    }
}
