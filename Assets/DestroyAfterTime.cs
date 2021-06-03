using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float TimeToDestroy;

    float timeAtCreation;
    float timeSinceCreation;

    private void Start()
    {
        timeAtCreation = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceCreation = Time.timeSinceLevelLoad - timeAtCreation;

        if (timeSinceCreation > TimeToDestroy) Destroy(gameObject);
    }
}
