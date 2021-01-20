using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestGTower : Scene
{
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer_A;
    public TextMeshProUGUI answer_B;
    public TextMeshProUGUI answer_C;
    public TextMeshProUGUI answer_D;

    public GameObject clockHandHours;
    public GameObject clockHandMinutes;

    public List<GameObject> cogs;
    public List<GameObject> letters;

    public Material m_glow;
    public Material m_base;

    public GameObject canvas;
    public GameObject legend;

    List<CogCombination> combination;

    bool rotatingCog;
    bool rotatingLetter;

    bool cutscenePlaying;
    bool firstInteraction;
    bool interacting;

    bool solved;

    int selectedCog;
    
    // Start is called before the first frame update
    void Start()
    {
        rotatingCog = false;
        rotatingLetter = false;
        solved = false;
        cutscenePlaying = false;
        firstInteraction = true;
        interacting = false;
        combination = new List<CogCombination>();
        for (int i = 0; i < 4; i++)
        {
            combination.Add(new CogCombination());
        }
        StartCoroutine(ClockHandsSpin());

        texts.Add("Start", "I want to finally stop this clock! But I need an object to help me with the mechanism.\nMaybe the cross could come in handy now...");

        texts.Add("Question_0", "What was the name of the poet and politician who took over Rijeka with his troops in 1919?");
        texts.Add("Answers_0_A", "A) Laval Nugent");
        texts.Add("Answers_0_B", "B) Gabriele D'Annunzio");
        texts.Add("Answers_0_C", "C) Thomas Fremantle");
        texts.Add("Answers_0_D", "D) Horatio Nelson");

        texts.Add("Question_1", "What was the name of the girl who allegedly saved Rijeka from a British attack?");
        texts.Add("Answers_1_A", "A) Katarina");
        texts.Add("Answers_1_B", "B) Koralina");
        texts.Add("Answers_1_C", "C) Karolina");
        texts.Add("Answers_1_D", "D) Katerina");

        texts.Add("Question_2", "When did the local heroine ask the admiral to stop the attack?");
        texts.Add("Answers_2_A", "A) 1919");
        texts.Add("Answers_2_B", "B) 1970");
        texts.Add("Answers_2_C", "C) 1509");
        texts.Add("Answers_2_D", "D) 1813");

        texts.Add("Question_3", "What was the name of one of Rijeka’s greatest modern poets?");
        texts.Add("Answers_3_A", "A) Janko Polić Kamov");
        texts.Add("Answers_3_B", "B) Antun Gustav Matoš");
        texts.Add("Answers_3_C", "C) Antun Branko Šimić");
        texts.Add("Answers_3_D", "D) Damir Martinović Mrle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlayerInteract();
        }

        if (cutscenePlaying || !interacting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
        {
            if (!rotatingCog)
            {
                StartCoroutine(RotateCog(cogs[selectedCog], true));
                UpdateCombination(1, "cogs");
                //solved = CheckSolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
        {
            if (!rotatingCog)
            {
                StartCoroutine(RotateCog(cogs[selectedCog], false));
                UpdateCombination(-1, "cogs");
                //solved = CheckSolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickCodes.Down)
        {
            if (!rotatingLetter)
            {
                StartCoroutine(RotateLetter(letters[selectedCog], true));
                UpdateCombination(1, "letters");
                //solved = CheckSolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickCodes.Up)
        {
            if (!rotatingLetter)
            {
                StartCoroutine(RotateLetter(letters[selectedCog], false));
                UpdateCombination(-1, "letters");
                //solved = CheckSolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("ActionB"))
        {
            selectedCog = (selectedCog + 1) % 4;
            ChangeQuestion();
            HighlightSelectedCog();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ActionA"))
        {
            selectedCog = (selectedCog - 1) % 4;
            if (selectedCog < 0) selectedCog += 4;
            ChangeQuestion();
            HighlightSelectedCog();
        }
        if (Input.GetButtonDown("Interact") && !solved)
        {
            if (CheckSolution())
            {
                solved = true;
                canvas.SetActive(false);
                ToggleKeybinds(false);
                StartCoroutine(EndScene());
                StartCoroutine(ClockHandsFinish());
            }
            else
            {
                ReduceTime(10);
            }
        }
    }

    public override void OnPlayerInteract()
    {
        if (firstInteraction)
        {
            ToggleKeybinds(false);
            Keybinds();
            PushMessageToMaster(texts["Start"]);
            StartCoroutine(PlayCutscene());
            firstInteraction = false;
        }
        else
        {
            ToggleKeybinds(true);
        }
        ChangeState(true);
    }

    void ChangeState(bool state)
    {
        foreach (GameObject cog in cogs)
        {
            cog.transform.parent.gameObject.SetActive(state);
        }
        GameController.Master.questSolving = state;
        GameController.Master._GUI_interaction_text.SetActive(!state);
        SceneCamera.gameObject.SetActive(state);
        interacting = state;
    }

    IEnumerator ClockHandsFinish()
    {
        Quaternion h_start = Quaternion.Euler(0, -60f, 0);
        Quaternion m_start = Quaternion.Euler(0, -135f, 0);

        clockHandHours.transform.localRotation = h_start;
        clockHandMinutes.transform.localRotation = m_start;

        float elapsed = 0f;
        float duration = 5f;

        yield return new WaitForSeconds(1.4f);

        while (elapsed < duration)
        {
            float x = (elapsed / duration) * 7;
            float y = 1 - Mathf.Exp(-x);
            clockHandHours.transform.localRotation = h_start * Quaternion.Euler(new Vector3(0, 1f, 0) * 60 * y);
            clockHandMinutes.transform.localRotation = m_start * Quaternion.Euler(new Vector3(0, 12f, 0) * 60 * y);

            elapsed += Time.deltaTime;
            yield return null;
        }
        GameController.Master.StopTheClock();
        finished = true;
    }

    IEnumerator EndScene()
    {
        #region Camera move up
        float startFoV = 60f;
        float targetFoV = 30f;

        Vector3 startPos = SceneCamera.transform.localPosition;
        Vector3 targetPos = SceneCamera.transform.localPosition;
        targetPos.y = 10.76f;

        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration)
        {
            SceneCamera.fieldOfView = Mathf.Lerp(startFoV, targetFoV, elapsed / duration);
            SceneCamera.transform.localPosition = new Vector3(targetPos.x, Mathf.Lerp(startPos.y, targetPos.y, elapsed / duration), targetPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        SceneCamera.fieldOfView = 30f;
        SceneCamera.transform.localPosition = new Vector3(SceneCamera.transform.localPosition.x, 10.76f, SceneCamera.transform.localPosition.z);
        #endregion 
        //gameObject.SetActive(false);
        //PlayerController._PlayerController.gameObject.SetActive(true);
        //GameController.Master._GUI_interaction_text.SetActive(true);
    }

    IEnumerator ClockHandsSpin()
    {
        Quaternion m = Quaternion.Euler(clockHandMinutes.transform.rotation.eulerAngles);
        Quaternion h = Quaternion.Euler(clockHandHours.transform.rotation.eulerAngles);
        while (!solved)
        {
            h *= Quaternion.Euler(new Vector3(0, 1f, 0) * 3);
            clockHandHours.transform.rotation = h;

            m *= Quaternion.Euler(new Vector3(0, 1f, 0) * 36);
            clockHandMinutes.transform.rotation = m;
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator PlayCutscene()
    {
        cutscenePlaying = true;
        yield return new WaitForSeconds(3f);
        float startFoV = SceneCamera.fieldOfView;
        float targetFoV = 60;

        Vector3 startPos = SceneCamera.transform.localPosition;
        Vector3 targetPos = SceneCamera.transform.localPosition;
        targetPos.y = -0.16f;
        
        float elapsed = 0f;
        float duration = 2.5f;

        while (elapsed < duration)
        {
            SceneCamera.fieldOfView = Mathf.Lerp(startFoV, targetFoV, elapsed / duration);
            SceneCamera.transform.localPosition = new Vector3(targetPos.x, Mathf.Lerp(startPos.y, targetPos.y, elapsed / duration), targetPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        SceneCamera.fieldOfView = 60f;
        SceneCamera.transform.localPosition = new Vector3(SceneCamera.transform.localPosition.x, -0.16f, SceneCamera.transform.localPosition.z);

        yield return new WaitForSeconds(0.5f);

        cogs[0].GetComponent<Renderer>().material = m_glow;
        letters[0].GetComponent<Renderer>().material = m_glow;
        canvas.gameObject.SetActive(true);
        cutscenePlaying = false;
        ToggleKeybinds(true);
        yield return null;
    }

    public override void Keybinds()
    {
        GameController.Master.SetupKeybinds(new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("X", "Submit"),
            new Tuple<string, string>("B", "Select cog to the right"),
            new Tuple<string, string>("A", "Select cog to the left"),
            new Tuple<string, string>("Y", "Leave")
        });
    }

    IEnumerator RotateCog(GameObject cog, bool direction)
    {
        rotatingCog = true;
        Quaternion from = cog.transform.rotation;
        Quaternion to = cog.transform.rotation;
        int dir = direction ? 1 : -1;
        to *= Quaternion.Euler(new Vector3(dir * 1f, 0, 0) * 36);

        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            cog.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cog.transform.rotation = to;
        rotatingCog = false;
        yield return null;
    }

    IEnumerator RotateLetter(GameObject letter, bool direction)
    {
        rotatingLetter = true;
        Quaternion from = letter.transform.rotation;
        Quaternion to = letter.transform.rotation;
        int dir = direction ? -1 : 1;
        to *= Quaternion.Euler(new Vector3(dir * 1f, 0, 0) * 90);

        float elapsed = 0f;
        float duration = 0.35f;

        while (elapsed < duration)
        {
            letter.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        letter.transform.rotation = to;
        rotatingLetter = false;
        yield return null;
    }

    void ChangeQuestion()
    {
        question.text = texts["Question_" + selectedCog.ToString()];
        answer_A.text = texts["Answers_" + selectedCog.ToString() + "_A"];
        answer_B.text = texts["Answers_" + selectedCog.ToString() + "_B"];
        answer_C.text = texts["Answers_" + selectedCog.ToString() + "_C"];
        answer_D.text = texts["Answers_" + selectedCog.ToString() + "_D"];
    }

    void UpdateCombination(int amount, string type)
    {
        if (type == "cogs")
        {
            combination[selectedCog].number = ((combination[selectedCog].number + amount) % 10);
            if (combination[selectedCog].number < 0) combination[selectedCog].number += 10;
        }
        else
        {
            if (combination[selectedCog].letter == 65 && amount == -1)
            {
                combination[selectedCog].letter = 'D';
            }
            else if (combination[selectedCog].letter == 'D' && amount == 1)
            {
                combination[selectedCog].letter = 'A';
            }
            else
            {
                if (amount == 1)
                {
                    combination[selectedCog].letter++;
                }
                else
                {
                    combination[selectedCog].letter--;
                }
            }
        }
    }

    void HighlightSelectedCog()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == selectedCog)
            {
                cogs[i].GetComponent<MeshRenderer>().material = m_glow;
                letters[i].GetComponent<MeshRenderer>().material = m_glow;
            }
            else
            {
                cogs[i].GetComponent<MeshRenderer>().material = m_base;
                letters[i].GetComponent<MeshRenderer>().material = m_base;
            }
        }
    }

    bool CheckSolution()
    {
        int counter = 0;
        if (combination[0].number == 1 && combination[0].letter == 'B') counter++;
        if (combination[1].number == 8 && combination[1].letter == 'C') counter++;
        if (combination[2].number == 1 && combination[2].letter == 'D') counter++;
        if (combination[3].number == 5 && combination[3].letter == 'A') counter++;

        return counter == 4;
    }

     class CogCombination
    {
        public int number;
        public char letter;

        public CogCombination()
        {
            number = 0;
            letter = 'A';
        }
    }
}
