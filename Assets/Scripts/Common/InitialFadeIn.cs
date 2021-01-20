using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class InitialFadeIn : MonoBehaviour
{
    public PostProcessProfile PPP;
    public RawImage FadeInBackground;

    float initialExposure;
    ColorGrading cg;
    private void Awake()
    {
        PPP.TryGetSettings<ColorGrading>(out cg);
        initialExposure = cg.postExposure.value;
        StartCoroutine(FadeIn());
    }
    
    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float duration = 1.5f;

        while (elapsed < duration)
        {
            FadeInBackground.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsed / duration));

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        FadeInBackground.color = new Color(0, 0, 0, 0);

        cg.postExposure.value = initialExposure;

        Destroy(this.gameObject);
    }
}
