using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhoulMovement : MonoBehaviour {
    private bool debug = false;

    public Transform player;
    public NavMeshAgent agent;
    public GameObject inGameMusic; 

    //Field of View 
    public float radius;
    [Range(0, 360)] public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer = false;
    public bool canHearPlayer = false;
    public float maxIdleTimer = 3;
    public float maxAlertWindowTimer = 1.5f;
    public float maxOutOfSiteTimer = 3;
    public float huntingSpeed = 4;
    public float walkingSpeed = 1.3f;
    public float investigatingSpeed = 2.1f;

    public AudioClip jumpscare;
    public AudioClip chaseAudio;
    public AudioClip mainAudio;
    

    private enum State {
        idle,
        wandering,
        hunting,
        attacking,
        investigate
    }
    private State state;
    private float idleTimer;
    private float alertWindowTimer;
    private float outOfSiteTimer;
    private bool playingChaseAudio = false;

    private Animator animator;
    private List<Vector3> randomLocations = new List<Vector3> {
                                                              new Vector3(28, -4, -33),
                                                              new Vector3(5, -3, -49),
                                                              new Vector3(-35, 9, -24),
                                                              new Vector3(-16, 3, 25),
                                                              new Vector3(-19, -4, 1),
                                                              new Vector3(-8, 11, -9),
                                                              new Vector3(-14, 1, -14),
                                                              new Vector3(-12, 1, 0),
                                                              new Vector3(-10, -1, -33),
                                                              new Vector3(11, 1, -12),
                                                              new Vector3(-5,0,-14),
                                                              new Vector3(-3.4f,0,-0.3f),
                                                              new Vector3(11.7f,0,-15.6f)
    };

    private int counter;
    
    void Start() {
        animator = GetComponentInChildren<Animator>();

        state = State.idle;
        agent.SetDestination(randomLocations[Random.Range(0, randomLocations.Count)]);
        agent.isStopped = true;

        //Initialize timers
        idleTimer = maxIdleTimer;
        alertWindowTimer = maxAlertWindowTimer;
        outOfSiteTimer = maxOutOfSiteTimer;

        inGameMusic.GetComponent<AudioSource>().clip = mainAudio;
        inGameMusic.GetComponent<AudioSource>().Play();

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine() {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true) {
            yield return wait;
            FieldOfViewCheck();
        }
    }


    void Update() {
        if (debug) {
           //debug mode
        }else {
            audioManager();
            setAnimation(state);
            if (state == State.idle) GhoulIdle();
            else if (state == State.wandering) { GhoulWander(); agent.speed = walkingSpeed; }
            else if (state == State.hunting) { GhoulHunt(); agent.speed = huntingSpeed; }
            else if (state == State.investigate) { GhoulInvestigate(); agent.speed = investigatingSpeed; }
        }
        
    }

    private void audioManager() {
        if (state == State.hunting && !playingChaseAudio) {
            inGameMusic.GetComponent<AudioSource>().clip = chaseAudio;
            inGameMusic.GetComponent<AudioSource>().Play();
            playingChaseAudio = true;
        }
        else if (state != State.hunting && playingChaseAudio) {
            inGameMusic.GetComponent<AudioSource>().clip = mainAudio;
            inGameMusic.GetComponent<AudioSource>().Play();
            playingChaseAudio = false;
            print(counter++);
        }
    }

    private void GhoulIdle() {
        agent.isStopped = true;
        if (idleTimer <= 0) {
            state = State.wandering;
            agent.isStopped = false;
            alertWindowTimer = maxAlertWindowTimer;
            idleTimer = maxIdleTimer;
        }
        else if (canSeePlayer) {
            if (alertWindowTimer == maxAlertWindowTimer) AudioSource.PlayClipAtPoint(jumpscare, player.position);
            alertWindowTimer -= Time.deltaTime * radius / Vector3.Distance(transform.position, player.position);

            if(alertWindowTimer <= 0) {
                agent.isStopped = false;
                state = State.wandering;
                alertWindowTimer = maxAlertWindowTimer;
                idleTimer = maxIdleTimer;
            }
        }
        else if (canHearPlayer) {
            state = State.investigate;
            alertWindowTimer = maxAlertWindowTimer;
            idleTimer = maxIdleTimer;
        }
        else {
            idleTimer -= Time.deltaTime * radius / Vector3.Distance(transform.position, player.position) ;
        }
    }

    private void GhoulWander() {

        if (DestinationReached() || agent.destination == player.position) {
            if (agent.isStopped)agent.isStopped = false;
            agent.SetDestination(randomLocations[Random.Range(0, randomLocations.Count)]);
        }
        if (canSeePlayer) {
            //if (alertWindowTimer == maxAlertWindowTimer) AudioSource.PlayClipAtPoint(jumpscare, player.position);

            alertWindowTimer -= Time.deltaTime * radius / Vector3.Distance(transform.position, player.position);

            if (alertWindowTimer <= 0) {
                state = State.hunting;
                alertWindowTimer = maxAlertWindowTimer;
                idleTimer = maxIdleTimer;
            }
        }
        else if (canHearPlayer) {
            state = State.investigate;
            alertWindowTimer = maxAlertWindowTimer;
            idleTimer = maxIdleTimer;
        }
    }

    private void GhoulInvestigate() {
        if (canHearPlayer) {
            agent.SetDestination(player.position);
        }
        if (DestinationReached()) {
            state = State.wandering;
            return;
        }
        if (canSeePlayer) {
            state = State.hunting;
        }
    }

    private void GhoulHunt() { 
        agent.SetDestination(player.position);

        if (!canSeePlayer) outOfSiteTimer -= Time.deltaTime;
        else outOfSiteTimer = maxOutOfSiteTimer;

        if (outOfSiteTimer <= 0) {
            outOfSiteTimer = maxOutOfSiteTimer;
            state = State.wandering;
        }
    }

    private bool DestinationReached() {
        return agent.destination.x == transform.position.x && agent.destination.z == transform.position.z;
    }

    private void setAnimation(State state) {
        if (state == State.idle && (animator.GetBool("isRunning") || animator.GetBool("isWalking"))) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
        else if (state == State.wandering && (animator.GetBool("isRunning") || !animator.GetBool("isWalking"))) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        else if (state == State.hunting && (!animator.GetBool("isRunning") || animator.GetBool("isWalking"))) {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if(state == State.attacking) {
            animator.SetTrigger("attackPlayer");
        }
        else if (state == State.investigate && (animator.GetBool("isRunning") || !animator.GetBool("isWalking"))) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
    }


    private void FieldOfViewCheck() {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                if (distanceToTarget <= 1) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                }

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;

            // Ghoul Hearing
            //if (!canSeePlayer) {
            //    if (player.GetComponent<FirstPersonMovement>().IsRunning) {
            //        //if(distanceToTarget >= radius/2) {

            //        //}
            //        canHearPlayer = true;

            //    }
            //    else {
            //        canHearPlayer = false;
            //    }


            //}

        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
