using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

  
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
