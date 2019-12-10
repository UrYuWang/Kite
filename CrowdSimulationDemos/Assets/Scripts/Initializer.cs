using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

public class Initializer : MonoBehaviour
{
    public int number_of_agents;
    public int op;
    public GameObject master;
    public GameObject nonmaster;
    private float[] heights = new float[3] { 0.66667f, 10.66667f, 20.66667f };
    private GameObject[] agents;
    //private List<Vector3>[] forces;
    private Vector3[] forces;
    private static int i = 2;
    // private List<Vector2Int> cpair;
    //private Stopwatch sw;
    //private bool stop;
    private int[,] skipper;
    //private int ran;
    private int flipper;
    // Start is called before the first frame update
    void Start()
    {
        //sw = new Stopwatch();
        skipper = new int[number_of_agents, number_of_agents];
        //stop = false;
        flipper = 0;
        // cpair = new List<Vector2Int>();
        //forces = new List<Vector3>[number_of_agents];
        forces = new Vector3[number_of_agents];
        for (int i = 0; i < number_of_agents; i++)
        {
            forces[i] = Vector3.zero;
            //forces[i] = new List<Vector3>();
        }

        agents = new GameObject[number_of_agents];
        Vector3[] spots = new Vector3[2];
        agentcontroller ascript;
        for (int i = 0; i < number_of_agents; i++)
        {
            spots = startandend();
            if (i == 0)
                agents[i] = Instantiate(master, spots[0], Quaternion.identity);
            else
                agents[i] = Instantiate(nonmaster, spots[0], Quaternion.identity);
            ascript = agents[i].GetComponent<agentcontroller>();
            ascript.destination = spots[1];
        }
        //ran = -1;
        //sw.Start();
    }

    Vector3[] startandend()
    {
        Vector3[] result = new Vector3[2];
        //print(i);
        switch (i)
        {
            case 0:
                {
                    result[0] = new Vector3(Random.Range(5.0f, 45.0f), heights[0], Random.Range(5.0f, 45.0f));
                    result[1] = new Vector3(Random.Range(5.0f, 45.0f), heights[2], Random.Range(5.0f, 45.0f));
                    break;
                }
            case 1:
                {
                    result[0] = new Vector3(Random.Range(-45.0f, -5.0f), heights[1], Random.Range(-45.0f, -5.0f));
                    //if(Random.Range(-1.0f,1.0f)>0)
                    //result[1]= new Vector3(Random.Range(5.0f, 45.0f), heights[2], Random.Range(5.0f, 45.0f));
                    //else
                    result[1] = new Vector3(Random.Range(5.0f, 45.0f), heights[0], Random.Range(5.0f, 45.0f));
                    break;
                }
            case 2:
                {
                    result[1] = new Vector3(Random.Range(5.0f, 45.0f), heights[0], Random.Range(5.0f, 45.0f));
                    result[0] = new Vector3(Random.Range(5.0f, 45.0f), heights[2], Random.Range(5.0f, 45.0f));
                    break;
                }

        }
        i += 1;
        i %= 3;
        //print(result[0]);
        //print(result[1]);
        return result;
    }

