using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DeadZone : MonoBehaviour
{
    public GameObject player;
    public PostProcessProfile PPP;

    [Range(0, 1)]
    public float closenessFactor;

    [Header("PostProcessing values")]
    public float LenseDistortion;
    public float PostExposure;

    private float init_LenseDistortion;
    private float init_PostExposure;

    private LensDistortion lense;
    private ColorGrading colorGrading;

    private bool playerTurned;

    [Range(5, 35)]
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        PPP.TryGetSettings<LensDistortion>(out lense);
        PPP.TryGetSettings<ColorGrading>(out colorGrading);

        init_LenseDistortion = lense.intensity.value;
        init_PostExposure = colorGrading.postExposure.value;

        playerTurned = false;
    }

    // Update is called once per frame
    void Update()
    {
        float closeness = GetCloseness();
        if (closeness < 1)
        {
            // izracunaj kolko blizu
            // postavi vrijednosti lense i exposure
            // ako je na critical udaljenosti okreni ga

            float amount = GetDistancePercentage(closeness);
            Debug.Log("Closeness: " + closeness);
            SetLenseAmount(init_LenseDistortion, LenseDistortion, amount);
            SetExposureAmount(init_PostExposure, PostExposure, amount);

            if (closeness <= 0.2 && !playerTurned)
            {
                player.transform.Rotate(Vector3.up, 180f);
                playerTurned = true;
                StartCoroutine(TurnCooldown());
            }
        }
        else
        {
            lense.intensity.value = init_LenseDistortion;
            colorGrading.postExposure.value = init_PostExposure;
        }
    }

    float GetCloseness()
    {
        float currentDistance = Vector3.Distance(player.transform.position, transform.position);
        return currentDistance / distance;
    }

    float GetDistancePercentage(float p)
    {
        return (1 - ((p - 0.2f) / 0.8f)) * 100;
    }

    void SetLenseAmount(float initial, float max, float current)
    {
        float newValue = (current * max + initial * (100 - current)) / 100;
        lense.intensity.value = newValue;
    }

    void SetExposureAmount(float initial, float max, float current)
    {
        float newValue = (current * max + initial * (100 - current)) / 100;
        colorGrading.postExposure.value = newValue;
    }

    IEnumerator TurnCooldown()
    {
        yield return new WaitForSeconds(2);
        playerTurned = false;
    }
}
