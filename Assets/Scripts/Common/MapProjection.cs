using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProjection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit info;
        if (Physics.Raycast(this.transform.position, Vector3.down, out info, Mathf.Infinity, LayerMask.NameToLayer("MapProjection")))
        {
            Vector3 playerPositionOnMap = info.point;

        }
    }
}
