using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const float deadZoneRotation = 2.0f;
    public Bullet bullet;
    public GameObject SpritesRoot;

    //Engines
    public Transform[] Engines;
    public Transform[] ReverseEngines;
    public Transform[] LeftLateralEngines;
    public Transform[] RightLateralEngines;
    public Transform[] LeftRotationEngines;
    public Transform[] RightRotationEngines;

    public Transform FirePoint;
    public GameObject Forcefield;
    public GameObject Rendering;
    public GameObject DestroyedRoot;
    public GameObject Reticle;

    //Components
    [HideInInspector]
    public Rigidbody2D rb;
    AudioSource aSource;
    public Animation anim;

    //Input
    float moveInputX = 0;
    float moveInputY = 0;
    float mouseX = 0;
    float mouseY = 0;
    float mousePosX;
    float mousePosY;

    //References
    GameManager gm;
    AudioManager am;
    Camera cam;

    LayerMask hitLayer;

    Vector2 shipForceDir;
    public bool damageable = true;
    public float VelClamp = 4f;
    public float RotSpeed = 100f;
    public float Force = 40f;
    float angleAimingDifference;

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
        cam = Camera.main;
    }

    private void Update()
    {
        //Catch lateral input
        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");

        //Calculate force direction
        shipForceDir = new Vector2(moveInputX, moveInputY).normalized;

        //Ship audio
        if (shipForceDir != Vector2.zero)
        {
            if (!am.CheckIsPlaying("Thrusters")) am.PlayOneShot("Thrusters");
        }
        else
        {
            if (am.CheckIsPlaying("Thrusters")) am.Stop("Thrusters");
        }

        //Catch mouse input
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        //Catch mouse position
        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;

        //Calculate angle difference between mouse and ship's up direction
        Vector3 shipScreenPoint = cam.WorldToScreenPoint(transform.position);
        Vector3 mouseWorldPoint = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, shipScreenPoint.z));
        angleAimingDifference = Vector2.SignedAngle(transform.up, mouseWorldPoint - transform.position);

        //Mouse Aiming
        Vector2 dir = (Input.mousePosition - cam.WorldToScreenPoint(transform.position)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        //Apply deadzone of 1 degree
        if (angleAimingDifference > deadZoneRotation || angleAimingDifference < -deadZoneRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, RotSpeed * Time.deltaTime);
        }

        if (angleAimingDifference > deadZoneRotation || angleAimingDifference < -deadZoneRotation)
        {
            if(angleAimingDifference > 1)
            {
                foreach (var item in RightRotationEngines)
                {
                    item.localScale = new Vector3(item.localScale.x, item.localScale.y + 1, item.localScale.z);
                    item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y, 0.6f, 3f), item.localScale.z);
                }

            }
            else if(angleAimingDifference < -1)
            {
                foreach (var item in LeftRotationEngines)
                {
                    item.localScale = new Vector3(item.localScale.x, item.localScale.y + 1, item.localScale.z);
                    item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y, 0.6f, 3f), item.localScale.z);
                }

            }
        }
        else
        {
            foreach (var item in RightRotationEngines)
            {
                item.localScale = new Vector3(item.localScale.x, item.localScale.x, item.localScale.z);
            }
            foreach (var item in LeftRotationEngines)
            {
                item.localScale = new Vector3(item.localScale.x, item.localScale.x, item.localScale.z);
            }
        }

        //Calculate lateral engine effects
        Vector2 normVec = (transform.position + transform.up) - transform.position;
        Vector2 normVec2 = (transform.position + transform.right) - transform.position;

        float mainEngineDot = Vector2.Dot(shipForceDir, normVec);
        float latEngineDot = Vector2.Dot(shipForceDir, normVec2);

        foreach (Transform item in Engines)
        {
            item.localScale = new Vector3(item.localScale.x,mainEngineDot * 6f, item.localScale.z);
            item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y, 0, 6), item.localScale.z);
        }
        foreach (Transform item in ReverseEngines)
        {
            item.localScale = new Vector3(item.localScale.x, mainEngineDot * -4f, item.localScale.z);
            item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y, 0, 6), item.localScale.z);
        }
        foreach (Transform item in LeftLateralEngines)
        {
            item.localScale = new Vector3(item.localScale.x, latEngineDot * 4f, item.localScale.z);
            item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y, 0, 6), item.localScale.z);
        }
        foreach (Transform item in RightLateralEngines)
        {
            item.localScale = new Vector3(item.localScale.x, latEngineDot * -4f, item.localScale.z);
            item.localScale = new Vector3(item.localScale.x, Mathf.Clamp(item.localScale.y,0,6), item.localScale.z);
        }

        //Fire Rate
        if (timeToNextShot <= 0)
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
                if (!gm.IsGamePaused)
                {
                    Quaternion bulletRot = Quaternion.Euler(0, 0, transform.eulerAngles.z);
                    Instantiate(bullet, FirePoint.position, bulletRot);

                    aSource.Play();

                    canShoot = false;
                    timeToNextShot = timeBetweenShots;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(shipForceDir.magnitude == 1)
        {
            rb.AddForce(shipForceDir * Force);
        }

        //Apply forces from input
        if(moveInputY != 0)
        {
            //rb.AddForce(moveInputY * Vector2.up * Force);
        }
        if (moveInputX != 0)
        {
            //rb.AddForce(moveInputX * Vector2.right * Force);
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
        am.PlayOneShot("ShipExplosion");
        Instantiate(gm.ExplosionShip, transform.position, Quaternion.identity);
        cam.GetComponent<CamShake>().ShakeStrength = 1.5f;
        cam.GetComponent<CamShake>().camShakeActive = true;
        Instantiate(DestroyedRoot, transform.position, transform.rotation);

        gm.ShipDestroyed();
        Destroy(gameObject);
    }



}
