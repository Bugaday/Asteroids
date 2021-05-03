using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameManager gm;
    Camera cam;
    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.CurrentShip)
        {
            Vector3 shipDir = gm.CurrentShip.rb.velocity.normalized;
            Vector2 shipDir2D = gm.CurrentShip.rb.velocity.normalized;

            Vector3 shipPosScreen = cam.WorldToViewportPoint(gm.CurrentShip.transform.position);
            Vector3 targetPos = new Vector3(shipDir2D.x * 10, shipDir2D.y * 10, transform.position.z);

            //Vector3 camPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, 1f);
        }
    }
}
