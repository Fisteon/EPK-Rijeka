using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestCMonkey : Scene
{
    public GameObject tarzanText;
    public GameObject tarzanNoise;
    public GameObject cannonBall;
    public GameObject particles;
    [Header("Monkey")]
    public GameObject monkey;
    public GameObject dustExplosion;
    public List<Transform> monkeyPositions;

    [Header("Circles")]
    public List<GameObject> targetCircles;
    [Space(5)]
    public GameObject aimCircle;
    public Vector3 initialSize;
   
    [Header("Banana")]
    public GameObject banana;
    public List<Vector3> bananaInstantiatePositions;

    [Space(15)]
    public Transform playerPosition;

    GameObject aimHolder;
    List<List<GameObject>> circles;
    bool offCooldown;

    System.Random rand;
    int selectedCircle = 0;
    int currentMonkeyPosition = 1;
    int hitCounter = 0;
    int missCounter = 0;

    bool interacting;
    // Start is called before the first frame update
    void Start()
    {
        circles = new List<List<GameObject>>();
        aimHolder = aimCircle.transform.parent.gameObject;
        PopulateCirclesList();
        rand = new System.Random();
        StartCoroutine(CircleShrinking());
        StartCoroutine(HelpWriter());
        StartCoroutine(MonkeyMovement());
        offCooldown = true;
        interacting = false;

        texts.Add("Start", "###You should throw him some bananas!");
        texts.Add("Hit_1", "###Yep, he seems to like it!");
        texts.Add("Hit_2", "###One more should do it!");
        texts.Add("Hit_3", "Tarzan dropped something before he disappeared! Let’s check it out.");

        texts.Add("Encounter", "1957... I read in the papers the other day Rijeka’s mayor is born that year. Other than that, I don’t know what the numbers mean.");
        texts.Add("JournalTarzan", "I encountered Tarzan, a monkey, and bribed him with fruit. In return, " +
                                   "I got something that looks like a cannonball. I think I know a place where it might fit nicely.");
        texts.Add("TarzanFruit", "Maybe I could bribe Tarzan with the fruit I found!");
        
        //texts.Add("JournalTarzan", "I read in papers once that Tarzan, the monkey, escaped from Rijeka's ZOO in 1957.");
    }

    // Update is called once per frame
    void Update()
    {
        if (!interacting) return;
        if (Input.GetKeyDown(KeyCode.A) || JoystickCodes.Left)
        {
            //TurnToRed(selectedCircle);
            selectedCircle = ((selectedCircle - 1) + 3) % 3;
            MoveSelection();
        }
        if (Input.GetKeyDown(KeyCode.S) || JoystickCodes.Right)
        {
            //TurnToRed(selectedCircle);
            selectedCircle = (selectedCircle + 1) % 3;
            MoveSelection();
        }

        if (Input.GetButtonDown("Interact") && offCooldown)
        {
            Shoot();
        }
    }

    void PopulateCirclesList()
    {
        foreach (GameObject g in targetCircles)
        {
            List<GameObject> c = new List<GameObject>();

            for (int i = 0; i < g.transform.childCount; i++)
            {
                c.Add(g.transform.GetChild(i).gameObject);
            }
            circles.Add(c);
        }
    }

    void MoveSelection()
    {
        aimHolder.transform.position = targetCircles[selectedCircle].transform.position;
        aimCircle.transform.localScale = initialSize;

    }

    void Shoot()
    {
        StartCoroutine(Cooldown());
        float precision = aimCircle.transform.localScale.x - 0.3f;

        GameObject projectile = Instantiate(banana, this.transform);
        projectile.transform.localPosition = bananaInstantiatePositions[selectedCircle];
        projectile.GetComponent<AppleProjectile>().targetLocation = monkeyPositions[selectedCircle].transform.position;
        projectile.GetComponent<AppleProjectile>().position = selectedCircle;

        if (precision < 0)
        {
            projectile.GetComponent<AppleProjectile>().isItPrecise = true;
        }
        else
        {
            missCounter++;
            projectile.GetComponent<AppleProjectile>().isItPrecise = false;
            float factor = Mathf.Lerp(1f, 2f, precision);
            projectile.GetComponent<AppleProjectile>().targetLocation +=
                new Vector3(factor * rand.Next(-1, 2),
                            factor * rand.Next(-1, 2),
                            factor * rand.Next(-1, 2));
        }       
    }

    IEnumerator HelpWriter()
    {
        while (true)
        {
            yield return new WaitUntil(() => missCounter % 3 == 0 && missCounter > 0);
            PushMessageToMaster("Shoot when the green circle is inside the smallest red circle.");
            yield return new WaitWhile(() => missCounter % 3 == 0);
        }
    }

    IEnumerator Cooldown()
    {
        offCooldown = false;
        //shootText.alpha = .3f;
        yield return new WaitForSeconds(1f);
        //shootText.alpha = 1f;
        offCooldown = true;
    }

    public void ThrowFinished(int pos, bool hit)
    {
        if (currentMonkeyPosition == pos && hit)
        {
            Debug.Log("HIT!");
            hitCounter++;
            PushMessageToMaster(texts["Hit_" + hitCounter.ToString()]);
            if (hitCounter == 3)
            {
                StartCoroutine(DustExplosion());
                EndScene();
            }
        }
        else
        {
            Debug.Log("MISS!");
        }
    }

    IEnumerator DustExplosion()
    {
        dustExplosion.SetActive(true);
        yield return new WaitForSeconds(2f);
        dustExplosion.SetActive(false);
    }

    IEnumerator CircleShrinking()
    {
        while (true)
        {
            while (aimCircle.transform.localScale.x > 0.17f)
            {
                aimCircle.transform.localScale *= 0.93f;
                yield return new WaitForSeconds(0.02f);
            }

            aimCircle.transform.localScale = initialSize;
        }
    }

    IEnumerator MonkeyMovement()
    {
        while (true)
        {
            monkey.transform.position = monkeyPositions[currentMonkeyPosition].position;
            float delay = (float)rand.NextDouble() + 4f;
            yield return new WaitForSeconds(delay);

            int nextPos = currentMonkeyPosition;
            while (nextPos == currentMonkeyPosition)
            {
                nextPos = rand.Next(0, 3);
            }
            currentMonkeyPosition = nextPos;
        }
    }

    public override void OnPlayerInteract()
    {
        GameController.Master.sectorMusic.SetActive(!GameController.Master.sectorMusic.activeInHierarchy);
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        interacting = true;
        StartCoroutine(Cutscene());
        /*SceneCamera.gameObject.SetActive(true);
        foreach (GameObject go in targetCircles)
        {
            go.SetActive(true);
        }
        aimCircle.SetActive(true);*/
    }

    IEnumerator Cutscene()
    {
        WriteTextToDreamJournalMaster(texts["JournalTarzan"]);
        PushMessageToMaster(texts["Encounter"]);
        particles.SetActive(false);
        SceneCamera.gameObject.SetActive(true);
        monkey.SetActive(true);

        yield return new WaitForSeconds(5f);
        tarzanNoise.SetActive(true);
        this.GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(2f);
        tarzanNoise.SetActive(false);

        //tarzanInfo.gameObject.SetActive(false);
        SceneCamera.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(SceneCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1f);
        //shootText.gameObject.SetActive(true);
        foreach (GameObject go in targetCircles)
        {
            go.SetActive(true);
        }
        aimCircle.SetActive(true);
        sound.SetActive(true);
        tarzanText.SetActive(true);

        Keybinds();
        ToggleKeybinds(true);
        yield return null;
    }

    void ExitScene()
    {
        PlayerController._PlayerController.TogglePlayerOnOff(true);
        SceneCamera.gameObject.SetActive(false);
        foreach (GameObject go in targetCircles)
        {
            go.SetActive(false);
        }
        aimCircle.SetActive(false);
        PlayerController._PlayerController.transform.position = playerPosition.position;
        PlayerController._PlayerController.transform.rotation = playerPosition.rotation;
        sound.SetActive(false);
        tarzanNoise.SetActive(false);
        tarzanText.SetActive(false);
    }

    public override void Keybinds()
    {
        List<Tuple<string, string>> keybinds = new List<Tuple<string, string>>();
        keybinds.Add(new Tuple<string, string>("←", "Select circle to the left"));
        keybinds.Add(new Tuple<string, string>("→", "Select circle to the right"));
        keybinds.Add(new Tuple<string, string>("X", "Shoot (1 sec cooldown)"));
        GameController.Master.SetupKeybinds(keybinds);
    }

    IEnumerator DropCannonBall()
    {
        ToggleKeybinds(false);
        yield return new WaitForSeconds(0.5f);
        //shootText.gameObject.SetActive(false);
        monkey.transform.position = monkeyPositions[1].position;

        cannonBall.SetActive(true);
        float animationDuration = cannonBall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration * 1.5f);

        ExitScene();
        this.gameObject.SetActive(false);
        finished = true;
        yield return null;
    }

    void EndScene()
    {
        RemoveFromInteractables();
        StartCoroutine(DropCannonBall());
    }
}
