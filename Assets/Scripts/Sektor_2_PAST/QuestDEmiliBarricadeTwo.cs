using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestDEmiliBarricadeTwo : Scene
{
    public Animator igorAnimator;
    public GameObject barricade;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("E1", "You’re doing good, kid, keep it up. I’ll have the way cleared up for you again, just hurry, hurry – there’s only a few of them left!");

        texts.Add("P1", "I wonder how he “clears up the way” every time. Those architects are always some kind of "+
                        "jack-of-all-trades characters... Nevermind, let’s get this done.");
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
        StartCoroutine(RemoveBarricade());
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

    IEnumerator RemoveBarricade()
    {
        yield return new WaitForSeconds(0.25f);
        igorAnimator.SetBool("Talking", true);
        PushSceneMessageToMaster(texts["E1"]);

        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        igorAnimator.SetBool("Talking", false);

        PlayerController._PlayerController.TogglePlayerOnOff(true);

        ToggleKeybinds(false);
        RemoveFromInteractables();

        PushMessageToMaster(texts["P1"]);

        Destroy(igorAnimator.gameObject);
        Destroy(barricade);
        Destroy(this.gameObject);
    }
}
