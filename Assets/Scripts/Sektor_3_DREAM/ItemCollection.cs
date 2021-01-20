using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : Scene
{
    public int addedSeconds;

    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.Find("CollectingSound");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerInteract()
    {
        IncreaseTime(addedSeconds);
        RemoveFromInteractables();
        PlaySound();
        Destroy(this.gameObject); 
    }

    void PlaySound()
    {
        sound.GetComponent<AudioSource>().Play();
    }
}
