using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour
{
    //4 ta poisition ha
    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;
    public GameObject pos4;
    Vector3 dir;
    float speed = 0.2f;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= pos2.transform.position.x && transform.position.y >= pos2.transform.position.y)
        {
            dir.x = pos2.transform.position.x - pos1.transform.position.x;
            dir.y = pos2.transform.position.y - pos1.transform.position.y;
            dir.z = pos2.transform.position.z - pos1.transform.position.z;
        }
        if (transform.position.x >= pos3.transform.position.x && transform.position.y >= pos3.transform.position.y)
        {
            dir.x = pos3.transform.position.x - pos2.transform.position.x;
            dir.y = pos3.transform.position.y - pos2.transform.position.y;
            dir.z = pos3.transform.position.z - pos2.transform.position.z;
        }
        if (transform.position.x >= pos4.transform.position.x && transform.position.y <= pos4.transform.position.y)
        {
            dir.x = pos4.transform.position.x - pos3.transform.position.x;
            dir.y = pos4.transform.position.y - pos3.transform.position.y;
            dir.z = pos4.transform.position.z - pos3.transform.position.z;
        }
        if (transform.position.x <= pos1.transform.position.x && transform.position.y <= pos1.transform.position.y)
        {
            dir.x = pos1.transform.position.x - pos4.transform.position.x;
            dir.y = pos1.transform.position.y - pos4.transform.position.y;
            dir.z = pos1.transform.position.z - pos4.transform.position.z;
        }
        transform.Translate(dir * speed * Time.deltaTime);
    }

}
