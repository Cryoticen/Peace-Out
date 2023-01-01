using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animation animation;

    void Start()
    {
        animation = GetComponentInChildren<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (agent.velocity.magnitude == 0) {
        //    animation.Blend("Run");
        //}
        agent.SetDestination(Vector3.zero);
    }
}
