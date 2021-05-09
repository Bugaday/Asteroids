using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    Vector3 worldToView;
    Camera cam;
    public bool canWrapAround = true;
    public float wrapTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        wrapTimer -= Time.deltaTime;
        


        if (wrapTimer <= 0) canWrapAround = true;

        worldToView = cam.WorldToViewportPoint(transform.position);
        //Vector3 newPosition = transform.position;
        if (canWrapAround)
        {
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

                wrapTimer = 1;
                canWrapAround = false;
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

                wrapTimer = 1;
                canWrapAround = false;
            }
        }

        transform.position = new Vector3(cam.ViewportToWorldPoint(worldToView).x, cam.ViewportToWorldPoint(worldToView).y,0);
    }
}
