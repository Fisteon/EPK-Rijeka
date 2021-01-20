using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestZForum : Scene
{
    public QuestECannon cannon;

    [Header("GameObjects")]
    public GameObject paperLatin;
    public GameObject amforaInside;
    public GameObject amforaOutside;

    [Header("Misc")]
    public Transform cameraPositionOutside;

    public Image waterLayer;

    bool pullingVaseOut;
    bool inspectingVase;

    bool interacting;

    // Start is called before the first frame update
    void Start()
    {
        pullingVaseOut = true;
        inspectingVase = false;
        interacting = false;

        texts.Add("InspectVase", "An ancient Roman vase... Wait - there's something inside! A message, and something that looks like a bag of gunpowder? I should inspect it.");
        texts.Add("LatinText", "I think it's Latin for: The exit is in the church... boom?\nOh my, what have I gotten myself into?");
        texts.Add("Journal", "I found what appears to be an ancient Roman vase with a message and a bag of gunpowder inside.\nThe message was \"Exitus in Ecclesia est. Boomus, boomae, boom!\"" +
                             "I think it's Latin for: The exit is in the church... boom? Oh my, what have I gotten myself into?");
    }

    // Update is called once per frame
    void Update()
    {
        if (!interacting) return;

        if (pullingVaseOut && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(PullVaseOut());
        }
        else if (inspectingVase && Input.GetButtonDown("Interact"))
        {
            EndScene();
        }
        else if (finished && Input.GetButtonDown("Interact"))
        {
            ChangeState(false);
        }
    }

    public override void OnPlayerInteract()
    {
        Keybinds(0);
        cannon.haveGunpowder = true;
        ChangeState(true);
    }

    void ChangeState(bool state)
    {
        PlayerController._PlayerController.TogglePlayerOnOff(!state);
        SceneCamera.gameObject.SetActive(state);
        ToggleKeybinds(state);
        interacting = state;
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        if (mode == 0)
        {
            keybinds.Add(new Tuple<string, string>("X", "Pull out Roman vase"));
        }
        else if (mode == 1)
        {
            keybinds.Add(new Tuple<string, string>("X", "Inspect the vase"));
        }
        else if (mode == 2)
        {
            keybinds.Add(new Tuple<string, string>("X", "Exit"));
        }
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator PullVaseOut()
    {
        pullingVaseOut = false;
        ToggleKeybinds(false);

        float elapsed = 0f;
        float duration = 2f;

        Color c = waterLayer.color;
        float initialAlpha = c.a;

        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(initialAlpha, 1, elapsed / duration);
            waterLayer.color = c;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        c.a = 1;
        waterLayer.color = c;

        amforaInside.SetActive(false);
        amforaOutside.SetActive(true);
        SceneCamera.transform.localPosition = cameraPositionOutside.localPosition;
        SceneCamera.transform.localRotation = cameraPositionOutside.localRotation;

        yield return new WaitForSeconds(0.5f);
        elapsed = 0f;
        duration = 1f;

        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(1, 0, elapsed / duration);
            waterLayer.color = c;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        waterLayer.gameObject.SetActive(false);

        Keybinds(1);
        PushMessageToMaster(texts["InspectVase"]);
        ToggleKeybinds(true);
        inspectingVase = true;

        yield return null;
    }

    void EndScene()
    {
        Keybinds(2);
        paperLatin.SetActive(true);
        PushMessageToMaster(texts["LatinText"]);
        WriteTextToDreamJournalMaster(texts["Journal"]);

        inspectingVase = false;
        finished = true;
        this.GetComponent<BoxCollider>().enabled = false;
        RemoveFromInteractables();
    }
}
