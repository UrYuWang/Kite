using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class clcontroller : MonoBehaviour
{
    public Camera FPC;
    public Camera TPC;
    void Start()
    {
        FPC.enabled = false;
        TPC.transform.LookAt(GetComponent<Transform>().position + new Vector3(0,3,0));
        TPC.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            FPC.enabled = !FPC.enabled;
            TPC.enabled = !TPC.enabled;
        }
    }
}
