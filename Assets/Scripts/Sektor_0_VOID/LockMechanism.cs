using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMechanism : MonoBehaviour
{
    public GameObject[] locks;
    public GameObject[] letters;
    public GameObject selectionLight;
    public GameObject spotLight;

    public GameObject leverHolder;
    public GameObject lever;
    public Material m_leverMaterial;

    public bool leverActive;

    char[] combination = { 'A', 'A', 'A', 'A' };
    string solution;
    int currentDial;
    public bool rotating;
    bool solved;
    List<Quaternion> rotations; //  0 - A   1 - B   2 - C   3 - D
    Dictionary<char, Quaternion> letterRotations;
    // Start is called before the first frame update
    void Start()
    {
        letterRotations = new Dictionary<char, Quaternion>();
        currentDial = 0;
        solution = GenerateSolution();
        rotations = GenerateRotations();
        solved = false;
        leverActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
        {
            if (currentDial != 0)
            {
                Vector3 lightPos = selectionLight.transform.localPosition;
                lightPos.x += 1.4f;
                selectionLight.transform.localPosition = lightPos;
                currentDial--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
        {
            if (currentDial != 3)
            {
                Vector3 lightPos = selectionLight.transform.localPosition;
                lightPos.x -= 1.4f;
                selectionLight.transform.localPosition = lightPos;
                //riddles[currentDial].SetActive(false);
                currentDial++;
                //riddles[currentDial].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickCodes.Up)
        {
            if (!rotating)
            {
                StartCoroutine(RotateDial(letters[currentDial], true));
                UpdateSolution(true);
                solved = CheckSolution();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickCodes.Down)
        {
            if (!rotating)
            {
                StartCoroutine(RotateDial(letters[currentDial], false));
                UpdateSolution(false);
                solved = CheckSolution();
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (CheckSolution())
            {
                StartCoroutine(EndScene());
            }
            else
            {
                GameController.Master.updateTimer(10);
            }
        }
        
    }

    IEnumerator EndScene()
    {
        lever.gameObject.SetActive(true);
        leverHolder.gameObject.SetActive(true);
        Color c = m_leverMaterial.GetColor("_EmissionColor");

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            float x = Mathf.Lerp(1, 35, t);
            m_leverMaterial.SetColor("_EmissionColor", new Color(1f * x, 0.66f * x, 0f, 1f * x));
            yield return new WaitForSeconds(0.01f);
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            float x = Mathf.Lerp(35, 10, t);
            m_leverMaterial.SetColor("_EmissionColor", new Color(1f * x, 0.66f * x, 0f, 1f * x));
            yield return new WaitForSeconds(0.03f);
        }
        leverActive = true;
        yield return null;
    }

    IEnumerator RotateDial(GameObject dial, bool direction)
    {
        rotating = true;
        Quaternion from = dial.transform.rotation;
        Quaternion to = dial.transform.rotation;
        int dir = direction ? -1 : 1;
        to *= Quaternion.Euler(new Vector3(dir * 1f, 0, 0) * 90);

        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            dial.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dial.transform.rotation = to;
        rotating = false;
        yield return null;
    }

    float GetAngleDifference(float x, float y)
    {
        return Mathf.Min(((x + (360 - y)) % 360), (((360 - x) + y)) % 360);
    }

    void UpdateSolution(bool direction)
    {
        if (direction && combination[currentDial] == 68 )
        {
            combination[currentDial] = 'A';
        }
        else if (!direction && combination[currentDial] == 65)
        {
            combination[currentDial] = 'D';
        }
        else
        {
            if (direction)
            {
                combination[currentDial]++;
            }
            else
            {
                combination[currentDial]--;
            }
        }
    }

    string GenerateSolution()
    {
        string s = "";
        System.Random rand = new System.Random();
        for (int i = 0; i < 4; i++)
        {
            s += (char)rand.Next(65, 69);
        }
        Debug.Log(s);
        return s;
    }

    bool CheckSolution()
    {
        int correctLetters = 0;
        for (int i = 0; i < 4; i++)
        {
            if (solution[i] == combination[i])
            {
                correctLetters++;
            }
        }
        SetLightIntensity(correctLetters);
        return correctLetters == 4 ? true : false;
    }

    void SetLightIntensity(int x)
    {
        spotLight.GetComponent<Light>().intensity = x == 1 ? 1 : 1 + (x - 1) * (2f/3f);
        spotLight.GetComponent<Light>().spotAngle = 15 + x * 15;
    }

    List<Quaternion> GenerateRotations()
    {
        List <Quaternion> r = new List<Quaternion>();
        Quaternion starting = letters[0].transform.rotation;
        r.Add(starting * Quaternion.Euler(new Vector3(0, 0, 0) * 90));
        r.Add(starting * Quaternion.Euler(new Vector3(1f, 0, 0) * -90));
        r.Add(starting * Quaternion.Euler(new Vector3(1f, 0, 0) * -180));
        r.Add(starting * Quaternion.Euler(new Vector3(1f, 0, 0) * -270));

        letterRotations.Add('A', r[0]);
        letterRotations.Add('B', r[1]);
        letterRotations.Add('C', r[2]);
        letterRotations.Add('D', r[3]);

        System.Random rand = new System.Random();
        for (int i = 0; i < 4; i++)
        {
            combination[i] = (char)rand.Next(65, 69);
            while (combination[i] == solution[i])
            {
                combination[i] = (char)rand.Next(65, 69);
            }
            letters[i].transform.rotation = letterRotations[combination[i]];
        }
        return r;
    }

}
