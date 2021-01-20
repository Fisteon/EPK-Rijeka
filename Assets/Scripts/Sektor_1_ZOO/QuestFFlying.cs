using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class QuestFFlying : Scene
{
    [Header("PPP stuff")]
    public PostProcessProfile PPP;
    public float targetPostExposure;
    public float init_PostExposure;

    private ColorGrading colorGrading;

    public GameObject spaceship;
    public GameObject afterFlyingPosition;
    public FlyingPathController flying;
    // Start is called before the first frame update
    void Start()
    {
        flying.gameObject.SetActive(true);
        flying.enabled = true;
        sound.SetActive(true);
        RenderSettings.fog = true;

        PPP.TryGetSettings<ColorGrading>(out colorGrading);
        init_PostExposure = colorGrading.postExposure.value;

        StartCoroutine(TransitionOutOfFlying());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RestorePlayer()
    {
        PlayerController._PlayerController.gameObject.SetActive(true);
        PlayerController._PlayerController.transform.position = afterFlyingPosition.transform.position;
        PlayerController._PlayerController.transform.rotation = afterFlyingPosition.transform.rotation;

        PlayerController._PlayerController.TogglePlayerOnOff(true);
        PlayerController._PlayerController.GetComponent<PlayerMovement>().enabled = true;
        PlayerController._PlayerController.GetComponent<NavMeshAgent>().enabled = true;
        PlayerController._PlayerController.GetComponent<CharacterAnimator>().enabled = true;

        RenderSettings.fog = false;
    }

    IEnumerator TransitionOutOfFlying()
    {
        yield return new WaitUntil(() => flying.finishFlying == true);

        float elapsed = 3f;
        float duration = 3f;

        while (elapsed > 0)
        {
            float x = (elapsed / duration) * 7;
            float y = Mathf.Exp(-x / 1.2f);

            colorGrading.postExposure.value = Mathf.Lerp(init_PostExposure, targetPostExposure, y);

            elapsed -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        colorGrading.postExposure.value = targetPostExposure;

        ToggleKeybinds(false);
        spaceship.SetActive(false);

        RestorePlayer();

        SceneCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        elapsed = 0f;

        while (elapsed < duration)
        {
            float x = (elapsed / duration) * 7;
            float y = 1 - Mathf.Exp(-x);

            colorGrading.postExposure.value = Mathf.Lerp(targetPostExposure, init_PostExposure, y);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        colorGrading.postExposure.value = init_PostExposure;

        sound.SetActive(false);
        finished = true;
        this.gameObject.SetActive(false);
    }
}
