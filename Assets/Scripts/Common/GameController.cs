// Decompiled with JetBrains decompiler
// Type: GameController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C57C325D-5678-4D04-9B68-2A70771730E3
// Assembly location: C:\My stuff\UnityBuilds\Rijeka\Rijeka_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("GUI elements")]
    public GameObject _GUI_interaction_text;
    public GameObject _GUI_wrongAnswer;

    public GameObject _GUI_notification_text;
    private TextMeshProUGUI GUI_notification_text;

    public GameObject _GUI_timer_text;
    private TextMeshProUGUI GUI_timer_text;

    public GameObject _GUI_location_text;
    private TextMeshProUGUI GUI_location_text;

    [Space(15)]
    public GameObject quitGamePanel;
    public GameObject quitGameParticles;
    public bool quitPanelOpen = false;

    [Space(15)]
    public GameObject minusTenSeconds;
    public GameObject plusSeconds;
    public Image gameOver;
    public GameObject gameWon;
    public RawImage fadeToSkyBackground;
    public GameObject outroNarration;
    public GameObject sectorMusic;
    public GameObject turnOffSound;
    public GameObject lowerVolumeForNarration;

    [Space(15f)]
    public GameObject loadIntro;

    [Space(10f)]
    public GameObject _DreamJournal;
    public List<GameObject> DreamJournalPages;
    public List<GameObject> DreamJournalPageCanvasTexts;
    public TextMeshProUGUI GUI_dream_journal_text;

    private int frontJournalPage;
    private bool dreamJournalIntroduced;
    private bool dreamJournalOpen;

    [Space(10f)]
    public GameObject _Map;
    public GameObject _Minimap;
    public UIKeybinds Keybinds;
    public Dictionary<string, GameController.GMScene> scenes;
    public List<Scene> testing;
    private GameController.GMScene activeScene;

    [Space(10f)]
    [Header("Dream Journal")]
    public Camera DreamJournalEffect;
    public GameObject DreamJournalButton;
    private int currentScene;

    [Header("Quest elements")]
    [Space(5f)]
    public GameObject _quest_years;
    public GameObject _quest_clock;
    public GameObject _quest_statue;
    public List<GameObject> initialInteractables;
    public Camera clockCutsceneCamera;
    public Camera playerMainCamera;
    public int minutes;
    public int seconds;
    private GameController.Timer timer;
    private bool QuestSolving;
    private bool Cutscene;
    private bool timerFlashing;
    private bool clockStarted = false;
    public List<string> messages;
    public bool permanentMessage;

    public System.Random randomNumberGenerator;
    private static GameController _GameController;

    public bool questSolving
    {
        get
        {
            return QuestSolving;
        }
        set
        {
            QuestSolving = value;
            PlayerController._PlayerController.enabled = !value;
            Debug.Log((object)("questSolving: " + value.ToString()));
            _GUI_interaction_text.SetActive(!value);
        }
    }

    public bool cutscene
    {
        get
        {
            return Cutscene;
        }
        set
        {
            if (Cutscene == value)
                return;
            Cutscene = value;
            StartCoroutine(SwapCameras(value));
        }
    }

    public static GameController Master
    {
        get
        {
            if (_GameController == null)
                _GameController = FindObjectOfType(typeof(GameController)) as GameController;
            if (_GameController == null)
                _GameController = new GameObject("GameMaster").AddComponent(typeof(GameController)) as GameController;
            return _GameController;
        }
    }

    private void Awake()
    {
        randomNumberGenerator = new System.Random();
        scenes = new Dictionary<string, GameController.GMScene>();
        GameController.GMScene gmScene1 = new GameController.GMScene(_quest_years);
        gmScene1.text.Add("Finished", "You read years 1508 and 1970");
        GameController.GMScene gmScene2 = new GameController.GMScene(_quest_clock);
        gmScene2.text.Add("Start", "You hear a loud noise somewhere near! The clock is ticking - hurry!");
        gmScene2.text.Add("Finished", "You picked up some wires");
        gmScene2.cutsceneCamera = clockCutsceneCamera;
        GameController.GMScene gmScene3 = new GameController.GMScene(_quest_statue);
        gmScene3.text.Add("Start", "There's too many trees blocking the way!");
        gmScene3.text.Add("Finished", "The trees move to form a path");
        gmScene3.startingCondition = false;
        gmScene3.finishCondition = false;
        activeScene = gmScene1;
        scenes.Add("Pillar", gmScene1);
        scenes.Add("Clock", gmScene2);
        scenes.Add("Statue", gmScene3);
    }

    public void Start()
    {
        messages = new List<string>();
        dreamJournalIntroduced = false;
        timerFlashing = false;
        StartCoroutine(WriteNotificationText());
        GUI_notification_text = _GUI_notification_text.GetComponentInChildren<TextMeshProUGUI>();
        GUI_timer_text = _GUI_timer_text.GetComponentInChildren<TextMeshProUGUI>();
        GUI_location_text = _GUI_location_text.GetComponentInChildren<TextMeshProUGUI>();
        currentScene = 1;
        timer = new GameController.Timer(minutes, seconds);
        StartCoroutine(SceneTransition());
        for (int index = 1; index < 4; ++index)
        {
            break;
            DreamJournalPageCanvasTexts[index].SetActive(false);
        }
        lowerVolumeForNarration.SetActive(false);
        AutomaticClockStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(GAMEWON());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(GAMEOVER());
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Quit"))
        {
            ToggleQuitConfirmation();
        }

        if (quitPanelOpen)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("ActionY"))
            {
                ToggleQuitConfirmation();
            }
            return;
        }
        if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("DreamJournal")) && dreamJournalIntroduced)
        {
            ToggleDreamJournal();
        }
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Map"))
        {
            ToggleMap();
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left) && dreamJournalOpen)
        {
            FlipJournalPage(true);
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right) && dreamJournalOpen)
        {
            FlipJournalPage(false);
        }
    }

    private IEnumerator SceneTransition()
    {
        GameController gameController = this;
        while (true)
        {
            yield return new WaitUntil(() => testing[0].finished == true);
            gameController.testing.RemoveAt(0);
            if (gameController.testing.Count != 0)
            {
                gameController.testing[0].gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(GAMEWON());
                break;
            }
        }
    }

    public void WriteLocation(string location)
    {
        GUI_location_text.text = "Somewhere around\n<i>" + location + "</i>";
    }

    public void GetInputFromPlayer(GameObject interactedObject)
    {
        if (initialInteractables.Contains(interactedObject))
        {
            activeScene.interacted = true;
        }
        else
        {
            if (!((UnityEngine.Object)activeScene.objectToInteractWith == (UnityEngine.Object)interactedObject) || !activeScene.finishCondition)
                return;
            messages.Add(activeScene.text["Finished"]);
            activeScene.finished = true;
            if (Enum.GetName(typeof(GameController.sceneOrder), (object)++currentScene) == null)
                return;
            //StartCoroutine(activateNextScene(Enum.GetName(typeof(GameController.sceneOrder), (object)currentScene)));
        }
    }

    public void WriteSceneMessage(string message)
    {
        permanentMessage = true;
        GUI_notification_text.text = message;
    }

    public void AutomaticClockStart()
    {
        Invoke("StartTheClock", 60f);
    }

    public void StartTheClock()
    {
        if (!clockStarted) clockStarted = true;
        else return;
        CancelInvoke("StartTheClock");
        _GUI_timer_text.SetActive(true);
        InvokeRepeating("updateTimer", 0.0f, 1f);
        StartCoroutine(LowerClockVolume(6f));
    }

    IEnumerator LowerClockVolume(float waitingTime)
    {
        AudioSource clockSound = _GUI_timer_text.GetComponent<AudioSource>();
        clockSound.enabled = true;
        yield return new WaitForSeconds(waitingTime);
        for (int i = 0; i < 5; i++)
        {
            clockSound.volume /= 1.6f;
            yield return new WaitForSeconds(0.95f);
        }
        clockSound.enabled = false;
    }

    public void StopTheClock()
    {
        CancelInvoke();
    }

    public void updateTimer()
    {
        --timer.seconds;
        GUI_timer_text.text = timer.ToString();
        int tickingTreshold;

        if (SceneManager.GetActiveScene().name == "Sector_3")
        {
            tickingTreshold = 15;
        }
        else
        {
            tickingTreshold = 60;
        }

        if (timer.seconds < tickingTreshold && !timerFlashing)
        {
            timerFlashing = true;
            StartCoroutine(NearEndTimerFlash());
        }
        if (timer.seconds == 0)
        {
            StopAllCoroutines();
            CancelInvoke();
            StartCoroutine(GAMEOVER());
        }
    }

    void ToggleQuitConfirmation()
    {
        quitPanelOpen = !quitPanelOpen;
        quitGamePanel.SetActive(quitPanelOpen);
        quitGameParticles.SetActive(quitPanelOpen);
    }

    IEnumerator GAMEOVER()
    {
        StartCoroutine(LowerClockVolume(0f));
        lowerVolumeForNarration.SetActive(false);
        _GUI_interaction_text.SetActive(false);
        questSolving = true;
        GameObject t_gameOver = gameOver.transform.GetChild(0).gameObject;
        gameOver.gameObject.SetActive(true);

        Color c = gameOver.color;

        float elapsed = 0f;
        float duration = 3f;

        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(0, 1, elapsed / duration);
            gameOver.color = c;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        c.a = 1;
        gameOver.color = c;

        t_gameOver.SetActive(true);
        //turnOffSound.SetActive(true);
        //gameOver.GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(4f);
        loadIntro.SetActive(true);      
    }

    IEnumerator GAMEWON()
    {
        StartCoroutine(LowerClockVolume(0f));
        lowerVolumeForNarration.SetActive(false);
        gameWon.SetActive(true);
        float elapsed = 0f;
        float duration = 3f;
        Color c = fadeToSkyBackground.color;

        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(0, 1, elapsed / duration);
            fadeToSkyBackground.color = c;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //sectorMusic.SetActive(false);
        //turnOffSound.SetActive(true);
        c.a = 1;
        fadeToSkyBackground.color = c;
        //loadIntro.SetActive(true);
        outroNarration.SetActive(true);
    }

    public void IncreaseTimer(int secondsToAdd)
    {
        timer.seconds += secondsToAdd;
        GUI_timer_text.text = timer.ToString();
        TextMeshProUGUI plusSec = Instantiate(plusSeconds, _GUI_timer_text.transform).GetComponent<TextMeshProUGUI>();
        plusSec.text += secondsToAdd.ToString();
    }

    public void updateTimer(int secondsToSubtract)
    {
        timer.seconds -= secondsToSubtract;
        GUI_timer_text.text = timer.ToString();
        Instantiate(minusTenSeconds, _GUI_timer_text.transform);
    }


    IEnumerator NearEndTimerFlash()
    {
        AudioSource clockSound = _GUI_timer_text.GetComponent<AudioSource>();
        clockSound.enabled = true;
        clockSound.volume = 1;
        while (true)
        {
            GUI_timer_text.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(.5f);
            GUI_timer_text.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void ToggleDreamJournal()
    {
        _DreamJournal.SetActive(!_DreamJournal.activeSelf);
        dreamJournalOpen = _DreamJournal.activeSelf;
    }

    public void StopListeningForDreamJournalActions()
    {
        dreamJournalIntroduced = false;
    }

    public void FlipJournalPage(bool direction)
    {
        DreamJournalPages[frontJournalPage].transform.localPosition = new Vector3(DreamJournalPages[frontJournalPage].transform.localPosition.x, -0.05f * frontJournalPage, DreamJournalPages[frontJournalPage].transform.localPosition.z);
        DreamJournalPageCanvasTexts[frontJournalPage].SetActive(false);
        if (direction)
            frontJournalPage = (frontJournalPage - 1 + 4) % 4;
        if (!direction)
            frontJournalPage = (frontJournalPage + 1) % 4;
        Vector3 localPosition = DreamJournalPages[frontJournalPage].transform.localPosition;
        localPosition.y = 0.1f;
        DreamJournalPageCanvasTexts[frontJournalPage].SetActive(true);
        DreamJournalPages[frontJournalPage].transform.localPosition = localPosition;
    }

    public void ToggleMap()
    {
        _Map.SetActive(!_Map.activeSelf);
        _Minimap.SetActive(!_Minimap.activeSelf);
    }

    public void CylinderWrongAnswer()
    {
        updateTimer(10);
        _GUI_wrongAnswer.SetActive(true);
        CancelInvoke("HideWrongAnswer");
        Invoke("HideWrongAnswer", 3f);
    }

    public void HideWrongAnswer()
    {
        _GUI_wrongAnswer.SetActive(false);
    }

    public void SetupKeybinds(List<Tuple<string, string>> keybinds)
    {
        Keybinds.SetupKeybinds(keybinds);
    }

    /*private IEnumerator activateNextScene(string nextScene)
    {
        GameController gameController = this;
        gameController.activeScene = gameController.scenes[nextScene];
        // ISSUE: reference to a compiler-generated method
        //yield return (object)new WaitUntil(new Func<bool>(gameController.\u003CactivateNextScene\u003Eb__67_0));
        gameController.messages.Add(gameController.activeScene.text["Start"]);
        // ISSUE: reference to a compiler-generated method
        //yield return (object)new WaitUntil(new Func<bool>(gameController.\u003CactivateNextScene\u003Eb__67_1));
        gameController.activeScene.started = true;
    }*/

    private IEnumerator WriteNotificationText()
    {
        GameController gameController = this;
        while (true)
        {
            if (gameController.messages.Count > 0)
            {
                if (gameController.messages[0].StartsWith("###"))
                {
                    gameController.messages[0] = gameController.messages[0].Substring(3);
                    gameController.GUI_notification_text.text = gameController.messages[0];
                    gameController.messages.RemoveAt(0);
                    float elapsed = 0.0f;
                    while (gameController.messages.Count <= 0 && (double)elapsed <= 2.0)
                    {
                        elapsed += Time.deltaTime;
                        yield return (object)new WaitForEndOfFrame();
                    }
                    gameController.GUI_notification_text.text = "";
                }
                else
                {
                    gameController.GUI_notification_text.text = gameController.messages[0];
                    yield return (object)new WaitForSeconds(5f);
                    if (!gameController.permanentMessage) gameController.GUI_notification_text.text = "";
                    yield return (object)new WaitForSeconds(0.5f);
                    gameController.messages.RemoveAt(0);
                }
            }
            yield return new WaitUntil(() => messages.Count > 0);
            permanentMessage = false;
        }
    }

    private IEnumerator IntroduceDreamJournal()
    {
        yield return (object)new WaitForSeconds(3f);
        DreamJournalButton.gameObject.SetActive(true);
        StartCoroutine(BlinkDreamJournalButton());
        DreamJournalEffect.gameObject.SetActive(true);
        yield return (object)new WaitForSeconds(2f);
        DreamJournalButton.transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator BlinkDreamJournalButton()
    {
        bool active = true;
        for (int i = 0; i < 4; ++i)
        {
            yield return (object)new WaitForSeconds(0.4f);
            DreamJournalButton.transform.GetChild(0).gameObject.SetActive(!active);
            DreamJournalButton.transform.GetChild(1).gameObject.SetActive(!active);
            active = !active;
        }
    }

    public void WriteInDreamJournal(string message)
    {
        if (!dreamJournalIntroduced)
        {
            StartCoroutine(IntroduceDreamJournal());
            dreamJournalIntroduced = true;
        }
        else
        {
            StartCoroutine(BlinkDreamJournalButton());
            this.GetComponent<AudioSource>().enabled = false;
            this.GetComponent<AudioSource>().enabled = true;
        }
        if (GUI_timer_text.IsActive())
        {
            TextMeshProUGUI dreamJournalText = GUI_dream_journal_text;
            dreamJournalText.text = dreamJournalText.text + "<color=#0E8AC0>" + GUI_timer_text.text + "</color>\n";
        }
        TextMeshProUGUI dreamJournalText1 = GUI_dream_journal_text;
        dreamJournalText1.text = dreamJournalText1.text + message + "\n\n";
    }

    private IEnumerator SwapCameras(bool value)
    {
        if (value)
        {
            playerMainCamera.enabled = false;
            clockCutsceneCamera.enabled = true;
        }
        if (!value)
        {
            yield return (object)new WaitForSeconds(1f);
            playerMainCamera.enabled = true;
            clockCutsceneCamera.enabled = false;
            RenderSettings.fogDensity = 0.04f;
        }
    }

    public class GMScene
    {
        public Dictionary<string, string> text = new Dictionary<string, string>();
        public bool started;
        public bool finished;
        public bool interacted;
        public bool startingCondition;
        public bool finishCondition;
        public GameObject objectToInteractWith;
        public Camera cutsceneCamera;

        public GMScene()
        {
            started = false;
            finished = false;
            interacted = false;
            startingCondition = true;
            finishCondition = true;
        }

        public GMScene(GameObject questObject)
        {
            objectToInteractWith = questObject;
            started = false;
            finished = false;
            interacted = false;
            startingCondition = true;
            finishCondition = true;
        }
    }

    private class Timer
    {
        private int Seconds;
        public int seconds
        {
            get
            {
                return Seconds;
            }
            set
            {
                if (value < 0)
                {
                    Seconds = 0;
                }
                else
                {
                    Seconds = value;
                }
            }
        }

        public Timer(int m, int s)
        {
            seconds = m * 60 + s;
        }

        public override string ToString()
        {
            return "" + (seconds / 60).ToString() + ":" + ((seconds % 60).ToString().Length == 2 ? (seconds % 60).ToString() : "0" + (seconds % 60).ToString());
        }
    }

    private enum sceneOrder
    {
        Pillar = 1,
        Clock = 2,
        Statue = 3,
    }
}
