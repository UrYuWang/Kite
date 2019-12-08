using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class agentcontroller : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    public Vector3 cv;
    private Animator anima;
    public boundingstuff bs;
    public bool flag = false;
    public bool stop = false;
    // Start is called before the first frame update
    public void Start()
    {
        cv = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        //agent.speed = Random.Range(5.0f, 10.0f);
        agent.speed = 5;
        agent.SetDestination(destination);
        agent.autoRepath = true;
        agent.autoBraking = true;
        bs = new boundingstuff(agent.speed, 1, 1, transform, agent.nextPosition);
        anima = GetComponent<Animator>();
        flag = true;
        Continue();
    }

    public void FixedUpdate()
    {
        if (agent != null)
            if (stop)
                bs.Update(transform, transform.position);
            else if (agent.nextPosition != null)
                bs.Update(transform, agent.nextPosition);
            else
                bs.Update(transform, transform.position);
        if (Vector3.Distance(agent.destination, transform.position) <= float.Epsilon||stop)
        {
            agent.SetDestination(transform.position);
            anima.SetBool("walk", false);
        }
    }

    public void Stop()
    {
        bs.front = 1;
        anima.SetBool("walk", false);
        stop = true;
    }

    public void Continue()
    {
        bs.front = agent.speed;
        anima.SetBool("walk", true);
        stop = false;
    }
}
