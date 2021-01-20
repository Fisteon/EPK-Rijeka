using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperActivation : Scene
{
    public EducationalObjects paperTrigger;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForTrigger());
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    IEnumerator WaitForTrigger()
    {
        yield return new WaitUntil(() => paperTrigger.visited == true);
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
