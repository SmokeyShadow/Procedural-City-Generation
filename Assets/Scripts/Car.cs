using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float timerDuration = 5f;
    Vector3 startPosition;
    Vector3 endPosition;
    bool toEnd = true;
    bool move = false;
    float timer = 0;
    // Start is called before the first frame update
    public void Init(Vector3 startPos, Vector3 endPos)
    {
        timerDuration = Random.Range(5, 10);
        startPosition = startPos;
        endPosition = endPos;
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
            return;
        timer += Time.deltaTime;
        if (toEnd)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timer / timerDuration);
            if (transform.position.Equals(endPosition))
            {
                toEnd = false;
                timer = 0;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, timer / timerDuration);
            if (transform.position.Equals(startPosition))
            {
                toEnd = true;
                timer = 0;
            }
        }
    }
}
