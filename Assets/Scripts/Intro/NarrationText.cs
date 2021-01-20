using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationText : MonoBehaviour
{
    public GameObject next;
    public GameObject fadeOutTrigger;
    public float duration;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
        StartCoroutine(FadeOutText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            this.GetComponent<TextMeshProUGUI>().alpha = Mathf.Lerp(0, 1, elapsed / 1.5f);

            elapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        this.GetComponent<TextMeshProUGUI>().alpha = 1;
        if (next != null)
        {
            next.SetActive(true);
        }
        else
        {
            if (MainMenuController.Menu.gameObject.activeInHierarchy)
            {
                MainMenuController.Menu.narrationDone = true;
            }
        }
    }

    IEnumerator FadeOutText()
    {
        if (fadeOutTrigger == null) yield break;
        yield return new WaitUntil(() => fadeOutTrigger.activeInHierarchy);

        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration)
        {
            this.GetComponent<TextMeshProUGUI>().alpha = Mathf.Lerp(1, 0, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        this.GetComponent<TextMeshProUGUI>().alpha = 0;
    }
}
