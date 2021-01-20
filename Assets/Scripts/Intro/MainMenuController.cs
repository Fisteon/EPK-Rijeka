using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Texts")]
    public List<GameObject> texts;
    public List<GameObject> PJTexts;
    public TextMeshProUGUI t_play;
    public TextMeshProUGUI t_play_r;
    public TextMeshProUGUI t_anyKey;
    public TextMeshProUGUI t_sectorSelector;

    [Header("Objects")]
    public GameObject PJs;
    public GameObject t_PJSelected;
    public GameObject firstIntro;

    public List<GameObject> sectorChoices;
    public int chosenPJ;

    [Header("Sectors")]
    //public List<GameObject> sectors;
    public Image BaseSector;
    public List<GameObject> sectorNarrations;

    [Header("Miscellanious")]
    public Image fadeOutPanel;
    public List<Color> fadeOutColors;
    public bool pressAnyKey;
    public bool PJSelection = false;
    public bool narrationDone = false;
    public bool CONTROL_VARIABLE;
    bool selectingSector = false;
    int selectedSector = 0;
    int textCounter = 0;

    const string intro = "Congratulations!\n" +
                         "You decided it’s time take a break from this crazy architectural exhibition.\n" +
                         "Relax now, lean back. Would you like to lie down? Take a nap?\n" +
                         "We have a fine collection of pyjamas at your disposal.";

    #region Singleton
    private static MainMenuController _MainMenuController = null;
    public static MainMenuController Menu
    {
        get
        {
            if (_MainMenuController == null)
            {
                _MainMenuController = FindObjectOfType(typeof(MainMenuController)) as MainMenuController;
            }
            if (_MainMenuController == null)
            {
                GameObject obj = new GameObject("MainMenuController");
                _MainMenuController = obj.AddComponent(typeof(MainMenuController)) as MainMenuController;
            }
            return _MainMenuController;
        }
    }

    #endregion

    private void Awake()
    {
        /*if (SceneManager.GetActiveScene().name != "Intro")
        {
            CONTROL_VARIABLE = false;
            this.gameObject.SetActive(false);
        }
        else
        {
            CONTROL_VARIABLE = true;
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;*/
    }

    void Start()
    {
        StartCoroutine(BlinkPlay());
        pressAnyKey = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && pressAnyKey)
        {
            pressAnyKey = false;
            StopAllCoroutines();
            StartCoroutine(InitiateIntro());
            StartCoroutine(WaitForPJs());
        }

        if (Input.GetButtonDown("Interact") && PJSelection)
        {
            //PJs.GetComponent<PJRotator>().SelectPJ();
            PJSelection = false;
            PJs.GetComponent<PJSelector>().SelectPJ();
            DataTransfer.material = PJs.GetComponent<PJSelector>().selected + 1;
        }

        if (selectingSector)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
            {
                selectedSector = (selectedSector + 1) % 4;
                ChangeSectorSelection();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
            {
                selectedSector = ((selectedSector - 1) + 4) % 4;
                ChangeSectorSelection();
            }

            if (Input.GetButtonDown("Interact"))
            {
                LoadSector(selectedSector);
            }
        }
    }

    IEnumerator BlinkPlay()
    {
        while (true)
        {
            float elapsed = 0f;
            float duration = 0.5f;

            Vector3 RectStart = t_play.rectTransform.anchoredPosition;
            float X_left_target = RectStart.x - 15;
            float X_right_target = RectStart.x + 15;

            float alphaStart = t_play.alpha;
            float alphaTarget = t_play.alpha / 6;

            while (elapsed < duration)
            {
                t_play.rectTransform.anchoredPosition = new Vector3(Mathf.Lerp(RectStart.x, X_left_target, elapsed / duration), RectStart.y, RectStart.z);
                t_play_r.rectTransform.anchoredPosition = new Vector3(Mathf.Lerp(RectStart.x, X_right_target, elapsed / duration), RectStart.y, RectStart.z);

                t_play.alpha = Mathf.Lerp(alphaStart, alphaTarget, elapsed / duration);
                t_play_r.alpha = Mathf.Lerp(alphaStart, alphaTarget, elapsed / duration);

                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.15f);
            elapsed = 0f;
            while (elapsed < duration)
            {
                t_play.rectTransform.anchoredPosition = new Vector3(Mathf.Lerp(X_left_target, RectStart.x, elapsed / duration), RectStart.y, RectStart.z);
                t_play_r.rectTransform.anchoredPosition = new Vector3(Mathf.Lerp(X_right_target, RectStart.x, elapsed / duration), RectStart.y, RectStart.z);

                t_play.alpha = Mathf.Lerp(alphaTarget, alphaStart, elapsed / duration);
                t_play_r.alpha = Mathf.Lerp(alphaTarget, alphaStart, elapsed / duration);

                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator InitiateIntro()
    {
        Destroy(t_play_r.gameObject);
        while (t_play.alpha > 0)
        {
            t_play.alpha -= (5f / 255f);
            t_anyKey.alpha -= (5f / 255f);
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1f);

        firstIntro.SetActive(true);
        t_play.gameObject.SetActive(false);
        t_anyKey.gameObject.SetActive(false);
    }

    public void ActivateNextText(GameObject t, float delay)
    {
        StartCoroutine(FadingTextActivator(t, delay));
    }

    IEnumerator FadingTextActivator(GameObject t, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (t == texts[2])
        {
            texts[0].SetActive(false);
            texts[1].SetActive(false);
        }
        t.SetActive(true);
    }

    IEnumerator WaitForPJs()
    {
        yield return new WaitUntil(() => PJs.activeInHierarchy == true);
        PJSelection = true;
    }
    
    public void DeactivateTexts()
    {
        foreach(GameObject go in texts)
        {
            go.SetActive(false);
        }
    }

    public void PJSelected()
    {
        t_PJSelected.SetActive(true);
        StartCoroutine(LoadSectorSelection());
    }

    IEnumerator LoadSectorSelection()
    {
        yield return new WaitForSeconds(4f);
        PJs.SetActive(false);
        t_PJSelected.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        t_sectorSelector.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        selectingSector = true;
        sectorChoices[0].SetActive(true);
        selectedSector = 0;
    }

    void SectorChosen(int sector)
    {
        for (int i = 0; i < sectorChoices.Count; i++)
        {
            if (i != sector)
            {
                sectorChoices[i].SetActive(false);
            }
        }
        //StartCoroutine(ZoomOnChosenSector(sector));
    }

    void ChangeSectorSelection()
    {
        for (int i = 0; i < sectorChoices.Count; i++)
        {
            if (i == selectedSector)
            {
                sectorChoices[i].SetActive(true);
            }
            else
            {
                sectorChoices[i].SetActive(false);
            }
        }
    }

    IEnumerator ZoomOnChosenSector(int s)
    {
        t_sectorSelector.gameObject.SetActive(false);
        sectorChoices[s].GetComponent<TMPFadeInText>().enabled = false;

        RectTransform sector = sectorChoices[s].GetComponent<RectTransform>();
        Vector3 rectStart = sector.anchoredPosition3D;
        Vector3 rectTarget = new Vector3(0f, 0f, 0f);

        sector.GetComponent<TextMeshProUGUI>().alpha = 255f;
        float fontSizeStart = sector.GetComponent<TextMeshProUGUI>().fontSize;
        float fontSizeTarget = 55f;

        sectorChoices[s].gameObject.transform.GetChild(0).gameObject.SetActive(false);

        float elapsed = 0f;
        float duration = 1.25f;

        while (elapsed < duration)
        {
            sector.anchoredPosition3D = Vector3.Lerp(rectStart, rectTarget, elapsed / duration);
            sector.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(fontSizeStart, fontSizeTarget, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        LoadSector(s);
    }

    void LoadSector(int sector)
    {
        StartCoroutine(SectorSelected(sector));
        //StartCoroutine(Narration(sector));
    }

    IEnumerator SectorSelected(int sector)
    {
        for (int i = 0; i < sectorChoices.Count; i++)
        {
            if (i != sector)
            {
                sectorChoices[i].SetActive(false);
            }
        }

        float elapsed = 0;
        float duration = 1.5f;

        AudioSource backgroundMusic = this.GetComponent<AudioSource>();
        Color c = fadeOutColors[sector];

        while (elapsed < duration)
        {
            fadeOutPanel.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1.1f, elapsed / duration));
            backgroundMusic.volume = Mathf.Lerp(0.025f, 0, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        fadeOutPanel.color = new Color(c.r, c.g, c.b, 1.1f);
        SceneManager.LoadScene("Sector_" + sector.ToString(), LoadSceneMode.Single);
        sectorChoices[sector].transform.parent.gameObject.SetActive(false);

        //StartCoroutine(Narration(sector));
    }

    IEnumerator Narration(int sector)
    {
        sectorNarrations[sector].SetActive(true);
        float elapsed = 0;
        float duration = 0.5f;
        AudioSource backgroundMusic = this.GetComponent<AudioSource>();
        float initialBackgroundMusicVolume = backgroundMusic.volume;

        while (elapsed < duration)
        {
            backgroundMusic.volume = Mathf.Lerp(initialBackgroundMusicVolume, 0.055f, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitUntil(() => narrationDone == true);

        elapsed = 0;
        duration = 1.5f;

        Color c = fadeOutColors[sector];

        while (elapsed < duration)
        {
            fadeOutPanel.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1.1f, elapsed / duration));
            backgroundMusic.volume = Mathf.Lerp(0.055f, 0, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        fadeOutPanel.color = new Color(c.r, c.g, c.b, 1.1f);
        SceneManager.LoadScene("Sector_" + sector.ToString(), LoadSceneMode.Single);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
