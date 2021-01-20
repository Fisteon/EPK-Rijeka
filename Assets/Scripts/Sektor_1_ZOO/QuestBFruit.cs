using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBFruit : Scene
{
    public GameObject fruit;
    public QuestXCannonBallHole cannonHole;
    public string whatDidYouFind;
    [Range(10, 150)]
    public int rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Picked up", "You found " + whatDidYouFind + "!");
        texts.Add("Apple", "An apple a day keeps the doctor away!");
        texts.Add("Other", "Another piece of fruit, in the middle of the street... Weird.");
        texts.Add("Journal", "I woke up in a strange city and found an apple.");
        rotationSpeed = 75;
    }

    // Update is called once per frame
    void Update()
    {
        fruit.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        if (whatDidYouFind == "an apple")
        {
            PushMessageToMaster(texts["Apple"]);
            WriteTextToDreamJournalMaster(texts["Journal"]);
        }
        else
        {
            PushMessageToMaster(texts["Other"]);
        }
        finished = true;
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        cannonHole.acquiredApple = this.transform.name.Contains("Apple") ? true : cannonHole.acquiredApple;
        cannonHole.acquiredBanana = this.transform.name.Contains("Banana") ? true : cannonHole.acquiredBanana;
        this.gameObject.SetActive(false);
    }
}
