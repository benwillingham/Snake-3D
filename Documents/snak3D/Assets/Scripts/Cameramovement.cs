using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    public GameObject head;

    private Vector3 offset;
    private Vector3 rotate;

    void Start()
    {
        offset = transform.position - head.transform.position;
        rotate = new Vector3(35, -135, 0);
    }

    void LateUpdate()
    {
        transform.position = head.transform.position + offset;
        //transform.eulerAngles = 
        //make camera rotate based on head position
    }
}
