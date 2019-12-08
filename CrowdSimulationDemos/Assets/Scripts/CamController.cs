using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
    }
}
