﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    GameManager gm;
    UIManager um;
    AudioManager am;
    CapsuleCollider2D capsuleCol;

    public int stage = 1;
    int chunks = 2;
    LayerMask bulletMask;

    private void Awake()
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        um = FindObjectOfType<UIManager>();
        am = FindObjectOfType<AudioManager>();

        Vector2 randDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
        GetComponent<Rigidbody2D>().AddForce(randDir * 600);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-9, 9));
        bulletMask = LayerMask.NameToLayer("Bullet");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.name + " : " + collision.transform.root.eulerAngles);
        if (collision.gameObject.layer == bulletMask) DestroyAndBreakup(collision.gameObject);
        
    }

    void DestroyAndBreakup(GameObject collided)
    {
        gm.AddScore(10);

        if (stage >= 3)
        {
            DestroyAsteroid();
        }
        else
        {
            for (int i = 0; i < chunks; i++)
            {
                Asteroid asteroidType = gm.asteroidTypes[Random.Range(0, gm.asteroidTypes.Length - 1)];
                float newRot = Random.Range(-180, 180);

                Asteroid newChunk = Instantiate(asteroidType, transform.position, Quaternion.Euler(0, 0, newRot));
                newChunk.stage = stage + 1;
                newChunk.transform.GetChild(0).localScale /= 2;
                newChunk.capsuleCol.size /= 2;

                float newRandRot = Random.Range(collided.transform.eulerAngles.z - 60, collided.transform.eulerAngles.z + 60);

                float xDir = Mathf.Sin(newRandRot * Mathf.Deg2Rad);
                float yDir = Mathf.Cos(newRandRot * Mathf.Deg2Rad);

                Vector2 newChunkDir =  new Vector2(xDir,yDir);

                GetComponent<Rigidbody2D>().AddTorque(Random.Range(-750 * newChunk.stage, 750 * newChunk.stage));
                newChunk.GetComponent<Rigidbody2D>().AddForce(newChunkDir * 500 * newChunk.stage);


            }

            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        Instantiate(gm.ExplosionAsteroid[stage-1], transform.position, Quaternion.identity);
        am.Play("Explosion");
        Destroy(gameObject);
    }
}
