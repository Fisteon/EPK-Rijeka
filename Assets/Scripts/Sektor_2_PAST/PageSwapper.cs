using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwapper : MonoBehaviour
{
    public List<GameObject> pages;
    public GameObject dreamJournal;

    int frontJournalPage = 0;
    bool playerTurnedOn;

    // Start is called before the first frame update
    void Start()
    {
        playerTurnedOn = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("DreamJournal") && !QuestPaperCollection.showingPaper)
        {
            dreamJournal.SetActive(!dreamJournal.activeInHierarchy);
            playerTurnedOn = !playerTurnedOn;
        }
        if (dreamJournal.activeInHierarchy)
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
            {
                GetFrontPage("left");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
            {
                GetFrontPage("right");
            }
        }
        Debug.Log(frontJournalPage);
    }

    void GetFrontPage(string direction)
    {
        pages[frontJournalPage].transform.localPosition = new Vector3(pages[frontJournalPage].transform.localPosition.x, -0.05f * (frontJournalPage + 1), pages[frontJournalPage].transform.localPosition.z);
        if (direction == "left")
        {
            do
            {
                frontJournalPage = (frontJournalPage - 1 + pages.Count) % pages.Count;
            } while (!pages[frontJournalPage].activeInHierarchy);
        }
        else
        {
            do
            {
                frontJournalPage = (frontJournalPage + 1) % pages.Count;
            } while (!pages[frontJournalPage].activeInHierarchy);
        }

        Vector3 localPosition = pages[frontJournalPage].transform.localPosition;
        localPosition.y = 0.1f;
        pages[frontJournalPage].transform.localPosition = localPosition;        
    }
}