using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    [HideInInspector]
    //public Vector2 dir;
    public float TimeToDie = 3;
    float timeCreated = 0;

    // Update is called once per frame
    void Update()
    {
        float angle = transform.eulerAngles.z;
        Vector3 dirLine = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0).normalized;
        print(transform.up);
        print("Dir: " + dirLine);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 10, Color.red);

        transform.Translate(Vector3.up * speed * Time.deltaTime);
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
