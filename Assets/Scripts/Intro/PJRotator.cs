using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJRotator : MonoBehaviour
{
    public List<GameObject> PJs;
    public GameObject selectionInstruction;
    public Camera camera;
    public int selected = 0;

    GameObject rotatingPJ;
    bool rotating = false;
    bool selecting = true;
    // Start is called before the first frame update
    void Start()
    {
        selectionInstruction.SetActive(true);
        StartCoroutine(RotatePJ());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !rotating)
        {
            StartCoroutine(Rotate(true));
            selected = (selected + 1) % 4;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !rotating)
        {
            StartCoroutine(Rotate(false));
            selected = ((selected - 1) + 4) % 4;
        }
    }

    IEnumerator Rotate(bool direction)
    {
        rotating = true;
        float elapsed = 0f;
        float duration = 1f;

        int dir = direction ? 1 : -1;

        Quaternion from = this.transform.rotation;
        Quaternion to = this.transform.rotation;
        to *= Quaternion.Euler(new Vector3(0f, dir * 1f, 0f) * 90);

        while (elapsed < duration)
        {
            this.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.transform.rotation = to;
        rotating = false;
    }

    IEnumerator RotatePJ()
    {
        while (selecting)
        {
            while (!rotating)
            {
                PJs[selected].transform.rotation *= Quaternion.Euler(new Vector3(0f, 1f, 0f) * 0.5f);
                yield return new WaitForEndOfFrame();
            }
            BackToInitial();
            yield return new WaitUntil(() => rotating == false);
        }
    }

    void BackToInitial()
    {
        foreach (GameObject o in PJs)
        {
            int factor = int.Parse(o.name.Split('_')[1]);
            o.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f - 90 * factor, 0f));
        }
    }

    IEnumerator RotateBackToInitial(Quaternion r, GameObject lastPJ)
    {
        float elapsed = 0f;
        float duration = 0.5f;

        Quaternion from = lastPJ.transform.localRotation;

        while (elapsed < duration)
        {
            lastPJ.transform.localRotation = Quaternion.Slerp(from, r, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        lastPJ.transform.localRotation = r;
    }

    public void SelectPJ()
    {
        StartCoroutine(ZoomOnPJ());
    }

    IEnumerator ZoomOnPJ()
    {
        MainMenuController.Menu.DeactivateTexts();
        selecting = false;
        rotating = true;
        selectionInstruction.SetActive(false);
        MainMenuController.Menu.PJSelection = false;
        for (int i = 0; i < PJs.Count; i++)
        {
            if (selected != i)
            {
                PJs[i].SetActive(false);
            }
        }

        int factor = int.Parse(PJs[selected].name.Split('_')[1]);
        Quaternion PJFrom = PJs[selected].transform.localRotation;
        Quaternion PJTo = Quaternion.Euler(new Vector3(0f, 180f - 90 * factor, 0f));

        Quaternion cameraFrom = camera.transform.rotation;
        Quaternion cameraTo = Quaternion.Euler(new Vector3(20f, 0f, 0f));

        Vector3 cameraStart = camera.transform.position;
        Vector3 cameraEnd = new Vector3(0f, 2f, -4f);

        float elapsed = 0f;
        float duration = 1f;
        while (elapsed < duration)
        {
            PJs[selected].transform.localRotation = Quaternion.Slerp(PJFrom, PJTo, elapsed / duration);
            camera.transform.position = Vector3.Lerp(cameraStart, cameraEnd, elapsed / duration);
            camera.transform.rotation = Quaternion.Slerp(cameraFrom, cameraTo, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        MainMenuController.Menu.PJSelected();
        yield return null;
    }
}
