using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class agentcontroller : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    private Animator anima;
    public boundingstuff bs;
    public bool flag = false;
    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(10.0f, 15.0f);
        agent.SetDestination(destination);
        agent.autoRepath = true;
        bs = new boundingstuff(1.1f, 0.9f, 1, GetComponent<Transform>(), agent.nextPosition);
        anima = GetComponent<Animator>();
        flag = true;
    }

    public void FixedUpdate()
    {
        Transform t = GetComponent<Transform>();
        if (agent!=null)
            bs.Update(t, agent.nextPosition);
        if (agent.speed <= float.Epsilon || (agent.destination-transform.position).magnitude <= 1)
        {
            if ((agent.destination - transform.position).magnitude <= 1)
                agent.SetDestination(GetComponent<Transform>().position);
            anima.SetBool("walk", false);
            //if ((agent.destination - transform.position).magnitude <= float.Epsilon)
            //    anima.SetBool("celebrate", true);
        }
        else
        {
            anima.SetBool("walk", true);
            //anima.SetBool("celebrate", false);
        }
    }

    public void Stop()
    {
        anima.SetBool("walk", false);
    }

    public void Continue()
    {
        anima.SetBool("walk", true);
    }
}
