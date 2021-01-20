using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathToNavmeshProjection : MonoBehaviour
{
    public GameObject pathFollower;
    RaycastHit rayInfo;

    NavMeshAgent playerAgent;

    Quaternion playerRotation;
    Quaternion correctRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerAgent = this.GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(pathFollower.transform.position, Vector3.down, out rayInfo))
        {
            playerAgent.SetDestination(rayInfo.point);
            Vector3 rotation = pathFollower.transform.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            PlayerController._PlayerController.transform.rotation = targetRotation;

        }
    }
}
