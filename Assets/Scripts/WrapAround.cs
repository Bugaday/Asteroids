using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    Vector3 worldToView;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        worldToView = cam.WorldToViewportPoint(transform.position);

        Vector3 newPosition = transform.position;

        if (worldToView.x > 1 || worldToView.x < 0)
        {
            newPosition.x = -newPosition.x;

            //float newX = cam.ViewportToWorldPoint(Vector3.zero).x;
            //transform.position = new Vector3(newX,transform.position.y, 0);
        }

        if (worldToView.y > 1 || worldToView.y < 0)
        {
            newPosition.y = -newPosition.y;
            //float newY = cam.ViewportToWorldPoint(Vector3.zero).y;
            //transform.position = new Vector3(transform.position.x, newY, 0);
        }
        else if (worldToView.y < 0)
        {
            //float newY = cam.ViewportToWorldPoint(Vector3.one).y;
            //transform.position = new Vector3(transform.position.x, newY, 0);
        }

        transform.position = newPosition;
    }
}
