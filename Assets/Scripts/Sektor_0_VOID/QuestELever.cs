using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QuestELever : Scene
{
    private int jankoInteractions = 1;
    public Animator animator;
    public GameObject ground;
    public GameObject leverHolder;
    public GameObject lever;
    public GameObject graffiti;
    public GameObject lockMechanism;
    public GameObject spotLight;
    public GameObject Janko;
    public GameObject t_pssst;
    public float fogValue;
    public PostProcessProfile PPP;
    private ChromaticAberration chromaticAberration;
    private float initialChromaticAberrationValue;
    public GameObject[] blockers;
    public Material m_luminousGraffiti;
    public Material m_baseGraffiti;
    public Material m_doneGraffiti;
    public Material m_doneLever;
    private bool reachedDestination;
    private bool JankoAvailable;

    private void Start()
    {
        TurnBlockersOnOff(true);
        //texts.Add("Finish", "The fog lifts a bit, you see a light in the distance.");
        texts.Add("Pssst", "Pssst");
        texts.Add("InitialMessage", "Hmm... This graffiti seems to be in the local language. I think it says something about a street lamp? If only someone could translate it for me...");
        texts.Add("Finish", "I bumped into Janko Polić Kamov, a famous poet. He had a few pieces of advice for me.");

        texts.Add("Janko_1", "That graffiti is hardly poetry...\nIt doesn't enlighten me, it needs more intensity!");
        texts.Add("Janko_2", "Didn't you understand? When I read something, I want it to burn inside me,\nlight me up! So do it, light it up! Turn on the light!");

        texts.Add("Janko_31", "Zašto sam obrijao bradu? To je pitanje velevažno i zanimljivo!");
        texts.Add("Janko_32", "Putovao sam mnogo, govorio malo; čitao uvijek!");
        texts.Add("Janko_33", "Apsurd postaje pjesma moja i nema ludnice za mene!");
        texts.Add("Janko_34", "Mrtav je svijet, ljubavi moja, i crno je u dosadi njegovoj!");
        texts.Add("Janko_35", "Pijem da se opijem i ljubim da se naljubim. Volim sve ono što moj otac osuđuje!");

        texts.Add("Player_1", "That was Janko Polić Kamov, the poet. No wonder he's not enlightened by this weird street art. But I feel like he was trying to help me.");
        texts.Add("Player_2", "I believe Janko is telling me to turn on the light.");
        texts.Add("Player_3", "I’m not sure Janko feels like helping me anymore. I just need to turn on all these lights.");
        reachedDestination = false;
        JankoAvailable = false;
        Keybinds(0);
    }

    private void Update()
    {
        if (!reachedDestination)
        {
            float playerDistance = Vector3.Distance(PlayerController._PlayerController.transform.position, transform.position);
            fogValue = 0.15f - (playerDistance - 5.0f) / 20.0f * 0.15f + 0.05f;
            RenderSettings.fogDensity = fogValue;
            if ((double)playerDistance <= 5.0)
                reachedDestination = true;
        }
        else
            RenderSettings.fogDensity = fogValue;
        if ((Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("ActionY")) && JankoAvailable)
        {
            AskJankoForHelp();
        }
    }

    private void TurnBlockersOnOff(bool status)
    {
        foreach (GameObject blocker in blockers)
            blocker.SetActive(status);
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        switch (mode)
        {
            case 0:
                keybinds.Add(new Tuple<string, string>("X", "Turn around"));
                break;
            case 1:
                keybinds.Add(new Tuple<string, string>("X", "Turn back"));
                break;
            case 2:
                keybinds.Add(new Tuple<string, string>("X", "Submit"));
                keybinds.Add(new Tuple<string, string>("Y", "Ask Janko for more help"));
                break;
        }
        GameController.Master.SetupKeybinds(keybinds);
    }

    private IEnumerator ExitLockMechanism()
    {
        yield return new WaitUntil(() => lockMechanism.GetComponent<LockMechanism>().rotating == false);
        //interacting = false;
        PlayerController._PlayerController.gameObject.SetActive(!PlayerController._PlayerController.gameObject.activeInHierarchy);
        GameController.Master._GUI_interaction_text.SetActive(true);
        SceneCamera.gameObject.SetActive(false);
        lockMechanism.SetActive(false);
        spotLight.SetActive(false);
        PPP.TryGetSettings<ChromaticAberration>(out chromaticAberration);
        chromaticAberration.intensity.value = initialChromaticAberrationValue;
        fogValue = 0.2f;
    }

    public override void OnPlayerInteract()
    {
        fogValue = 0.6f;
        reachedDestination = true;
        //PlayerController._PlayerController.gameObject.SetActive(!PlayerController._PlayerController.gameObject.activeInHierarchy);
        //GameController.Master._GUI_interaction_text.SetActive(false);
        //GameController.Master.questSolving = true;
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        GameController.Master.WriteSceneMessage(texts["InitialMessage"]);
        SceneCamera.gameObject.SetActive(true);
        Keybinds(0);
        spotLight.SetActive(true);
        graffiti.GetComponent<Renderer>().material = m_luminousGraffiti;
        PPP.TryGetSettings<ChromaticAberration>(out chromaticAberration);
        initialChromaticAberrationValue = chromaticAberration.intensity.value;
        chromaticAberration.intensity.value = 0.0f;
        StartCoroutine(IntroduceJanko());
    }

    private IEnumerator IntroduceJanko()
    {
        QuestELever questElever = this;
        yield return (object)new WaitForSeconds(3f);
        t_pssst.SetActive(true);
        yield return (object)new WaitForSeconds(1.5f);
        ToggleKeybinds(true);
        yield return (object)new WaitUntil((Func<bool>)(() => Input.GetButtonDown("Interact")));
        ToggleKeybinds(false);
        t_pssst.SetActive(false);
        PushMessageToMaster("");
        Janko.SetActive(true);
        animator.SetBool(nameof(IntroduceJanko), true);
        StartCoroutine(ChangeFogTo(0.2f));
        yield return (object)new WaitForSeconds(2f);
        GameController.Master.WriteSceneMessage(texts["Janko_1"]);
        animator.SetBool(nameof(IntroduceJanko), false);
        yield return (object)new WaitForSeconds(3f);
        Keybinds(1);
        ToggleKeybinds(true);
        yield return (object)new WaitUntil((Func<bool>)(() => Input.GetButtonDown("Interact")));
        ToggleKeybinds(false);
        Keybinds(2);
        PushMessageToMaster("");
        animator.SetBool("ReturnToGraffiti", true);
        StartCoroutine(ChangeFogTo(0.6f));
        lockMechanism.SetActive(true);
        yield return (object)new WaitForSeconds(2f);
        ToggleKeybinds(true);
        animator.SetBool("ReturnToGraffiti", false);
        GameController.Master.WriteSceneMessage(texts["Player_1"]);
        JankoAvailable = true;
        StartCoroutine(WaitForLever());
    }

    private IEnumerator ChangeFogTo(float fv)
    {
        float initialFV = fogValue;
        float elapsed = 0.0f;
        float duration = 2f;
        while ((double)elapsed < (double)duration)
        {
            fogValue = Mathf.Lerp(initialFV, fv, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return (object)new WaitForEndOfFrame();
        }
        fogValue = fv;
    }

    private void AskJankoForHelp()
    {
        ToggleKeybinds(false);
        Keybinds(1);
        StartCoroutine(TurnCamera());
    }

    private IEnumerator TurnCamera()
    {
        QuestELever questElever = this;
        if (jankoInteractions >= 2)
        {
            System.Random rand = new System.Random();
            jankoInteractions = 30 + rand.Next(1, 6);
            Debug.Log(jankoInteractions);
        }
        else
        {
            jankoInteractions++;
        }
        lockMechanism.GetComponent<LockMechanism>().enabled = false;
        PushMessageToMaster("");
        animator.SetBool("IntroduceJanko", true);
        StartCoroutine(ChangeFogTo(0.2f));
        yield return (object)new WaitForSeconds(2f);
        GameController.Master.WriteSceneMessage(texts["Janko_" + jankoInteractions.ToString()]);
        animator.SetBool("IntroduceJanko", false);
        yield return (object)new WaitForSeconds(3f);
        Keybinds(1);
        ToggleKeybinds(true);
        yield return (object)new WaitUntil((Func<bool>)(() => Input.GetButtonDown("Interact")));
        ToggleKeybinds(false);
        Keybinds(2);
        PushMessageToMaster("");
        animator.SetBool("ReturnToGraffiti", true);
        StartCoroutine(ChangeFogTo(0.6f));
        yield return (object)new WaitForSeconds(2f);
        ToggleKeybinds(true);
        animator.SetBool("ReturnToGraffiti", false);
        GameController.Master.WriteSceneMessage(texts["Player_" + (jankoInteractions < 3 ? jankoInteractions : jankoInteractions / 10).ToString()]);
        lockMechanism.GetComponent<LockMechanism>().enabled = true;
    }

    private IEnumerator WaitForLever()
    {
        yield return new WaitUntil(() => lockMechanism.GetComponent<LockMechanism>().leverActive == true);
        GameController.Master.WriteSceneMessage("");
        WriteTextToDreamJournalMaster(texts["Finish"]);
        PlayerController._PlayerController.TogglePlayerOnOff(true);
        ToggleKeybinds(false);
        Janko.SetActive(false);
        SceneCamera.gameObject.SetActive(false);
        lockMechanism.SetActive(false);
        spotLight.SetActive(false);
        PPP.TryGetSettings<ChromaticAberration>(out chromaticAberration);
        chromaticAberration.intensity.value = initialChromaticAberrationValue;
        fogValue = 0.15f;

        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        this.GetComponent<BoxCollider>().enabled = false;
        lever.GetComponent<BoxCollider>().enabled = true;
    }

    public void OnPlayerLeverPull()
    {
        StartCoroutine(LeverPulled());
    }

    private IEnumerator LeverPulled()
    {
        foreach (UnityEngine.Object blocker in blockers)
            UnityEngine.Object.Destroy(blocker);
        Quaternion from = lever.transform.rotation;
        Quaternion to = lever.transform.rotation;
        to *= Quaternion.Euler(new Vector3(0.0f, 0.0f, 1f) * 120f);
        float elapsed = 0.0f;
        float duration = 0.5f;
        graffiti.GetComponent<Renderer>().material = m_doneGraffiti;
        lever.GetComponent<Renderer>().material = m_doneLever;
        while ((double)elapsed < (double)duration)
        {
            lever.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            fogValue = Mathf.Lerp(0.15f, 0.04f, elapsed / duration);
            yield return new WaitForEndOfFrame();
        }
        //PushMessageToMaster(texts["Finish"]);
        fogValue = 0.04f;
        lever.GetComponent<BoxCollider>().enabled = false;
        PlayerController._PlayerController.interactables.Remove(lever);
        transform.parent.gameObject.GetComponent<Scene>().finished = true;
        gameObject.SetActive(false);
    }
}
