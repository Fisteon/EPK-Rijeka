using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAPillar : Scene
{
    public PillarRotation pillar;
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Finished", "It says 1509 and 1970 on the statue... Funny numbers, I wonder what they mean.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        StartCoroutine(EndScene());
    }

    IEnumerator EndScene()
    {
        pillar.rotateBase = true;
        PushMessageToMaster(texts["Finished"]);
        WriteTextToDreamJournalMaster(texts["Finished"]);
        Destroy(particles.gameObject);
        yield return new WaitForSeconds(5);
        finished = true;
        this.gameObject.SetActive(false);
    }
}
