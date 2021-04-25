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
        worldToView = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, cam.nearClipPlane));

        if (worldToView.x > 1)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(0, worldToView.y, cam.nearClipPlane));
        }
        else if (worldToView.x < 0)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(1, worldToView.y, cam.nearClipPlane));
        }

        if (worldToView.y > 1)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(worldToView.x, 0, cam.nearClipPlane));
        }
        else if (worldToView.y < 0)
        {
            transform.position = cam.ViewportToWorldPoint(new Vector3(worldToView.x, 1, cam.nearClipPlane));
        }
    }
}
