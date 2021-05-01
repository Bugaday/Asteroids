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

        //Vector3 newPosition = transform.position;

        if (worldToView.x > 1.0f || worldToView.x < 0f)
        {
            if (worldToView.x > 1.0f)
            {
                worldToView.x = 0f;
            }
            else if (worldToView.x < 0f)
            {
                worldToView.x = 1f;
            }
        }

        if (worldToView.y > 1.0f || worldToView.y < 0f)
        {
            if (worldToView.y > 1.0f)
            {
                worldToView.y = 0f;
            }
            else if (worldToView.y < 0f)
            {
                worldToView.y = 1f;
            }
        }

        transform.position = cam.ViewportToWorldPoint(worldToView);
    }
}
