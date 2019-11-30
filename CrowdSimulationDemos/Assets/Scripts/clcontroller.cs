using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class clcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var camera = GetComponentInChildren<Camera>();
        camera.transform.position = GetComponent<Transform>().position + new Vector3(3, 7, 7);
        camera.transform.LookAt(GetComponent<Transform>().position + new Vector3(0, 3, 0));
        var light = GetComponentInChildren<Light>();
        light.transform.position = GetComponent<Transform>().position + new Vector3(3, 7, 7);
        light.transform.LookAt(GetComponent<Transform>().position+new Vector3(0,3,0));
    }
}
