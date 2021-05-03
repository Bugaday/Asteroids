using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO3D : MonoBehaviour
{
    public UFO ufoParent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Hit 3d UFO");
        ufoParent.DestroyUFO();
    }

}