    private void FixedUpdate()
    {
        Vector3 temp1;
        Vector3 temp2;
        float time;
        for (int i = 0; i < number_of_agents - 1; i++)
        {
            var aci = agents[i].GetComponent<agentcontroller>();
            if (!aci.flag)
                return;
            for (int j = i + 1; j < number_of_agents; j++)
            {
                var acj = agents[j].GetComponent<agentcontroller>();
                if (!acj.flag)
                    return;
                if (skipper[i, j] >= 1)
                {
                    skipper[i, j] -= 1;
                    continue;
                }
                time = (aci.transform.position - acj.transform.position).magnitude / (aci.bs.front + acj.bs.front);
                skipper[i, j] = (int)time;
                if (time > 1)
                {
                    continue;
                }
                temp1 = aci.bs.CDs(acj.bs.Steparound(aci.transform.position), op);
                //if (Vector3.Angle(temp1, acj.transform.forward) < 90)
                //temp1 = Vector3.zero;
                temp2 = acj.bs.CDs(aci.bs.Steparound(acj.transform.position), op);
                //if (Vector3.Angle(temp2, aci.transform.forward) < 90)
                    //temp2 = Vector3.zero;
                if (temp1 != Vector3.zero || temp2 != Vector3.zero)
                {
                    float ratioi;
                    float ratioj;
                    if (aci.stop&&acj.stop)
                    {
                        ratioi=0;
                        ratioj=0;
                    }
                    else if (aci.stop)
                    {
                        ratioi=0;
                        ratioj=1;
                    }
                    else if (acj.stop)
                    {
                        ratioi=1;
                        ratioj=0;
                    }
                    else
                    {
                        ratioi = aci.agent.speed / (aci.agent.speed + acj.agent.speed);
                        ratioj = 1 - ratioi;
                    }
                    // cpair.Add(new Vector2Int(i, j));
                    if (temp2 != Vector3.zero && ratioj != 0)
                        forces[i] += ratioi * (temp2-temp1)/2;
                    else if (temp2 != Vector3.zero)
                        forces[i] += temp2;
                    if (temp1 != Vector3.zero && ratioi != 0)
                        forces[j] += ratioj * (temp1-temp2)/2;
                    else if (temp1 != Vector3.zero)
                        forces[j] += temp1;
                }
                // else
                    // cpair.Remove(new Vector2Int(i, j));
            }
        }
        //NavMeshHit hit;
        Vector3 cross;
        for (int i = 0; i < number_of_agents; i++)
        {
            //var maxi = Sum(forces[i]);
            var aci = agents[i].GetComponent<agentcontroller>();
            if (aci.stop)
                continue;
            else if (Vector3.Angle(forces[i], aci.agent.velocity) < 30)
                continue;
            else if (forces[i] != Vector3.zero)
            {
                //aci.agent.velocity = aci.agent.velocity * 0.9f + (Vector3.Cross(forces[i], agents[i].transform.up).normalized - 0.1f * agents[i].transform.forward).normalized * aci.agent.speed / 20;
                cross = Vector3.Cross(forces[i], agents[i].transform.up);
                cross = cross.magnitude > 1 ? cross.normalized : cross;
                if (i%2==flipper)
                    aci.agent.velocity = aci.agent.velocity * 0.9f + (cross + forces[i] / 2 - 0.1f * aci.transform.forward).normalized * aci.agent.speed / 10;
                if (i == 0)
                {
                    print("desired v:" + aci.agent.desiredVelocity);
                }
            }
            forces[i] = Vector3.zero;
            //forces[i].Clear();
        }
        //ran *= -1;
        flipper = 1 - flipper;
        //stop = true;
        //foreach (GameObject agent in agents)
        //{
        //    if (!agent.GetComponent<agentcontroller>().stop)
        //    {
        //        stop = false;
        //        break;
        //    }
        //}
        //if (stop)
        //{
            //sw.Stop();
            //print(sw.ElapsedTicks.ToString());
            //Time.timeScale = 0.0f;
        //}
    }

    //private Vector3 Max(List<Vector3> forces)
    //{
    //    if (forces.Count == 0)
    //        return Vector3.zero;
    //    else if (forces.Count == 1)
    //        return forces[0];
    //    Vector3 max = forces[0];
    //    float maxm = max.magnitude;
    //    for (int i = 1; i < forces.Count; i++)
    //    {
    //        if (forces[i].magnitude > maxm)
    //        {
    //            max = forces[i];
    //            maxm = max.magnitude;
    //        }
    //    }
    //    return max;
    //}

    //private Vector3 Sum(List<Vector3> forces)
    //{
    //    if (forces.Count == 0)
    //        return Vector3.zero;
    //    else if (forces.Count == 1)
    //        return forces[0];
    //    Vector3 sum = forces[0];
    //    for (int i = 1; i < forces.Count; i++)
    //    {
    //        sum += forces[i];
    //    }
    //    return sum;
    //}
}
