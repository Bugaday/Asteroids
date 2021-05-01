using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Bullet bullet;
    public GameObject SpritesRoot;
    public Transform Engine;
    public Transform EngineSide;
    public GameObject DestroyedRoot;

    public Rigidbody2D rb;
    AudioSource aSource;
    Animation anim;
    float moveInputX = 0;
    float moveInputY = 0;
    float mouseX = 0;
    float mouseY = 0;
    float rotateInput = 0;

    GameManager gm;
    AudioManager am;

    LayerMask hitLayer;

    bool damageable = true;
    public float VelClamp = 4f;
    public float RotSpeed = 100f;
    public float Force = 10f;

    float timeToNextShot = 0;
    [SerializeField]
    float timeBetweenShots = 0.05f;
    bool canShoot = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
    }

    private void Start()
    {
        hitLayer = LayerMask.NameToLayer("CanHitShip");
        gm = FindObjectOfType<GameManager>();
        am = FindObjectOfType<AudioManager>();

        StartCoroutine(StartInvincible());
    }

    private void Update()
    {
        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Rotate");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //Mouse Aiming
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, RotSpeed * Time.deltaTime);

        //Rotate Ship - unused
        if (rotateInput != 0)
        {
            //transform.Rotate(0, 0, -rotateInput * RotSpeed * Time.deltaTime);
        }

        //Fire Rate
        if(timeToNextShot <= 0)
        {
            canShoot = true;
        }
        else
        {
            timeToNextShot -= Time.deltaTime;
        }

        //Shoot
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

        Engine.localScale = new Vector3(Engine.localScale.x,moveInputY * 1.5f,Engine.localScale.z);
        EngineSide.localScale = new Vector3(Engine.localScale.x, moveInputX, Engine.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Apply forces from input
        if(moveInputY != 0)
        {
            rb.AddForce(moveInputY * Vector2.up * Force);
        }
        if (moveInputX != 0)
        {
            rb.AddForce(moveInputX * Vector2.right * Force);
        }

        //Max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, VelClamp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == hitLayer)
        {
            if (damageable)
            {
                DestroyShip();
            }
        }
    }

    void DestroyShip()
    {
        am.Play("ShipExplosion");

        GameObject destroyedShip = Instantiate(DestroyedRoot, transform.position, transform.rotation);
        foreach (Transform piece in destroyedShip.transform)
        {
            Vector2 directionOfPiece = (piece.position - transform.position).normalized;

            Rigidbody2D pieceRb = piece.GetComponent<Rigidbody2D>();

            pieceRb.AddForce(directionOfPiece * 100);
            pieceRb.AddTorque(Random.Range(-200, 0));
        }

        gm.ShipDestroyed();
        Destroy(gameObject);
    }

    public IEnumerator StartInvincible()
    {
        damageable = false;
        //anim.Play();
        yield return new WaitForSeconds(2);
        //anim.Stop();
        //SpritesRoot.SetActive(true);
        damageable = true;
    }


}
