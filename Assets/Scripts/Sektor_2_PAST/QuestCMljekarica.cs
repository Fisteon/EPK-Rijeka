using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestCMljekarica : Scene
{
    public Animator igorAnimator;

    public QuestPaperCollection collectPaper;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("E1", "Look, I’m sorry if I was a bit harsh earlier... I’m just very nervous tonight, " +
                        "a lot is at stake for me here. Here, take this as a token of friendship.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPlayerInteract()
    {
        Keybinds();
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        StartCoroutine(TalkToEmili());
        ToggleKeybinds(true);
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("X", "Leave"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator TalkToEmili()
    {
        yield return new WaitForSeconds(0.25f);
        igorAnimator.SetBool("Talking", true);
        PushSceneMessageToMaster(texts["E1"]);

        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        igorAnimator.SetBool("Talking", false);
        collectPaper.OnPlayerInteract();
        
        PlayerController._PlayerController.TogglePlayerOnOff(true);

        ToggleKeybinds(false);
        RemoveFromInteractables();

        Destroy(igorAnimator.gameObject);
        Destroy(this.gameObject);
    }
}
