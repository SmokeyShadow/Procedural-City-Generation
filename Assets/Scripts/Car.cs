using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;
    bool toEnd = true;
    bool move = false;
    // Start is called before the first frame update
    public void Init(Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
            return;
        if (toEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, 0.2f);
            if (transform.position.Equals(endPosition))
            {
                toEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.2f);
            if (transform.position.Equals(startPosition))
            {
                toEnd = true;
            }
        }
    }
}
