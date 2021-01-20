using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class QuestECannon : Scene
{
    public AudioSource cannonFireSound;
    public Camera cutsceneCamera;
    public GameObject playerProjectile;
    public GameObject explosion;
    public GameObject smokeTrail;

    public SkinnedMeshRenderer rootPlayer;

    public bool haveGunpowder = false;

    [Header("PPP stuff")]
    public PostProcessProfile PPP;
    public float targetPostExposure;
    public float init_PostExposure;

    private ColorGrading colorGrading;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Start", "Oh, I've got an idea! That gunpowder I found earlier might be of use now. Let me try...");
        texts.Add("Gunpowder", "###I'd like to fire this cannon but I'm still missing something... I should look around and find it.");

        PPP.TryGetSettings<ColorGrading>(out colorGrading);
        init_PostExposure = colorGrading.postExposure.value;
        animator = this.GetComponent<Animator>();
        ApplyMaterialToProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        if (!haveGunpowder)
        {
            PushMessageToMaster(texts["Gunpowder"]);
            return;
        }
        PushMessageToMaster(texts["Start"]);
        RemoveFromInteractables();
        StartCoroutine(CannonSetup());
    }

    IEnumerator CannonSetup()
    {
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        
        PlayerController._PlayerController.GetComponent<PlayerMovement>().enabled = false;
        PlayerController._PlayerController.GetComponent<NavMeshAgent>().enabled = false;

        SceneCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        PlayerController._PlayerController.GetComponent<CharacterAnimator>().enabled = false;

        float elapsed = 3f;
        float duration = 3f;

        while (elapsed > 0)
        {
            float x = (elapsed / duration) * 7;
            float y = Mathf.Exp(-x/1.2f);

            colorGrading.postExposure.value = Mathf.Lerp(init_PostExposure, targetPostExposure, y);

            elapsed -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        colorGrading.postExposure.value = targetPostExposure;

        SceneCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        playerProjectile.SetActive(true);
        //PlayerController._PlayerController.gameObject.SetActive(false);
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
        animator.SetBool("startLifting", true);

        //float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(animationDuration + 1.5f);
        yield return new WaitForSeconds(7.5f);

        playerProjectile.GetComponent<Animator>().SetBool("cannonFired", true);
        explosion.SetActive(true);
        smokeTrail.SetActive(true);

        yield return new WaitForSeconds(0.75f);
        playerProjectile.gameObject.SetActive(false);

        finished = true;

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    void ApplyMaterialToProjectile()
    {
        Material[] mats = new Material[] { rootPlayer.GetComponent<Renderer>().materials[1] };
        GameObject[] clothes = new GameObject[] { playerProjectile.transform.GetChild(playerProjectile.transform.childCount - 1).gameObject,
                                                  playerProjectile.transform.GetChild(playerProjectile.transform.childCount - 2).gameObject  };
        foreach (GameObject c in clothes)
        {
            c.GetComponent<Renderer>().materials = mats;
        }
    }
}
