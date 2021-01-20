using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAUmbrella : Scene
{
    public Animator umbrellaAnimator;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Interacted", "Oops, gone with the wind!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        umbrellaAnimator.SetBool("interacted", true);

        yield return new WaitForSeconds(1.5f);
        PushMessageToMaster(texts["Interacted"]);
        yield return new WaitForSeconds(3.5f);
        EndScene();
        yield return null;
    }

    void EndScene()
    {
        SceneCamera.gameObject.SetActive(false);

        finished = true;
        PlayerController._PlayerController.TogglePlayerOnOff(true);
        this.gameObject.SetActive(false);
    }
}
