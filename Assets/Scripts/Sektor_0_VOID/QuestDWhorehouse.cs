#region OLD
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDWhorehouse : Scene
{
    public TextMeshProUGUI PlayerChoiceA;
    public TextMeshProUGUI PlayerChoiceB;
    public TextMeshProUGUI PlayerChoiceC;

    public TextMeshProUGUI HostResponse;

    public TextMeshProUGUI InstructionText;

    public GameObject selection;
    public GameObject stalagmites;

    public GameObject sceneTrigger;
    public Animator womanInWindow;

    public List<GameObject> options;
    public List<string> wrongAnswers;
    public List<string> finishAnswers;
    public List<string> repeatQuestion;

    private string previousChoices;
    private int selectedOption;
    private bool playerInteracting;
    private bool returningToStart;
    private bool allOptionsExplored;
    private string finishedBranch;

    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Host_0", "Good evening honey.\nHow can I be of service?");
        texts.Add("Host_1", "Oh yes, we get a lot of shield-wielding folks in here. Shields and swords, big ones. I've seen hundreds of them.\n\n" +
            "Let me see... the engraving on your shield represents a two-headed eagle, the crest of the Habsburg family. "+
            "I believe the owner is inside. I'm going to hold on to it for him.\nThank you.");
        texts.Add("Host_2", "Happy to oblige, dear.\nMy name is Karolina. I was named after a local heroine. "+
            "In 1813 she saved the city by charming the British admiral Fremantle out of a vicious attack. "+
            "It seems charm is my forte, as well.\n\nHere's something for you, to keep you safe tonight.");
        texts.Add("Player_0", "Um... Hi.\nI found this old shield outside, and I was wondering if one of your... "+
            "customers... left it outside.\nCare to take a look?");
        texts.Add("Player_1", "No problem.\nIt was nice talking to you, it's a bit lonely outside tonight. What's your name?");
        texts.Add("Finish", "I don't know what to do with this cross, but it might be useful later.");



        StartCoroutine(WaitForSceneStart());
        wrongAnswers.Add("AB_Host");
        wrongAnswers.Add("AC_Host");
        wrongAnswers.Add("AAB_Host");
        wrongAnswers.Add("AAC_Host");
        wrongAnswers.Add("AAAA_Host");
        wrongAnswers.Add("AAAC_Host");
        wrongAnswers.Add("BXB_Host");
        wrongAnswers.Add("CXC_Host");
        wrongAnswers.Add("CXXB_Host");
        wrongAnswers.Add("CXXC_Host");

        finishAnswers.Add("AAAB_Host");
        finishAnswers.Add("BXXC_Host");

        repeatQuestion.Add("BXXA_Host");
        repeatQuestion.Add("BXXB_Host");

        playerInteracting = false;
        returningToStart = false;
        allOptionsExplored = false;
        selectedOption = 0;
        previousChoices = "";
        finishedBranch = "";
        #region INTERACTION TEXTS
        texts.Add("Start", "Those stalagmites are blocking the way!");
        texts.Add("Finish", "You received a cross from the Lady.");
        texts.Add("Welcome", "\"Welcome to this fine establishment, weary dreamer! How can we be of service tonight?\"");
        texts.Add("SecondBranch", "\"Is there anything else we can do for you?\"");

        texts.Add("Greeting_Player_A", "I would like to quench my thirst.");
        texts.Add("Greeting_Player_B", "I would like to see your prettiest selection, please.");

        texts.Add("Greeting_Host_A", "\"Look no further, dear. We have everything to quench your thirst.\"");
        texts.Add("Greeting_Host_B", "\"You are correct. We service fine people of Fiume, and have for a century now.\"");

        // Concat this to the greeting_host
        texts.Add("Menu", "Have a few moments to yourself, as you inspect our list of rates. See anything you like?");

        // Menu
        string menu = "<INSERT MENU HERE>";

        texts.Add("Player_A", "I would like to see your finest selection please.");
        texts.Add("Player_B", "I need a drink. Or two.");
        //texts.Add("Player_C", "My child, how can I help you?");

        // OPTION A
        texts.Add("A_Host", "\"But of course! .. That is, the payment is up front. 35 liras for civilians.\"");
        texts.Add("A_Player_A", "I'm not a civilian. I am an important officer of the Empire!");
        texts.Add("A_Player_B", "It seems I left my wallet in my other... attire.");
        texts.Add("A_Player_C", "Can I just watch?");

        texts.Add("AA_Host", "\"And I am not new at  Which house do you serve, or better, what is your rank.. officer?\"");
        texts.Add("AB_Host", "\"And I left my patience in my other corset.\""); // ???
        texts.Add("AC_Host", "\"I hear Trsat is amazing for sightseeing this time of year.\""); // ???

        texts.Add("AA_Player_A", "I Come from the Capitol. You will address me as Rittmeister.");
        texts.Add("AA_Player_B", "Silly girl. Don't you know better than to provoke a Capitano delle Navi?");
        texts.Add("AA_Player_C", "I just enlisted as a Dragoon. Please?");

        texts.Add("AAA_Host", "\"That is a high position. What is it again? Navy?\"");
        texts.Add("AAB_Host", "You used a military term. Just from the Venetian navy. " +
                              "There was a time when they were welcome here, but not anymore. " +
                              "Try working more on your charm."); // ???
        texts.Add("AAC_Host", "\"And you think you can just ride in for free?\""); // ???

        texts.Add("AAA_Player_A", "Sea salt in my veins, all the way!");
        texts.Add("AAA_Player_B", "Cavalry. I always feel like prancing around.");
        texts.Add("AAA_Player_C", "Infantry, ma'am.");

        texts.Add("AAAA_Host", "\"We have all the salt we need, mister.\"\n" +
                               "You tried to cheat your way in. You should learn the proper military ranks to try this con."); // ???
        texts.Add("AAAB_Host", "\"If prancing around is what you need, we can be of service.\""); // !!!
        texts.Add("AAAC_Host", "\"You're wearing the wrong colors for it.\"\n" +
                               "She is not amused by your scheme. She shows you the door."); // ???

        // OPTION B
        texts.Add("B_Host", "\"What can I get you?\"");

        texts.Add("B_Player_A", "Schnapps, and stat!");
        texts.Add("B_Player_B", "Beer, and one for you.");
        texts.Add("B_Player_C", "Do you have some herbal tea?");

        texts.Add("BA_Host", "\"I see you know your spirits.\"\n" +
                             "Walking to you, she trips over your shield.\n" +
                             "\"You should know better than to pull these tricks just to look under my skirt!\"");
        texts.Add("BB_Host", "\"Still on duty, dear, but you can have mine.\"\n" +
                             "She trips over your things on the floor. The beer was saved.\n" +
                             "\"You should know better than to pull these tricks just to look under my skirt!\"");
        texts.Add("BC_Host", "\"I guess I can pick something from the garden...\"\n After a long while, she comes back. " +
                             "Her glumness darkens when she falls over your things.\n" +
                             "\"You should know better than to pull these tricks just to look under my skirt!\"");

        texts.Add("BX_Player_A", "I'm sorry. I just found this outside.");
        texts.Add("BX_Player_B", "Walk better then, stupid!");
        texts.Add("BX_Player_C", "Many pardons, miss.");

        texts.Add("BXA_Host", "She puts the drink next to you.\n\"It looks like it has some engravings on it.\"");
        texts.Add("BXB_Host", "The look she gives you! Chills! She waves to a big guy in the back. Within two seconds you are outside.\n" +
                              "\"Learn some manners if you ever want to come back.\""); // ???
        texts.Add("BXC_Host", "\"Aye, take better care of your things!\"\nShe puts the drink down, and picks up your shield.\n" +
                              "\"It looks like someone chiseled the engravings off.");

        texts.Add("BXX_Player_A", "It's a lion, I think");
        texts.Add("BXX_Player_B", "Am I crazy or does it look like bees?");
        texts.Add("BXX_Player_C", "A bird.. an eagle, maybe?"); 

        texts.Add("BXXA_Host", "\"Lions left, thank the gods. Or just the Austrians and the French\". She looks at you suspiciously. " +
                               "\"They did rule for a long time. I was 17 then. " +
                               "\"The Doge surrendered to Napoleon, and they came to us. "+
                               "I hope you are not a Lion. It would be a shame to hurt such a pretty face.\" " +
                               "Her hand touches your face, softly. A small slap yanks you from your fantasies. " +
                               "\"Drink your drink, Horse lord!\"");
        texts.Add("BXXB_Host", "\"If you're wearing bees OR stripes, I will go Karolina on you!\" " +
                               "You promptly apologize. She sits next to you. " +
                               "\"See, there was a war. Three armies surrounding one city, and a woman, tired of petty man squabbles. " +
                               "Everywhere you look, the French. Then the British. When they started destroying the city, " +
                               "this goddess in black walked straight to admiral Freemantle and said \"stop\".\n" +
                               "And he did.\"");
        texts.Add("BXXC_Host", "\"Yes, it looks like it.\" In morning hours of August 26th 1813 Nugant's Hussars took Fiume back " +
                               "from the French and the British. Without Karolina's help, the city would be in ruins. " +
                               "She stopped the British Navy's attack, just by going to the admiral Freemantle " +
                               "and saying \"stop\". Then the Hussars came and did the rest. This is a Habsburg shield. \n\n" +
                               "If you're a collector, I do have something for you. Interested?\"\n" +
                               "She gives you a beautiful cross for the rest of your liras."); // !!!
        /*
        // OPTION C
        texts.Add("C_Host", "Ah, there is one of you every week, père. Just go in.");

        texts.Add("C_Player_A", "No one would know...");
        texts.Add("C_Player_B", "I can talk with the girls. Set them straight.");
        texts.Add("C_Player_C", "There are couple of heathens at the bar.");

        texts.Add("CA_Host", "You walk in. There is so much sin and dirt in one place. An atrocious thought comes to you – 'no one would know..\"" +
                             "You sit at the bar, ready to quietly ask for the safe room to explore. You mean, save this place from itself." +
                             "\n'You still sure you want to help us?\"");
        texts.Add("CB_Host", "There are so many girls, just sitting on rocks. Gambling, drinks, and laughter. And mucis. And a funny looking " +
                             "mascot dancing with the soldiers. You are not ready to just approach the girls so you stand close to the bar." +
                             "\n'What can I get you stranger?\"");
        texts.Add("CC_Host", "You know the girls are not to blame. And with a broken system you know you destroy it from the head. Holding " +
                             "your head high to show you are not here to make friends, you boldly walk through the room towards them." +
                             "\n'Hey! Join us! We have a tie here, and we need a tiebreaker!\"");

        texts.Add("CX_Player_A", "I just want a glass of milk, please. My body is a temple.");
        texts.Add("CX_Player_B", "I'll just rest here for a bit. If you don't mind.");
        texts.Add("CX_Player_C", "I will save you all! Do not fear the light!");

        texts.Add("CXA_Host", "The barmen firmly tells you there is no milk. You did know that. You laugh at yourself and ask for mead." +
                              "Water would not be safe here. You know better. A man sits next to you." +
                               "\n'Hey! Hey! Hey! You new here? I see, I see. You up for a friendly game?\"");
        texts.Add("CXB_Host", "A shady looking man glances towards you for a couple of seconds. You try to ignore him while you figure your plan out." +
                              "He comes to you and starts with bad small talk. You try to ignore him." +
                               "\n'Hey! Hey! Hey! You new here? I see, I see. You up for a friendly game?\"");
        texts.Add("CXC_Host", "That was not a good conversation starter. He walks around the bar, couple of guests joining him as well. " +
                              "Suddenly, you are surrounded by tall, angry-looking men. They ask you to repeat that. You do it. " +
                              "Next second, you are getting dragged by your collar out on the street. 'Do not sell your ideologies here! " +
                              "They are for the real world. We want the fantasy of not having one. Good bye!\""); // ???

        texts.Add("CXX_Player_A", "I'm not here to gamble, my friend.");
        texts.Add("CXX_Player_B", "I need to talk to the manager.");
        texts.Add("CXX_Player_C", "You smell like cabbage");

        texts.Add("CXXA_Host", "He looks around to check if anyone is listening. As you follow his glance, he opens up his jacket. " +
                                "\"If not to gamble, how about a trade?' He shows you a beautiful old cross. Couple of minutes of bartering " +
                                "and you have it in your hands. You ask where it is from, and the only answer you get from him is 'Celts'. " +
                                "Maybe you didn't bring peace today to the wretched souls, but you still scored.");
        texts.Add("CXXB_Host", "As you say this, barmen covertly makes a signal to the man in the corner playing cards. " +
                                "He approaches you in full force. 'What seems to be the problem?', his voice echoes softly. " +
                                "As you try to explain that this is a place there is no room for in the new millenium, " +
                                "he swiftly calls for his men. Before you know it, they take you in the room behind the bar. " +
                                "Couple of hours later, you exit. This will be swollen tommorow - a small sigh exits your bloody mouth."); // ???
        texts.Add("CXXC_Host", "His fake smile turns into rage. You can see his rotten teeth grinding. This is not the place you want to be in. " +
                                "He is pouring insults, as you stand up. Today is not the day, you say to yourself while exiting."); // ???

#endregion
    }

    // Update is called once per frame
    void Update()
    {

        if (!playerInteracting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(true);
        }
        if (Input.GetKeyDown(KeyCode.Return) && !returningToStart)
        {
            StartCoroutine(MakeChoice());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !allOptionsExplored)
        {
            ExitWhorehouse(allOptionsExplored);
            GameController.Master.questSolving = false;
        }
    }

    IEnumerator WaitForSceneStart()
    {
        yield return new WaitUntil(() => sceneTrigger.GetComponent<SceneStarter>().playerEntered);
        PushMessageToMaster(texts["Start"]);
        WriteTextToDreamJournalMaster(texts["Start"]);
    }

    public override void OnPlayerInteract()
    {
        playerInteracting = true;
        StartCoroutine(SetupGreetings());

        GameController.Master._GUI_interaction_text.SetActive(false);
        GameController.Master.questSolving = true;
        SceneCamera.gameObject.SetActive(true);
        PlayerController._PlayerController.enabled = false;
        PlayerController._PlayerController.camera.enabled = false;
        womanInWindow.SetBool("playerInteracted", true);
    }

    void ExitWhorehouse(bool f)
    {
        playerInteracting = false;
        GameController.Master._GUI_interaction_text.SetActive(true);
        GameController.Master.questSolving = false;
        PlayerController._PlayerController.enabled = true;
        PlayerController._PlayerController.camera.enabled = true;
        selection.SetActive(false);
        previousChoices = "";
        selectedOption = 0;
        if (f) gameObject.SetActive(false);
    }

    // true = up | false = down
    void ChangeSelection(bool direction)
    {
        if (direction && selectedOption > 0)
        {
            if (options[selectedOption - 1].activeInHierarchy) selectedOption--;
        }
        else if (!direction && selectedOption < 2)
        {
            if (selectedOption == 0 && !options[1].activeInHierarchy) return;
            if (selectedOption == 1 && !options[2].activeInHierarchy) return;

            selectedOption++;
        }
        selection.transform.position = options[selectedOption].transform.position;
    }

    IEnumerator SetupGreetings()
    {
        playerInteracting = false;
        PlayerChoiceA.gameObject.SetActive(false);
        PlayerChoiceB.gameObject.SetActive(false);
        PlayerChoiceC.gameObject.SetActive(false);
        HostResponse.text = texts["Welcome"];
        StartCoroutine(FadeText(HostResponse));
        yield return new WaitForSeconds(2);

        if (finishedBranch != "")
        {
            if (finishedBranch == "A")
            {
                PlayerChoiceB.gameObject.SetActive(true);
                StartCoroutine(FadeText(PlayerChoiceB));
                selection.SetActive(true);
                selection.transform.position = options[1].transform.position;

                yield return new WaitForEndOfFrame();

                PlayerChoiceB.text = texts["Player_B"];
            }
            else
            {
                PlayerChoiceA.gameObject.SetActive(true);
                StartCoroutine(FadeText(PlayerChoiceA));
                selection.SetActive(true);
                selection.transform.position = options[0].transform.position;

                yield return new WaitForEndOfFrame();

                PlayerChoiceA.text = texts["Player_A"];
            }
        }
        else
        {
            PlayerChoiceA.gameObject.SetActive(true);
            PlayerChoiceB.gameObject.SetActive(true);
            StartCoroutine(FadeText(PlayerChoiceA));
            StartCoroutine(FadeText(PlayerChoiceB));
            selection.SetActive(true);
            selection.transform.position = options[0].transform.position;

            yield return new WaitForEndOfFrame();

            PlayerChoiceA.text = texts["Player_A"];
            PlayerChoiceB.text = texts["Player_B"];
        }      
        PlayerChoiceC.gameObject.SetActive(false);
        playerInteracting = true;
    }

    IEnumerator MakeChoice()
    {
        previousChoices += options[selectedOption].name; 
        HostResponse.text = texts[previousChoices + "_Host"];
        StartCoroutine(FadeText(HostResponse));
        RemoveNonselectedOptions(options[selectedOption].name);
        playerInteracting = false;
        yield return new WaitForSeconds(1f);
        playerInteracting = true;
        if (repeatQuestion.Contains(previousChoices + "_Host"))
        {
            string lastChoice = previousChoices.Substring(previousChoices.Length - 1, 1);
            previousChoices = previousChoices.Remove(previousChoices.Length - 1, 1);
            texts.Remove(previousChoices + "_Player_" + lastChoice);
            PopulatePlayerOptions();
            yield break;
        }
        if (finishAnswers.Contains(previousChoices + "_Host"))
        {
            if (finishedBranch == "")
            {
                finishedBranch += previousChoices[0];
                StartCoroutine(ReturnToStart());
                yield break;
            }
            else
            {
                StartCoroutine(EndWhorehouse());
                yield break;
            }
        }
        if (wrongAnswers.Contains(previousChoices + "_Host"))
        {
            StartCoroutine(KickedOut());
            yield break;
        }
        if (PopulatePlayerOptions() == 0)
        {
            StartCoroutine(KickedOut());
        }
        Debug.Log(options[selectedOption].name);
    }

    IEnumerator FadeText(TextMeshProUGUI textBox)
    {
        Color temp;
        temp = textBox.color;
        temp.a = 0;
        textBox.color = temp;
        while (temp.a < 1)
        {
            temp.a += 0.1f;
            textBox.color = temp;
            yield return new WaitForSeconds(0.03f);
        }
    }

    void RemoveNonselectedOptions(string x)
    {
        if (x != "A")
        {
            PlayerChoiceA.gameObject.SetActive(false);
        }
        if (x != "B")
        {
            PlayerChoiceB.gameObject.SetActive(false);
        }
        if (x != "C")
        {
            PlayerChoiceC.gameObject.SetActive(false);
        }
    }

    IEnumerator ReturnToStart()
    {
        returningToStart = true;
        InstructionText.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        InstructionText.gameObject.SetActive(false);
        returningToStart = false;
        previousChoices = "";
        HostResponse.text = texts["SecondBranch"];

        PlayerChoiceA.gameObject.SetActive(false);
        PlayerChoiceB.gameObject.SetActive(false);
        PlayerChoiceC.gameObject.SetActive(false);

        if (finishedBranch == "A")
        {
            PlayerChoiceB.text = texts["Player_B"];
            PlayerChoiceB.gameObject.SetActive(true);
            selection.transform.position = options[1].transform.position;
            selectedOption = 1;
        }
        else
        {
            PlayerChoiceA.text = texts["Player_A"];
            PlayerChoiceA.gameObject.SetActive(true);
            selection.transform.position = options[0].transform.position;
            selectedOption = 0;
        }
    }

    IEnumerator MenuAfterGreeting()
    {
        // show menu
        yield return new WaitForSeconds(1);

        PlayerChoiceA.text = texts["Player_A"];
        PlayerChoiceB.text = texts["Player_B"];
    }

    IEnumerator KickedOut()
    {
        selection.SetActive(false);
        PlayerChoiceA.gameObject.SetActive(false);
        PlayerChoiceB.gameObject.SetActive(false);
        PlayerChoiceC.gameObject.SetActive(false);

        playerInteracting = false;
        yield return new WaitForSeconds(4f);
        ExitWhorehouse(allOptionsExplored);
    }

    IEnumerator EndWhorehouse()
    {
        InstructionText.text = "[Escape] Finish conversation";
        InstructionText.gameObject.SetActive(true);
        allOptionsExplored = true;
        selection.SetActive(false);
        PlayerChoiceA.gameObject.SetActive(false);
        PlayerChoiceB.gameObject.SetActive(false);
        PlayerChoiceC.gameObject.SetActive(false);

        playerInteracting = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
        PlayerController._PlayerController.interactables.Remove(gameObject);
        PushMessageToMaster(texts["Finish"]);
        DreamJournalEntry();
        Destroy(stalagmites);
        finished = true;

        ExitWhorehouse(allOptionsExplored);
    }

    int PopulatePlayerOptions()
    {
        int counter = 0;
        if (CheckKey(previousChoices, "_Player_A"))
        {
            counter++;
            PlayerChoiceA.gameObject.SetActive(true);
            options[0].SetActive(true);
            PlayerChoiceA.text = texts[previousChoices + "_Player_A"];
            StartCoroutine(FadeText(PlayerChoiceA));
            StartCoroutine(FadeText(options[0].GetComponent<TextMeshProUGUI>()));
        }
        else
        {
            PlayerChoiceA.gameObject.SetActive(false);
        }

        if (CheckKey(previousChoices, "_Player_B"))
        {
            counter++;
            PlayerChoiceB.gameObject.SetActive(true);
            options[1].SetActive(true);
            PlayerChoiceB.text = texts[previousChoices + "_Player_B"];
            StartCoroutine(FadeText(PlayerChoiceB));
            StartCoroutine(FadeText(options[1].GetComponent<TextMeshProUGUI>()));
        }
        else
        {
            PlayerChoiceB.gameObject.SetActive(false);
        }

        if (CheckKey(previousChoices, "_Player_C"))
        {
            counter++;
            PlayerChoiceC.gameObject.SetActive(true);
            options[2].SetActive(true);
            PlayerChoiceC.text = texts[previousChoices + "_Player_C"];
            StartCoroutine(FadeText(PlayerChoiceC));
            StartCoroutine(FadeText(options[2].GetComponent<TextMeshProUGUI>()));
        }
        else
        {
            PlayerChoiceC.gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            selectedOption = i;
            if (options[selectedOption].activeInHierarchy)
            {
                selection.transform.position = options[selectedOption].transform.position;
                break;
            }
        }
        return counter;
    }

    bool CheckKey(string previous, string suffix)
    {
        string wildcardCheck = previous.Remove(previous.Length - 1) + "X";
        if (texts.ContainsKey(wildcardCheck + suffix))
        {
            previousChoices = wildcardCheck;
            return true;
        }
        return texts.ContainsKey(previous + suffix);
    }

    void DreamJournalEntry()
    {
        string journalEntry = "You successfully posed as Rittmeister, a highly ranked cavalry officer." +
                              "The nice lady told you how on August 26th 1813, brave Karolina approached admiral Freemantle " +
                              "and demanded he stops the attack on Fiume. She also sold you a beautiful old cross. " +
                              "The engraving on your shield represents a two headed eagle, the crest of the Habsburg family.";
        WriteTextToDreamJournalMaster(journalEntry);
    }
}
*/
#endregion
// Decompiled with JetBrains decompiler
// Type: QuestDWhorehouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C57C325D-5678-4D04-9B68-2A70771730E3
// Assembly location: C:\My stuff\UnityBuilds\Rijeka\Rijeka_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDWhorehouse : Scene
{
    public TextMeshProUGUI t_Host;
    public TextMeshProUGUI t_Player;
    public GameObject stalagmites;
    public GameObject particles;
    public GameObject cross;
    public Animator womanInWindow;
    private bool playerInteracting;
    private bool updatingText;
    private int playerResponse;
    private int hostResponse;

    private void Start()
    {
        texts.Add("Host_0", "Good evening honey.\nHow can I be of service?");
        texts.Add("Host_1", "Oh yes, we get a lot of shield-wielding folks in here. Shields and swords, big ones. I've seen hundreds of them.\n\nLet me see... the engraving on your shield represents a two-headed eagle, the crest of the Habsburg family. I believe the owner is inside. I'm going to hold on to it for him.\nThank you.");
        texts.Add("Host_2", "Happy to oblige, dear.\nMy name is Karolina. I was named after a local heroine. In 1813 she saved the city by charming the British admiral Fremantle out of a vicious attack. It seems charm is my forte, as well.\n\nHere's something for you, to keep you safe tonight.");
        texts.Add("Player_0", "Um... Hi.\nI found this old shield outside, and I was wondering if one of your... customers... left it outside.\nCare to take a look?");
        texts.Add("Player_1", "No problem.\nIt was nice talking to you, it's a bit lonely outside tonight. What's your name?");
        texts.Add("Finish", "I met a lady who told me about a local heroine called Karolina. In 1813 she saved the city by charming the British admiral Fremantle out of a vicious attack.\nI don't know what to do with this cross, but it might be useful later.");
    }

    private void Update()
    {
        if (playerResponse == 2)
        {
            finished = true;
            cross.SetActive(true);
            FinishKeybinds();
            //t_Player.gameObject.SetActive(false);
        }
        if (!playerInteracting || updatingText || !Input.GetButtonDown("Interact"))
            return;
        if (finished)
            Finish();
        else
            Respond();
    }

    public override void OnPlayerInteract()
    {
        Destroy(particles);
        ToggleSound(true);
        t_Host.gameObject.SetActive(true);
        t_Player.gameObject.SetActive(true);
        playerInteracting = true;
        playerResponse = 0;
        hostResponse = 0;
        PlayerController._PlayerController.TogglePlayerOnOff(false);
        SceneCamera.gameObject.SetActive(true);
        PlayerController._PlayerController.enabled = false;
        womanInWindow.SetBool("playerInteracted", true);
        StartCoroutine(ShowTextAfterAnimation());
        Keybinds();
    }

    private void Respond()
    {
        StartCoroutine(UpdateTexts());
    }

    private void Finish()
    {
        WriteTextToDreamJournalMaster(texts["Finish"]);
        PlayerController._PlayerController.TogglePlayerOnOff(true);
        PlayerController._PlayerController.enabled = true;
        RemoveFromInteractables();
        ToggleKeybinds(false);
        ToggleSound(false);
        Destroy(t_Host.transform.parent.gameObject);
        Destroy(gameObject);
        Destroy(stalagmites);
    }

    private IEnumerator UpdateTexts()
    {
        updatingText = true;
        t_Player.text = texts["Player_" + playerResponse.ToString()];
        t_Player.alpha = 0.0f;
        float elapsed = 0.0f;
        while ((double)elapsed < 1.5)
        {
            t_Player.alpha = Mathf.Lerp(0.0f, 1f, elapsed / 1.5f);
            elapsed += Time.deltaTime;
            yield return (object)new WaitForFixedUpdate();
        }
        t_Player.alpha = 1f;
        playerResponse++;

        hostResponse++;
        t_Host.text = texts["Host_" + hostResponse.ToString()];
        t_Host.alpha = 0.0f;
        elapsed = 0.0f;
        while ((double)elapsed < 1.5)
        {
            t_Host.alpha = Mathf.Lerp(0.0f, 1f, elapsed / 1.5f);
            elapsed += Time.deltaTime;
            yield return (object)new WaitForFixedUpdate();
        }
        t_Host.alpha = 1f;
        updatingText = false;
    }

    private IEnumerator ShowTextAfterAnimation()
    {
        //QuestDWhorehouse questDwhorehouse1 = this;
        updatingText = true;
        yield return (object)new WaitForSeconds(5f);
        t_Host.text = texts["Host_" + hostResponse.ToString()];
        t_Host.alpha = 0.0f;
        float elapsed = 0.0f;
        while ((double)elapsed < 1.5)
        {
            t_Host.alpha = Mathf.Lerp(0.0f, 1f, elapsed / 1.5f);
            elapsed += Time.deltaTime;
            yield return (object)new WaitForFixedUpdate();
        }
        ToggleKeybinds(true);
        t_Host.gameObject.SetActive(true);
        t_Player.gameObject.SetActive(true);
        updatingText = false;
    }

    public override void Keybinds()
    {
        GameController.Master.SetupKeybinds(new List<Tuple<string, string>>()
    {
      new Tuple<string, string>("X", "Respond")
    });
    }

    private void FinishKeybinds()
    {
        GameController.Master.SetupKeybinds(new List<Tuple<string, string>>()
    {
      new Tuple<string, string>("X", "Take the cross and leave")
    });
    }
}
