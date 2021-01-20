using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFLayout : Scene
{
    bool alreadyStandingOnLayout;
    bool madeMistake;
    bool inBirdView;
    bool firstBirdView;
    bool animating;
    Vector3 playerLastPosition;

    public GameObject layouts;
    public bool playerEntered;
    public Light towerLight;

    public float fogValue;
    // Start is called before the first frame update
    void Start()
    {
        alreadyStandingOnLayout = false;
        madeMistake = false;
        inBirdView = true;
        firstBirdView = true;
        animating = true;
        ToggleSound(true);
        texts.Add("Start", "");
        texts.Add("Misstep_1", "###Woops! I should tread more carefuly!");
        texts.Add("Misstep_2", "###I better watch my step!");
        texts.Add("Misstep_3", "###Damn, I stepped on something!");
        texts.Add("Misstep_4", "###Maybe I should go around?");
        texts.Add("Birdview", "Oh, cool, it looks like floor plans of buildings... It seems the outlines represent buildings that were here before. Too bad it's just an empty parking lot now.");
        texts.Add("FirstEntrance", "Wow, what's that on the floor? Wait, maybe I should switch perspectives for a bit...");
        texts.Add("ExitFromBirdview", "Oh, and look at all these signs! They have the names of the old streets written on them.");
        Keybinds(0);
        StartCoroutine(LightUpLayouts());
        StartCoroutine(ActivateTower());
        StartCoroutine(KeybindManagement());
        StartCoroutine(WaitForFirstEntrance());
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity = fogValue;
        if (animating) return;

        if (Input.GetButtonDown("Interact") && playerEntered)
        {
            SwapToBirdView(!inBirdView);
        }
        if (PlayerController._PlayerController.transform.position != playerLastPosition)
        {
            playerLastPosition = PlayerController._PlayerController.transform.position;
            #region Check if player is standing on Layout
            RaycastHit info;
            Vector3 origin = PlayerController._PlayerController.transform.position;
            origin.y += 3f;
            if (Physics.Raycast(origin, Vector3.down, out info, 10f, 1 << 10))
            {
                if (!alreadyStandingOnLayout)
                {
                    alreadyStandingOnLayout = true;
                    GameController.Master.updateTimer(10);
                    System.Random rand = new System.Random();
                    PushMessageToMaster(texts["Misstep_" + rand.Next(1,5).ToString()]);
                }
            }
            else
            {
                alreadyStandingOnLayout = false;
                Debug.Log("Not hitting anything!");
            }
            #endregion
            towerLight.intensity = 6f - 4.5f * (1 - ((Vector3.Distance(towerLight.transform.position, PlayerController._PlayerController.transform.position)) - 10) / 40);
        }
    }

    IEnumerator LightUpLayouts()
    {
        animating = true;
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);

        List<GameObject> allLayouts = new List<GameObject>();
        for (int i = 0; i < layouts.transform.childCount; i++)
        {
            allLayouts.Add(layouts.transform.GetChild(i).gameObject);
        }
        float delay = 0.6f;

        foreach (GameObject l in allLayouts)
        {
            l.SetActive(true);
            yield return new WaitForSeconds(delay);
            delay *= 0.85f;
        }
        PushMessageToMaster(texts["FirstEntrance"]);
        yield return new WaitForSeconds(2f);
        this.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        GameController.Master.WriteSceneMessage(texts["Birdview"]);
        Keybinds(1);
        ToggleKeybinds(true);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact") == true);
        animating = false;
        this.GetComponent<Animator>().enabled = false;
        SwapToBirdView(false);
    }

    IEnumerator WaitForFirstEntrance()
    {
        yield return new WaitUntil(() => playerEntered == true);
        PushMessageToMaster(texts["ExitFromBirdview"]);
    }

    private void SwapToBirdView(bool bird)
    {
        inBirdView = !inBirdView;
        PlayerController._PlayerController.TogglePlayerOnOff(!bird);
        SceneCamera.gameObject.SetActive(bird);
        if (bird)
        {
            Keybinds(1);
            //GameController.Master.WriteSceneMessage(texts["Birdview"]);
            fogValue = 0.015f;
        }
        else
        {
            Keybinds(0);
            fogValue = 0.04f;
        }
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        if (mode == 0)
            keybinds.Add(new Tuple<string, string>("X", "Bird view"));
        if (mode == 1)
            keybinds.Add(new Tuple<string, string>("X", "Return to the ground"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator ActivateTower()
    {
        yield return new WaitForSeconds(0.1f);
        finished = true;
    }

    private IEnumerator KeybindManagement()
    {
        QuestFLayout questFlayout = this;
        while (true)
        {
            yield return new WaitUntil(() => playerEntered == true);
            questFlayout.ToggleKeybinds(true);

            yield return new WaitUntil(() => playerEntered == false);
            questFlayout.ToggleKeybinds(false);
        }
    }
}
