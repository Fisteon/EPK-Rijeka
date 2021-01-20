using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestYBalcony : Scene
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForVisit());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitForVisit()
    {
        yield return new WaitUntil(() => this.GetComponent<EducationalObjects>().writtenInJournal == true);
        yield return new WaitForSeconds(6f);
        finished = true;
    }
}
