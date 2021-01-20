using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : Scene
{
    GameObject star;
    ParticleSystem starlight;

    int collectedStars = 0;
    int counter = 0;

    bool catchable;
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Instruction", "I should catch those stars! They might be useful later!");
        catchable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && catchable)
        {
            CatchAFallingStar();
        }
    }

    void CatchAFallingStar()
    {
        star.GetComponent<MeshRenderer>().enabled = false;
        LeftStar(star);
        GetComponent<AudioSource>().Play();
        StartCoroutine(ExpandStarlight());
    }

    IEnumerator ExpandStarlight()
    {
        star.GetComponent<BoxCollider>().enabled = false;
        float elapsed = 0f;
        float duration = 1.75f;

        Quaternion from = star.transform.rotation;
        Quaternion to = star.transform.rotation;
        to *= Quaternion.Euler(new Vector3(0f, 1f, 0f) * 180);

        while (elapsed < duration)
        {
            star.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CatchFirstStar()
    {
        yield return new WaitForSeconds(1.5f);
        star.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "star") return;
        if (counter == 0)
        {
            PushMessageToMaster(texts["Instruction"]);
            Keybinds();
            ToggleKeybinds(true);
            StartCoroutine(CatchFirstStar());
        }
        else
        {
            //shootText.SetActive(true);
            ToggleKeybinds(true);
            catchable = true;
        }
        star = other.gameObject;
        counter++;
    }

    void LeftStar(GameObject other)
    {
        catchable = false;
        ToggleKeybinds(false);
        Destroy(other.transform.parent.gameObject, 2f);
    }

    private void OnTriggerExit(Collider other)
    {
        LeftStar(other.gameObject);
    }

    public override void Keybinds()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("X", "Catch star"));
        GameController.Master.SetupKeybinds(keybinds);
    }
}
