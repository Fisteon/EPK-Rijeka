using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController.Master.messages.Add("It’s raining... why am I not surprised? Classic Rijeka.\n" +
                "Maybe I could ask that guy at the window for an umbrella, although he looks a bit weird – let me see...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
