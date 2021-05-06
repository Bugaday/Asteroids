using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 moveDir;
    public Bullet bullet;
    public GameObject DestroyedRoot;

    GameManager gm;
    AudioManager am;

    Camera cam;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        cam = Camera.main;
        am = FindObjectOfType<AudioManager>();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1) Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1);
        Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        //newBullet.dir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * Random.Range(-180, 180)),Mathf.Cos(Mathf.Deg2Rad * Random.Range(-180, 180))).normalized;
        StartCoroutine(Shoot());
    }

    public void DestroyUFO()
    {
        am.Play("UFOExplosion");
        Instantiate(gm.ExplosionShip, transform.position, Quaternion.identity);

        Instantiate(DestroyedRoot, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
