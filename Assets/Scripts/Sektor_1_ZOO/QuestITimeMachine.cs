using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using System;

public class QuestITimeMachine : Scene
{
    [Header("Post processing")]
    public PostProcessProfile PPP;
    private LensDistortion lense;
    private ChromaticAberration chrom;

    [Header("Text stuff")]
    public TextMeshProUGUI t_action;
    public TextMeshProUGUI t_submit;

    [Header("Selection")]
    public int selectedCircle;
    public int selectedItem;

    public GameObject selectionHolder;
    public GameObject selectionCircle;
    public GameObject itemSelector;
    public GameObject itemHolder;
    public GameObject placedItemsHolder;
    public List<GameObject> items;
    public List<GameObject> placedItems;
    public List<int> circleItems;

    [Space(10)]
    public Material m_unused;
    public Material m_used;

    [Space(20)]
    public List<Animator> machineAnimators;
    public GameObject successParticles;
    public GameObject babyDino;
    public GameObject clockGettingEaten;
    public GameObject clockToTurnOff;
    public Camera cinematicCamera;
    public static int itemsCollected = 0;

    List<int> solution = new List<int>() { 0, 1, 2, 3, 4, 5};

    Vector3 initialCirclePosition;
    enum actions
    {
        move = 0,
        add = 1,
        remove = 2
    }

    int action;
    bool interacting;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("InteractBeforeReady", "###You should collect all six items first!");

        texts.Add("Description_0", "Nothing ever comes for free. If you are stingy – you´ll never try me.");                                    // COIN
        texts.Add("Description_1", "To read, to read – what a marvellous deed.");                                                               // BOOK
        texts.Add("Description_2", "Tibia, fibia, femur and pelvis – everyone has them, from Marylin to Elvis.");                               // BONE
        texts.Add("Description_3", "It starts with an F and rhymes with a riddle – play every tune nicely, make sure you don´t twiddle.");      // VIOLIN
        texts.Add("Description_4", "You get it at the theatre, you get it at the fair – keep it in your pocket, or it´ll vanish in thin air."); // TICKET
        texts.Add("Description_5", "Brew it well and let it bubble – mess it up and you´re in trouble!");                                       // POTION
        
        PPP.TryGetSettings<LensDistortion>(out lense);
        PPP.TryGetSettings<ChromaticAberration>(out chrom);

