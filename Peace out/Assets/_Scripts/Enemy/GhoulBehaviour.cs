using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhoulBehaviour : MonoBehaviour {
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
    public float maxTenTo14Timer = 45;
    public float maxOutOfSiteTimer = 3;
    public float maxInvestigatingSpeed = 2.1f;

    public float huntingSpeed = 4;
    public float walkingSpeed = 1.3f;
    public float investigatingSpeed = 2.1f;

    public AudioSource audioSource;
    public AudioClip jumpscare;
    public AudioClip chaseAudio;
    public AudioClip mainAudio;
    public AudioClip nearPlayerAudio;



    public State state;
    public enum State {
        idle,
        wandering,
        hunting,
        attacking,
        investigate
    }
    private float idleTimer;
    private float alertWindowTimer;
    private float outOfSiteTimer;
    public float TenTo14Timer = 0;
    private bool playingChaseAudio = false;

    private Animator animator;
    private List<Vector3> randomLocations = new List<Vector3> {
                                                              new Vector3(5.6f,11.2f,-9.2f), // upper left wing library
                                                              new Vector3(-17.2f,11.2f,-7.8f), // upper left wing storage
                                                              new Vector3(-34.9f,9.3f,-24.3f), // upper left big room
                                                              new Vector3(8.2f,9.3f,18), // upper left wing lifted door cell
                                                              new Vector3(-16,3.2f,25.7f), // upper left wing 4 barrels
                                                              new Vector3(-10,1.3f,3.2f), // kitchen
                                                              new Vector3(-26.3f,-3.7f,4.6f), //left wing jails (room 1)
                                                              new Vector3(-19.1f,-3.7f,13.1f), // left wing jails (room 2)
                                                              new Vector3(-14.4f,-3.7f,-7.3f), //left wing jails (room 3)
                                                              new Vector3(20.9f,1.7f,-42.9f), // right wing library
                                                              new Vector3(-7.9f,-0.5f,-33.1f), // right wing big barrel
                                                              new Vector3(5.4f,-2.6f,-53.1f), // right wing jail 1
                                                              new Vector3(29.4f,-3.7f,-33.2f), // right wing jail 2
                                                              new Vector3(9.8f,-3.7f,-25.2f), // right wing jail 3
                                                              new Vector3(-0.1f,2.1f,-24.3f), // main room exit
                                                              new Vector3(2.9f,1.2f,-1.2f), // main room near table
                                                              new Vector3(0.8f,6.4f,3.5f), //main room central balcony
    };
    
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
        if (player.GetComponent<ScrollCounter>().scrollCount >= player.GetComponent<ScrollCounter>().scrollToWin) {
            state = State.hunting;
            agent.speed = 4.8f;
            agent.SetDestination(player.position);
            audioManager();
            setAnimation(state);
        }else {
            if(state != State.hunting && player.GetComponent<ScrollCounter>().scrollCount >= 10 && player.GetComponent<ScrollCounter>().scrollCount <= 14) {
                if (TenTo14Timer <= 0) {
                    agent.SetDestination(player.position);
                    TenTo14Timer = maxTenTo14Timer * 2;
                }
                TenTo14Timer -= Time.deltaTime;
            } 
            else if (state != State.hunting && player.GetComponent<ScrollCounter>().scrollCount >= 15) {
                if (TenTo14Timer <= 0) {
                    agent.SetDestination(player.position);
                    TenTo14Timer = maxTenTo14Timer;
                }
                TenTo14Timer -= Time.deltaTime;
            }
            audioManager();
            setAnimation(state);
            if (state == State.idle) GhoulIdle();
            else if (state == State.wandering) {
                if (player.GetComponent<ScrollCounter>().scrollCount >= 10 && player.GetComponent<ScrollCounter>().scrollCount <= 14) {
                    agent.speed = walkingSpeed + 1;
                } else if(player.GetComponent<ScrollCounter>().scrollCount >= 15) {
                    agent.speed = walkingSpeed + 2;
                }   
                
                GhoulWander(); 
            }
            else if (state == State.hunting) { agent.speed = huntingSpeed; GhoulHunt(); }
            else if (state == State.investigate) { agent.speed = investigatingSpeed; GhoulInvestigate(); }
        }
    }

    private void audioManager() {
        if (state == State.hunting && !playingChaseAudio) {
            inGameMusic.GetComponent<AudioSource>().clip = chaseAudio;
            inGameMusic.GetComponent<AudioSource>().Play();
            playingChaseAudio = true;
            audioSource.Stop();
        }
        else if (state != State.hunting && Vector3.Distance(transform.position, new Vector3(player.position.x, player.position.y + 1.488f, player.position.z)) <= 2 * radius / 3){
            if (!audioSource.isPlaying) { 
                audioSource.clip = nearPlayerAudio;
                audioSource.Play();
            }
        }else if (state != State.hunting && playingChaseAudio) {
            inGameMusic.GetComponent<AudioSource>().clip = mainAudio;
            inGameMusic.GetComponent<AudioSource>().Play();
            playingChaseAudio = false;
        }
        else if(Vector3.Distance(transform.position, new Vector3(player.position.x, player.position.y + 1.488f, player.position.z)) > 2 * radius / 3) {
            audioSource.Stop();
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
            //if (alertWindowTimer == maxAlertWindowTimer) AudioSource.PlayClipAtPoint(jumpscare, player.position);
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
            if (alertWindowTimer == maxAlertWindowTimer) AudioSource.PlayClipAtPoint(jumpscare, player.position);

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

        if (rangeChecks.Length != 0 && Mathf.Abs(transform.position.y - player.position.y) + 1.488f <= 3) {
            Transform target = null;
            foreach (Collider collider in rangeChecks) {
                if(collider.gameObject.tag == "Player") {
                    target = collider.transform;
                }
            }
            if (target == null) return;

            Vector3 directionToTarget = (player.GetComponentInChildren<Camera>().transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, player.GetComponentInChildren<Camera>().transform.position);

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                if (distanceToTarget <= 1) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                }

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            // Ghoul Hearing
            else if (!canSeePlayer) {
                // Player running in investigating area 
                if (player.GetComponent<FirstPersonMovement>().IsRunning) {
                    if (distanceToTarget >= radius / 2) {
                        investigatingSpeed = maxInvestigatingSpeed;
                    }
                    else {
                        investigatingSpeed = maxInvestigatingSpeed + 1;
                    }

                    canHearPlayer = true;
                }
                // Player walking in investigating area 
                else if (!Input.GetKey(player.GetComponent<Crouch>().key) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))){
                    if (distanceToTarget <= radius / 2) {
                        investigatingSpeed = maxInvestigatingSpeed;
                        canHearPlayer = true;
                    }
                    else {
                        canHearPlayer = false;
                    }
                }
                // Player crouching in investigating area 
                else {
                    canHearPlayer = false;
                }
            }
        }
        else if (canSeePlayer) {
            canSeePlayer = false;
        }
        else if (canHearPlayer) {
            canHearPlayer = false;
        }
    }
}
