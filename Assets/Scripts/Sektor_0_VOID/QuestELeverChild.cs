using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestELeverChild : Scene
{
    public GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        main.GetComponent<QuestELever>().OnPlayerLeverPull();
    }
}
