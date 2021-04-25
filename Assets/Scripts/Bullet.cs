using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    [HideInInspector]
    public Vector2 dir;
    public float TimeToDie = 3;
    float timeCreated = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
        timeCreated += Time.deltaTime;

        if(timeCreated > TimeToDie)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
