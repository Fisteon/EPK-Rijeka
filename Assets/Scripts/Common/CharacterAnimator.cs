using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CharacterAnimator : MonoBehaviour {

    NavMeshAgent agent;
    public Animator animator;
    public bool UNUSED;

    float speed;
    float direction;
    PlayerController player;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponentInChildren<Animator>();
        player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        // TODO : CONTROLLER INPUT AXIS TO DIRECTION %
        if (SceneManager.GetActiveScene().name != "Sector_3")
        {
            direction = Input.GetAxis("JoystickHorizontal");
            animator.SetFloat("direction", direction, 0.01f, Time.deltaTime);
        }
        
        /*int forward = WASDMovement.walkingForward ? 1 : 0;
        int left    = WASDMovement.rotationLeft ? 1 : 0;
        int right   = WASDMovement.rotationRight ? 1 : 0;
        direction = (-1 + 0.5f * forward) * left + (1 - 0.5f * forward) * right;*/

        if (WASDMovement.walkingBack)
        {
            speed = -0.5f;
        }
        else
        {
            speed = agent.velocity.magnitude / agent.speed;
            if (speed > 0 && speed < 0.1f)
            {
                speed = 0;
            }
        }

        animator.SetFloat("speed", speed, 0.01f, Time.deltaTime);

        /*float speedPercent = agent.velocity.magnitude / agent.speed;
        if (speedPercent < 0.1)
        {
            speedPercent = 0;
        }
        animator.SetFloat("speedPercent", speedPercent, 0.01f, Time.deltaTime);

        if (WASDMovement.rotationLeft)
        {
            animator.SetBool("rotatingLeft", true);
            this.transform.Rotate(Vector3.up, Time.deltaTime * -75, Space.Self);
        }
        else if (WASDMovement.rotationRight)
        {
            animator.SetBool("rotatingRight", true);
            this.transform.Rotate(Vector3.up, Time.deltaTime * 75, Space.Self);
        }
        else if (WASDMovement.turnAround)
        {
            animator.SetBool("turnAround", true);
        }
        else
        {
            animator.SetBool("rotatingLeft", false);
            animator.SetBool("rotatingRight", false);
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            agent.SetDestination(this.transform.position);
            animator.SetTrigger("attack");
            player.Attack(PlayerController.AttackTypes.Normal);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            agent.SetDestination(this.transform.position);
            animator.SetTrigger("attackSweep");
            player.Attack(PlayerController.AttackTypes.Sweep);
        }*/
    }
}
