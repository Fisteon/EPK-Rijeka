using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestELastEmili : Scene
{
    public Animator igorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("E1", "You did it, kid, against all odds – you got them all! Better than catching Pokemon, eh?");
        texts.Add("E2", "Thank you, and now get out of the rain already. Here’s an umbrella. See you some other time!");

        PushMessageToMaster(texts["E1"]);

        Keybinds();
        ToggleKeybinds(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Finish());
        }
    }

    public override void Keybinds()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("X", "Finish"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator Finish()
    {
        igorAnimator.SetBool("Talking", true);
        PushMessageToMaster(texts["E2"]);
        yield return new WaitForSeconds(4f);
        igorAnimator.SetBool("Talking", false);
        finished = true;
    }
}
