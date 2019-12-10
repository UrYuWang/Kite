using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

public class CrowdGenerator : MonoBehaviour
{
    public Camera cam;
    public int number_of_agents;
    public int op;
    public GameObject nonmaster;
    private GameObject[] agents;
    private Vector3[] forces;
    //private List<Vector2Int> cpair;
    //private Stopwatch sw;
    //private bool stop;
    // Start is called before the first frame update

    void Start()
    {
        //sw = new Stopwatch();
        //stop = false;
        //cpair = new List<Vector2Int>();
        forces = new Vector3[number_of_agents];
        for (int i = 0; i < number_of_agents; i++)
        {
            forces[i] = Vector3.zero;
        }
        agents = new GameObject[number_of_agents];
        Vector3[] spots = new Vector3[2];
        agentcontroller ascript;
        for (int i = 0; i < number_of_agents; i++)
        {
            spots = startandend(i);
            agents[i] = Instantiate(nonmaster, spots[0], Quaternion.identity);
            ascript = agents[i].GetComponent<agentcontroller>();
            ascript.destination = spots[1];
        }
        //sw.Start();
    }

    private Vector3[] startandend(int i)
    {
        int radius = number_of_agents > 20 ? number_of_agents : 20;
        float angle = 2 * Mathf.PI / number_of_agents;
        //print(angle);
        Vector3[] result = new Vector3[2];
        result[0] = new Vector3(radius * Mathf.Sin(angle * i), 0.66667f, radius * Mathf.Cos(angle * i));
        result[1] = new Vector3(-result[0].x, result[0].y, -result[0].z);
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
                //if (skipper[i, j] >= 1)
                //{
                //    skipper[i, j] -= 1;
                //    continue;
                //}
                time = (aci.transform.position - acj.transform.position).magnitude / (aci.bs.front + acj.bs.front);
                if (time > 1)
                {
                    //skipper[i, j] = (int)time;
                    continue;
                }
                temp1 = aci.bs.CDs(acj.bs.Steparound(aci.agent.nextPosition), op);
                temp2 = acj.bs.CDs(aci.bs.Steparound(acj.agent.nextPosition), op);
                if (temp1 != Vector3.zero || temp2 != Vector3.zero)
                {
                    float ratioi;
                    float ratioj;
                    if (aci.stop && acj.stop)
                    {
                        ratioi = 0;
                        ratioj = 0;
                    }
                    else if (aci.stop)
                    {
                        ratioi = 0;
                        ratioj = 1;
                    }
                    else if (acj.stop)
                    {
                        ratioi = 1;
                        ratioj = 0;
                    }
                    else
                    {
                        ratioi = aci.agent.velocity.magnitude / (aci.agent.velocity.magnitude + acj.agent.velocity.magnitude);
                        ratioj = 1 - ratioi;
                    }
                    // cpair.Add(new Vector2Int(i, j));
                    if (temp2 != Vector3.zero && ratioj != 0)
                        forces[i] += ratioi * (temp2 - temp1) / 2;
                    else if (temp2 != Vector3.zero)
                        forces[i] += temp2;
                    if (temp1 != Vector3.zero && ratioi != 0)
                        forces[j] += ratioj * (temp1 - temp2) / 2;
                    else if (temp1 != Vector3.zero)
                        forces[j] += temp1;
                }
                // else
                // cpair.Remove(new Vector2Int(i, j));
            }
        }
        //NavMeshHit hit;
        for (int i = 0; i < number_of_agents; i++)
        {
            var aci = agents[i].GetComponent<agentcontroller>();
            if (aci.stop)
                continue;
            else if (Vector3.Angle(forces[i], aci.agent.velocity) < 90)
                continue;
            else if (forces[i] != Vector3.zero)
            {
                //if (NavMesh.SamplePosition(agents[i].transform.position + ((Vector3.Cross(forces[i], agents[i].transform.up).normalized - 0.3f * agents[i].transform.forward).normalized * aci.agent.speed / 10), out hit, 1.0f, NavMesh.AllAreas))
                //{
                //print("Move it.");
                aci.agent.velocity = aci.agent.velocity * 0.9f + (Vector3.Cross(forces[i], agents[i].transform.up).normalized - 0.1f * agents[i].transform.forward).normalized * aci.agent.speed / 20;
                //}
            }
            forces[i] = Vector3.zero;
        }
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
        //    sw.Stop();
        //    //print(sw.ElapsedTicks.ToString());
        //    Time.timeScale = 0.0f;
        //}
    }
}