using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    private NavMeshAgent agent;
    private Camera camera;
    private Plane ground;
    public Terrain terrain;

    public GameObject testpoint;
    public GameObject movementPoint;

    private Vector3 lastPosition;

    float planeHeight;
    int camRotationSpeed;

    // Use this for initialization
    void Start () {
        agent = this.GetComponent<NavMeshAgent>();
        camera = GameObject.Find("PlayerMainCamera").GetComponent<Camera>();
        //ground = new Plane(new Vector3(0f, 1f, 0f), new Vector3(0f, planeHeight, 0f));
        lastPosition = transform.position;
        //camRotationSpeed = 10;
    }
	
	// Update is called once per frame
	void Update () {
        if (!camera.enabled)
        {
            return;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(movementPoint.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, 1 << 9))
        {
            if (Vector3.Distance(lastPosition, hitInfo.point) > 0.1f)
            {
                agent.destination = hitInfo.point;
                lastPosition = hitInfo.point;
            }
            //agent.Move(hitInfo.point);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleSpeed();
        }
    }

    void ToggleSpeed()
    {
        if (agent.speed < 5f) agent.speed = 20f;
        else if (agent.speed > 5f) agent.speed = 3.5f;
        return;
    }
}
