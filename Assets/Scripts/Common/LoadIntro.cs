using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadIntro : MonoBehaviour
{
    public RawImage fadeToBlack;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Intro()
    {
        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration)
        {
            fadeToBlack.color = new Color(0, 0, 0, Mathf.Lerp(0, 1.1f, elapsed / duration));

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fadeToBlack.color = new Color(0, 0, 0);
        SceneManager.LoadScene("Intro");
    }
}
