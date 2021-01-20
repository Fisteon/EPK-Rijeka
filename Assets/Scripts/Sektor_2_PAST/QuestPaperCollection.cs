using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPaperCollection : Scene
{
    public GameObject paperHolder;
    public GameObject dreamJournal;

    public static bool showingPaper = false;

    List<GameObject> papers;
    List<bool> paperState;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("First", "This must be one of the drawings Emili was talking about. Well, I might just pick it up... He’s lucky I don’t have anything better to do.");

        texts.Add("Random_1", "###Oh, there’s another one. It seems to be in good condition, in spite of the rain.");
        texts.Add("Random_2", "###One more – gotcha!");
        texts.Add("Random_3", "###There, another drawing... I wonder how many more are flying around.");
        texts.Add("Random_4", "###Aha! Found a new one!");
        texts.Add("Random_5", "###I’m on a roll – here’s one more.");
        texts.Add("Random_6", "###Well, this guy surely did a lot of drawings, there’s another one.");

        texts.Add("Pickup_2", "Oh, this seems to be a facade of the big bank here in Rijeka. It’s actually right next to me.");
        texts.Add("Pickup_3", "Looks like a plan of a part of the bank complex... It’s from the 70ies.");
        texts.Add("Pickup_4", "Oh, wait, did I just pass through this black passage on the drawing?");
        texts.Add("Pickup_5", "These look familiar... almost every square in Rijeka’s Old Town is here. This guy seems to have a plan for everything.");
        texts.Add("Pickup_6", "The Kraš building... I’ve been there a few times, anyone with a sweet tooth has. If I think hard enough I can almost taste the Bajaderas!");
        texts.Add("Pickup_7", "Well, well, look what we have here. I’m literally in front of this building as we speak, what are the odds?");
        texts.Add("Pickup_8", "Another drawing? How... useful. But okay, I guess it’s pretty, and at least he’s trying. I’m going to help him and get out of here.");
        texts.Add("Pickup_9", "This staircase part on the facade looks familiar... I think this building isn’t far from here.");
        texts.Add("Pickup_10", "Hmmm... This looks like some sort of urban planners’ analysis. Cool drawing.");
        texts.Add("Pickup_11", "Oh, I’ve already found a drawing of this building. This seems to be the opposite facade, viewed from the side of the main street.");
        texts.Add("Pickup_12", "This guy really thought a lot about Rijeka – especially the Old Town, it seems. It’s kind of like his playground, I guess.");
        texts.Add("Pickup_13", "Oh, the city tower. Wait, that’s really close to where I am right now!");
        texts.Add("Pickup_14", "If I had a pen I could draw an X on this drawing of my current location, I’m literally here.");

        paperState = new List<bool>();
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        QuestXFinalPuzzle.papersCollected++;
        GameController.Master.GetComponent<AudioSource>().Play();
        RemoveFromInteractables();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        PushMessageToMaster(texts["Pickup_" + this.transform.name.Split('_')[1]]);
        StartCoroutine(ShowPaper());
    }

    IEnumerator ShowPaper()
    {
        showingPaper = true;
        for (int i = 0; i < paperHolder.transform.childCount; i++)
        {
            GameObject p = paperHolder.transform.GetChild(i).gameObject;
            if (p.name.StartsWith("Page"))
            {
                paperState.Add(p.activeInHierarchy);
                p.SetActive(false);
                //paperState[int.Parse(p.name.Split('_')[1]) - 1] = p.activeInHierarchy;
            }
        }

        int currentPaperIndex = int.Parse(this.name.Split('_')[1]) - 1;
        paperState[currentPaperIndex] = true;
        paperHolder.transform.GetChild(currentPaperIndex + 1).gameObject.SetActive(true);
        dreamJournal.SetActive(true);
        yield return new WaitForSeconds(3f);
        dreamJournal.SetActive(false);

        for (int i = 0; i < paperHolder.transform.childCount; i++)
        {
            GameObject p = paperHolder.transform.GetChild(i).gameObject;
            if (p.name.StartsWith("Page"))
            {
                p.SetActive(paperState[i - 1]);
            }
        }

        showingPaper = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PushMessageToMaster(texts["Random_" + Random.Range(1, 7).ToString()]);
    }
}
