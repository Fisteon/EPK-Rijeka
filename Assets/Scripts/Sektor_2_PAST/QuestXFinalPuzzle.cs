using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QuestXFinalPuzzle : Scene
{
    public PostProcessProfile PPP;
    ChromaticAberration aberration;

    public static int papersCollected = 1;
    public GameObject firstBarricade;
    public GameObject secondBarricade;

    public GameObject LastEmili;

    public List<GameObject> shapes;
    int selectedShape;
    public GameObject shapeSelector;

    public List<GameObject> outlines;
    int selectedOutline;

    List<int> order = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

    bool interacting = false;
    bool puzzleFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountPapers());
        PPP.TryGetSettings<ChromaticAberration>(out aberration);

        texts.Add("Introduction", "Thank you for helping my friend Igor. He’s done a lot for me – do you realize how much? Let’s see if you do...");

        texts.Add("1", "Ri-Adria banka");
        texts.Add("2", "Zgrada Kraša na Korzu");
        texts.Add("3", "Robna kuća Varteks");
        texts.Add("4", "Građevno projektni zavod");
        texts.Add("5", "PBZ Privredna banka");
        texts.Add("6", "Zgrada brodomaterijala");
        texts.Add("7", "Konzervatorski zavod");

        ShuffleBuildingOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            papersCollected++;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlayerInteract();
        }

        if (!interacting) return;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("ActionA"))
        {
            CycleSelectedOutline(false);
            //selectedOutline = 
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("ActionB"))
        {
            CycleSelectedOutline(true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickCodes.Up)
        {
            CycleSelectedShape(false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickCodes.Down)
        {
            CycleSelectedShape(true);
        }

        if (Input.GetButtonDown("Interact") && !puzzleFinished)
        {
            PlaceShape();
        }
    }

    public override void OnPlayerInteract()
    {
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        aberration.active = false;
        Keybinds(0);
        ToggleKeybinds(true);
        //this.GetComponent<AudioSource>().enabled = true;
        StartCoroutine(Introduction());
    }

    IEnumerator Introduction()
    {
        PushSceneMessageToMaster(texts["Introduction"]);
        yield return new WaitForSeconds(5f);
        shapeSelector.SetActive(true);
        interacting = true;
        PushSceneMessageToMaster("Place the building on the map: " + texts[order[0].ToString()] + ".");
    }

    public override void Keybinds(int mode)
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        if (mode == 0)
        {
            keybinds.Add(new Tuple<string, string>("B", "Cycle outlines - right")); // →
            keybinds.Add(new Tuple<string, string>("A", "Cycle outlines - left")); // ←
            //keybinds.Add(new Tuple<string, string>("T", "Check drawings"));
            keybinds.Add(new Tuple<string, string>("X", "Place"));
        }
        else if (mode == 1)
        {
            keybinds.Add(new Tuple<string, string>("X", "Talk to Igor Emili"));
        }
        
        GameController.Master.SetupKeybinds(keybinds);
    }

    void PlaceShape()
    {
        //Debug.Log("Shape: " + selectedShape.ToString() + "\tOutline: " + selectedOutline.ToString() + "\tRequired: " + order[0].ToString());
        if ((outlines[selectedOutline].name.Split('_')[1] == shapes[selectedShape].name.Split('_')[1]) && 
            (shapes[selectedShape].name.Split('_')[1] == order[0].ToString()))
        {
            shapes[selectedShape].transform.position = outlines[selectedOutline].transform.position;
            shapes[selectedShape].transform.rotation = outlines[selectedOutline].transform.rotation;
            shapes[selectedShape].transform.localScale = outlines[selectedOutline].transform.localScale;

            outlines[selectedOutline].SetActive(false);
            outlines.RemoveAt(selectedOutline);
            shapes.RemoveAt(selectedShape);
            order.RemoveAt(0);

            selectedOutline = 0;
            selectedShape = 0;

            if (order.Count > 0)
            {
                shapeSelector.transform.position = shapes[0].transform.position;
                PushSceneMessageToMaster("Place the building on the map: " + texts[order[0].ToString()] + ".");
                return;
            }
            else
            {
                shapeSelector.SetActive(false);
                StartCoroutine(ExitPuzzle());
            }
        }
        else
        {
            ReduceTime(10);
        }
    }

    IEnumerator ExitPuzzle()
    {
        puzzleFinished = true;
        Keybinds(1);
        interacting = false;
        yield return new WaitForSeconds(.75f);
        interacting = true;
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        interacting = false;
        LastEmili.SetActive(true);
    }

    void CycleSelectedShape(bool direction)
    {
        if (direction)
        {
            selectedShape = (selectedShape + 1) % shapes.Count;
        }
        if (!direction)
        {
            selectedShape = (selectedShape - 1 + shapes.Count) % shapes.Count;
        }
        shapeSelector.transform.position = shapes[selectedShape].transform.position;
    }

    void CycleSelectedOutline(bool direction)
    {
        outlines[selectedOutline].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        if (direction)
        {
            selectedOutline = (selectedOutline + 1) % outlines.Count;
        }
        if (!direction)
        {
            selectedOutline = (selectedOutline - 1 + outlines.Count) % outlines.Count;
        }
        outlines[selectedOutline].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    void ShuffleBuildingOrder()
    {
        int count = order.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int r = UnityEngine.Random.Range(i, count);

            int temp = order[i];
            order[i] = order[r];
            order[r] = temp;
        }
    }

    IEnumerator CountPapers()
    {
        yield return new WaitUntil(() => papersCollected == 5);
        firstBarricade.SetActive(true);
        yield return new WaitUntil(() => papersCollected == 10);
        secondBarricade.SetActive(true);
        yield return new WaitUntil(() => papersCollected == 14);
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
