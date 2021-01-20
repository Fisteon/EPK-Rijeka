using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestShield : Scene
{
    private void Start()
    {
        texts.Add("Finish", "Why would anyone leave a shield in the middle of the street?\nMust have been in a hurry to get somewhere...");
    }

    private void Update()
    {
    }

    public override void OnPlayerInteract()
    {
        finished = true;
        PushMessageToMaster(texts["Finish"]);
        WriteTextToDreamJournalMaster(texts["Finish"]);
        PlayerController._PlayerController.interactables.Remove(gameObject);
        gameObject.SetActive(false);
    }
}
