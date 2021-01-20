using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class HistoricalTexts : MonoBehaviour
{
    public int sector;

    #region COMMON TEXTS
    List<string> timeRunningOut = new List<string> {
        "Damn, my time is running out quickly!",
        "I should hurry up!",
        "Will I be able to beat the clock?",
        "Let's go – I need to make it on time!",
        "I need to get going, the clock is ticking!",
        "Tick-tock, tick-tock... This timer is pressuring me.",
        "Faster, faster, I need to step it up!",
        "It's me against the clock... let's do this thing!"
    };

    List<string> dreamRealization = new List<string> {
        "Am I really dreaming?",
        "This is the weirdest dream I've ever had.",
        "It's going to be funny to read my dream journal when I wake up.",
        "My feet will be killing me tomorrow from all this walking around."
    };

    List<string> loneliness = new List<string> {
        "I feel like I'm the only person in town right now... At least outside.",
        "Where is everybody?",
        "There aren't many folks out and about tonight...",
        "They took social distancing to a whole new level here – I'm all alone!",
        "Judging by the window lights, some folks are still awake, but they decided to stay home."
    };

    List<string> weather = new List<string> {
        "It's rather chilly outside tonight, isn't it?",
        "I should have worn some shoes.",
        "The weather in Rijeka is always so gloomy.",
        "Brrr! I should have worn a jacket.",
        "These pyjamas are not nearly warm enough.",
        "I haven't seen a sky quite like this before.",
        "Why is this place always so windy?",
        "I wish it was sunny... But let's face it, we're in Rijeka."
    };

    List<string> architecture = new List<string> {
        "I wonder when all these buildings were built...",
        "Some of these buildings look a bit different than I remember.",
        "I think some of these façades could use a bit of polishing.",
        "It feels like this part of town is centuries old.",
        "I love how a lot of the streets here are so narrow and crooked.",
        "It seems like most of these houses have lived through a lot.",
        "I like historic architecture, so this is pretty cool.",
        "I love how they squeeze a modern building every now and then in between the old ones.",
        "I listened to a lecture about Rijeka's Old Town once. They kept mentioning an architect called Igor Emili... I wonder if I'll see any of his buildings here."
    };

    List<string> deadEnd = new List<string> {
        "I'm not sure this is the right way...",
        "Where am I going?",
        "Hmm... I think I should turn around.",
        "It's pretty dark ahead, better turn back.",
        "I feel a bit lost.",
        "No, no, I don't think I'm headed right.",
        "This is definitely not the right direction."
    };
    #endregion
    #region SECTOR TEXTS
    List<string> voidTexts = new List<string>()
    {
        "This part of the Old Town seems kind of empty.",
        "Damn! This fog makes it really hard to navigate.",
        "When will this fog lift already?",
        "It looks like one could meet all kinds of people in this part of town... if you know what I mean.",
        "The general atmosphere here creeps me out a bit.",
        "The floor seems slippery... I should watch my step.",
        "I'm feeling an air of mystery here.",
        "There aren't that many new buildings in this part of town, are there?",
        "I keep hearing some sort of whispering... I hope it's just the wind."
    };

    List<string> zooTexts = new List<string>()
    {
        "What on earth is going on here? This place looks like a circus!",
        "Is it just me, or have they gone a bit crazy with the colors in this part of town?",
        "Everything here looks like it came straight out of the Professor Balthazar cartoon.",
        "It looks like confetti are flying through the air. Pretty festive!",
        "This part of town makes me feel funky!",
        "Am I tripping on something?",
        "Lucy in the Sky with Diamonds, eh?",
        "There are so many funny objects scattered around this place... Who knows what I'll bump into next.",
        "When I stop to think about it, it’s really cool Rijeka has a circular cathedral. Haven't seen many of those around."
    };
    #endregion
    enum events
    {
        timeRunningOut,     // 0
        dreamRealization,   // 1
        loneliness,         // 2
        weather,            // 3
        architecture,       // 4

        deadEnd,            // NE ULAZI U RANDOM

        voidTexts,          // 5 - salje se varijabla sector
        zooTexts            // 5 - salje se varijabla sector
    }

    System.Random rand;

    Dictionary<events, List<string>> texts = new Dictionary<events, List<string>>();

    
    // Start is called before the first frame update
    void Start()
    {
        texts.Add(events.timeRunningOut, timeRunningOut);
        texts.Add(events.dreamRealization, dreamRealization);
        texts.Add(events.loneliness, loneliness);
        texts.Add(events.weather, weather);
        texts.Add(events.architecture, architecture);
        texts.Add(events.deadEnd, deadEnd);

        texts.Add(events.voidTexts, voidTexts);
        texts.Add(events.zooTexts, zooTexts);

        rand = new System.Random();
        StartCoroutine(WriteRandomText());
        StartCoroutine(DeadzoningText());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WriteRandomText()
    {
        while (true)
        {
            yield return new WaitForSeconds(rand.Next(35, 51));
            int eventType = rand.Next(0, 6);
            if (eventType == 5) eventType = sector;
            if (!GameController.Master.questSolving && GameController.Master._GUI_notification_text.GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                GameController.Master.messages.Add(texts[(events)eventType][rand.Next(0, texts[(events)eventType].Count)]);
            }
        }
    }

    IEnumerator DeadzoningText()
    {
        while (true)
        {
            yield return new WaitUntil(() => WASDMovement.deadzoning == true);
            GameController.Master.messages.Add(texts[events.deadEnd][rand.Next(0, texts[events.deadEnd].Count)]);

            yield return new WaitUntil(() => WASDMovement.deadzoning == false);
        }
    }
}
