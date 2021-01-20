using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAEmiliIntroduction : Scene
{
    public Animator igorAnimator;
    public GameObject dreamJournal;
    public GameObject barricade;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("E1", "Good evening. It is a grim one, indeed – this wretched storm blew all my drawings away!\n" +
                         "By the way, I’m Igor, Igor Emili, but we don’t have time for niceties.");
        texts.Add("E2", "You must help me, it is a matter of utmost importance. I have a deadline tomorrow, " +
                        "I have to present all those drawings to the mayor...");
        texts.Add("E3", "Here, I have this one left, just to show you what they look like. Take it and go, fast, " +
                        "before the rain ruins them all! You have 10 minutes!");
        texts.Add("P1", "Well, wow, thanks for asking so nicely, dude. Jesus, those architects always seem to " +
                        "think they’re the center of the universe. Unbelievable! ");
        texts.Add("P2", "And he gave me only 10 minutes, what’s that about? Well, I’m going to hurry along anyways " +
                        "cause it’s freaking cold, but not because some guy told me to.");

        StartCoroutine(SafetyClockStarter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        Keybinds(0);
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        StartCoroutine(Introduction());
        ToggleKeybinds(true);
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        if (mode == 0)
        {
            keybinds.Add(new Tuple<string, string>("X", "Next"));
        }
        else if (mode == 1)
        {
            keybinds.Add(new Tuple<string, string>("X", "Leave"));
        }
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator SafetyClockStarter()
    {
        yield return new WaitForSeconds(60f);
        GameController.Master.StartTheClock();
    }

    IEnumerator Introduction()
    {
        yield return new WaitForSeconds(0.25f);
        igorAnimator.SetBool("Talking", true);
        PushSceneMessageToMaster(texts["E1"]);

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        PushSceneMessageToMaster(texts["E2"]);

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        PushSceneMessageToMaster(texts["E3"]);

        yield return new WaitForSeconds(2.5f);
        DrawingPopup();

        igorAnimator.SetBool("Talking", false);
        StopCoroutine(SafetyClockStarter());
        GameController.Master.StartTheClock();
        WriteTextToDreamJournalMaster("");
        GameController.Master.StopListeningForDreamJournalActions();
        Keybinds(1);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        PlayerController._PlayerController.TogglePlayerOnOff(true);

        ToggleKeybinds(false);
        RemoveFromInteractables();
        
        PushMessageToMaster(texts["P1"]);
        PushMessageToMaster(texts["P2"]);
        DrawingPopdown();

        //Destroy(igorAnimator.gameObject);
        Destroy(barricade);
        Destroy(this.gameObject);
    }

    void DrawingPopup()
    {
        dreamJournal.SetActive(true);
    }

    void DrawingPopdown()
    {
        for (int i = 0; i < dreamJournal.transform.childCount; i++)
        {
            dreamJournal.transform.GetChild(i).gameObject.SetActive(true);
        }
        dreamJournal.SetActive(false);

    }
}
