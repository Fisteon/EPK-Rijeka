using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DinosaurSound : MonoBehaviour
{
    public AudioClip omNomNom;
    public bool eatClockSound;

    AudioSource audio;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        audio = this.GetComponent<AudioSource>();
        animator = this.GetComponent<Animator>();

        StartCoroutine(WaitForAnimationFinish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForAnimationFinish()
    {
        yield return new WaitUntil(() => eatClockSound);

        audio.clip = omNomNom;
        audio.Play();
    }
}
