using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBlockade : Scene
{
    // Start is called before the first frame update
    void Start()
    {
        texts.Add("Interact", "This is blocking my way... I need to find a way around it. ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        PushMessageToMaster(texts["Interact"]);
    }
}
