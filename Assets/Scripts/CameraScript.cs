using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameManager gm;
    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.CurrentShip)
        {
            Vector3 shipDir = gm.CurrentShip.rb.velocity.normalized;
            Vector2 shipDir2D = gm.CurrentShip.rb.velocity.normalized;

            Vector3 targetPos = new Vector3(shipDir2D.x * 10, shipDir2D.y * 10, transform.position.z);

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, 1f);
        }
    }
}
