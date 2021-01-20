using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestXCannonBallHole : Scene
{
    public GameObject particles;
    [Header("Checks")]
    public bool acquiredCannonBall;
    public bool acquiredApple;
    public bool acquiredBanana;

    [Header("Text")]
    public Canvas canvas;
    public TextMeshProUGUI t_options;
    public TextMeshProUGUI t_question;
    public TextMeshProUGUI t_introduction;

    public float widthSmall;
    public float widthLarge;
    public float height;

    [Header("Hole objects")]
    public GameObject cannonBall;
    public List<GameObject> insertionObjects;
    bool animating;
    bool introductionDone;

    bool appleUsed;
    bool bananaUsed;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("OnInspect", "There's a hole and an inscription in the wall: \nISTA DABAT GALLOS PVLSVRA HINC ANGLIA POMA.\n" +
                               "My Latin is a bit rusty, but it mentions some fruit sent by the English?");
        texts.Add("AfterAnimation", "I have a feeling something would happen if I place the right item here...");

        texts.Add("WithoutBall", "Hmm, looks like something is missing here..");
        texts.Add("Apple", "###Doesn't seem like a right fit.");
        texts.Add("Banana", "###This is not right, what was I thinking?");
        texts.Add("Correct", "It fits perfectly! But what's that noise?!");

        texts.Add("Journal", "The inscription above the hole in cathedral's wall said \"This fruit is a gift from Venice.\"");
        animating = false;
        appleUsed = false;
        bananaUsed = false;
        introductionDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SceneCamera.gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            PopulateText();
        }
        if (animating) return;

        if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("ActionY"))
        {
            ChangeView(false);
        }

        if (Input.GetButtonDown("Interact") && acquiredCannonBall)    // cannon ball
        {
            StartCoroutine(EndScene());
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ActionA")) && acquiredApple && !appleUsed)    // apple
        {
            appleUsed = true;
            StartCoroutine(Insert(insertionObjects[0], "Apple"));
        }
        if ((Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("ActionB")) && acquiredBanana && !bananaUsed)    // banana
        {
            bananaUsed = true;
            StartCoroutine(Insert(insertionObjects[1], "Banana"));
        }
    }

    public override void OnPlayerInteract()
    {
        PopulateText();
        ChangeView(true);
    }

    IEnumerator EndScene()
    {
        particles.SetActive(false);
        cannonBall.SetActive(true);
        finished = true;
        PushMessageToMaster(texts["Correct"]);
        yield return new WaitForSeconds(2f);

        RemoveFromInteractables();
        ToggleKeybinds(false);
        this.GetComponent<BoxCollider>().enabled = false;
        SceneCamera.gameObject.SetActive(false);
        PlayerController._PlayerController.TogglePlayerOnOff(true);
    }

    void PopulateText()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        if (acquiredCannonBall)
        {
            keybinds.Add(new Tuple<string, string>("X", "Place cannon ball"));
        }
        if (acquiredApple && !appleUsed)
        {
            keybinds.Add(new Tuple<string, string>("A", "Place apple"));
        }
        if (acquiredBanana && !bananaUsed)
        {
            keybinds.Add(new Tuple<string, string>("B", "Place banana"));
        }

        keybinds.Add(new Tuple<string, string>("Y", "Leave"));
        GameController.Master.SetupKeybinds(keybinds);

        /*t_options.text = "";
        if (acquiredCannonBall)
        {
            t_options.text += "[X] Cannon ball\n";
        }
        if (acquiredApple && !appleUsed)
        {
            t_options.text += "[Y] Apple\n";
        }
        if (acquiredBanana && !bananaUsed)
        {
            t_options.text += "[A] Banana";
        }*/

        if ((!acquiredApple || appleUsed) && 
            (!acquiredBanana || bananaUsed) && 
            !acquiredCannonBall )
        {
            t_options.alignment = TextAlignmentOptions.Center;
            t_options.rectTransform.sizeDelta = new Vector2(widthLarge, height);
            t_options.text = "I don't have anything at the moment.";
        }
        else
        {
            t_options.alignment = TextAlignmentOptions.TopLeft;
            t_options.rectTransform.sizeDelta = new Vector2(widthSmall, height);
        }
    }

    IEnumerator Insert(GameObject i, string messsage)
    {
        animating = true;
        //canvas.gameObject.SetActive(false);
        i.SetActive(true);

        float animationDuration = i.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds((animationDuration / 3) * 2);
        PushMessageToMaster(texts[messsage]);
        yield return new WaitForSeconds((animationDuration / 3) + 0.25f);

        i.SetActive(false);
        PopulateText();
        //canvas.gameObject.SetActive(true);
        animating = false;
    }

    void ChangeView(bool state)
    {
        PlayerController._PlayerController.TogglePlayerOnOff(!state);
        particles.SetActive(!state);
        SceneCamera.gameObject.SetActive(state);
        if (!introductionDone)
            StartCoroutine(ZoomOut());
        else
        {
            ToggleKeybinds(state);
        }
    }

    IEnumerator ZoomOut()
    {
        introductionDone = true;
        animating = true;
        PushMessageToMaster(texts["OnInspect"]);
        WriteTextToDreamJournalMaster(texts["OnInspect"]);
        yield return new WaitForSeconds(6f);
        t_introduction.gameObject.SetActive(false);

        float elapsed = 0f;
        float duration = 1.5f;

        while (elapsed < duration)
        {
            float x = (elapsed / duration) * 7;
            float y = 1 - Mathf.Exp(-x);

            SceneCamera.fieldOfView = Mathf.Lerp(30, 58, y);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        SceneCamera.fieldOfView = 58;
        t_options.gameObject.SetActive(true);
        t_question.gameObject.SetActive(true);
        ToggleKeybinds(true);
        animating = false;
    }
}
