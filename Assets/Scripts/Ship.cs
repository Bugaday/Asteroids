using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Bullet bullet;

    Rigidbody2D rb;
    AudioSource aSource;
    float moveInputX = 0;
    float moveInputY = 0;
    Transform engine;

    GameManager gm;

    LayerMask hitLayer;

    bool damageable = true;
    public float VelClamp = 4f;
    public float RotSpeed = 100f;
    public float Force = 10f;

    float timeToNextShot = 0;
    [SerializeField]
    float timeBetweenShots = 0.05f;
    bool canShoot = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();
        engine = transform.GetChild(0);
        hitLayer = LayerMask.NameToLayer("CanHitShip");
        gm = FindObjectOfType<GameManager>();

        StartCoroutine(StartInvincible());
    }

    private void Update()
    {
        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");

        if(moveInputX != 0)
        {
            transform.Rotate(0, 0, -moveInputX * RotSpeed * Time.deltaTime);
        }

        if(timeToNextShot <= 0)
        {
            canShoot = true;
        }
        else
        {
            timeToNextShot -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1"))
        {
            if (canShoot)
            {
                Bullet bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(transform.up));
                bulletInstance.dir = transform.up;
                aSource.Play();

                canShoot = false;
                timeToNextShot = timeBetweenShots;
            }
        }

        engine.localScale = new Vector3(engine.localScale.x,moveInputY * 1.5f,engine.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moveInputY != 0)
        {
            rb.AddForce(transform.up * Force);
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, VelClamp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == hitLayer)
        {
            if (damageable)
            {
                gm.ShipDestroyed();
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator StartInvincible()
    {
        damageable = false;
        yield return new WaitForSeconds(2);
        damageable = true;
    }
}
