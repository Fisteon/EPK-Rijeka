using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCCylinder : Scene
{
    public GameObject cylinder;
    public GameObject sceneTrigger;
    public List<GameObject> trees;

    public Camera cylinderCamera;

    public ParticleSystem particles;

    bool cylinderEnabled;

    // Start is called before the first frame update
    void Start()
    {
        cylinderEnabled = false;

        texts.Add("Start", "Whoooaa... Did that statue there just move?!");
        texts.Add("Finished", "I'm having the weirdest night of my life...\nOn that note, those trees just moved to form a path...");

        StartCoroutine(WaitForSceneStart());
        Keybinds();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Y))) && cylinderEnabled)
        {
            ToggleCylinder(false);
        }

        if (cylinder.GetComponent<StatueCylinder>().solved)
        {
            EndScene();
        }

    }

    IEnumerator WaitForSceneStart()
    {
        yield return new WaitUntil(() => sceneTrigger.GetComponent<SceneStarter>().playerEntered);
        PushMessageToMaster(texts["Start"]);
        WriteTextToDreamJournalMaster(texts["Start"]);
    }

    private void ToggleCylinder(bool state)
    {
        GameController.Master.questSolving = state;
        GameController.Master._GUI_interaction_text.SetActive(!state);
        PlayerController._PlayerController.enabled = !state;
        PlayerController._PlayerController.camera.enabled = !state;

        if (!state) GameController.Master.WriteSceneMessage("");
        ToggleKeybinds(state);
        cylinderCamera.enabled = state;
        cylinder.SetActive(state);
        cylinderEnabled = state;
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.WriteSceneMessage("<i>Gabriele the poet, with fire and arm, took over the city and did me great harm:\nA soldier of his stole something of mine – if you tell me the year, you'll make it on time!</i>");
        ToggleCylinder(true);
    }

    public override void Keybinds()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("X", "Submit"));
        keybinds.Add(new Tuple<string, string>("Y", "Exit"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    void EndScene()
    {
        ToggleCylinder(false);
        PushMessageToMaster(texts["Finished"]);
        WriteTextToDreamJournalMaster(texts["Finished"]);
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        foreach (GameObject tree in trees)
        {
            Destroy(tree);
        }
        this.finished = true;
        this.gameObject.SetActive(false);
    }
}
