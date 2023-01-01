using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    private string state;
    private Transform head;
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
                                                              new Vector3(11, 1, -12)
    };

    void Start() {
        head = transform.GetChild(0);
        agent.SetDestination(transform.position);
        state = "wandering";
    }


    void FixedUpdate()
    {
        //if (state == "wandering") Wonder();
        //if (state == "hunting") Hunt();

        
    }

    //private void Wonder() {
    //    if (agent.destination == transform.position || agent.destination == player.position) {
    //        agent.SetDestination(randomLocations[Random.Range(0, randomLocations.Count)]);
    //        print("wondering");
    //    }


    //    Collider playerCollider = player.GetComponent<CapsuleCollider>();
    //    RaycastHit hit;
    //    if(Physics.Raycast(transform.position, (player.position - transform.position), out hit, 5)) {
    //        if (hit.transform == player) {
    //            state = "hunting";
    //            print("found YOU");
    //        }
    //    }
    //}

    //private void Hunt() {
    //    agent.SetDestination(player.position);
    //    head.LookAt(player.GetChild(0).position);// When Chasing player, look at player
    //}
}
