using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    #region  sources
    public Transform target;
    float smoothspeed = 0.125f;
    Vector3 offset;
    Vector3 velocity = Vector3.zero;
    Vector3 destpos;
    #endregion
    void Start()
    {
        offset = new Vector3(transform.position.x - target.position.x, 0, -10);
    }
    void LateUpdate()
    {

        destpos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, destpos, ref velocity, 0.5f);

    }
}
