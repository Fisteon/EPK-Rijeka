using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public bool cutscenePlayed;
    public List<ClockCrazySpinning> crazyClocks;

    public GameObject wayBlocker;

    public bool triggerSound;
    public GameObject sound;

    void Start()
    {
        cutscenePlayed = false;
        StartCoroutine(BlockedWayNotification());
    }

    void Update()
    {
        
    }

    public void Fall()
    {
        StartCoroutine(FallingSequence());
        StartCoroutine(WaitForSound());
    }

    public void StartCrazyClocks()
    {
        foreach(ClockCrazySpinning ccs in crazyClocks)
        {
            ccs.enabled = true;
        }
        this.GetComponent<ClockCrazySpinning>().enabled = true;
    }

    IEnumerator BlockedWayNotification()
    {
        while (true)
        {
            if (wayBlocker == null) break;
            else if (wayBlocker.GetComponent<SceneStarter>().playerEntered == true)
            {
                GameController.Master.messages.Add("You should finish up here before moving on");
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WaitForSound()
    {
        yield return new WaitUntil(() => triggerSound == true);
        sound.SetActive(true);
    }

    IEnumerator FallingSequence()
    {
        GameController.Master.cutscene = true;
        RenderSettings.fogDensity = 0.02f;

        this.GetComponent<Animator>().SetBool("clockFalling", true);
        yield return new WaitForSeconds(5f);
        cutscenePlayed = true;
        GameController.Master.cutscene = false;
    }
}
