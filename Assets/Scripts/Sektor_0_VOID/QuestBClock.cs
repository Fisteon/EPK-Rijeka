using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBClock : Scene
{
    public Clock clock;
    public ParticleSystem particles;

    public GameObject wayBlocker;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Finished", "Oh god, I started the clock – and my time here started running out!");
        clock.Fall();
        ToggleSound(true);
        StartCoroutine(WaitForCutscene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        EndScene();
    }

    IEnumerator WaitForCutscene()
    {
        yield return new WaitUntil(() => clock.cutscenePlayed == true);
        particles.gameObject.SetActive(true);
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

    void EndScene()
    {
        Destroy(wayBlocker.gameObject);
        GameController.Master.StartTheClock();
        PushMessageToMaster(texts["Finished"]);
        WriteTextToDreamJournalMaster(texts["Finished"]);
        finished = true;
        clock.StartCrazyClocks();
        this.gameObject.SetActive(false);
    }
}
