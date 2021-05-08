using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Bullet bullet;
    public GameObject SpritesRoot;
    public Transform Engine;
    public Transform ReverseEngine;
    public Transform LeftLateralEngine;
    public Transform RightLateralEngine;
    public Transform LeftRotationEngine;
    public Transform RightRotationEngine;
    public Transform FirePoint;

    public GameObject DestroyedRoot;

    //Components
    [HideInInspector]
    public Rigidbody2D rb;
    AudioSource aSource;
    Animation anim;

    //Input
    float moveInputX = 0;
    float moveInputY = 0;
    float mouseX = 0;
    float mouseY = 0;
    float mousePosX;
    float mousePosY;
    float rotateInput = 0;

    //References
    GameManager gm;
    AudioManager am;
    Camera cam;

    LayerMask hitLayer;

    Vector2 shipForceDir;
    bool damageable = true;
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

        StartCoroutine(StartInvincible());
    }

    private void Update()
    {
        /*
        print("Transform: " + transform.up);
        print("TransformDirection: " + transform.TransformDirection(transform.up));
        print("TransformInvDirection: " + transform.InverseTransformDirection(transform.up));
        print("TransformPoint: " + transform.TransformPoint(transform.up));
        print("TransformPoint: " + transform.TransformVector(transform.up));
        */

        //Catch lateral input
        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");
        //Unused
        rotateInput = Input.GetAxis("Rotate");

        //Calculate force direction
        shipForceDir = new Vector2(moveInputX, moveInputY).normalized;

        //Catch mouse input
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        //Catch mouse position
        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;

        //Mouse Aiming
        Vector2 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, RotSpeed * Time.deltaTime);

        //Calculate rotation engine effects
        Vector3 shipScreenPoint = cam.WorldToScreenPoint(transform.position);
        Vector3 rayPosScreen = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, shipScreenPoint.z));
        angleAimingDifference = Vector2.SignedAngle((transform.position + transform.up)-transform.position,rayPosScreen - transform.position);

        if (angleAimingDifference != 0)
        {
            if(angleAimingDifference > 0)
            {
                RightRotationEngine.localScale = new Vector3(RightRotationEngine.localScale.x, RightRotationEngine.localScale.y + 1, RightRotationEngine.localScale.z);
                RightRotationEngine.localScale = new Vector3(RightRotationEngine.localScale.x, Mathf.Clamp(RightRotationEngine.localScale.y, 0.6f, 3f), RightRotationEngine.localScale.z);
            }
            else if(angleAimingDifference < 0)
            {
                LeftRotationEngine.localScale = new Vector3(LeftRotationEngine.localScale.x, LeftRotationEngine.localScale.y + 1, LeftRotationEngine.localScale.z);
                LeftRotationEngine.localScale = new Vector3(LeftRotationEngine.localScale.x,Mathf.Clamp(LeftRotationEngine.localScale.y,0.6f,3f),LeftRotationEngine.localScale.z);
            }
        }
        else
        {
            RightRotationEngine.localScale = new Vector3(RightRotationEngine.localScale.x, RightRotationEngine.localScale.x, RightRotationEngine.localScale.z);
            LeftRotationEngine.localScale = new Vector3(LeftRotationEngine.localScale.x, LeftRotationEngine.localScale.x, LeftRotationEngine.localScale.z);
        }

        //Calculate lateral engine effects
        Vector2 normVec = (transform.position + transform.up) - transform.position;
        Vector2 normVec2 = (transform.position + transform.right) - transform.position;

        float mainEngineDot = Vector2.Dot(shipForceDir, normVec);
        float latEngineDot = Vector2.Dot(shipForceDir, normVec2);

        foreach (Transform item in Engine.transform)
        {
            item.localScale = new Vector3(item.localScale.x, mainEngineDot * 6f, item.localScale.z);
        }
        foreach (Transform item in ReverseEngine.transform)
        {
            item.localScale = new Vector3(item.localScale.x, mainEngineDot * -6f, item.localScale.z);
        }
        foreach (Transform item in LeftLateralEngine.transform)
        {
            item.localScale = new Vector3(item.localScale.x, latEngineDot * 5f, item.localScale.z);
        }
        foreach (Transform item in RightLateralEngine.transform)
        {
            item.localScale = new Vector3(item.localScale.x, latEngineDot * -5f, item.localScale.z);
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

                Quaternion bulletRot = Quaternion.Euler(0, 0, transform.eulerAngles.z);
                Bullet bulletInstance = Instantiate(bullet, FirePoint.position, bulletRot);
                //bulletInstance.dir = transform.up;

                float bulletAngle = bulletInstance.transform.eulerAngles.z + 90;
                Vector3 dirLine = new Vector3(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad), 0).normalized;
                print("Ship Z rot: " + bulletInstance.transform.eulerAngles.z);
                print("Bullet Z rot: " + bulletInstance.transform.eulerAngles.z);
                print("Dir line: " + dirLine);
                print("Bullet transform up: " + bullet.transform.transform.TransformDirection(bullet.transform.up));

                //Debug.DrawRay(bulletInstance.transform.position, dirLine * 10, Color.red);

                aSource.Play();

                canShoot = false;
                timeToNextShot = timeBetweenShots;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shipForceDir != Vector2.zero)
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
        am.Play("ShipExplosion");
        Instantiate(gm.ExplosionShip, transform.position, Quaternion.identity);

        Instantiate(DestroyedRoot, transform.position, transform.rotation);

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
