using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollToZoom : MonoBehaviour
{
    public GameObject lookAt;

    void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        // To Move it left/right
        transform.Rotate(Vector3.up, horiz * 2);

        // To Move it up/Down
        transform.Rotate(Vector3.right, vert * 2);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            RaycastHit hit;
            Ray ray = this.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Vector3 desiredPosition;

            if (Physics.Raycast(ray, out hit))
            {
                desiredPosition = hit.point;
            }
            else
            {
                desiredPosition = transform.position;
            }
            float distance = Vector3.Distance(desiredPosition, transform.position);
            Vector3 direction = Vector3.Normalize(desiredPosition - transform.position) * (distance * Input.GetAxis("Mouse ScrollWheel"));

            transform.position += direction;
        }
    }
}