        selectedCircle = 0;
        selectedItem = 0;
        initialCirclePosition = selectionCircle.transform.localPosition;
        /*float[] distances = new float[32];
        distances[20] = 3.28f;
        SceneCamera.layerCullDistances = distances;*/
        placedItems = new List<GameObject> { null, null, null, null, null, null };
        circleItems = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            circleItems.Add(-1);
        }

        GenerateSolution();
        StartCoroutine(ToggleSubmitButton());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            itemsCollected = 6;
            OnPlayerInteract();
        }
        if (!interacting) return;

        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
        {
            selectedCircle = ((selectedCircle - 1 + 6) % 6);
            MoveSelectionCircle();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
        {
            selectedCircle = (selectedCircle + 1) % 6;
            MoveSelectionCircle();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickCodes.Up)
        {
            selectedItem = ((selectedItem - 1 + 6) % 6);
            ChangeSelectedItem();
            //RotateItemSelection(true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickCodes.Down)
        {
            selectedItem = (selectedItem + 1) % 6;
            ChangeSelectedItem();
            //RotateItemSelection(false);
        }

        if (Input.GetButtonDown("Interact"))
        {
            PerformAction(action);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("ActionY"))
        {
            Submit();
        }

        UpdateAction();
    }

    public override void OnPlayerInteract()
    {
        if (itemsCollected < 6)
        {
            PushMessageToMaster(texts["InteractBeforeReady"]);
            return;
        }
        foreach(Animator a in machineAnimators)
        {
            a.enabled = true;
        }
        ToggleSceneView(true);
        Keybinds();
    }

    void ToggleSceneView(bool state)
    {
        sound.SetActive(state);
        PlayerController._PlayerController.TogglePlayerOnOff(!state);
        SceneCamera.gameObject.SetActive(state);
        selectionHolder.SetActive(state);
        itemHolder.SetActive(state);
        selectionCircle.SetActive(state);
        interacting = state;
        ToggleKeybinds(state);
        GameController.Master.WriteSceneMessage(texts["Description_" + solution[selectedCircle].ToString()]);

        lense.enabled.value = !state;
        chrom.enabled.value = !state;
    }

    void Submit()
    {
        for (int i = 0; i < 6; i++)
        {
            if (circleItems[i] != solution[i]) return;
        }
        SceneCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
        successParticles.SetActive(true);
        babyDino.SetActive(true);
        clockToTurnOff.SetActive(false);
        clockGettingEaten.SetActive(true);
        PushSceneMessageToMaster("");
        ToggleKeybinds(false);
        StartCoroutine(WaitForEndOfAnimation());

        sound.SetActive(false);
        selectionCircle.SetActive(false);
        placedItemsHolder.SetActive(false);
        itemHolder.SetActive(false);
        //ToggleSceneView(false);
    }

    IEnumerator WaitForEndOfAnimation()
    {
        yield return new WaitForSeconds(6f);
        finished = true;
    }

    void PerformAction(int action)
    {
        if (action == (int)actions.add)
        {
            PlaceItemInCircle();

            //items[selectedItem].GetComponent<Renderer>().material = m_used;
            circleItems[selectedCircle] = selectedItem;
        }
        else if (action == (int)actions.remove)
        {
            RemoveItemFromCircle(selectedCircle);

            //items[circleItems[selectedCircle]].GetComponent<Renderer>().material = m_unused;
            circleItems[selectedCircle] = -1;
        }
        else if (action == (int)actions.move)
        {
            int currentPlaceOfItem = circleItems.IndexOf(selectedItem);
            RemoveItemFromCircle(currentPlaceOfItem);
            PlaceItemInCircle();

            circleItems[currentPlaceOfItem] = -1;
            circleItems[selectedCircle] = selectedItem;
        }
    }

    public override void Keybinds()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("X", "Place/Remove item"));
        keybinds.Add(new Tuple<string, string>("Y", "Submit"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    void PlaceItemInCircle()
    {    
        placedItems[selectedCircle] = Instantiate(items[selectedItem], selectionCircle.transform.position, items[selectedItem].transform.rotation, placedItemsHolder.transform);
        placedItems[selectedCircle].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //placedItems[selectedCircle].GetComponent<Renderer>().material = m_unused;
    }

    void RemoveItemFromCircle(int circle)
    {
        Destroy(placedItems[circle]);
        placedItems[selectedCircle] = null;
    }

    void ChangeSelectedItem()
    {
        Vector3 selectorPosition = itemSelector.transform.position;
        selectorPosition.y = items[selectedItem].transform.position.y;
        itemSelector.transform.position = selectorPosition;
    }

    void MoveSelectionCircle()
    {
        selectionCircle.transform.localPosition = new Vector3(initialCirclePosition.x + 0.39f * selectedCircle, initialCirclePosition.y, initialCirclePosition.z);
        GameController.Master.WriteSceneMessage(texts["Description_" + solution[selectedCircle].ToString()]);
    }

    void RotateItemSelection(bool direction)
    {
        StartCoroutine(Rotate(direction));
    }

    IEnumerator Rotate(bool dir)
    {
        int factor = dir ? 1 : -1;
        float elapsed = 0f;
        float duration = 0.5f;

        Quaternion from = itemHolder.transform.localRotation;
        Quaternion to = itemHolder.transform.localRotation;
        to *= Quaternion.Euler(new Vector3(factor * 1f, 0f, 0f) * 60);

        while (elapsed < duration)
        {
            itemHolder.transform.localRotation = Quaternion.Slerp(from, to, elapsed / duration);

            foreach(GameObject item in items)
            {
                item.transform.LookAt(-Vector3.forward);
            }

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        itemHolder.transform.localRotation = to;
    }

    void UpdateAction()
    {
        if (circleItems[selectedCircle] == -1)
        {
            if (circleItems.Contains(selectedItem))
            {
                t_action.text = "[X] Move item to selected circle";
                action = (int)actions.move;
            }
            else
            {
                t_action.text = "[X] Place item in circle";
                action = (int)actions.add;
            }
        }
        else
        {
            t_action.text = "[X] Remove item from circle";
            action = (int)actions.remove;
        }
    }

    IEnumerator ToggleSubmitButton()
    {
        while (true)
        {
            yield return new WaitUntil(() => circleItems.IndexOf(-1) == -1);
            t_submit.gameObject.SetActive(true);

            yield return new WaitUntil(() => circleItems.IndexOf(-1) != -1);
            t_submit.gameObject.SetActive(false);
        }
    }

    void GenerateSolution()
    {
        int count = solution.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int r = UnityEngine.Random.Range(i, count);

            int temp = solution[i];
            solution[i] = solution[r];
            solution[r] = temp;
        }
    }
}