using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollower : MonoBehaviour
{
    Vector3 rainOffset;
    Vector3 lastPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        rainOffset = -PlayerController._PlayerController.transform.position + this.transform.position;
        lastPlayerPosition = PlayerController._PlayerController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPlayerPosition, PlayerController._PlayerController.transform.position) > 10f)
        {
            this.transform.position = PlayerController._PlayerController.transform.position + rainOffset;
            lastPlayerPosition = PlayerController._PlayerController.transform.position;
        }
    }
}
