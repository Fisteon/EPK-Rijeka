using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishSector : MonoBehaviour
{
    public Image x;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReturnToIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReturnToIntro()
    {
        float elapsed = 0f;
        float duration = 3f;

        Color c = x.color;

        while (elapsed < duration)
        {
            c.a = Mathf.Lerp(0, 1, elapsed / duration);
            x.color = c;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        c.a = 1;
        x.color = c;

        SceneManager.LoadScene("Intro");
    }
}
