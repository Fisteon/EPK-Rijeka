using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ModifiedDeadZone : MonoBehaviour
{
    public GameObject player;
    [Header("PostProcessing values")]
    public PostProcessProfile PPP;
    [Space(10)]
    public float targetLenseDistortion;
    public float targetPostExposure;

    private float init_LenseDistortion;
    private float init_PostExposure;

    private LensDistortion lense;
    private ColorGrading colorGrading;

    // Start is called before the first frame update
    void Start()
    {
        PPP.TryGetSettings<LensDistortion>(out lense);
        PPP.TryGetSettings<ColorGrading>(out colorGrading);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            init_LenseDistortion = lense.intensity.value;
            init_PostExposure = colorGrading.postExposure.value;
            StartCoroutine(DeadZoneEffect());
        }
    }

    // disable player movement controls
    // keep him moving forward
    // increase PP effects
    // flip him
    // decrease PP effects
    // move him forward for the same amount of time
    IEnumerator DeadZoneEffect()
    {
        WASDMovement.deadzoning = true;
        WASDMovement.deadzoningINSIDE = true;

        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration)
        {
            lense.intensity.value = Mathf.Lerp(init_LenseDistortion, targetLenseDistortion, elapsed / duration);
            colorGrading.postExposure.value = Mathf.Lerp(init_PostExposure, targetPostExposure, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        WASDMovement.deadzoning = false;
        /*
        Quaternion to = player.transform.rotation;
        to *= (Quaternion.Euler(new Vector3(0f, 1f, 0f) * 180));
        player.transform.rotation = to;*/

        player.transform.rotation = this.transform.GetChild(0).transform.rotation;
        player.transform.position = this.transform.GetChild(0).transform.position;

        WASDMovement.deadzoning = true;
        WASDMovement.deadzoningINSIDE = false;
        WASDMovement.deadzoningOUTSIDE = true;
        elapsed = 0f;
        duration = 2.5f;

        while (elapsed < duration)
        {
            lense.intensity.value = Mathf.Lerp(targetLenseDistortion, init_LenseDistortion, elapsed / duration);
            colorGrading.postExposure.value = Mathf.Lerp(targetPostExposure, init_PostExposure, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        lense.intensity.value = init_LenseDistortion;
        colorGrading.postExposure.value = init_PostExposure;
        WASDMovement.deadzoning = false;
    }
}
