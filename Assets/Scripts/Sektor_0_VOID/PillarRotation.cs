using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarRotation : MonoBehaviour {

    [Range(10,50)]
    public int rotationSpeed;

    public GameObject pillar;
    public GameObject pillarBase;

    public bool rotateBase;

    void Start () {
        rotateBase = false;
    }
	
	void Update () {
        if (rotateBase)
        {
            pillarBase.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
        }
        pillar.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
    }
}