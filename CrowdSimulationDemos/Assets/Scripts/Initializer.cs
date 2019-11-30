using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public int number_of_agents;
    public int op;
    public GameObject master;
    public GameObject nonmaster;
    private int[] story_height = new int[3] {1,11,21};
    private GameObject[] agents;
    private Vector3[] forces;
    private static int i = 2;
    private List<Vector2Int> cpair;
    // Start is called before the first frame update
    void Start()
    {
        cpair = new List<Vector2Int>();
        forces = new Vector3[number_of_agents];
        for (int i=0; i<number_of_agents;i++)
        {
            forces[i] = Vector3.zero;
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
        
    }

    Vector3[] startandend()
    {
        Vector3[] result = new Vector3[2];
        float[] heights = new float[3] { 0.66667f, 10.66667f, 20.66667f };
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
        for (int i = 0; i < number_of_agents - 1; i++)
        {
            if (!agents[i].GetComponent<agentcontroller>().flag)
                return;
            for (int j = i + 1; j < number_of_agents; j++)
            {
                if (!agents[j].GetComponent<agentcontroller>().flag)
                    return;
                if (cpair.Contains(new Vector2Int(i, j)))
                    continue;
                temp1 = agents[i].GetComponent<agentcontroller>().bs.CDs(agents[j].GetComponent<agentcontroller>().bs.Steparound(agents[i].GetComponent<agentcontroller>().agent.nextPosition),op);
                temp2 = agents[j].GetComponent<agentcontroller>().bs.CDs(agents[i].GetComponent<agentcontroller>().bs.Steparound(agents[j].GetComponent<agentcontroller>().agent.nextPosition),op);
                if (temp1 != Vector3.zero || temp2 != Vector3.zero)
                {
                    float ratioi = agents[i].GetComponent<agentcontroller>().agent.speed / (agents[i].GetComponent<agentcontroller>().agent.speed + agents[j].GetComponent<agentcontroller>().agent.speed);
                    float ratioj = 1 - ratioi;
                    cpair.Add(new Vector2Int(i, j));
                    if (temp2 != Vector3.zero)
                        forces[i] += ratioi*temp2;
                    if (temp1 != Vector3.zero)
                        forces[j] += ratioj*temp1;
                }
                else
                    cpair.Remove(new Vector2Int(i, j));
            }
        }
        for (int i = 0; i < number_of_agents; i++)
        {
            if (forces[i] != Vector3.zero)
            {
                if (agents[i].GetComponent<agentcontroller>().agent.isStopped)
                    agents[i].GetComponent<agentcontroller>().Stop();
                agents[i].GetComponent<agentcontroller>().agent.Move(forces[i] * Time.fixedDeltaTime);
                agents[i].GetComponent<agentcontroller>().Continue();
                //agents[i].GetComponent<agentcontroller>().agent.ResetPath();
                //agents[i].GetComponent<agentcontroller>().agent.velocity = agents[i].GetComponent<agentcontroller>().agent.velocity.magnitude * forces[i].normalized;
            }
            else
            {
                agents[i].GetComponent<agentcontroller>().Continue();
            }
            forces[i] = Vector3.zero;
        }
    }
}
