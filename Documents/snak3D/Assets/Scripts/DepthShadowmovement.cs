using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthShadowmovement: MonoBehaviour
{
    public GameObject head;

    void LateUpdate()
    {
        transform.position = head.transform.position;
    }
}
