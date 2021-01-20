using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBEmiliBarricadeOne : Scene
{
    public Animator igorAnimator;
    public GameObject barricade;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("E1", "How many have you found? Only 4? That’s not that much, I must say I’m a bit disappointed..." +
                        "\nYou should try harder, it’s really important to me.");
        texts.Add("E2", "Why didn’t you proceed south? Were you too lazy to move a few rocks? " +
                        "\nOh, it’s so hard to find a decent worker nowadays... Come on, I’ll have it cleared up for you! Go!");

        texts.Add("P1", "Whoooa, somebody needs to take a chill pill... What an attitude. I just want to get out of this place as soon as I can. ");
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

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        PushSceneMessageToMaster(texts["E2"]);

        yield return new WaitForSeconds(1f);
        igorAnimator.SetBool("Talking", false);
        Keybinds(1);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        PlayerController._PlayerController.TogglePlayerOnOff(true);

        ToggleKeybinds(false);
        RemoveFromInteractables();

        PushMessageToMaster(texts["P1"]);

        Destroy(igorAnimator.gameObject);
        Destroy(barricade);
        Destroy(this.gameObject);
    }
}
