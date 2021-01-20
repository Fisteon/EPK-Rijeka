using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockCinematic : Scene
{
    public GameObject clock;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Start", "Well, that sure is a mighty big clock... I should take a look.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StartCinematic());
    }

    IEnumerator StartCinematic()
    {
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        clock.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(4f);

        clock.GetComponent<QuestYClock>().enabled = true;
        SceneCamera.gameObject.SetActive(false);
        PlayerController._PlayerController.TogglePlayerOnOff(true);

        PushMessageToMaster(texts["Start"]);
        this.GetComponent<BoxCollider>().enabled = false;
        yield return null;
    }
}
