using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    GameManager gm;
    UIManager um;
    AudioManager am;

    public int stage = 1;
    int chunks = 2;
    LayerMask bulletMask;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        um = FindObjectOfType<UIManager>();
        am = FindObjectOfType<AudioManager>();

        Vector2 randDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
        GetComponent<Rigidbody2D>().AddForce(randDir * 20);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-3, 3));
        bulletMask = LayerMask.NameToLayer("Bullet");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == bulletMask) DestroyAndBreakup(collision.gameObject);
    }

    void DestroyAndBreakup(GameObject collided)
    {
        gm.AddScore(10);

        if (stage >= 3)
        {
            am.Play("Explosion");
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < chunks; i++)
            {
                Asteroid asteroidType = gm.asteroidTypes[Random.Range(0, gm.asteroidTypes.Length - 1)];
                float newRot = Random.Range(-180, 180);

                Asteroid newChunk = Instantiate(asteroidType, transform.position, Quaternion.Euler(0, 0, newRot));
                newChunk.stage = stage + 1;
                newChunk.transform.localScale /= 3;

                Vector2 randDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
                GetComponent<Rigidbody2D>().AddForce(randDir * 20);
                GetComponent<Rigidbody2D>().AddTorque(Random.Range(-75 * newChunk.stage, 75 * newChunk.stage));
                newChunk.GetComponent<Rigidbody2D>().AddForce(randDir * 50 * newChunk.stage);
            }
            am.Play("Explosion");
            Destroy(gameObject);
        }
    }
}
